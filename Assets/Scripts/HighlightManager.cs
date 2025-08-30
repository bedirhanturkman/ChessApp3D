using UnityEngine;
using System.Collections.Generic;

public class HighlightManager : MonoBehaviour
{
    public static HighlightManager Instance;

    public GameObject highlightPrefab;
    private List<GameObject> highlights = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void HighlightTiles(List<Tile> tilesToHighlight)
    {
        ClearHighlights();

        foreach (Tile tile in tilesToHighlight)
        {
            GameObject hl = Instantiate(highlightPrefab, tile.transform.position + Vector3.up * 0.1f,  Quaternion.Euler(90,0,0));
           
            hl.transform.SetParent(tile.transform);
            highlights.Add(hl);
        }
    }

    public void ClearHighlights()
    {
        foreach (GameObject hl in highlights)
        {
            Destroy(hl);
        }
        highlights.Clear();
    }
}
