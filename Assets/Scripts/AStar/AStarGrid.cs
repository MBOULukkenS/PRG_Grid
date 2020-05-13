using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEngine;

namespace AStar
{
    [RequireComponent(typeof(TileGrid))]
    public class AStarGrid : MonoBehaviour
    {
        private TileGrid _cachedTileGrid;

        public TileGrid TileGrid
        {
            get
            {
                if (!_cachedTileGrid)
                    _cachedTileGrid = GetComponent<TileGrid>();
                return _cachedTileGrid;
            }
        }
        
        public AStarTile[] this[int x, int y] => this[new Vector2Int(x, y)];

        public AStarTile[] this[Vector2Int pos]
        {
            get
            {
                return TileGrid[pos]
                       .Select(t => t.GetComponent<AStarTile>())
                       .ToArray(); 
            }
        }

        public Tile[] GetShortestPath(AStarTile from, AStarTile to)
        {
            List<AStarNode> openNodes = new List<AStarNode>
            {
                new AStarNode(from, null, 0, Vector2Int.Distance(from.Tile.GridPosition, to.Tile.GridPosition))
            };

            while (openNodes.Count > 0)
            {
                openNodes = openNodes.OrderBy(n => n.FScore)
                                     .ToList();
                
                AStarNode currentNode = openNodes.First();
                
                if (currentNode.AStarTile == to)
                    return TraceBackPath(currentNode).ToArray();
                
                IEnumerable<AStarTile> tileNeighbors = TileGrid.GetNeighboringTiles(currentNode.AStarTile.Tile)
                                                               .Select(t => t.GetComponent<AStarTile>())
                                                               .Where(t => t.Walkable);
                
                openNodes.Remove(currentNode);
                foreach (AStarTile neighbor in tileNeighbors)
                {
                    AStarNode neighborNode = openNodes.FirstOrDefault(n => n.AStarTile == neighbor);

                    float tentativeGScore = currentNode.GScore 
                                            + Vector2Int.Distance(currentNode.AStarTile.Tile.GridPosition, neighbor.Tile.GridPosition)
                                            + neighbor.TileWeight;

                    if (neighborNode == null || tentativeGScore < neighborNode.GScore)
                    {
                        if (neighborNode == null)
                            neighborNode = new AStarNode(neighbor);

                        neighborNode.Previous = currentNode;
                        neighborNode.GScore = tentativeGScore;
                        
                        //FScore = Cost + Heuristic
                        neighborNode.FScore = neighborNode.GScore + Vector2Int.Distance(to.Tile.GridPosition, neighbor.Tile.GridPosition);
                        
                        openNodes.Add(neighborNode);
                    }
                }
            }
            
            return null;
        }

        private IEnumerable<Tile> TraceBackPath(AStarNode end)
        {
            List<Tile> result = new List<Tile>();
            AStarNode current = end;

            while (current != null)
            {
                result.Add(current.AStarTile.Tile);
                current = current.Previous;
            } 

            result.Reverse();
            return result;
        }

        public sealed class AStarNode
        {
            public float GScore { get; set; }
            public float FScore { get; set; }
            public AStarTile AStarTile { get; set; }
            public AStarNode Previous { get; set; }

            public AStarNode(AStarTile tile, AStarNode previous = null, float gScore = float.PositiveInfinity, float fScore = float.PositiveInfinity)
            {
                AStarTile = tile;
                Previous = previous;
                GScore = gScore;
                FScore = fScore;
            }
        }
    }
}