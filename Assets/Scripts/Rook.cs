using UnityEngine;
using System.Collections.Generic;

public class Rook : ChessPiece
{
    public override List<Tile> GetAvailableMoves(Tile[,] board)
    {
        List<Tile> moves = new List<Tile>();

        // 4 yön: yukarı, aşağı, sağ, sol
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // yukarı (row artar)
            new Vector2Int(-1, 0),  // aşağı
            new Vector2Int(0, 1),   // sağ
            new Vector2Int(0, -1)   // sol
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
                    moves.Add(nextTile); // boş kare, ekle
                }
                else
                {
                    // Rakip taşı varsa ekle ve dur
                    if (nextTile.currentPiece.color != this.color)
                    {
                        moves.Add(nextTile);
                    }
                    break; // kendi taşı da olabilir, dur
                }
            }
        }

        Debug.Log($"{name} (Rook) moves: {moves.Count}");
        return moves;
    }
}
