using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UserInputToChangeGridTiles : MonoBehaviour
{
    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private TileBase redTile;
    [SerializeField]
    private TileBase greenTile;
    [SerializeField]
    private TileBase blackTile;
    [SerializeField]
    private TileBase whiteTile;

    [HideInInspector]
    public List<Vector3> blackPosition = new List<Vector3>();
    private Vector3 storedMousePosition;
    private Vector3Int cellPosition;

    [HideInInspector]
    public Vector3Int endPosition;
    [HideInInspector]
    public Vector3Int startPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            storedMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cellPosition = tileMap.WorldToCell(storedMousePosition);

            if (startPosition != null)
            {
                tileMap.SetTile(startPosition, whiteTile);
            }
            tileMap.SetTile(cellPosition, greenTile);

            if (blackPosition.Contains(cellPosition))
            {
                blackPosition.Remove(cellPosition);
            }

            Debug.Log("Start position: " + cellPosition);

            startPosition = cellPosition;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            storedMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cellPosition = tileMap.WorldToCell(storedMousePosition);

            if (startPosition != null)
            {
                tileMap.SetTile(endPosition, whiteTile);
            }
            tileMap.SetTile(cellPosition, redTile);

            if (blackPosition.Contains(cellPosition))
            {
                blackPosition.Remove(cellPosition);
            }

            Debug.Log("End position: " + cellPosition);

            endPosition = cellPosition;
        }
        else if (Input.GetMouseButton(0))
        {
            storedMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cellPosition = tileMap.WorldToCell(storedMousePosition);

            if ((startPosition != null || endPosition != null) && (cellPosition == startPosition || cellPosition == endPosition))
            {
                return;
            }

            tileMap.SetTile(cellPosition, blackTile);

            if (!blackPosition.Contains(cellPosition))
            {
                blackPosition.Add(cellPosition);
            }

            Debug.Log("Black position: " + cellPosition);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            tileMap.ClearAllTiles();
            for (int i = 0; i < blackPosition.Count; i++)
            {
                blackPosition.Remove(blackPosition[i]);
            }
        }
    }

    public bool Walkable(Vector3Int neighbour)
    {
        if (blackPosition.Contains(neighbour))
        {
            return false;
        }
        else if (neighbour.x < 0 || neighbour.y < 0 || neighbour.x > GridMap.width || neighbour.y > GridMap.height)
        {
            return false;
        }
        return true;
    }
}