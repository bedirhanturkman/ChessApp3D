using UnityEngine;
using System.Collections.Generic;

public class ChessPiece : MonoBehaviour
{
    public enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King }
    public enum PieceColor { White, Black }

    public PieceType type;
    public PieceColor color;
    
    public Tile currentTile; // Bulunduğu kare

    public virtual List<Tile> GetAvailableMoves(Tile[,] board)
    {
        return new List<Tile>(); // override edilince taş türüne göre hesaplanacak
    }
    
}
