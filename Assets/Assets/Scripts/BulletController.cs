using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float backspin = 0.0001f;
    float radius = 0.003f;
    private LineRenderer lineRenderer;
    private List<Vector3> pathPoints = new List<Vector3>();

    private float speed = 100f;
    private float airDensity = 1.225f; // kg/m^3 at sea level

    private float dragCoefficient = 0.47f; // Typical value for a sphere

    private double magnusCoefficient = 0.000013;

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
        rb.AddForce(transform.up.normalized * speed * rb.mass + -transform.forward * (float)Math.Sqrt(speed) * backspin, ForceMode.Impulse);
        //rb.AddForce(transform.up * speed * rb.mass * (float)Math.Sqrt(speed) * backspin, ForceMode.Impulse);        
        Destroy(gameObject, 6f);
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


        //Debug.Log(rb.velocity.magnitude);



        Vector3 dragDirection = -rb.linearVelocity.normalized;
        Vector3 dragForce = 0.5f * airDensity * Mathf.Pow(rb.linearVelocity.magnitude, 2) * dragCoefficient * Mathf.PI * Mathf.Pow(radius, 2) * dragDirection;
        rb.AddForce(dragForce, ForceMode.Force);

        // Vector3 magnusDirection = Vector3.Cross(rb.angularVelocity, rb.velocity).normalized;
        Vector3 magnusDirection = Vector3.Cross(rb.linearVelocity, transform.right).normalized;
        Vector3 magnusForce = Mathf.Sqrt(rb.linearVelocity.magnitude) * magnusDirection * backspin;


        // Vector3 magnusDirection = Vector3.Cross(rb.velocity, transform.right).normalized;
        // float magnusMagnitude = 0.5f * backspin * airDensity * Mathf.Pow(radius, 2) * Mathf.PI * magnusCoefficient;
        // Vector3 magnusForce = Mathf.Sqrt(rb.velocity.magnitude) * magnusDirection * magnusMagnitude * Time.fixedDeltaTime;

        // Vector3 magnusDirection = Vector3.Cross(rb.velocity, transform.right).normalized;
        // Vector3 magnusForce = Mathf.Sqrt(rb.velocity.magnitude) * magnusDirection * backspin * Time.fixedDeltaTime;

        rb.AddForce(magnusForce, ForceMode.Force);

    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("OnTriggerEnter: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Target"))
        {
            GameController.Instance.UpdatePoints(1, collision.gameObject);
            Destroy(gameObject);
        }

    }
    
    public void SetBackspin(float backspin)
    {
        this.backspin = backspin;
    }
}
