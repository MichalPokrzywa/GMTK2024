using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileContent : MonoBehaviour
{
    [SerializeField] GameTileContentType type = default;
    public bool BlocksPath => Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;
    private GameTileContentFactory originFactory;
    public GameTileContentType Type => type;
    public GameTileContentFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }
    public virtual void GameUpdate() { }
    public void Recycle()
    {
        originFactory.Reclaim(this);
    }
}
public enum GameTileContentType
{
    Empty=0, Wall=1, Destination=2, SpawnPoint=3, Tower
}