using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map
{
    public static int MapCount => MAPS.Length;
    private static readonly MapObj[][][] MAPS = ReadMap();

    public const int X = 1;
    public const int Y = 0;
    public const int Z = 1;

    public enum MapObj
    {
        WallBlock = 0,
        BrickBlock = 1,
        UnBrickBlock = 2,
        StartBlock = 3,
        FinishBlock = 4,
        UnBrickAndBrick = 8,
        None = 9,
    }

    public static Vector3 GridToVector3(Grid grid)
    {
        if (grid != null)
        {
            return new Vector3(grid.Col * X, Y, grid.Row * Z);
        }
        return Vector3.zero;
    }

    public static MapObj[][] GetMap(int level)
    {
        if (level < MAPS.Length)
        {
            return MAPS[level].Select(a => a.ToArray()).ToArray();
        }
        return new MapObj[][] { };
    }

    private static MapObj[][] ReadMap(int level)
    {
        string resourcePath = "Maps/Map" + level;

        TextAsset textMap = Resources.Load<TextAsset>(resourcePath);
        List<MapObj> tmpX = new();
        List<MapObj[]> tmpY = new();
        if (textMap != null)
        {
            foreach (byte byteData in textMap.bytes)
            {
                if (byteData != 13 && byteData != 10)
                {
                    MapObj mapObj = GetMapEnumByInt(byteData - 48);
                    tmpX.Add(mapObj);
                }
                else if (tmpX.Count != 0)
                {
                    tmpY.Insert(0, tmpX.ToArray());
                    tmpX.Clear();
                }
            }
            if (tmpX.Count != 0)
            {
                tmpY.Insert(0, tmpX.ToArray());
            }
        }
        return tmpY.ToArray();
    }

    private static MapObj[][][] ReadMap()
    {
        List<MapObj[][]> mapObjs = new List<MapObj[][]>();
        MapObj[][] tmpZ;
        int index = 0;
        while ((tmpZ = ReadMap(index)).Length != 0)
        {
            mapObjs.Add(tmpZ);
            index++;
        }
        return mapObjs.ToArray();
    }

    private static MapObj GetMapEnumByInt(int valueMap)
    {
        return Enum.IsDefined(typeof(MapObj), valueMap) ? (MapObj)valueMap : MapObj.None;
    }
}
