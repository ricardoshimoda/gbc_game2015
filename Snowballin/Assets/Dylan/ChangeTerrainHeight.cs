using UnityEngine;

using System.Collections;

public class ChangeTerrainHeight : MonoBehaviour
{
    public enum TerrainModificationAction
    {
        Raise,
        Lower,
        Flatten,
    }

    public float range;

    public int areaOfEffectSize;

    [Range(0.001f, 0.1f)]
    public float effectIncrement;

    public float sampledHeight;

    public TerrainModificationAction modificationAction;
    

    private Terrain targetTerrain;

    private TerrainData targetTerrainData;

    private float[,] terrainHeightMap;

    private int terrainHeightMapWidth;
    private int terrainHeightMapHeight;

    private void Update()
    {
        if (Camera.main)
        {
            if (Input.GetButton("Primary Action"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;


                if (Physics.Raycast(ray, out hit, range))
                {
                    Debug.Log("1");
                    if (GetTerrainAtObject(hit.transform.gameObject))
                    {
                        Debug.Log("2");
                        targetTerrain = GetTerrainAtObject(hit.transform.gameObject);
                        SetEditValues(targetTerrain);
                        Debug.Log("TerrainFound " + targetTerrain.name);
                    }

                    switch (modificationAction)
                    {
                        case TerrainModificationAction.Raise:
                            RaiseTerrain(targetTerrain, hit.point, effectIncrement);
                            break;

                        case TerrainModificationAction.Lower:
                            LowerTerrain(targetTerrain, hit.point, effectIncrement);
                            break;

                        case TerrainModificationAction.Flatten:
                            FlattenTerrain(targetTerrain, hit.point, sampledHeight);
                            break;
                    }
                }
            }

            if (Input.GetButton("Secondary Action"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, range))
                {
                    if (GetTerrainAtObject(hit.transform.gameObject))
                    {
                        targetTerrain = GetTerrainAtObject(hit.transform.gameObject);
                        SetEditValues(targetTerrain);
                    }

                    switch (modificationAction)
                    {
                        case TerrainModificationAction.Flatten:
                            sampledHeight = SampleHeight(targetTerrain, hit.point);
                            break;
                    }
                }
            }
        }
    }

    public void SetEditValues(Terrain terrain)
    {
        targetTerrainData = GetCurrentTerrainData();
        terrainHeightMap = GetCurrentTerrainHeightMap();
        terrainHeightMapWidth = GetCurrentTerrainWidth();
        terrainHeightMapHeight = GetCurrentTerrainHeight();
    }

    public void RaiseTerrain(Terrain terrain, Vector3 location, float effectIncrement)
    {
        int offset = areaOfEffectSize / 2;

        Vector3 tempCoord = (location - terrain.GetPosition());
        Vector3 coord;

        coord = new Vector3
            (
            (tempCoord.x / GetTerrainSize().x),
            (tempCoord.y / GetTerrainSize().y),
            (tempCoord.z / GetTerrainSize().z)
            );

        Vector3 locationInTerrain = new Vector3(coord.x * terrainHeightMapWidth, 0, coord.z * terrainHeightMapHeight);

        int terX = (int)locationInTerrain.x - offset;

        int terZ = (int)locationInTerrain.z - offset;
        
        float[,] heights = targetTerrainData.GetHeights(terX, terZ, areaOfEffectSize, areaOfEffectSize);

        for (int xx = 0; xx < areaOfEffectSize; xx++)
        {
            for (int yy = 0; yy < areaOfEffectSize; yy++)
            {
                heights[xx, yy] += (effectIncrement * Time.smoothDeltaTime);
            }
        }

        targetTerrainData.SetHeights(terX, terZ, heights);
    }

    public void LowerTerrain(Terrain terrain, Vector3 location, float effectIncrement)
    {
        int offset = areaOfEffectSize / 2;

        Vector3 tempCoord = (location - terrain.GetPosition());
        Vector3 coord;

        coord = new Vector3
            (
            (tempCoord.x / GetTerrainSize().x),
            (tempCoord.y / GetTerrainSize().y),
            (tempCoord.z / GetTerrainSize().z)
            );

        Vector3 locationInTerrain = new Vector3(coord.x * terrainHeightMapWidth, 0, coord.z * terrainHeightMapHeight);

        int terX = (int)locationInTerrain.x - offset;

        int terZ = (int)locationInTerrain.z - offset;

        float[,] heights = targetTerrainData.GetHeights(terX, terZ, areaOfEffectSize, areaOfEffectSize);

        for (int xx = 0; xx < areaOfEffectSize; xx++)
        {
            for (int yy = 0; yy < areaOfEffectSize; yy++)
            {
                heights[xx, yy] -= (effectIncrement * Time.smoothDeltaTime);
            }
        }

        targetTerrainData.SetHeights(terX, terZ, heights);
    }

