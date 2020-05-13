using Grid;
using UnityEngine;

namespace AStar
{
    [RequireComponent(typeof(Tile))]
    public class AStarTile : MonoBehaviour
    {
        public int TileWeight = 2;
        public bool Walkable = true;
        
        private Tile _cachedTile;

        public Tile Tile
        {
            get
            {
                if (!_cachedTile)
                    _cachedTile = GetComponent<Tile>();
                return _cachedTile;
            }
        }
    }
}