using System.Collections.Generic;
using UnityEngine;

public class WallRow : MonoBehaviour
{
    public List<GameObject> wall_row;

    [SerializeField]
    private float baseHeight = 0f;    
    [SerializeField]
    private float speed = 1f;

    private float totalAmplitude;

    [SerializeField]
    private float amplitude = 30;
    private bool reachedPos = true;
    private bool reachedBottom = false;

    private float targetY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalAmplitude = amplitude;   
        baseHeight = transform.position.y - 10f;
    }

    // Update is called once per frame
    void Update()
    {           
        if (!reachedPos)
        {
            float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * speed);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (Mathf.Abs(transform.position.y - targetY) < 0.05f)
            {
                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
                reachedPos = true;
            }
        }
        //transform.position = new Vector3(transform.position.x, baseHeight * amplitude, transform.position.z);
    }

    public void updateAmplitude(float factor)
    {
        factor = Mathf.Clamp01(factor);

        amplitude = totalAmplitude * factor;
        targetY = baseHeight + amplitude;
        reachedPos = false;
        
    }
}
