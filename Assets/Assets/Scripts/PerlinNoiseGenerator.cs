using System;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    [SerializeField]
    private float scale = 5;
    [SerializeField]
    private float offsetX;
    [SerializeField]
    private float offsetY;
    [SerializeField]
    private int width = 10;
    [SerializeField]
    private int height = 10;

    private Renderer renderer;

    public float[,] noiseMap {private set; get;}


    void Start()
    {
        noiseMap = new float[width,height];
        renderer = GetComponent<Renderer>();
        // row_walls = new GameObject[10];     
    }

    void Update()
    {
        renderer.material.mainTexture = TextureGenerator();
        offsetX += MathF.Sin(Time.deltaTime * 0.1f);
        offsetY += MathF.Cos(Time.deltaTime * 0.1f);
    }

    Texture2D TextureGenerator()
    {
        Texture2D texture = new Texture2D(width, height);        
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {                
                float p_number = PerlinoiseNumberGenerator(i, j);
                noiseMap[i, j] = p_number;
                Color color = new Color(p_number, p_number, p_number);
                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();
        return texture;
    }

    private float PerlinoiseNumberGenerator(int x, int y)
    {
        float new_x = (float)x / width * scale + offsetX;
        float new_y = (float)y / height * scale + offsetY;
        float perlinoise = Mathf.PerlinNoise(new_x, new_y);
        return perlinoise;
    }
}
