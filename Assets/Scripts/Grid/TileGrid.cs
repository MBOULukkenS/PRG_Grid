using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Grid
{
    public sealed class TileGrid : MonoBehaviour, IEnumerable<Tile>
    {
        [SerializeField, HideInInspector]
        private int _height;
        [SerializeField, HideInInspector]
        private int _width;
        
        private readonly ObservableCollection<Tile> _tiles = new ObservableCollection<Tile>();
        
        public Vector2Int GridSize
        {
            get => new Vector2Int(_width, _height);
            set
            {
                Width = value.x;
                Height = value.y;
            }
        }

        public Tile[] this[int x, int y] => this[new Vector2Int(x, y)];

        public Tile[] this[Vector2Int pos]
        {
            get
            {
                return _tiles
                       .Where(t => t.GridPosition == pos)
                       .ToArray(); 
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value <= 0)
                    return;
                
                _width = value;
                UpdateGrid();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value <= 0)
                    return;
                
                _height = value;
                UpdateGrid();
            }
        }
        
        public IEnumerator<Tile> GetEnumerator()
        {
            return _tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Tile> GetNeighboringTiles(Tile tile)
        {
            return new[]
            {
                this[tile.GridPosition.x - 1, tile.GridPosition.y].FirstOrDefault(),
                this[tile.GridPosition.x + 1, tile.GridPosition.y].FirstOrDefault(),
                this[tile.GridPosition.x, tile.GridPosition.y - 1].FirstOrDefault(),
                this[tile.GridPosition.x, tile.GridPosition.y + 1].FirstOrDefault(),
                this[tile.GridPosition.x - 1, tile.GridPosition.y - 1].FirstOrDefault(),
                this[tile.GridPosition.x + 1, tile.GridPosition.y + 1].FirstOrDefault(),
                this[tile.GridPosition.x + 1, tile.GridPosition.y - 1].FirstOrDefault(),
                this[tile.GridPosition.x - 1, tile.GridPosition.y + 1].FirstOrDefault()
            }.Where(t => t != default);
        }

        public void AddTile(Tile tile)
        {
            if (!IsPositionValid(tile.GridPosition))
                throw new IndexOutOfRangeException("Tile GridPosition is out of range!");

            tile.transform.parent = transform;
            _tiles.Add(tile);
        }

        public void ReplaceTile(Tile oldTile, Tile newTile, bool destroy = true)
        {
            newTile.GridPosition = oldTile.GridPosition;
            
            RemoveTile(oldTile, destroy);
            AddTile(newTile);
        }

        public void RemoveAllAt(Vector2Int position, bool destroy = true)
        {
            if (!IsPositionValid(position))
                return;
            
            foreach (Tile gridTile in _tiles.Where(t => t.GridPosition == position)) 
                RemoveTile(gridTile, destroy);
        }
        
        public void RemoveTile(Tile tile, bool destroy = true)
        {
            if (!IsPositionValid(tile.GridPosition))
                return;
            
            _tiles.Remove(tile);
            
            if (destroy)
                Destroy(tile);
            else
                tile.transform.parent = null;
        }

        // Start is called before the first frame update
        void Awake()
        {
            UpdateGrid();
        }

        private void UpdateTilePosition(Tile tile)
        {
            tile.transform.localPosition = new Vector3(tile.SpriteRenderer.size.x * tile.GridPosition.x * tile.transform.localScale.x, 
                                                       -(tile.SpriteRenderer.size.y * tile.GridPosition.y * tile.transform.localScale.y));
        }

        private bool IsPositionValid(Vector2Int position)
        {
            return (position.x < _width && position.x >= 0)
                   && (position.y < _height && position.y >= 0);
        }

        internal void UpdateGrid()
        {
            foreach (Tile tile in _tiles) 
                UpdateTilePosition(tile);
        }
    }
}
