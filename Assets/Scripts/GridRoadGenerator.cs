using System;
using System.Collections.Generic;
using System.Linq;
using AStar;
using Grid;
using UnityEngine;

public class GridRoadGenerator : MonoBehaviour
{
    public AStarGrid Grid;
    public Sprite RoadSprite;

    public Vector2Int RoadStartPosition;
    public Vector2Int RoadEndPosition;
    
    private readonly Dictionary<Tile, Sprite> _previousTileInfo = new Dictionary<Tile, Sprite>();

    public void GenerateRoad()
    {
        ResetTiles();
        
        Tile[] result = Grid.GetShortestPath(Grid[RoadStartPosition].First(), 
                                             Grid[RoadEndPosition].First());

        foreach (Tile tile in result)
        {
            _previousTileInfo.Add(tile, tile.SpriteRenderer.sprite);
            tile.SpriteRenderer.sprite = RoadSprite;
        }
    }

    private void ResetTiles()
    {
        if (_previousTileInfo.Count == 0)
            return;

        foreach (KeyValuePair<Tile,Sprite> pair in _previousTileInfo) 
            pair.Key.SpriteRenderer.sprite = pair.Value;
        
        _previousTileInfo.Clear();
    }
}