using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float backspin = .02f;
    private LineRenderer lineRenderer;
    private List<Vector3> pathPoints = new List<Vector3>();

    private float Initialvelocity = 10f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Initialvelocity, ForceMode.VelocityChange);

    }

    void Update()
    {
        // Add current position to the path
        pathPoints.Add(transform.position);

        // Update LineRenderer
        lineRenderer.positionCount = pathPoints.Count;
        lineRenderer.SetPositions(pathPoints.ToArray());
    }
    void FixedUpdate()
    {


        Debug.Log(rb.velocity.magnitude);
        Vector3 magnusDirection = Vector3.Cross(rb.angularVelocity, rb.velocity).normalized;

        // float magnusMagnitude = (4f / 3f) * Mathf.PI * airDensity * Mathf.Pow(ballRadius, 3) * 
        //                             Vector3.Cross(rb.angularVelocity, rb.velocity).magnitude;
        Vector3 magnusForce = Mathf.Sqrt(rb.velocity.magnitude) * magnusDirection * backspin * Time.fixedDeltaTime;
        
        rb.AddForce(magnusForce);
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 2f);
    }

    public void SetInitialVelocity(float velocity)
    {
        Initialvelocity = velocity;
    }
}
