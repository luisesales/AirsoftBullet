using UnityEngine;

public class CameraController : MonoBehaviour
{

     [SerializeField]
    private float sensX;
    [SerializeField]
    private float sensY;

    [SerializeField]
    private Transform orientation;
  
    
    private float rotationX;
    private float rotationY;

    private Transform camPosition;
    void Start()
    {
        camPosition = GameObject.FindWithTag("CameraPos").transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }    
    // Update is called once per frame
    void Update()
    {
        transform.position = camPosition.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.fixedDeltaTime;

        rotationY += mouseX;
        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        orientation.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }
}
