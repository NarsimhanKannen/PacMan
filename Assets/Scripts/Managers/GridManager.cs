using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    #region VARIABLES
    public static GridManager Instance;

    private int maxRow;
    private int maxCol;

    private int rowIdx;
    private int colIdx;

    private int count = 0;
    #endregion

    #region INITILIZATION
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
    }

    private void Start()
    {
        maxRow = LevelManager.Instance.GetMaxRow();
        maxCol = LevelManager.Instance.GetMaxCol();
    }

    #endregion

    #region Class Functions
    //Starts to Check from the last cell
    public void CheckLastCell()
    {
        List<ObjectsInGame> oj = LevelManager.Instance.GetSpawnedWall();

        if (oj.Count > 0)
        {
            count = 0;

            for (int row = -1; row < 2; row += 2)
            {
                rowIdx = oj[oj.Count - 1].GetRow + row;
                colIdx = oj[oj.Count - 1].GetCol;
                //print("Row: " + rowIdx + " Col: " + colIdx);
                if (!LevelManager.Instance.isCellEmpty(rowIdx, colIdx))
                {
                    if (LevelManager.Instance.GetGridTag(rowIdx, colIdx) =="Wall")
                        count++;
                }

            }

            for (int col = -1; col < 2; col += 2)
            {
                rowIdx = oj[oj.Count - 1].GetRow;
                colIdx = oj[oj.Count - 1].GetCol + col;
                //print("Row: " + rowIdx + " Col: " + colIdx);
                if (!LevelManager.Instance.isCellEmpty(rowIdx, colIdx))
                {
                    if (LevelManager.Instance.GetGridTag(rowIdx, colIdx) == "Wall")
                        count++;
                }
            }

            if (count > 1)
            {
                //print(count);
                AddAdjacentCells(ref oj);
                return;
            }
        }
    }

    //Adds Adjacent Tiles to the list
    private void AddAdjacentCells(ref List<ObjectsInGame> oj)
    {
        List<ObjectsInGame> parallel = new List<ObjectsInGame>();

        for (int i = oj.Count - 1; i > -1; i--)
        {
            if (IsConnected(ref oj, i))
            {
                LevelManager.Instance.SetConnection(oj[i].GetRow, oj[i].GetCol);
                parallel.Add(oj[i]);
            }
        }

        if (parallel.Count > 0)
        {
            foreach (var p in parallel)
            {
                p.GetGameObject.transform.name = "Connected";
            }
            AddPoints();
           // print("Connected Cell Count: " + parallel.Count);
            return;
        }
        else
        {
           // print("No cells connected");
            return;
        }
    }

    //Checks If Tiles are connected with eachother
    private bool IsConnected(ref List<ObjectsInGame> oj, int currentIdk)
    {
        int idxToCheck = currentIdk - 1;

        if (idxToCheck < 0)
        {
            idxToCheck = oj.Count - 1;
        }

        if ((oj[idxToCheck].GetRow == oj[currentIdk].GetRow) || (oj[idxToCheck].GetCol == oj[currentIdk].GetCol))
        {
            return true;
        }
        return false;
    }

    //Algorithm To Fill Enclosed Area
    private void AddPoints()
    {
        for (int row = 1; row < maxRow; row++)
        {
            int startPos = 0;
            for (int col = 1; col < maxCol; col++)
            {
                int rIdx = row;
                int cIdx = col;

                if (LevelManager.Instance.GetGridTag(rIdx, cIdx) == "Wall")
                {
                    if (startPos == 0)
                    {
                        startPos = cIdx;
                    }
                    else if (startPos != 0)
                    {
                        for (int v = 0; v < cIdx - startPos; v++)
                        {
                            if (LevelManager.Instance.isCellEmpty(rIdx, startPos + v))
                            {
                                LevelManager.Instance.SetConnection(rIdx, startPos + v);
                                
                                GameObject gb = Instantiate(LevelManager.Instance.wallPrefab, new Vector3(startPos + v, 0, rIdx), transform.rotation);
                               
                                LevelManager.Instance.GetSpawnedWall().Add(new ObjectsInGame(gb, rIdx, startPos + v));
                                LevelManager.Instance.FillGridPosition(rIdx, startPos + v);
                                LevelManager.Instance.FillGridTag("Wall", rIdx, startPos + v);

                            }
                            else if (LevelManager.Instance.GetGridTag(rIdx, startPos + v) == "Monster")
                            {
                                LevelManager.Instance.SetConnection(rIdx, startPos + v);
                                
                                GameObject gb = Instantiate(LevelManager.Instance.wallPrefab, new Vector3(startPos + v, 0, rIdx), transform.rotation);
                                
                                LevelManager.Instance.GetSpawnedWall().Add(new ObjectsInGame(gb, rIdx, startPos + v));
                                LevelManager.Instance.FillGridPosition(rIdx, startPos + v);
                                LevelManager.Instance.FillGridTag("Wall", rIdx, startPos + v);
                            }
                        }
                        startPos = 0;
                    }
                }
            }
        }
    }
    #endregion
}
