using System.Collections.Generic;
using UnityEngine;

public class WallRow : MonoBehaviour
{
    public List<GameObject> wall_row;

    [SerializeField]
    private float baseHeight = 5f;    
    [SerializeField]
    private float speed = 1f;

    private float totalAmplitude;

    [SerializeField]
    private float amplitude = 15.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalAmplitude = amplitude;   
    }

    // Update is called once per frame
    void Update()
    {
        float newY = baseHeight + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void updateAmplitude(float factor)
    {
        amplitude = totalAmplitude * factor;
    }
}
