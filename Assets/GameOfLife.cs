using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameOfLife : MonoBehaviour
{
    public int SizeX, SizeY;

    bool[] grid;
    bool[,] grid2;
    bool[,] copygrid;
    Texture2D tex;

    public int seed;
    Color[] pixels;

    public float timeStep;
    float t;
    bool generated;
    int[,] neighbours;

    [Range(0, 1)]
    public float startCells;

    public bool HighLife;

    public bool start;

    public Color AliveCol, DeadCol;
    // Start is called before the first frame update
    void Start()
    {
        
        neighbours = new int[,]{ { 1,-1,0},
        { 1,-1,0 } };
        t = timeStep;
        Random.InitState(seed);

        grid2 = new bool[SizeX, SizeY];

        if (!start) 
        {
            pixels = new Color[SizeX * SizeY];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color(Random.Range(0f, 0.3f), Random.Range(0f, 0.3f), Random.Range(0f, 0.3f), 1);
            }

            GenerateTexture(true); return; 
        }


        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                if(Random.Range(0f,1f) < startCells)
                {
                    grid2[x, y] = true;
                }
            }
        }
        GenerateTexture(false);
    }

    void GenerateTexture(bool rando)
    {
        tex = new Texture2D(SizeX , SizeY);
        tex.filterMode = FilterMode.Point;

        GetComponent<Renderer>().material.mainTexture = tex;

        if (!rando) { pixels = new Color[SizeX * SizeY]; }

        tex.SetPixels(pixels);
        tex.Apply();
        generated = true;
    }

    private void Update()
    {
        if (!generated || !start) { return; }
        t -= Time.deltaTime;
        if(t <= 0) { UpdateGrid(); t += timeStep; }
    }

    void UpdateGrid()
    {
        copygrid = (bool[,])grid2.Clone();

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                int ns = 0;
                if(neighbours2(x + 1, y)) { ns++; }
                if(neighbours2(x -1, y)) { ns++; }
                if(neighbours2(x, y + 1)) { ns++; }
                if(neighbours2(x, y - 1)) { ns++; }
                if(neighbours2(x -1, y - 1)) { ns++; }
                if(neighbours2(x + 1 , y + 1)) { ns++; }
                if(neighbours2(x - 1, y + 1)) { ns++; }
                if(neighbours2(x + 1, y - 1)) { ns++; }

                if(grid2[x,y] && ns < 2) { copygrid[x, y] = false; }
                if(grid2[x,y] && (ns == 2 || ns == 3)) { copygrid[x, y] = true; }
                if(grid2[x,y] && (ns > 3)) { copygrid[x, y] = false; }
                if(!grid2[x,y] && ns == 3) { copygrid[x, y] = true; }
                if(!grid2[x,y] && HighLife && ns == 6) { copygrid[x, y] = true; }
            }
        }

        grid2 = (bool[,])copygrid.Clone();

        updateTexture();
        
    }

    void updateTexture()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                if (grid2[x, y])
                {
                    pixels[x + (y * SizeX)] = AliveCol;
                }
                else { pixels[x + (y * SizeX)] = DeadCol; }
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
    }

    public void DrawupdateGrid(int x, int y, bool alive)
    {
        //grid2[index % SizeX, index/SizeX] = alive;
        grid2[x, y] = alive;
    }

    bool neighbours2 (int x, int y)
    {
        if(x < 0) {x += SizeX;}
        else if(x >= SizeX) { x -= SizeX; }

        if(y < 0) { y += SizeY; }
        else if(y >= SizeY) { y -= SizeY; }

        return grid2[x,y];
    }
}
