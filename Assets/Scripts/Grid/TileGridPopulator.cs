using System;
using System.Linq;
using UnityEngine;

namespace Grid
{
    public class TileGridPopulator : MonoBehaviour
    {
        public Tile[] TilePrefabs;
        public TileGrid TargetGrid;
        
        private Tile RandomTilePrefab
        {
            get
            {
                return TilePrefabs.OrderBy(item => Guid.NewGuid())
                                  .First();
            }
        }
        
        private void Awake()
        {
            for (int x = 0; x < TargetGrid.Width; x++)
            for (int y = 0; y < TargetGrid.Height; y++)
            {
                TargetGrid.AddTile(TileFactory.CreateTile(RandomTilePrefab, new Vector2Int(x, y)));
            }
        }
    }
}