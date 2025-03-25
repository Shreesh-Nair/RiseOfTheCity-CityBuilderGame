using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Movement speed
    public float moveSpeed = 10f;
    
    // Mouse sensitivity
    public float mouseSensitivity = 2f;
    
    // Movement boundaries
    public Vector2 xBounds = new Vector2(-30f, 30f);
    public Vector2 yBounds = new Vector2(1f, 30f);
    public Vector2 zBounds = new Vector2(-30f, 30f);
    
    // Rotation limits (in degrees)
    public float minXRotation = -80f;
    public float maxXRotation = 80f;
    
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        
        // Store initial rotation
        rotationX = transform.eulerAngles.y;
        rotationY = transform.eulerAngles.x;
    }

    void Update()
    {
        ResetPositionAndRotation();
        // Mouse look
        if (Input.GetMouseButton(1)) // Right mouse button held down
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Adjust rotation based on mouse movement
            rotationX += mouseX;
            rotationY -= mouseY; // Inverted for intuitive control
            
            // Clamp vertical rotation
            rotationY = Mathf.Clamp(rotationY, minXRotation, maxXRotation);
            
            // Apply rotation
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }
        
        // Toggle cursor lock with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? 
                               CursorLockMode.None : CursorLockMode.Locked;
        }
        
        // Movement
        Vector3 moveDirection = Vector3.zero;
        
        // Get input values
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            moveDirection += transform.right;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.Space))
            moveDirection += Vector3.up;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            moveDirection += Vector3.down;
            
        // Normalize and apply movement
        if (moveDirection.magnitude > 0)
        {
            moveDirection.Normalize();
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        
        // Enforce bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, xBounds.x, xBounds.y);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, yBounds.x, yBounds.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, zBounds.x, zBounds.y);
        transform.position = clampedPosition;
    }

    void ResetPositionAndRotation()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            transform.position = new Vector3(10f, 20f, 10f);
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}