using System.Collections.Generic;
using UnityEngine;

public class WallsController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] wall_matrix;

    [SerializeField]
    private float updateThreshold = 5f;
    private float currentThershold;

    private float[,] noiseMap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentThershold = updateThreshold;
        wall_matrix = GameObject.FindGameObjectsWithTag("TerrainPillar");
    }

    // Update is called once per frame
    void Update()
    {
        currentThershold -= Time.deltaTime;
        if (currentThershold <= 0f)
        {
            currentThershold = updateThreshold;
            updateWalls();
        }
        
    }

    public void updateWalls() // Variable with the texture
    {        
        for (int y = 0; y < noiseMap.GetLength(1); y++)
        {
            for (int x = 0; x < noiseMap.GetLength(0); x++)
            {
                wall_matrix[x + noiseMap.GetLength(0) * y].GetComponent<WallRow>().updateAmplitude(noiseMap[x,y]);                
            }
        }
    }
    public void updateNoiseMap(float[,] newNoiseMap)
    {
        noiseMap = newNoiseMap;
    }
}
