using UnityEngine;
using System.Collections.Generic;

public class Queen : ChessPiece
{
    public override List<Tile> GetAvailableMoves(Tile[,] board)
    {
        List<Tile> moves = new List<Tile>();

        // 8 yön (kale + fil)
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // yukarı
            new Vector2Int(-1, 0),  // aşağı
            new Vector2Int(0, 1),   // sağ
            new Vector2Int(0, -1),  // sol
            new Vector2Int(1, 1),   // sağ yukarı
            new Vector2Int(1, -1),  // sol yukarı
            new Vector2Int(-1, 1),  // sağ aşağı
            new Vector2Int(-1, -1)  // sol aşağı
        };

        foreach (var dir in directions)
        {
            int row = currentTile.row;
            int col = currentTile.col;

            while (true)
            {
                row += dir.x;
                col += dir.y;

                // Tahta dışına çıktı mı?
                if (row < 0 || row >= 8 || col < 0 || col >= 8)
                    break;

                Tile nextTile = board[row, col];

                if (!nextTile.IsOccupied())
                {
                    moves.Add(nextTile);
                }
                else
                {
                    if (nextTile.currentPiece.color != this.color)
                    {
                        moves.Add(nextTile);
                    }
                    break;
                }
            }
        }

        Debug.Log($"{name} (Queen) moves: {moves.Count}");
        return moves;
    }
}
