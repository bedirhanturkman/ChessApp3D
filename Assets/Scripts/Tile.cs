using UnityEngine;

public class Tile : MonoBehaviour
{
    public int row;
    public int col;
    public ChessPiece currentPiece; // Bu karede hangi taş var

    public void SetPiece(ChessPiece piece)
    {
        currentPiece = piece;
        if (piece != null)
        {
            piece.currentTile = this;
            piece.transform.position = transform.position; // Taşı bu kareye taşı
        }
    }

    public bool IsOccupied()
    {
        return currentPiece != null;
    }
}
