using UnityEngine;

public class ObjectsInGame
{
    public ObjectsInGame(GameObject gb, int row, int col)
    {
        this.GetGameObject = gb;
        this.GetRow = row;
        this.GetCol = col;
    }

    public GameObject GetGameObject { get; }
    public int GetRow { get; }
    public int GetCol { get; }

    public string GetTag => GetGameObject.tag;
}
