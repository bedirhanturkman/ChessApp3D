using UnityEngine;
using System.Collections.Generic;

public class Knight : ChessPiece
{
    public override List<Tile> GetAvailableMoves(Tile[,] board)
    {
        List<Tile> moves = new List<Tile>();

        // Atın 8 olası hareketi
        Vector2Int[] knightMoves = new Vector2Int[]
        {
            new Vector2Int(2, 1),
            new Vector2Int(2, -1),
            new Vector2Int(-2, 1),
            new Vector2Int(-2, -1),
            new Vector2Int(1, 2),
            new Vector2Int(1, -2),
            new Vector2Int(-1, 2),
            new Vector2Int(-1, -2)
        };

        foreach (var move in knightMoves)
        {
            int newRow = currentTile.row + move.x;
            int newCol = currentTile.col + move.y;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                Tile target = board[newRow, newCol];

                if (!target.IsOccupied() || target.currentPiece.color != this.color)
                {
                    moves.Add(target);
                }
            }
        }

        Debug.Log($"{name} (Knight) moves: {moves.Count}");
        return moves;
    }
}
