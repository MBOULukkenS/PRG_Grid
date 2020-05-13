using UnityEngine;

namespace Grid
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        private SpriteRenderer _cachedRenderer;
        
        public Vector2Int GridPosition { get; set; }

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (!_cachedRenderer)
                    _cachedRenderer = GetComponent<SpriteRenderer>();
                return _cachedRenderer;
            }
        }
    }
}