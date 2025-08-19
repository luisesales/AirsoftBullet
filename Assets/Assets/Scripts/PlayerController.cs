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

    [SerializeField]
    private Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent physics from rotating the player        
    }

    private void Update()
    {

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, isGround);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


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
   
}
