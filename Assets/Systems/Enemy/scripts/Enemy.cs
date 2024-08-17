using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BgTools.Dialogs.TextValidator;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int hp;
    private float speed;
    private float lightArmor;
    private float mediumArmor;
    private float heavyArmor;
    private bool curse;
    private float cursePower;
    private int tilesToEnd;
    GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    float progress;
    public enum ArmorType
    {
        Light,
        Medium,
        Heavy,
    }

    // Start is called before the first frame update
    void Start()
    {
        curse = false;
        cursePower = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool GameUpdate()
    {
        progress += Time.deltaTime * speed;
        while (progress >= 1f)
        {
            tileFrom = tileTo;
            tileTo = tileTo.NextTileOnPath;
            if (tileTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }
            positionFrom = positionTo;
            positionTo = tileFrom.ExitPoint; ;
            progress -= 1f;
        }
        transform.localPosition =
            Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        return true;
    }
    public void SpawnOn(GameTile tile)
    {
        //transform.localPosition = tile.transform.localPosition;
        Debug.Assert(tile.NextTileOnPath != null, "Nowhere to go!", this);
        tileFrom = tile;
        tileTo = tile.NextTileOnPath;
        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileFrom.ExitPoint;
        progress = 0f;
    }

    public void OnHit(int dmg, ArmorType dmgType)
    {
        dmg = dmg + (int)(dmg * cursePower);
        switch (dmgType)
        {
            case ArmorType.Light:
                hp = hp - (int)(dmg - dmg * lightArmor);
                break;
            case ArmorType.Medium:
                hp = hp - (int)(dmg - dmg * mediumArmor);
                break;
            case ArmorType.Heavy:
                hp = hp - (int)(dmg - dmg * heavyArmor);
                break;
        }
    }
    public bool getCurse()
    {
        return curse;
    }

    public void setCurse(float time, float power)
    {
        curse = true;
        cursePower = power;
        StartCoroutine(WaitAndExecute(time));
    }
    IEnumerator WaitAndExecute(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Minê³y 4 sekundy, teraz wykonujê akcjê!");
        curse = false;
        cursePower = 0;
    }

    public Enemy(int hp, float speed, float lightArmor, float mediumArmor, float heavyArmor)
    {
        this.hp = hp;
        this.speed = speed;
        this.lightArmor = lightArmor;
        this.mediumArmor = mediumArmor;
        this.heavyArmor = heavyArmor;
    }
}