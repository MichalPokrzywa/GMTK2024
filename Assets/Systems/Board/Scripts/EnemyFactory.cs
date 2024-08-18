using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField] List<Enemy> prefab = default;

    [SerializeField, FloatRangeSlider(-0.4f, 0.4f)]
    FloatRange pathOffset = new FloatRange(0f);


    public Enemy Get(Enemy ene)
    {
        Enemy instance = CreateGameObjectInstance(prefab.Find(o=>o.Equals(ene)));
        instance.OriginFactory = this;
        //tu do zmiany ta prêdkoœæ tak ¿eby nie by³a losowa tylko odpowiednia dla danego przeciwnika
        instance.Initialize(pathOffset.RandomValueInRange);
        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(enemy.gameObject);
    }
}
