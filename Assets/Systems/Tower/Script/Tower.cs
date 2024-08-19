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
    public UnityEvent<Tower> onSupport;
    public UnityEvent<Tower> onUnsupport;

    TargetPoint target;

    public Transform towerVisual;
    public string towerName;
    public int tier;

    public float sizeXZ = 1;
    public float sizeY = 1;

    public int tileId;

    public int damage;
    public DamageType damageType = DamageType.Medium;
    [SerializeField, Range(1.5f, 10.5f)]
    public float range = 1.5f;
    public float attackSpeed = 1;
    private float timeFromlastAttack;
    private bool isShooting = false;

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
        //Czy mo�emy by� supportowani
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
        //DLA TESTU
        attackProjectalsCount = 3;
        currentSerie = attackProjectalsCount;

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
                    int min = ((int) range) + 1; ;
                    float max = 0.0f;

                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;   
                        float progress = target.Enemy.getProgress();
                        int progressInt = (int) progress;
                        if (min > progressInt) 
                            min = progressInt;
                        if (max < progress - progressInt)
                            max = progress - progressInt;                       
                    }
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;
                        float progress = target.Enemy.getProgress();
                        int progressInt = (int)progress;
                        if (min == progressInt && max == progress - progressInt)
                        {
                            Shoot(target);
                            return true;
                        }
                    }
                    break;
                case "Mortal":
                    int maxNumberOfTargets = 0;
                    for (int i = 0; i < targets.Length; i++)
                    {
                        target = targets[i].GetComponent<TargetPoint>();
                        if (target == null) continue;
                        //create small colider around target
                        //count colliders
                        //find maxCount
                    }
                    
                    Shoot(target);
                    break;
            }
            return true;
        }
        target = null;
        return false;
    }
    
    private bool Shoot (TargetPoint target)
    {
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
        //circle -> list -> enemies hitted (for aoe attacks)
        Debug.DrawLine(gameObject.transform.position, target.transform.position, UnityEngine.Color.red, 0.5f);        
        target.Enemy.OnHit(damage, damageType);
        target.Enemy.setCurse(curseDuration, cursePower);       
        timeFromlastAttack = 0;        
        return true;
    }
}
