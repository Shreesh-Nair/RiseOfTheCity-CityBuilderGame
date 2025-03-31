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
    public float vehicleSpeed = 5f;
    public int maxVehiclesOnRoad = 10;
    
    private List<GameObject> activeVehicles = new List<GameObject>();
    private bool isSimulationRunning = false;
    
    void Start()
    {
        if (roadManager == null)
            roadManager = FindObjectOfType<RoadManager>();
            
        StartSimulation();
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
        
        // Clean up any remaining vehicles
        foreach (GameObject vehicle in activeVehicles)
        {
            if (vehicle != null)
                Destroy(vehicle);
        }
        
        activeVehicles.Clear();
    }
    
    private IEnumerator SpawnVehicles()
    {
        while (isSimulationRunning)
        {
            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval);
            
            // Only spawn if we're under the vehicle limit
            if (activeVehicles.Count < maxVehiclesOnRoad)
            {
                SpawnVehicle();
            }
        }
    }
    
    private void SpawnVehicle()
    {
        List<Vector3> roadNodes = roadManager.GetRoadNodes();
        
        // Make sure we have at least two road nodes (start and end)
        if (roadNodes.Count < 2)
            return;
            
        // Get a random vehicle prefab
        GameObject vehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
        
        // Find a valid start node (one that has connected roads)
        List<Vector3> startNodes = new List<Vector3>();
        foreach (Vector3 node in roadNodes)
        {
            if (roadManager.GetConnectedRoads(node).Count > 0)
            {
                startNodes.Add(node);
            }
        }
        
        if (startNodes.Count == 0)
            return;
            
        // Pick a random start node
        Vector3 startNode = startNodes[Random.Range(0, startNodes.Count)];
        
        // Spawn the vehicle
        GameObject vehicle = Instantiate(vehiclePrefab, startNode + new Vector3(0, 0.1f, 0), Quaternion.identity);
        activeVehicles.Add(vehicle);
        
        // Start vehicle movement coroutine
        StartCoroutine(MoveVehicle(vehicle, startNode));
    }
    
    private IEnumerator MoveVehicle(GameObject vehicle, Vector3 startNode)
    {
        Vector3 currentNode = startNode;
        bool reachedEnd = false;
        
        while (!reachedEnd && vehicle != null && isSimulationRunning)
        {
            // Get connected roads from current position
            List<Vector3> connectedRoads = roadManager.GetConnectedRoads(currentNode);
            
            // Remove the node we just came from (if applicable)
            if (connectedRoads.Contains(currentNode))
                connectedRoads.Remove(currentNode);
                
            // If no more connected roads, we've reached the end
            if (connectedRoads.Count == 0)
            {
                reachedEnd = true;
                break;
            }
            
            // Pick a random connected road to move to
            Vector3 nextNode = connectedRoads[Random.Range(0, connectedRoads.Count)];
            
            // Calculate direction to next node
            Vector3 direction = (nextNode - currentNode).normalized;
            
            // Rotate vehicle to face the direction
            if (vehicle != null)
            {
                vehicle.transform.rotation = Quaternion.LookRotation(direction);
                
                // Move vehicle towards next node
                float distance = Vector3.Distance(currentNode, nextNode);
                float travelTime = distance / vehicleSpeed;
                float elapsedTime = 0;
                
                Vector3 startPos = vehicle.transform.position;
                Vector3 targetPos = nextNode + new Vector3(0, 0.1f, 0); // Slight elevation to avoid z-fighting
                
                while (elapsedTime < travelTime && vehicle != null && isSimulationRunning)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / travelTime);
                    
                    if (vehicle != null)
                        vehicle.transform.position = Vector3.Lerp(startPos, targetPos, t);
                        
                    yield return null;
                }
                
                // Update current node
                currentNode = nextNode;
            }
            else
            {
                // Vehicle was destroyed
                break;
            }
        }
        
        // Despawn the vehicle
        if (vehicle != null)
        {
            activeVehicles.Remove(vehicle);
            Destroy(vehicle);
        }
    }
}
