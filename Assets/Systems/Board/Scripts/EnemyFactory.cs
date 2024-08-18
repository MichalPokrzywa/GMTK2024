using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField] Enemy prefab = default;

    [SerializeField, FloatRangeSlider(-0.4f, 0.4f)]
    FloatRange pathOffset = new FloatRange(0f);

    [SerializeField, FloatRangeSlider(0.2f, 5f)]
    FloatRange speed = new FloatRange(1f);

    public Enemy Get()
    {
        Enemy instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        //tu do zmiany ta prêdkoœæ tak ¿eby nie by³a losowa tylko odpowiednia dla danego przeciwnika
        instance.Initialize(speed.RandomValueInRange, pathOffset.RandomValueInRange);
        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(enemy.gameObject);
    }
}
