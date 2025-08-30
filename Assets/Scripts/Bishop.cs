using UnityEngine;
using System.Collections.Generic;

public class Bishop : ChessPiece
{
    public override List<Tile> GetAvailableMoves(Tile[,] board)
    {
        List<Tile> moves = new List<Tile>();

        // 4 çapraz yön
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 1),    // sağ yukarı
            new Vector2Int(1, -1),   // sol yukarı
            new Vector2Int(-1, 1),   // sağ aşağı
            new Vector2Int(-1, -1)   // sol aşağı
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
                    moves.Add(nextTile); // boş kare
                }
                else
                {
                    // Rakip taşı varsa ekle ve dur
                    if (nextTile.currentPiece.color != this.color)
                    {
                        moves.Add(nextTile);
                    }
                    break; // engel var, devam edemez
                }
            }
        }

        Debug.Log($"{name} (Bishop) moves: {moves.Count}");
        return moves;
    }
}
