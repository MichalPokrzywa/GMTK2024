using System.Collections;
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
    GameTileContent towerPrefab = default;

    public void Reclaim(GameTileContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject);
    }

    public GameTileContent Get(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Empty: return Get(emptyPrefab);
            case GameTileContentType.Wall: return Get(wallPrefab);
            case GameTileContentType.Destination: return Get(destinationPrefab);
            case GameTileContentType.SpawnPoint: return Get(spawnPointPrefab);
            case GameTileContentType.Tower: return Get(towerPrefab);
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
