using UnityEngine;

namespace Grid
{
    public static class TileFactory
    {
        public static Tile CreateTile(Tile prefab, Vector2Int gridPosition)
        {
            Tile result = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            result.GridPosition = gridPosition;
            
            return result;
        }

        public struct TileCreateInfo
        {
            public Tile Prefab;
            public Vector2Int Position;
            public Sprite Sprite;

            public TileCreateInfo(Tile prefab, Vector2Int position, Sprite sprite)
            {
                Prefab = prefab;
                Position = position;
                Sprite = sprite;
            }
        }
    }
}