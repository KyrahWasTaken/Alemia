using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public class TerrainGenerator : MonoBehaviour
{
    #region Разное
    [System.Serializable]
    public struct Generator
    {
        public TileHeight[] tiles;
        public Noise[] noises;
    }
    [System.Serializable]
    public struct TileHeight
    {
        public TileBase tile;
        [Range(0, 1)]
        public float height;
        public bool Boundary;
        public bool isMask;
        public string maskName;
    }
    [System.Serializable]
    public class Noise
    {
        public float strength;
        public float seed;
        public float scaleX;
        public float scaleY;

        public float GetFloat(float x, float y)
        {
            return Mathf.PerlinNoise(x / scaleX + seed, y /scaleY + seed)*strength;
        }
    }
    public float GetNoise(Generator generator,float x, float y)
    {
        float output = 0;
        foreach(Noise n in generator.noises)
        {
            output += n.GetFloat(x, y);
        }
        return output / generator.noises.Sum(a=>a.strength);
    }
    #endregion
    #region Параметры
    public Generator[] generators;

    public int chunkSize;
    public int chunkRenderDistance;
    public int maxChunksLoaded;

    [HideInInspector]
    public Tilemap ground;
    [HideInInspector]
    public Tilemap boundary;
    
    private Transform player;

    private List<Vector2Int> loadedChunks;

    #endregion
    public void generateChunk(int chunkX, int chunkY)
    {
        foreach(Generator g in generators)
        for (int i = 0; i < chunkSize; i++)
        {
            for (int j = 0; j < chunkSize; j++)
            {
                float value = GetNoise(g,chunkX * chunkSize + i, chunkY * chunkSize + j);
                foreach (TileHeight t in g.tiles)
                {
                    if (value < t.height)
                    {
                        ground.SetTile(new Vector3Int(chunkX * chunkSize + i, chunkY * chunkSize + j, 0), t.tile);
                        if(t.Boundary)
                        {
                        boundary.SetTile(new Vector3Int(chunkX * chunkSize + i, chunkY * chunkSize + j, 0), t.tile);
                        }
                        break;
                    }
                }
            }
        }
        if(loadedChunks != null)
        loadedChunks.Add(new Vector2Int(chunkX, chunkY));
        if (boundary != null)
        boundary.GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
    }
    private void Start()
    {
        ground = transform.GetChild(0).Find("Ground").GetComponent<Tilemap>();
        boundary = transform.GetChild(0).Find("Boundary").GetComponent<Tilemap>();
        loadedChunks = new List<Vector2Int>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        Vector2Int playerChunk;
        playerChunk = new Vector2Int((int)player.position.x / chunkSize, (int)player.position.y / chunkSize);
        for (int i = -chunkRenderDistance; i < chunkRenderDistance + 1; i++)
            for (int j = -chunkRenderDistance; j < chunkRenderDistance + 1; j++) 
            {
                if(!loadedChunks.Contains(new Vector2Int(playerChunk.x + i, playerChunk.y + j)))
                {
                    int cX = playerChunk.x + i;
                    int cY = playerChunk.y + j;
                    generateChunk(cX, cY);
                }
            }
    }
}
