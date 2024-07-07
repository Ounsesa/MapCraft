using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    public Map Map;
    public GameObject TilePrefab;
    int TileSize = 1;
    public GameObject Player;

    private Camera mainCamera;

    private void Start()
    {
    }

    private void Update()
    {
        mainCamera = Camera.main;
        DetectMouseTile();
    }

    public void RenderMap()
    {
        for (int i = 0; i < Map.MapMatrix.Length; i++)
        {
            for (int j = 0; j < Map.MapMatrix[i].Length; j++)
            {
                if (Map.MapMatrix[i][j] < 0)
                {
                    continue;
                }
                // Calculate the position for the current tile
                Vector2 tilePosition = new Vector2(Map.MapPosition.x + j * TileSize, Map.MapPosition.y - i * TileSize);

                // Instantiate the tile prefab at the calculated position
                GameObject tile = Instantiate(TilePrefab, tilePosition, Quaternion.identity);

                SetBiomeColor(tile, Map.MapMatrix[i][j]);
                // Optionally, you can set the tile's parent to keep the hierarchy clean
                tile.transform.parent = this.transform;
            }
        }
    }

    void SetBiomeColor(GameObject Tile, int Biome)
    {
        switch (Biome)
        {
            case 0:
                Tile.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case 1:
                Tile.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 2:
                Tile.GetComponent<SpriteRenderer>().color = Color.gray;
                break;
            case 3:
                Tile.GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
        }
    }

    void DetectMouseTile()
    {
        // Get the mouse position in the world
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Adjust the z position to zero if you are working in 2D
        mousePosition.z = 0;

        // Calculate the grid position based on the mouse position
        int x = Mathf.FloorToInt((mousePosition.x + 0.5f) / TileSize);
        int y = Mathf.FloorToInt((mousePosition.y + 0.5f) / TileSize);

        //Debug.Log($"Mouse is over tile at ({x}, {y})");
        Player.transform.position = new Vector3(x , y , 0);
        //Check if the calculated position is within the bounds of the map
        if (x >= 0 && y >= 0 && x < Map.MapMatrix[0].Length && y < Map.MapMatrix.Length)
        {
            //Debug.Log($"Mouse is over tile at ({x}, {y}) with value {Map.MapMatrix[y][x]}");
        }
    }
}
