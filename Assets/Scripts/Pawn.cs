using UnityEngine;
using System.Collections.Generic;

public class Pawn : ChessPiece
{
    public override List<Tile> GetAvailableMoves(Tile[,] board)
    {
        List<Tile> moves = new List<Tile>();

        int dir = (color == PieceColor.White) ? 1 : -1; // beyaz yukarı, siyah aşağı
        int startRow = (color == PieceColor.White) ? 1 : 6;

        int newRow = currentTile.row + dir;

        // 1 kare ileri boşsa
        if (newRow >= 0 && newRow < 8 && !board[newRow, currentTile.col].IsOccupied())
        {
            moves.Add(board[newRow, currentTile.col]);

            // ilk hamlede 2 kare
            if (currentTile.row == startRow)
            {
                int twoStepRow = currentTile.row + dir * 2;
                if (!board[twoStepRow, currentTile.col].IsOccupied())
                {
                    moves.Add(board[twoStepRow, currentTile.col]);
                }
            }
        }

        // Çapraz yeme
        int[] colOffsets = { -1, 1 };
        foreach (int offset in colOffsets)
        {
            int newCol = currentTile.col + offset;
            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                Tile diagonal = board[newRow, newCol];
                if (diagonal.IsOccupied() && diagonal.currentPiece.color != this.color)
                {
                    moves.Add(diagonal);
                }
            }
        }

        // 🔹 En Passant
        if (BoardManager.Instance.lastDoubleStepPawn != null)
        {
            Pawn targetPawn = BoardManager.Instance.lastDoubleStepPawn;

            // hedef piyon aynı satırda ve yanındaysa
            if (targetPawn.currentTile.row == currentTile.row &&
                Mathf.Abs(targetPawn.currentTile.col - currentTile.col) == 1)
            {
                int enPassantRow = currentTile.row + dir;
                Tile enPassantTile = board[enPassantRow, targetPawn.currentTile.col];
                moves.Add(enPassantTile);
            }
        }

        Debug.Log($"{name} - currentTile: {currentTile?.row},{currentTile?.col}");
        Debug.Log("Pawn moves: " + moves.Count);

        return moves;
    }
}
