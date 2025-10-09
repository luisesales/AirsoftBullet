using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TheWallController : MonoBehaviour
{
    [SerializeField]
    private GameObject targetPrefab;
    [SerializeField]
    private int maxTargets = 5;    
    private List<GameObject> spawnedTargets = new List<GameObject>();

    [SerializeField]
    private float cooldown = 1f;
    private float spawnRate;
     
    private List<Vector3> spawnerPositions = new List<Vector3>(); 
    private Queue<Vector3> activeQueue = new Queue<Vector3>();
    private Queue<Vector3> nextQueue = new Queue<Vector3>();

    [SerializeField]
    private float reshuffleThreshold = 0.2f; // 20% restante
    private bool preparingNext = false;

    public Vector3 GetNextSpawnPoint()
    {
        if (activeQueue.Count == 0) SwapQueues();
        return activeQueue.Count > 0 ? activeQueue.Dequeue() : Vector3.zero;
    }

    private IEnumerator PrepareNextQueue()
    {
        preparingNext = true;
        yield return null; // evita travar frame
        FillQueue(nextQueue);
        preparingNext = false;
    }

    private void FillQueue(Queue<Vector3> queue)
    {
        var shuffled = spawnerPositions.OrderBy(x => Random.value).ToList();
        queue.Clear();
        foreach (var p in shuffled) queue.Enqueue(p);
    }

    private void SwapQueues()
    {
        var temp = activeQueue;
        activeQueue = nextQueue;
        nextQueue = temp;
        nextQueue.Clear();
    }
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnerPositions.Add(transform.GetChild(i).position);
        }
        spawnRate = cooldown;
        FillQueue(activeQueue);
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown <= 0f)
        {
            if (activeQueue.Count == 0)
            {
                SwapQueues();
            }
            else if (!preparingNext && activeQueue.Count <= spawnerPositions.Count * reshuffleThreshold)
            {
                StartCoroutine(PrepareNextQueue());
            }
            cooldown = spawnRate;
            SpawnTarget();
        }
        else
        {
            cooldown -= Time.deltaTime;
        }

    }
    private void SpawnTarget()
    {
        if (spawnedTargets.Count >= maxTargets)
        {
            spawnedTargets.RemoveAt(0);
            Destroy(spawnedTargets[0]);            
        }
        spawnedTargets.Add(Instantiate(targetPrefab, GetNextSpawnPoint(), targetPrefab.transform.rotation));
    }

    public void DestroyTarget(GameObject target)
    {
        spawnedTargets.Remove(target);
        Destroy(target);        
    }    

}
