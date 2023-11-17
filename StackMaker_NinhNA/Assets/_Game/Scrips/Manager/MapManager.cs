using System.Collections.Generic;
using UnityEngine;
using static Map;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public static MapManager Instance => instance = instance != null ? instance : FindObjectOfType<MapManager>();

    [SerializeField] private Transform plane;
    [SerializeField] private GameObject[] blockPref;

    public MapObj[][] CurrentMap => currentMap;
    public Grid StartPoint => startPoint;

    private MapObj[][] currentMap;
    private GameObject[][] _mapObject;
    private Grid startPoint;

    public void RenderMap(int level)
    {
        ClearMap();
        currentMap = GetMap(level);
        if (currentMap.Length != 0)
        {
            _mapObject = new GameObject[currentMap.Length][];
            for (int row = 0; row < currentMap.Length; row++)
            {
                List<GameObject> listGameObject = new();
                for (int col = 0; col < currentMap[row].Length; col++)
                {
                    Grid blockLocale = new(row, col);
                    MapObj blockValue = currentMap[row][col];
                    listGameObject.Add(InstantiateBlock(blockValue, blockLocale));
                    if (blockValue == MapObj.StartBlock)
                    {
                        startPoint = blockLocale;
                    }
                }
                _mapObject[row] = listGameObject.ToArray();
            }
        }
    }

    public void ReplaceMap(Grid grid, MapObj value)
    {
        CurrentMap[grid.Row][grid.Col] = value;
    }

    public void SetObject(Grid grid, GameObject gameObject)
    {
        gameObject.transform.SetParent(plane);
        _mapObject[grid.Row][grid.Col] = gameObject;
    }

    public GameObject GetObject(Grid grid)
    {
        GameObject gameObject = _mapObject[grid.Row][grid.Col];
        if (gameObject != null)
        {
            gameObject.transform.SetParent(null);
            _mapObject[grid.Row][grid.Col] = null;
        }
        return gameObject;
    }

    public bool CheckMapObj(Grid grid, MapObj mapObj)
    {
        return CurrentMap[grid.Row][grid.Col] == mapObj;
    }

    private void ClearMap()
    {
        foreach (Transform child in plane)
        {
            Destroy(child.gameObject);
        }
    }

    private GameObject InstantiateBlock(MapObj value, Grid grid)
    {
        Vector3 blockLocale = Map.GridToVector3(grid);
        GameObject blockPrefab = GetPrefab((int)value);
        if (blockPrefab != null)
        {
            GameObject gameObject = Instantiate(blockPrefab, blockLocale, Quaternion.identity);
            gameObject.transform.SetParent(plane);
            return gameObject;
        }
        return null;
    }

    private GameObject GetPrefab(int index)
    {
        if (index < blockPref.Length && index >= 0)
        {
            return blockPref[index];
        }
        return null;
    }
}
