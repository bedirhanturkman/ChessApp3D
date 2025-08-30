using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    public Tile[,] tiles = new Tile[8, 8];
    public Transform tilesParent;

    public ChessPiece.PieceColor currentTurn = ChessPiece.PieceColor.White;

    [Header("UI")]
    public TextMeshProUGUI turnText;
    public GameObject gameOverPanel;  // Inspector’dan bağla (UI panel)
    public TextMeshProUGUI gameOverText;
    public CinemachineCamera whiteCam;
    public CinemachineCamera blackCam;

    [Header("Prefabs")]
    public GameObject whiteQueenPrefab;
    public GameObject blackQueenPrefab;

    [HideInInspector] public Pawn lastDoubleStepPawn = null;

    private bool gameEnded = false; // ✅ Oyun bitince hareketi engelle
    

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadTiles();
        UpdateTurnUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false); // başta kapalı
    }

    void LoadTiles()
    {
        foreach (Transform child in tilesParent)
        {
            Tile tile = child.GetComponent<Tile>();
            if (tile != null)
            {
                tiles[tile.row, tile.col] = tile;
            }
        }
        Debug.Log("Tüm tile'lar yüklendi!");
    }

    public void MovePiece(ChessPiece piece, Tile targetTile)
    {
        if (gameEnded) return; // ✅ Oyun bittiyse hareket etme

        Tile oldTile = piece.currentTile;

        // 🔹 En Passant özel kontrolü
        if (piece is Pawn pawn)
        {
            if (lastDoubleStepPawn != null)
            {
                if (targetTile.col == lastDoubleStepPawn.currentTile.col &&
                    targetTile.row == (pawn.currentTile.row + ((pawn.color == ChessPiece.PieceColor.White) ? 1 : -1)))
                {
                    Destroy(lastDoubleStepPawn.gameObject);
                    lastDoubleStepPawn.currentTile.currentPiece = null;
                    lastDoubleStepPawn = null;
                }
            }
        }

        // Normal yeme
        if (targetTile.IsOccupied() && targetTile.currentPiece.color != piece.color)
        {
            Destroy(targetTile.currentPiece.gameObject);
        }

        if (piece.currentTile != null)
            piece.currentTile.currentPiece = null;

        targetTile.SetPiece(piece);

        if (piece is Pawn movedPawn)
        {
            int dir = (movedPawn.color == ChessPiece.PieceColor.White) ? 1 : -1;
            int startRow = (movedPawn.color == ChessPiece.PieceColor.White) ? 1 : 6;

            if (Mathf.Abs(targetTile.row - oldTile.row) == 2 && oldTile.row == startRow)
            {
                lastDoubleStepPawn = movedPawn;
            }
            else
            {
                lastDoubleStepPawn = null;
            }

            if ((movedPawn.color == ChessPiece.PieceColor.White && targetTile.row == 7) ||
                (movedPawn.color == ChessPiece.PieceColor.Black && targetTile.row == 0))
            {
                PromotePawn(movedPawn, targetTile);
            }
        }
        else
        {
            lastDoubleStepPawn = null;
        }

        currentTurn = (currentTurn == ChessPiece.PieceColor.White) ? ChessPiece.PieceColor.Black : ChessPiece.PieceColor.White;
        if (currentTurn == ChessPiece.PieceColor.Black)
        {
            whiteCam.Priority = 0;
            blackCam.Priority = 10;
         }

        if (currentTurn == ChessPiece.PieceColor.White)
        {
            whiteCam.Priority = 10;
            blackCam.Priority = 0;
         }
        Debug.Log("Sıra geçti: " + currentTurn);

        UpdateTurnUI();

        CheckGameOver();
    }

    private void UpdateTurnUI()
    {
        if (turnText != null && !gameEnded)
        {
            turnText.text = (currentTurn == ChessPiece.PieceColor.White) ? "White's Turn" : "Black's Turn";
        }
    }

    private void PromotePawn(Pawn pawn, Tile tile)
    {
        GameObject prefab = (pawn.color == ChessPiece.PieceColor.White) ? whiteQueenPrefab : blackQueenPrefab;

        if (prefab == null)
        {
            Debug.LogError("Queen prefab atanmadı!");
            return;
        }

        GameObject newQueenGO = Instantiate(prefab, tile.transform.position, Quaternion.identity);
        Queen queen = newQueenGO.GetComponent<Queen>();
        if (queen == null)
        {
            Debug.LogError("Queen prefabında Queen script'i yok!");
            Destroy(newQueenGO);
            return;
        }

        queen.color = pawn.color;
        tile.SetPiece(queen);
        Destroy(pawn.gameObject);

        Debug.Log("Pawn terfi etti: " + queen.color + " Queen");
    }

    public bool IsInCheck(ChessPiece.PieceColor color)
    {
        King king = null;
        foreach (Tile tile in tiles)
        {
            if (tile != null && tile.currentPiece is King k && k.color == color)
            {
                king = k;
                break;
            }
        }

        if (king == null) return false;

        ChessPiece.PieceColor opponent = (color == ChessPiece.PieceColor.White) ?
                                         ChessPiece.PieceColor.Black :
                                         ChessPiece.PieceColor.White;

        foreach (Tile tile in tiles)
        {
            if (tile != null && tile.IsOccupied() && tile.currentPiece.color == opponent)
            {
                var moves = tile.currentPiece.GetAvailableMoves(tiles);
                foreach (Tile move in moves)
                {
                    if (move == king.currentTile)
                        return true;
                }
            }
        }

        return false;
    }

    public bool WouldBeInCheck(ChessPiece piece, Tile targetTile)
    {
        Tile oldTile = piece.currentTile;
        ChessPiece captured = targetTile.currentPiece;

        oldTile.currentPiece = null;
        piece.currentTile = targetTile;
        targetTile.currentPiece = piece;

        bool inCheck = IsInCheck(piece.color);

        piece.currentTile = oldTile;
        oldTile.currentPiece = piece;
        targetTile.currentPiece = captured;

        return inCheck;
    }

    public List<Tile> GetLegalMoves(ChessPiece piece)
    {
        List<Tile> legalMoves = new List<Tile>();
        var candidateMoves = piece.GetAvailableMoves(tiles);

        foreach (Tile move in candidateMoves)
        {
            if (!WouldBeInCheck(piece, move))
                legalMoves.Add(move);
        }

        return legalMoves;
    }

    public void CheckGameOver()
    {
        if (gameEnded) return;

        ChessPiece.PieceColor current = currentTurn;
        bool inCheck = IsInCheck(current);

        bool hasLegalMove = false;
        foreach (Tile tile in tiles)
        {
            if (tile != null && tile.IsOccupied() && tile.currentPiece.color == current)
            {
                var legalMoves = GetLegalMoves(tile.currentPiece);
                if (legalMoves.Count > 0)
                {
                    hasLegalMove = true;
                    break;
                }
            }
        }

        if (inCheck && !hasLegalMove)
        {
            Debug.Log(current + " is checkmated! Game Over!");
            EndGame((current == ChessPiece.PieceColor.White ? "Black Wins!" : "White Wins!"));
            return;
        }

        if (!inCheck && !hasLegalMove)
        {
            Debug.Log("Stalemate! Draw!");
            EndGame("Draw!");
            return;
        }

        if (inCheck)
        {
            Debug.Log(current + " is in CHECK!");
            if (turnText != null) turnText.text = current + " is in CHECK!";
        }
    }

    private void EndGame(string message)
    {
        gameEnded = true;
        if (turnText != null) turnText.text = message;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (gameOverText != null)
                gameOverText.text = message;
        }
    }
}
