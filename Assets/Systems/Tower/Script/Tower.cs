using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;
using UnityEngine.Rendering;
using UnityEditor;
using System.Drawing;
using UnityEditor.UIElements;

[Serializable]
public enum DamageType
{
    Light,
    Medium,
    Heavy
}

public enum TypeTower
{
    Bowman =0,
    Mage = 1,
    Mortar =2,
}

[Serializable]
public struct BasicStats
{
    public int damage;
    public float range;
    public float attackSpeed;
    public BonusStats bonusStats;
}


[Serializable]
public struct BonusStats
{
    public int attackProjectalsCount;
    public float attackSize;
    public float curseDuration;
    public float cursePower;
}


public class Tower : GameTileContent
{
    public TypeTower typeTower = TypeTower.Bowman;
    public UnityEvent<Tower> onSupport;
    public UnityEvent<Tower> onUnsupport;
    
    TargetPoint target;

    public Transform towerVisual;
    public string towerName;
    public int tier;

    public float sizeXZ = 1;
    public float sizeY = 1;

    public int tileId;

    //Basic Stats
    public int damage;
    [SerializeField, Range(1.5f, 10.5f)]
    public float range = 1.5f;
    public float attackSpeed = 1;
    public BasicStats levelUpStats;

    public DamageType damageType = DamageType.Medium;
    private float timeFromlastAttack;
    private bool isShooting = false;

   

    //Specyfic Stats
    public int attackProjectalsCount = 1;
    public float attackSize = 0;
    public float curseDuration = 0;
    public float cursePower = 0;
    public BonusStats supportStats;


    public List<Tower> supporters = new List<Tower>();
    public Tower supportedTower;

    private int currentSerie;
    private TargetPoint currentTarget;

    public void Support(Tower towerToSuport)
    {
        if (!CanSupport()) return;
        
        if(!towerToSuport.AddSupporter(this)) return;

        onSupport.Invoke(towerToSuport);
        supportedTower = towerToSuport;
        UpdateStats();
    }

    public void UnSupport()
    {
        if (supportedTower != null)
        {
            onUnsupport.Invoke(supportedTower);
            supportedTower.RemoveSupporter(this);
            supportedTower = null;
            UpdateStats();
        }
    }

    public bool AddSupporter(Tower tower)
    {
        //Czy mo¿emy byæ supportowani
        if (!CanBeSupported()) return false;

        AddBonusStats(tower.supportStats);
        supporters.Add(tower);
        UpdateStats();

        return true;

    }

    public void RemoveSupporter(Tower tower)
    {
        RemoveBonusStats(tower.supportStats);
        supporters.Remove(tower);
        UpdateStats();
    }

    private bool CanSupport()
    {
        if (supporters.Count == 0 && supportedTower == null)
            return true;
        return false;
    }

    private bool CanBeSupported()
    {
        if (supporters.Count == 1 + ((tier - 1) * 2) || supportedTower != null)
            return false;
        return true;
    }

    private void UpdateStats()
    {
        if(supportedTower == null)
        {
            int supportersCount = supporters.Count;
            sizeXZ = 1 + (0.06f * supportersCount);
            sizeY = 1 + (0.12f * supportersCount);

            if(supportersCount>0) damageType = DamageType.Heavy;
            else damageType = DamageType.Medium;
        }
        else
        {
            damageType = DamageType.Light;
            sizeXZ = 0.7f;
            sizeY = 0.7f;
        }

        towerVisual.transform.localScale = new Vector3(sizeXZ, sizeY, sizeXZ);

    }

    public void AddStats(BasicStats stats)
    {
        damage += stats.damage;
        range += stats.range;
        attackSpeed += stats.attackSpeed;
        attackProjectalsCount += stats.bonusStats.attackProjectalsCount;
        attackSize += stats.bonusStats.attackSize;
        curseDuration += stats.bonusStats.curseDuration;
        cursePower += stats.bonusStats.cursePower;
    }
    public void AddBonusStats(BonusStats stats)
    {
        attackProjectalsCount += stats.attackProjectalsCount;
        attackSize += stats.attackSize;
        curseDuration += stats.curseDuration;
        cursePower += stats.cursePower;
    }

    public void RemoveBonusStats(BonusStats stats)
    {
        attackProjectalsCount -= stats.attackProjectalsCount;
        attackSize -= stats.attackSize;
        curseDuration -= stats.curseDuration;
        cursePower -= stats.cursePower;
    }


