using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int boardSize = new Vector2Int(11, 11);
    [SerializeField] private GameBoard board = default;
    [SerializeField] private GameTileContentFactory tileContentFactory = default;
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    void Awake()
    {
        board.Initialize(boardSize, tileContentFactory);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
    }

    void HandleTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            board.ToggleDestination(tile);
        }
    }
    void OnValidate()
    {
        if (boardSize.x < 2)
        {
            boardSize.x = 2;
        }
        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }
}
