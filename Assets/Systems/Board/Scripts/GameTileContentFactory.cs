using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField]
    GameTileContent destinationPrefab = default;

    [SerializeField]
    GameTileContent emptyPrefab = default;

    [SerializeField]
    GameTileContent wallPrefab = default;

    [SerializeField]
    GameTileContent spawnPointPrefab = default;

    [SerializeField]
    List<GameTileContent> towerPrefab = default;

    public void Reclaim(GameTileContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject);
    }

    public GameTileContent Get(GameTileContentType type, int value = 0)
    {
        switch (type)
        {
            case GameTileContentType.Empty: return Get(emptyPrefab);
            case GameTileContentType.Wall: return Get(wallPrefab);
            case GameTileContentType.Destination: return Get(destinationPrefab);
            case GameTileContentType.SpawnPoint: return Get(spawnPointPrefab);
            case GameTileContentType.Tower: return Get(towerPrefab[value]);
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }
    private GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }

}
