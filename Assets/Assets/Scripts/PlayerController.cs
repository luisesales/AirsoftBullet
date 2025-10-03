
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;


    [SerializeField]
    private float playerHeight;
    [SerializeField]
    private LayerMask isGround;

    [SerializeField]
    private float groundDrag;

    private bool grounded;
    private Transform selectedGunPosition;

    [SerializeField]
    private Transform orientation;
    private bool inCollectRange = false;



    // Input variables

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private GameObject selectedGun;
    private GameObject inRangeCollectable;
    private bool inRangeGun;

    private GameObject canvas;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent physics from rotating the player        
        selectedGunPosition = GameObject.FindWithTag("GunPos").transform;        
        canvas = GameObject.FindWithTag("Canvas");
        canvas.SetActive(false);
    }

    private void Update()
    {

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, isGround);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (inCollectRange && Input.GetKeyDown(KeyCode.E))
        {
            if (inRangeGun)
            {
                collectGun(inRangeCollectable);
            }
            else
            {
                collectMagazine(inRangeCollectable);
            }
            inCollectRange = false;
            canvas.SetActive(false);
        }
        // Cria um vetor de velocidade 2D (plano) a partir da velocidade do Rigidbody
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limita a velocidade se necessário
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;

            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }


        if (grounded)
        {
            rb.drag = groundDrag; // Apply ground linearDamping when grounded
        }
        else
        {
            rb.drag = 0; // No linearDamping when in the air
        }
    }

    private void FixedUpdate()
    {
        // Calculate the move direction based on input and orientation
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.Normalize(); // Normalize to prevent faster diagonal movement

        // Apply the movement force
        rb.AddForce(moveDirection * speed * 10f, ForceMode.Acceleration);
    }

    public void collectGun(GameObject newGun)
    {
        if (selectedGun != null)
        {
            Destroy(selectedGun); // Destroy the old selectedGun if it exists
        }
        selectedGun = newGun; // Assign the new selectedGun 
        GameObject camera = Camera.main.gameObject;       
        selectedGun.transform.SetParent(camera.transform); // Set the selectedGun as a child of the player        
        selectedGun.transform.position = selectedGunPosition.position; // Reset position relative to player
        selectedGun.transform.rotation = selectedGunPosition.rotation;
        selectedGun.GetComponent<GunController>().SelectGun(); // Call the SelectGun method to activate it
    }

    public void collectMagazine(GameObject magazine)
    {
        if (selectedGun != null)
        {
            selectedGun.GetComponent<GunController>().CollectAmmo(1);
            Destroy(magazine);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("OnTriggerEnter: " + collision.gameObject.name);
        inRangeGun = collision.gameObject.CompareTag("Weapon");        
        if ((inRangeGun && collision.gameObject != selectedGun) || (collision.gameObject.CompareTag("Magazine") && selectedGun != null))
        {
            canvas.SetActive(true);
            canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Press E to collect " + collision.gameObject.name;
            inCollectRange = true;
            inRangeCollectable = collision.gameObject;
        }
        
    }
    void OnTriggerExit(Collider collision)
    {
        Debug.Log("OnTriggerExit: " + collision.gameObject.name);
        inRangeGun = collision.gameObject.CompareTag("Weapon");
        if (inRangeGun || collision.gameObject.CompareTag("Magazine"))
        {
            canvas.SetActive(false);
            inCollectRange = false;            
        }        
    }
}
