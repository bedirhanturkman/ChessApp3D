using UnityEngine;
using System.Collections.Generic;

public class King : ChessPiece
{
    public override List<Tile> GetAvailableMoves(Tile[,] board)
    {
        List<Tile> moves = new List<Tile>();

        // Şahın 8 yönü
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // yukarı
            new Vector2Int(-1, 0),  // aşağı
            new Vector2Int(0, 1),   // sağ
            new Vector2Int(0, -1),  // sol
            new Vector2Int(1, 1),   // sağ üst çapraz
            new Vector2Int(1, -1),  // sol üst çapraz
            new Vector2Int(-1, 1),  // sağ alt çapraz
            new Vector2Int(-1, -1)  // sol alt çapraz
        };

        foreach (var dir in directions)
        {
            int newRow = currentTile.row + dir.x;
            int newCol = currentTile.col + dir.y;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                Tile target = board[newRow, newCol];

                // boş veya rakip taşı varsa gidebilir
                if (!target.IsOccupied() || target.currentPiece.color != this.color)
                {
                    moves.Add(target);
                }
            }
        }

        Debug.Log($"{name} (King) moves: {moves.Count}");
        return moves;
    }
}
