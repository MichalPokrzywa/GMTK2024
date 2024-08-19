using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEnemy : MonoBehaviour
{
    [SerializeField] private Collider healingRange;
    private bool cooldown = false;
    [SerializeField] private float range = 1.5f;
    TargetPoint target;
    [SerializeField] private float healCooldown;
    [SerializeField] private int healPower;

    void Update()
    {
        if (!cooldown)
        {
            StartCoroutine(WaitForHeal(healCooldown));     
            cooldown = true;
        }
    }

    IEnumerator WaitForHeal(float time) {
        yield return new WaitForSeconds(time);
        Healing();
        cooldown = false;
    }

    private void Healing()
    {
        Collider[] targets = Physics.OverlapSphere(
            transform.localPosition, range
        );
        for ( int i = 0;i<targets.Length;i++)
        {
            target = targets[i].GetComponent<TargetPoint>();
            if( target == null ) continue;
            target.Enemy.SetHp(healPower);
        }
    }
}
