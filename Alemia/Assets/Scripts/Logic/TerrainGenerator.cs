using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    #region Разное
    [System.Serializable]
    public struct Generator
    {
        public Noise[] noises;
    }
    [System.Serializable]
    public struct Biome
    {
        public string name;
        public Vector2 Humidity;
        public Vector2 Evil;
        public Vector2 Temperature;
        public Vector2 Magic;
        public Vector2 Life;
        public bool IsBiomeAllowed(float h, float t, float e, float m, float l)
        {
            if (h > Humidity.x && h < Humidity.y)
                if (e > Evil.x && e < Evil.y)
                    if (t > Temperature.x && t < Temperature.y)
                        if (m > Magic.x && m < Magic.y)
                            if (l > Life.x && l < Life.y)
                                return true;
            return false;
        }
        public TileBase biomeTile;
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

    public Generator hGen;
    public Generator tGen;
    public Generator eGen;
    public Generator mGen;
    public Generator lGen;

    public Biome[] biomes;

    public int chunkSize;
    public int chunkRenderDistance;
    public int maxChunksLoaded;

    private Tilemap ground;
    private Tilemap boundary;
    private Tilemap biomesTM;
    private Tilemap flowers;
    private Tilemap trees;

    private Transform player;

    private List<Vector2Int> loadedChunks;

    #endregion
    void NewGenerator(int chunkX, int chunkY)
    {
        GenerateBioms(chunkX, chunkY);
        //GenerateTerrain(chunkX, chunkY);
        //GenerateFlowers(chunkX, chunkY);
        //GenerateTrees(chunkX, chunkY);
        if (loadedChunks != null)
            loadedChunks.Add(new Vector2Int(chunkX, chunkY));
        if (boundary != null)
            boundary.GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
    }

    private void GenerateBioms(int chunkX, int chunkY)
    {

        for(int i = 0; i<chunkSize; i++)
        {
            for(int j = 0; j<chunkSize; j++)
            {
                int x = chunkX * chunkSize + i;
                int y = chunkY * chunkSize + j;
                
                float humidity, temperature, evil, magic, life;
                
                humidity =      GetNoise(hGen, x, y);
                temperature =   GetNoise(tGen, x, y);
                evil =          GetNoise(eGen, x, y);
                magic =         GetNoise(mGen, x, y);
                life =          GetNoise(lGen, x, y);
                //Debug.Log($"Params:{humidity},{temperature},{evil},{magic},{life}");
                biomesTM.SetTile(new Vector3Int(x, y, 0), biomes.Last().biomeTile);
                foreach(Biome b in biomes)
                {
                    if(b.IsBiomeAllowed(humidity,temperature,evil,magic,life))
                    {
                        biomesTM.SetTile(new Vector3Int(x,y,0), b.biomeTile);
                        break;
                    }
                }
            }
        }
    }

    private void Start()
    {
        ground = transform.GetChild(0).Find("Ground").GetComponent<Tilemap>();
        boundary = transform.GetChild(0).Find("Boundary").GetComponent<Tilemap>();
        biomesTM = transform.GetChild(0).Find("Biomes").GetComponent<Tilemap>();
        trees = transform.GetChild(0).Find("Biomes").GetComponent<Tilemap>();
        flowers = transform.GetChild(0).Find("Flowers").GetComponent<Tilemap>();

        loadedChunks = new List<Vector2Int>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        bool random=false;
        random = true;
        if(random)
        {
            foreach (Noise n in hGen.noises) n.seed = Random.Range(0, 100000);
            foreach (Noise n in tGen.noises) n.seed = Random.Range(0, 100000);
            foreach (Noise n in lGen.noises) n.seed = Random.Range(0, 100000);
            foreach (Noise n in eGen.noises) n.seed = Random.Range(0, 100000);
            foreach (Noise n in mGen.noises) n.seed = Random.Range(0, 100000);
        }
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
                    //generateChunk(cX, cY);
                    NewGenerator(cX, cY);
                }
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    float humidity, temperature, evil, magic, life;
                    int x = (int)player.position.x;
                    int y = (int)player.position.y;
                    humidity = GetNoise(hGen, x, y);
                    temperature = GetNoise(tGen, x, y);
                    evil = GetNoise(eGen, x, y);
                    magic = GetNoise(mGen, x, y);
                    life = GetNoise(lGen, x, y);
                    Biome a = new Biome();
                    foreach(Biome b in biomes)
                        if(b.IsBiomeAllowed(humidity,temperature,evil,magic,life))
                        {
                            a = b;
                            break;
                        }
                    Debug.Log($"This is {a.name}\nParams:{humidity},{temperature},{evil},{magic},{life}");
                }
            }
    }
}
