using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheWallController : MonoBehaviour
{
    [SerializeField]
    private GameObject targetPrefab;
    [SerializeField]
    private int maxTargets = 5;
    private List<Vector3> spawnerPositions = new List<Vector3>();
    private List<GameObject> spawnedTargets = new List<GameObject>();

    [SerializeField]
    private float cooldown = 1f;
    private float spawnRate;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnerPositions.Add(transform.GetChild(i).position);
        }
        spawnRate = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown <= 0f)
        {
            SpawnTarget();
            cooldown = spawnRate;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }

    }
    public void SpawnTarget()
    {
        if (spawnedTargets.Count >= maxTargets)
        {
            Destroy(spawnedTargets[0]);
            spawnedTargets.RemoveAt(0);
        }
        int randomIndex = Random.Range(0, spawnerPositions.Count);        
        spawnedTargets.Add(Instantiate(targetPrefab, spawnerPositions[randomIndex], targetPrefab.transform.rotation));
    }
}
