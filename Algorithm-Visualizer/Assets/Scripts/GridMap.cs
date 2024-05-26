using UnityEngine;

public class GridMap : MonoBehaviour
{
    public static int width = 60;
    public static int height = 35;
    public GameObject[,] cells;
    public bool showGrid = true;

    [SerializeField]
    private GameObject grid;
    [SerializeField]
    private float cellSize;
    [SerializeField]
    private Vector3 originPosition;
    [SerializeField]
    private GameObject cellPrefab;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        cells = new GameObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 cellPosition = GetWorldPosition(x, y);
                GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                cell.transform.SetParent(grid.transform);
                cells[x, y] = cell;
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void OnDrawGizmos()
    {
        if (showGrid)
        {
            Gizmos.color = Color.black;
            for (int x = 0; x < width; x++)
            {
                Gizmos.DrawLine(GetWorldPosition(x, 0), GetWorldPosition(x, height));
            }
            for (int y = 0; y < height; y++)
            {
                Gizmos.DrawLine(GetWorldPosition(0, y), GetWorldPosition(width, y));
            }
        }
    }
}
