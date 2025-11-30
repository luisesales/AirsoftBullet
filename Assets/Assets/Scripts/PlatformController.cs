using UnityEngine;

public class PlatformController : MonoBehaviour
{

    [SerializeField]
    private float baseHeight = 5f;
    [SerializeField]
    private float amplitude = 10.5f;
    [SerializeField]
    private float speed = 1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newY = baseHeight + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
