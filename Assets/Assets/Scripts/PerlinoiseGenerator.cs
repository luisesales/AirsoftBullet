using System.Collections.Generic;
using UnityEngine;

public class PerlinoiseGenerator : MonoBehaviour
{
    [SerializeField]
    private float scale;
    [SerializeField]
    private float offsetX;
    [SerializeField]
    private float offsetY;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    private Renderer renderer;

    [SerializeField]
    private GameObject[] row_walls;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        row_walls = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            
        }
    }

    void Update()
    {
        renderer.material.mainTexture = TextureGenerator();
    }

    Texture2D TextureGenerator()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                float p_number = PerlinoiseNumberGenerator(i, j);
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
