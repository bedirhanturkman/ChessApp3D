using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    private ChessPiece selectedPiece;
    private List<Tile> validMoves = new List<Tile>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol click
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile clickedTile = hit.collider.GetComponent<Tile>();

            if (clickedTile != null)
            {
                // Eğer taş seçili değilse -> taş seç
                if (selectedPiece == null && clickedTile.IsOccupied())
                {
                    // ✅ Sadece sırası gelen rengi seçebilsin
                    if (clickedTile.currentPiece.color == BoardManager.Instance.currentTurn)
                    {
                        selectedPiece = clickedTile.currentPiece;
                        Debug.Log("Seçilen taş: " + selectedPiece.type + " " + selectedPiece.color);

                  
                        // LegalMoves (şah kontrolünü içeriyor)
                        validMoves = BoardManager.Instance.GetLegalMoves(selectedPiece);
                        Debug.Log("Legal moves found: " + validMoves.Count);

                        HighlightManager.Instance.HighlightTiles(validMoves);
                    }
                    else
                    {
                        Debug.Log("Sıra bu taşın renginde değil!");
                    }
                }
                // Eğer taş seçiliyse -> taşı hamle et
                else if (selectedPiece != null)
                {
                    if (validMoves.Contains(clickedTile))
                    {
                        BoardManager.Instance.MovePiece(selectedPiece, clickedTile);
                    }
                    else
                    {
                        Debug.Log("Geçersiz hamle!");
                    }

                    selectedPiece = null;
                    validMoves.Clear();
                    HighlightManager.Instance.ClearHighlights();
                }
            }
        }
    }
}
