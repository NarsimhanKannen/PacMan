using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region VARIABLES
    public static LevelManager Instance;

    public int maxRow;
    public int maxColumn;

    private bool[,] canMove;
    private string[,] objectTag;
    private bool[,] connected;

    public GameObject wallPrefab;
    public GameObject wallParent;
    public GameObject playerBuiltWallParent;

    public List<ObjectsInGame> spawnedWall = new List<ObjectsInGame>();
    #endregion

    #region INITIALIZATION
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        Init();
    }

    private void Init()
    {
        canMove = new bool[maxRow, maxColumn];
        connected = new bool[maxRow, maxColumn];
        objectTag = new string[maxRow, maxColumn];

        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxColumn; col++)
            {
                canMove[row, col] = true;
            }
        }
    }

    private void Start()
    {
        GenerateWall();
    }
    #endregion

    #region WALL FUNCTIONS
    private void GenerateWall()
    {
        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxColumn; col++)
            {
                Vector3 pos = transform.position;
                pos.x += col;
                pos.z += row;

                if ((row == 0 || row == maxRow - 1) || (col == 0 || col == maxColumn - 1))
                {
                    CreateWall(pos, row, col);

                }
            }
        }
    }

    private void CreateWall(Vector3 pos, int row, int col)
    {
        GameObject gb = Instantiate(wallPrefab, pos, transform.rotation);
        gb.transform.parent = wallParent.transform;
        gb.transform.tag = "Bounds";
        gb.transform.name = "[Row: " + row + "] [Col: " + col + "";

        FillGridPosition(row, col);
        FillGridTag(gb.transform.tag, row, col);
    }

    public void SpawnWall(int row, int col)
    {
        if (isCellEmpty(row, col))
        {
            GameObject gb = Instantiate(wallPrefab, new Vector3(col, 0, row), transform.rotation);
            gb.transform.parent = playerBuiltWallParent.transform;
            gb.transform.tag = "Wall";
            gb.transform.name = "[Row: " + row + "] [Col: " + col + "";

            spawnedWall.Add(new ObjectsInGame(gb, row, col));

            FillGridPosition(row, col);
            FillGridTag(gb.transform.tag, row, col);

            GridManager.Instance.CheckLastCell();
        }
    }
   

    public List<ObjectsInGame> GetSpawnedWall()
    {
        if (spawnedWall != null)
            return spawnedWall;
        return null;
    }

    public void SetConnection(int row, int col)
    {
        connected[row, col] = true;
    }

    public bool IsConnected(int row, int col)
    {
        if ((row > -1 && row < maxRow) && (col > -1 && col < maxColumn))
            return connected[row, col];
        return false;
    }
    #endregion

    #region GRID FUNCTIONS

    public int GetMaxRow()
    {
        return maxRow;
    }

    public int GetMaxCol()
    {
        return maxColumn;
    }

    public void FillGridPosition(int row, int col)
    {
        canMove[row, col] = false;
    }

    public void EmptyGridPosition(int row, int col)
    {
        canMove[row, col] = true;
    }

    public void FillGridTag(string tag, int row, int col)
    {
        objectTag[row, col] = tag;
    }


    public void RemoveGridTag(string tag, int row, int col)
    {
        if (objectTag[row, col] == tag)
            objectTag[row, col] = null;
    }

    public string GetGridTag(int row, int col)
    {
        if ((row > -1 && row < maxRow) && (col > -1 && col < maxColumn))
            return objectTag[row, col];
        return null;
    }

    public bool isCellEmpty(int row, int col)
    {
        if ((row > -1 && row < maxRow) && (col > -1 && col < maxColumn))
            return canMove[row, col];
        return false;
    }

    public float GetPercentageFilled()
    {
        if (GetSpawnedWall().Count > 0)
        {
            float totalS = (GetMaxCol() - 2) * (GetMaxRow() - 2);
            return GetSpawnedWall().Count / totalS * 100;
        }
        return 0;
    }
    #endregion
}

