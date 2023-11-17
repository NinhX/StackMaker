using System;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using static InputManager;
using static Map;

public class Player : MonoBehaviour
{
    [SerializeField] private int speed = 10;
    [SerializeField] private int layerBrickMap;
    [SerializeField] private int layerBrickPlayer;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform chan;
    [SerializeField] private Transform people;

    private static Player instance;
    public static event Action<Grid> OnAddBrick;
    public static event Action<Grid> OnRemoveBrick;
    public static Player Instance => instance = instance != null ? instance : FindObjectOfType<Player>();
    public int Diem => listBrick.Count;

    private MapManager mapManager;
    private Grid nextPoint;
    private Queue<Grid> lineGrid = new();
    private Queue<MapObj> lineMapObj = new();
    private MapObj nextMapObj;
    private Vector3 nextPointVector;
    private Stack<GameObject> listBrick = new();
    private Direct direct;
    private bool isMoving;

    private void Update()
    {
        people.position = chan.position;
        if (isMoving)
        {
            Move(nextPointVector);
            if (transform.position == nextPointVector)
            {
                CheckPoint();
                NextPoint();
            }
        }
    }

    public void OnInit()
    {
        anim.SetInteger("renwu", 0);
        people.position = chan.position = transform.position;
        mapManager = MapManager.Instance;
        nextPoint = mapManager.StartPoint;
        transform.position = Map.GridToVector3(nextPoint);
        nextPointVector = transform.position;
        isMoving = false;
        ClearBrick();
    }

    public void Control(Direct direct)
    {
        if (!isMoving && direct != Direct.None)
        {
            this.direct = direct;
            AddLine();
            NextPoint();
        }
    }

    public void AddBrick()
    {
        if (mapManager.CheckMapObj(nextPoint, MapObj.UnBrickAndBrick))
        {
            mapManager.ReplaceMap(nextPoint, MapObj.UnBrickBlock);
        }
        else
        {
            mapManager.ReplaceMap(nextPoint, MapObj.None);
        }
        GameObject brick = mapManager.GetObject(nextPoint);
        brick.transform.SetParent(transform);
        brick.layer = layerBrickPlayer;
        listBrick.Push(brick);

        OnAddBrick?.Invoke(nextPoint);
    }

    public void RemoveBrick()
    {
        if (listBrick.Count != 0)
        {
            if (mapManager.CheckMapObj(nextPoint, MapObj.UnBrickBlock))
            {
                mapManager.ReplaceMap(nextPoint, MapObj.UnBrickAndBrick);
            }
            else
            {
                mapManager.ReplaceMap(nextPoint, MapObj.BrickBlock);
            }
            GameObject brick = listBrick.Pop();
            brick.layer = layerBrickMap;
            mapManager.SetObject(nextPoint, brick);

            OnRemoveBrick?.Invoke(nextPoint);
        }
    }

    public void ClearBrick()
    {
        lineGrid.Clear();
        lineMapObj.Clear();
        while (listBrick.Count > 0)
        {
            Destroy(listBrick.Pop());
        }
    }

    private void AddLine()
    {
        int row = nextPoint.Row;
        int col = nextPoint.Col;

        while (true)
        {
            switch (direct)
            {
                case Direct.Forward:
                    row++;
                    break;
                case Direct.Back:
                    row--;
                    break;
                case Direct.Left:
                    col--;
                    break;
                case Direct.Right:
                    col++;
                    break;
            }

            MapObj mapObj = GetMapObj(row, col);

            if (mapObj == MapObj.WallBlock)
            {
                break;
            }
            isMoving = true;
            lineMapObj.Enqueue(mapObj);
            lineGrid.Enqueue(new Grid(row, col));
        }
    }

    private void NextPoint()
    {
        if (lineGrid.Count > 0)
        {
            nextMapObj = lineMapObj.Dequeue();
            nextPoint = lineGrid.Dequeue();
            nextPointVector = Map.GridToVector3(nextPoint);
        }
        else
        {
            isMoving = false;
        }
    }

    private void CheckPoint()
    {
        switch (nextMapObj)
        {
            case MapObj.BrickBlock:
            case MapObj.UnBrickAndBrick:
                AddBrick();
                break;
            case MapObj.UnBrickBlock:
                RemoveBrick();
                break;
            case MapObj.FinishBlock:
                anim.SetInteger("renwu", 2);
                GameManager.Instance.ChangeState(StateManager.Victory);
                break;
        }
    }

    private void Move(Vector3 endPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);
    }

    private MapObj GetMapObj(int row, int col)
    {
        if (row < 0
            || col < 0
            || row >= mapManager.CurrentMap.Length
            || col >= mapManager.CurrentMap[row].Length)
        {
            return MapObj.WallBlock;
        }
        return mapManager.CurrentMap[row][col];
    }
}