    public void FlattenTerrain(Terrain terrain, Vector3 location, float sampledHeight)
    {
        int offset = areaOfEffectSize / 2;

        Vector3 tempCoord = (location - terrain.GetPosition());
        Vector3 coord;

        coord = new Vector3
            (
            (tempCoord.x / GetTerrainSize().x),
            (tempCoord.y / GetTerrainSize().y),
            (tempCoord.z / GetTerrainSize().z)
            );

        Vector3 locationInTerrain = new Vector3(coord.x * terrainHeightMapWidth, 0, coord.z * terrainHeightMapHeight);

        int terX = (int)locationInTerrain.x - offset;

        int terZ = (int)locationInTerrain.z - offset;

        float[,] heights = targetTerrainData.GetHeights(terX, terZ, areaOfEffectSize, areaOfEffectSize);

        for (int xx = 0; xx < areaOfEffectSize; xx++)
        {
            for (int yy = 0; yy < areaOfEffectSize; yy++)
            {
                if (heights[xx, yy] != sampledHeight)
                {
                    heights[xx, yy] = sampledHeight;
                }
            }
        }

        targetTerrainData.SetHeights(terX, terZ, heights);
    }

    public float SampleHeight(Terrain terrain, Vector3 location)
    {
        Vector3 tempCoord = (location - terrain.GetPosition());
        Vector3 coord;

        coord = new Vector3
            (
            (tempCoord.x / GetTerrainSize().x),
            (tempCoord.y / GetTerrainSize().y),
            (tempCoord.z / GetTerrainSize().z)
            );

        Vector3 locationInTerrain = new Vector3(coord.x * terrainHeightMapWidth, 0, coord.z * terrainHeightMapHeight);

        int terX = (int)locationInTerrain.x;

        int terZ = (int)locationInTerrain.z;

        return Mathf.LerpUnclamped(0f, 1f, (terrain.terrainData.GetHeight(terX, terZ) / terrain.terrainData.size.y));
    }

    public Terrain GetTerrainAtObject(GameObject gameObject)
    {
        if (gameObject.GetComponent<Terrain>())
        {
            return gameObject.GetComponent<Terrain>();
        }

        return default(Terrain);
    }

    public Terrain GetCurrentTerrain()
    {
        if (targetTerrain)
        {
            return targetTerrain;
        }

        return default(Terrain);
    }

    public TerrainData GetCurrentTerrainData()
    {
        if (targetTerrain)
        {
            return targetTerrain.terrainData;
        }

        return default(TerrainData);
    }

    public Vector3 GetTerrainSize()
    {
        if (targetTerrain)
        {
            return targetTerrain.terrainData.size;
        }

        return Vector3.zero;
    }

    public float[,] GetCurrentTerrainHeightMap()
    {
        if (targetTerrain)
        {
            return targetTerrain.terrainData.GetHeights(0, 0, targetTerrain.terrainData.heightmapWidth, targetTerrain.terrainData.heightmapHeight);
        }

        return default(float[,]);
    }

    public int GetCurrentTerrainWidth()
    {
        if (targetTerrain)
        {
            return targetTerrain.terrainData.heightmapWidth;
        }

        return 0;
    }

    public int GetCurrentTerrainHeight()
    {
        if (targetTerrain)
        {
            return targetTerrain.terrainData.heightmapHeight;
        }

        return 0;
    }
}
/*
 * public Terrain terrain;
    public float strength = 0.01f;
    public float maxHeight = 0.2f;

    private int heightmapWidth, heightmapHeight;
    private float[,] heights;
    private TerrainData terrainData;

    private void Start()
    {
        terrainData = terrain.terrainData;
        heightmapWidth = terrainData.heightmapWidth;
        heightmapHeight = terrainData.heightmapHeight;
        heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raise Terrain
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                RaiseTerrain(hit.point);
            }
        }

        // Lower Terrain
        if (Input.GetMouseButton(1))
        {
            if(Physics.Raycast(ray,out hit))
            {
                LowerTerrain(hit.point);
            }
        }
    }

    private void RaiseTerrain(Vector3 point)
    {
        int mouseX = (int)((point.x / terrainData.size.x) * heightmapWidth);
        int mouseZ = (int)((point.z / terrainData.size.z) * heightmapHeight);


        float[,] modifiedHeights = new float[1, 1];
        float y = heights[mouseX, mouseZ];
        float[] s = new float[4];
        s[0] = (heights[mouseX, mouseZ + 1]);
        s[1] = (heights[mouseX - 1, mouseZ]);
        s[2] = (heights[mouseX + 1, mouseZ]);
        s[3] = (heights[mouseX, mouseZ - 1]);

        y += strength * Time.deltaTime;
        for (int i = 0; i < s.Length; i++)
        {
            float f = s[i];
            f += (strength /2 ) * Time.deltaTime;
            
            if (f > maxHeight)
            {
                f = maxHeight;
            }

            modifiedHeights[0, 0] = f;
            heights[mouseX, mouseZ] = f;
            terrainData.SetHeights(mouseX, mouseZ, modifiedHeights);
        }

        if (y > maxHeight)
        {
            y = maxHeight;
        }

        modifiedHeights[0, 0] = y;
        heights[mouseX, mouseZ] = y;
        terrainData.SetHeights(mouseX, mouseZ, modifiedHeights);
    }


    private void LowerTerrain(Vector3 point)
    {
        int mouseX = (int)((point.x / terrainData.size.x) * heightmapWidth);
        int mouseZ = (int)((point.z / terrainData.size.z) * heightmapHeight);


        float[,] modifiedHeights = new float[1, 1];
        float y = heights[mouseX, mouseZ];
        y -= strength * Time.deltaTime;

        if (y < 0)
        {
            y = 0;
        }

        modifiedHeights[0, 0] = y;
        heights[mouseX, mouseZ] = y;
        terrainData.SetHeights(mouseX, mouseZ, modifiedHeights);
    }
 */
