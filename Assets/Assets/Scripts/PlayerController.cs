using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpForce;


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
    private float scrollInput;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private GameObject selectedGun;
    private GameObject inRangeCollectable;
    private bool inRangeGun;

    private GameObject proximityCanvas;

    [SerializeField]
    private BulletController bulletController;

    private float currentBackspin;
    private bool isGunSelected;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent physics from rotating the player        
        selectedGunPosition = GameObject.FindWithTag("GunPos").transform;
        proximityCanvas = GameObject.FindWithTag("ProximityCanvas");
        proximityCanvas.SetActive(false);

        currentBackspin = 0.0001f;
        bulletController.SetBackspin(currentBackspin);
    }

    private void Update()
    {

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, isGround);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if(isGunSelected)
        {
            ScrollInput();
        }
        

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
            proximityCanvas.SetActive(false);
        }
        // Cria um vetor de velocidade 2D (plano) a partir da velocidade do Rigidbody
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Limita a velocidade se necessário
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;

            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }


        if (grounded)
        {
            rb.linearDamping = groundDrag; // Apply ground linearDamping when grounded
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Jump force
            }
        }
        else
        {
            rb.linearDamping = 0; // No linearDamping when in the air
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
            selectedGun.transform.SetParent(null); //Remove the old selectedGun from child of the player 
            selectedGun.transform.rotation = Quaternion.identity; //Set old selectedGun to default rotation

            GunController gunController = new GunController(); //Create a new GunController
            gunController = selectedGun.GetComponent<GunController>(); //Gets the GunController component if it exists
            if (gunController != null)
            {
                gunController.TeleportToGunHolder(); //Teleport old selectedGun to gunHolder
            }
        }
        selectedGun = newGun; // Assign the new selectedGun 
        GameObject camera = Camera.main.gameObject;
        selectedGun.transform.SetParent(camera.transform); // Set the selectedGun as a child of the player        
        selectedGun.transform.position = selectedGunPosition.position; // Reset position relative to player
        selectedGun.transform.rotation = selectedGunPosition.rotation;
        selectedGun.GetComponent<GunController>().SelectGun(); // Call the SelectGun method to activate it
        isGunSelected = true;
    }

    public void collectMagazine(GameObject magazine)
    {
        if (selectedGun != null)
        {
            selectedGun.GetComponent<GunController>().CollectAmmo(1,magazine);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("OnTriggerEnter: " + collision.gameObject.name);
        inRangeGun = collision.gameObject.CompareTag("Weapon");
        if (selectedGun == null)
        {
            if (
                inRangeGun && collision.gameObject != selectedGun
            )
            {
                proximityCanvas.SetActive(true);
                proximityCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Press E to collect " + collision.gameObject.name;
                inCollectRange = true;
                inRangeCollectable = collision.gameObject;
            }
        }
        else
        {
            if (
                (inRangeGun && collision.gameObject != selectedGun) ||
                (collision.gameObject.CompareTag("Magazine") && (selectedGun.name == "Pistol" || selectedGun.name == "Rifle")) ||
                (collision.gameObject.CompareTag("ShotgunMagazine") && selectedGun.name == "Shotgun") ||
                (collision.gameObject.CompareTag("DrumMagazine") && selectedGun.name == "MotorRifle")
            )
            {
                proximityCanvas.SetActive(true);
                
                MagazineController magazineController = collision.gameObject.GetComponent<MagazineController>();
                if (magazineController != null)
                {
                    proximityCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Press E to collect " + collision.gameObject.name + "Mass:" +magazineController.GetBulletMass().ToString();
                }
                else
                {
                    proximityCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Press E to collect " + collision.gameObject.name;
                }

                inCollectRange = true;
                inRangeCollectable = collision.gameObject;
            }
        }
    }
    void OnTriggerExit(Collider collision)
    {
        Debug.Log("OnTriggerExit: " + collision.gameObject.name);
        inRangeGun = collision.gameObject.CompareTag("Weapon");
        if (inRangeGun ||
            collision.gameObject.CompareTag("Magazine") ||
            collision.gameObject.CompareTag("ShotgunMagazine") ||
            collision.gameObject.CompareTag("DrumMagazine"))
        {
            proximityCanvas.SetActive(false);
            inCollectRange = false;
        }
    }

    public void ScrollInput()
    {
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log($"ScrollInput:{scrollInput} - CurrentBackspin:{currentBackspin}");
        if (scrollInput > 0)
        {
            currentBackspin += scrollInput / 1000;
            currentBackspin = (float)Math.Round(currentBackspin, 5);
            bulletController.SetBackspin(currentBackspin);
        }
        else if (scrollInput < 0)
        {
            currentBackspin += scrollInput / 1000;
            currentBackspin = (float)Math.Round(currentBackspin, 5);
            bulletController.SetBackspin(currentBackspin);
        }
    }
    
    public float GetCurrentBackspin()
    {
        return currentBackspin;
    }
}
