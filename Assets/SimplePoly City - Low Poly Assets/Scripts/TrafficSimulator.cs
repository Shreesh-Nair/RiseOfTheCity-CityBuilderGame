using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSimulator : MonoBehaviour
{
    [Header("References")]
    public RoadManager roadManager;

    [Header("Vehicle Prefabs")]
    public GameObject[] vehiclePrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public float minVehicleSpeed = 3f;
    public float maxVehicleSpeed = 7f;
    [Tooltip("Base number of vehicles per road segment")]
    public float vehiclesPerRoadRatio = 0.5f;
    [Tooltip("Maximum vehicles regardless of road count")]
    public int absoluteMaxVehicles = 50;
    
    [Header("Traffic Behavior")]
    [Tooltip("Distance to maintain between vehicles")]
    public float safeDistance = 2.5f;
    [Tooltip("How far ahead vehicles check for obstacles")]
    public float detectionDistance = 5f;
    [Tooltip("Lane offset from center (positive = left side driving)")]
    public float laneOffset = 0.4f;
    [Tooltip("How quickly vehicles can change direction")]
    public float steeringSpeed = 2f;
    [Tooltip("Minimum time to follow current path before considering a turn")]
    public float minStraightPathTime = 5f;
    
    [Header("Debug")]
    public bool showDebugRays = false;

    private List<VehicleController> activeVehicles = new List<VehicleController>();
    private bool isSimulationRunning = false;
    private Dictionary<Vector3, float> nodeLastUsedTime = new Dictionary<Vector3, float>();
    private int maxVehiclesOnRoad = 10;
    
    private class VehicleController
    {
        public GameObject vehicle;
        public Vector3 currentNode;
        public Vector3 targetNode;
        public Vector3 previousNode;
        public float speed;
        public float pathFollowTime;
        public Quaternion targetRotation;
        public List<Vector3> visitedNodes = new List<Vector3>();
        
        public VehicleController(GameObject vehicle, Vector3 startNode)
        {
            this.vehicle = vehicle;
            this.currentNode = startNode;
            this.previousNode = startNode;
            this.speed = 0f;
            this.pathFollowTime = 0f;
            this.visitedNodes.Add(startNode);
        }
    }

    void Start()
    {
        if (roadManager == null)
            roadManager = FindFirstObjectByType<RoadManager>();
        StartSimulation();
    }

    void Update()
    {
        UpdateVehicleLimit();
        for (int i = activeVehicles.Count - 1; i >= 0; i--)
        {
            VehicleController vehicle = activeVehicles[i];
            if (vehicle != null && vehicle.vehicle != null)
            {
                vehicle.pathFollowTime += Time.deltaTime;
            }
        }
    }
    
    void UpdateVehicleLimit()
    {
        List<Vector3> roadNodes = roadManager.GetRoadNodes();
        int roadCount = roadNodes.Count;
        maxVehiclesOnRoad = Mathf.Min(
            Mathf.RoundToInt(roadCount * vehiclesPerRoadRatio), 
            absoluteMaxVehicles
        );
        if (roadCount > 0 && maxVehiclesOnRoad < 1)
            maxVehiclesOnRoad = 1;
    }

    public void StartSimulation()
    {
        if (!isSimulationRunning && roadManager != null)
        {
            isSimulationRunning = true;
            StartCoroutine(SpawnVehicles());
        }
    }

    public void StopSimulation()
    {
        isSimulationRunning = false;
        StopAllCoroutines();
        foreach (VehicleController controller in activeVehicles)
        {
            if (controller != null && controller.vehicle != null)
                Destroy(controller.vehicle);
        }
        activeVehicles.Clear();
        nodeLastUsedTime.Clear();
    }

    private IEnumerator SpawnVehicles()
    {
        while (isSimulationRunning)
        {
            yield return new WaitForSeconds(spawnInterval * Random.Range(0.8f, 1.2f));
            if (activeVehicles.Count < maxVehiclesOnRoad)
            {
                SpawnVehicle();
            }
        }
    }

    private void SpawnVehicle()
    {
        List<Vector3> roadNodes = roadManager.GetRoadNodes();
        if (roadNodes.Count < 2) return;

        List<Vector3> startNodes = new List<Vector3>();
        foreach (Vector3 node in roadNodes)
        {
            List<Vector3> connections = roadManager.GetConnectedRoads(node);
            if (connections.Count > 0)
            {
                if (!nodeLastUsedTime.ContainsKey(node) ||
                    Time.time - nodeLastUsedTime[node] > spawnInterval * 2)
                {
                    startNodes.Add(node);
                }
            }
        }

        if (startNodes.Count == 0) return;

        Vector3 startNode = startNodes[Random.Range(0, startNodes.Count)];
        nodeLastUsedTime[startNode] = Time.time;

        GameObject vehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];

        List<Vector3> initialConnections = roadManager.GetConnectedRoads(startNode);
        if (initialConnections.Count == 0) return;

        Vector3 nextNode = initialConnections[Random.Range(0, initialConnections.Count)];
        Vector3 direction = (nextNode - startNode).normalized;

        Vector3 spawnPosition = startNode + new Vector3(0, 0.1f, 0);
        Vector3 rightVector = Vector3.Cross(Vector3.up, direction);
        spawnPosition += rightVector * laneOffset;

        GameObject vehicle = Instantiate(vehiclePrefab, spawnPosition, Quaternion.LookRotation(direction));
        
        VehicleController controller = new VehicleController(vehicle, startNode);
        controller.targetNode = nextNode;
        controller.speed = Random.Range(minVehicleSpeed, maxVehicleSpeed);
        controller.targetRotation = Quaternion.LookRotation(direction);
        activeVehicles.Add(controller);

        StartCoroutine(MoveVehicle(controller));
    }

    private IEnumerator MoveVehicle(VehicleController controller)
    {
        bool reachedEnd = false;
        int stuckCounter = 0;
        Vector3 lastPosition = controller.vehicle.transform.position;

        while (!reachedEnd && controller.vehicle != null && isSimulationRunning)
        {
            Vector3 currentPosition = controller.vehicle.transform.position;
            Vector3 direction = (controller.targetNode - currentPosition).normalized;
            
            Vector3 rightVector = Vector3.Cross(Vector3.up, direction);
            Vector3 targetPosition = controller.targetNode + rightVector * laneOffset;
            
            float distanceToTarget = Vector3.Distance(currentPosition, targetPosition);
            
            bool obstacleDetected = false;
            float currentSpeed = controller.speed;
            
            if (Physics.SphereCast(currentPosition, 0.5f, controller.vehicle.transform.forward, 
                out RaycastHit hit, detectionDistance))
            {
                VehicleController hitVehicle = GetVehicleAtPosition(hit.transform.position);
                if (hitVehicle != null)
                {
                    obstacleDetected = true;
                    float distanceToVehicle = hit.distance;
                    if (distanceToVehicle < safeDistance)
                    {
                        currentSpeed = Mathf.Min(currentSpeed * 0.5f, hitVehicle.speed * 0.8f);
                    }
                    else
                    {
                        currentSpeed = Mathf.Min(currentSpeed, hitVehicle.speed);
                    }
                }
            }
            
            if (showDebugRays)
            {
                Debug.DrawRay(currentPosition, controller.vehicle.transform.forward * detectionDistance, 
                    obstacleDetected ? Color.red : Color.green);
            }
            
            if (Vector3.Distance(currentPosition, lastPosition) < 0.01f)
            {
                stuckCounter++;
                if (stuckCounter > 100)
                {
                    reachedEnd = true;
                    break;
                }
            }
            else
            {
                stuckCounter = 0;
                lastPosition = currentPosition;
            }
            
            if (distanceToTarget < 0.5f)
            {
                controller.previousNode = controller.currentNode;
                controller.currentNode = controller.targetNode;
                controller.visitedNodes.Add(controller.currentNode);
                
                List<Vector3> nextConnections = roadManager.GetConnectedRoads(controller.currentNode);
                nextConnections.RemoveAll(node => Vector3.Distance(node, controller.previousNode) < 0.1f);
                
                if (nextConnections.Count == 0)
                {
                    reachedEnd = true;
                    break;
                }
                
                if (controller.visitedNodes.Count > 3)
                {
                    List<Vector3> recentNodes = controller.visitedNodes.GetRange(
                        controller.visitedNodes.Count - 3, 3);
                    
                    if (nextConnections.Count > 1)
                    {
                        List<Vector3> filteredConnections = new List<Vector3>();
                        foreach (Vector3 node in nextConnections)
                        {
                            if (!recentNodes.Contains(node))
                                filteredConnections.Add(node);
                        }
                        
                        if (filteredConnections.Count > 0)
                            nextConnections = filteredConnections;
                    }
                }
                
                // Modified path selection logic to allow more turns
                float turnChance = controller.pathFollowTime < minStraightPathTime ? 0.5f : 0.7f;
                Vector3 bestNextNode;

                if (Random.value < turnChance && nextConnections.Count > 1)
                {
                    // Choose a random direction that isn't the way we came
                    bestNextNode = nextConnections[Random.Range(0, nextConnections.Count)];
                }
                else
                {
                    // Original logic - prefer continuing straight
                    Vector3 forwardDirection = (controller.currentNode - controller.previousNode).normalized;
                    bestNextNode = nextConnections[0];
                    float bestDirectionDot = -1f;
                    
                    foreach (Vector3 nextNode in nextConnections)
                    {
                        Vector3 nextDirection = (nextNode - controller.currentNode).normalized;
                        float directionDot = Vector3.Dot(forwardDirection, nextDirection);
                        
                        if (directionDot > bestDirectionDot)
                        {
                            bestDirectionDot = directionDot;
                            bestNextNode = nextNode;
                        }
                    }
                }

                // Verify the next node is actually a road
                if (!roadManager.IsRoadAt(bestNextNode))
                {
                    reachedEnd = true;
                    break;
                }
                
                Vector3 newDirection = (bestNextNode - controller.currentNode).normalized;
                if (Vector3.Dot((controller.currentNode - controller.previousNode).normalized, newDirection) < 0.7f)
                {
                    controller.pathFollowTime = 0f;
                }
                
                controller.targetNode = bestNextNode;
            }
            
            direction = (targetPosition - currentPosition).normalized;
            controller.targetRotation = Quaternion.LookRotation(direction);
            
            controller.vehicle.transform.rotation = Quaternion.Slerp(
                controller.vehicle.transform.rotation,
                controller.targetRotation,
                Time.deltaTime * steeringSpeed
            );
            
            float moveDistance = currentSpeed * Time.deltaTime;
            controller.vehicle.transform.position += controller.vehicle.transform.forward * moveDistance;
            
            yield return null;
        }

        if (controller.vehicle != null)
        {
            activeVehicles.Remove(controller);
            Destroy(controller.vehicle);
        }
    }
    
    private VehicleController GetVehicleAtPosition(Vector3 position)
    {
        foreach (VehicleController controller in activeVehicles)
        {
            if (controller.vehicle != null && 
                Vector3.Distance(controller.vehicle.transform.position, position) < 1.0f)
            {
                return controller;
            }
        }
        return null;
    }
}