    public override void GameUpdate()
    {
        if (!isShooting)
        {
            if (timeFromlastAttack >= attackSpeed)
                AcquireTarget();
            else
                timeFromlastAttack += Time.deltaTime;
        } 
        else
        {
            if (timeFromlastAttack >= 0.2f)
                Shoot(currentTarget);
            else
                timeFromlastAttack += Time.deltaTime;
        }
    }

    public bool TierUp()
    {
        if (tier < 3)
        {
            tier++;
            AddStats(levelUpStats);
            return true;
        }
        return false;
    }

    public int Sell()
    {
        UnSupport();
        foreach(var i in supporters)
        {
            i.UnSupport();
        }
        //usun wierzyczke, przywroc tile
        return 100;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, range);
    }

    bool AcquireTarget()
    {
        Collider[] targets = Physics.OverlapSphere(
            transform.localPosition, range
        );
        currentSerie = attackProjectalsCount;
        attackSize = 2;
        damage = 100;
        if (targets.Length > 0)
        {
            switch (towerName)
            {
                case "Mage":
                    //try to fing uncursed target
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;
                        if (!target.Enemy.getCurse())
                        {
                            Shoot(target);
                            return true;
                        }
                    }
                    //attack random cursed target
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;
                        Shoot(target);
                        return true;
                    }
                    break;
                case "Bowman":
                    //find minimum (min przed przecinkiem, max po)

                    int min = 100; ;
                    float max = 0.0f;

                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;   
                        float progress = target.Enemy.getProgress();
                        int progressInt = (int) progress;
                        if (min > progressInt)
                        {
                            min = progressInt;
                            max = progress - progressInt;
                            currentTarget = target;
                        } else if (min == progressInt)
                        {
                            if (max < progress - progressInt)
                            {
                                max = progress - progressInt;
                                currentTarget = target;
                            }
                        }
                    }
                    Shoot(currentTarget);
                    break;
                case "Mortal":
                    int maxNumberOfTargets = 1;
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;
                        Collider[] mortalTargets = Physics.OverlapSphere(
                            target.transform.localPosition, attackSize
                        );
                        int targetNum = 1;
                        for (int j = 0; j < mortalTargets.Length; j++)
                        {
                            TargetPoint anotherTarget = mortalTargets[j].GetComponent<TargetPoint>();
                            if (anotherTarget != null) targetNum++;
                        }
                        if (maxNumberOfTargets < targetNum)
                        {
                            maxNumberOfTargets = targetNum;
                            currentTarget = target;
                        }
                    }                    
                    Shoot(currentTarget);
                    break;
            }
            return true;
        }
        target = null;
        return false;
    }
    
    private bool Shoot (TargetPoint target)
    { 
        //ta funkcja wysyla liste targetow do korutyny, ustawia attacs speed
        currentTarget = target;
        isShooting = true;
        if (target == null)
        {
            isShooting = false;
            return false;
        }
        currentSerie--;
        if (currentSerie <= 0)
            isShooting = false;
        Collider[] otherTargets = Physics.OverlapSphere(
            currentTarget.transform.localPosition, attackSize
        );
        List<TargetPoint> targets = new List<TargetPoint>();
        for (int i = 0; i < otherTargets.Length; i++)
        {
            TargetPoint anotherTarget = otherTargets[i].GetComponent<TargetPoint>();
            if (anotherTarget != null) targets.Add(anotherTarget);
        }
        timeFromlastAttack = 0;
        IEnumerator coroutine = CreateProjectile(targets, currentTarget);
        StartCoroutine(coroutine);
        return true;
    }

    private IEnumerator CreateProjectile(List<TargetPoint> targets, TargetPoint mainTarget)
    {
        //tworzenie pocisku, nadanie predkosci, usuniecie po trafieniu
        yield return new WaitForSeconds(0.5f);
        if (mainTarget != null)
        {
            Debug.DrawLine(gameObject.transform.position, mainTarget.transform.position, UnityEngine.Color.red, 0.5f);
        }
        if (targets.Count == 0 && mainTarget != null)
        {
            mainTarget.Enemy.OnHit(damage, damageType);
            mainTarget.Enemy.setCurse(curseDuration, cursePower);
        } else
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null) continue; 
                targets[i].Enemy.OnHit(damage, damageType);
                targets[i].Enemy.setCurse(curseDuration, cursePower);
            }
        }
        print("Strzelilem po 0.5 sek");
    }
}
