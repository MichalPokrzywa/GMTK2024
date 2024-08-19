using System.Collections;
using System.Collections.Generic;
using Hover;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.X86.Avx;

public class HoverInteraction : Singleton<HoverInteraction>
{
    private Vector2 _origin;
    [SerializeField] private InputAction clickAction;

    private Outline hoveredTile;
    private GameTileContent openedTile = null;
    // Update is called once per frame
    void Update()
    {
        _origin = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(_origin);
        ProcessRaycast(ray);

        if (Input.GetMouseButtonDown(0))
        {
            ProcessInput(ray);
        }
    }

    public void ProcessRaycast(Ray ray)
    {
        GameTile tile = GameBoard.Instance.GetTile(ray);
        if (tile != null)
        {
            HandleOutline(tile.Content);
        }
    }

    private void ProcessInput(Ray ray)
    {
        GameTile tile = GameBoard.Instance.GetTile(ray);
        if (tile != null && tile.Content != openedTile)
        {
            switch (tile.Content)
            {
                case Wall:
                    Debug.Log(tile.name);
                    GameBarUI.Instance.ChoseTower(tile);
                    break;
                case Tower:
                    Debug.Log(tile.name);
                    break;
            }
            //HandleOutline(tile.Content);
            ResetCurrentObject();
            openedTile = tile.Content;
            if (hoveredTile != null)
            {
                hoveredTile.OutlineColor = Color.yellow;
            }
        }
    }

    private void ResetOutline()
    {
        if (hoveredTile != null)
        {
            if (openedTile == null || (openedTile != null && hoveredTile != openedTile.GetComponent<Outline>()))
            {
                hoveredTile.enabled = false;
            }
        }
    }

    private void ResetCurrentObject()
    {
        if (openedTile != null)
        {
            var tmp = openedTile.GetComponent<Outline>();
            tmp.OutlineColor = Color.white;
            tmp.UpdateMaterialProperties();
            tmp.enabled = false;
        }
    }
    private void HandleOutline(GameTileContent tile)
    {
        ResetOutline();
        hoveredTile = tile.GetComponent<Outline>() ?? tile.GetComponentInParent<Outline>();
        if (hoveredTile != null)
        {
            hoveredTile.enabled = true;
        }
    }
}
