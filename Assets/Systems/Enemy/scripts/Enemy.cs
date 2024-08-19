using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    Transform model = default;

    EnemyFactory originFactory;

    GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    Direction direction;
    DirectionChange directionChange;
    float directionAngleFrom, directionAngleTo;
    float progress, progressFactor;
    float pathOffset;
    [SerializeField] float speed;
    [SerializeField] private int hp;
    [SerializeField] private int gold;
    [SerializeField] private float lightArmor;
    [SerializeField] private float mediumArmor;
    [SerializeField] private float heavyArmor;
    private bool curse;
    private float cursePower;
    private int tilesToEnd;

    public EnemyFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    public bool GameUpdate()
    {
        progress += Time.deltaTime * progressFactor;
        while (progress >= 1f)
        {
            if (tileTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }
            progress = (progress - 1f) / progressFactor;
            PrepareNextState();
            progress *= progressFactor;
        }
        if (directionChange == DirectionChange.None)
        {
            transform.localPosition =
                Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(
                directionAngleFrom, directionAngleTo, progress
            );
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
        return true;
    }

    public void Initialize(float pathOffset)
    {
        this.pathOffset = pathOffset;
        curse = false;
        cursePower = 0;
    }

    public void SpawnOn(GameTile tile)
    {
        tileFrom = tile;
        tileTo = tile.NextTileOnPath;
        progress = 0f;
        PrepareIntro();
    }

    void PrepareNextState()
    {
        tileFrom = tileTo;
        tileTo = tileTo.NextTileOnPath;
        positionFrom = positionTo;
        if (tileTo == null)
        {
            PrepareOutro();
            return;
        }
        positionTo = tileFrom.ExitPoint;
        directionChange = direction.GetDirectionChangeTo(tileFrom.PathDirection);
        direction = tileFrom.PathDirection;
        directionAngleFrom = directionAngleTo;
        switch (directionChange)
        {
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
            default: PrepareTurnAround(); break;
        }
    }

    void PrepareForward()
    {
        transform.localRotation = direction.GetRotation();
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        progressFactor = speed;
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(pathOffset - 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speed / (Mathf.PI * 0.5f * (0.5f - pathOffset));
    }

    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(pathOffset + 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speed / (Mathf.PI * 0.5f * (0.5f + pathOffset));
    }

    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + (pathOffset < 0f ? 180f : -180f);
        model.localPosition = new Vector3(pathOffset, 0f);
        transform.localPosition = positionFrom;
        progressFactor =
            speed / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffset), 0.2f));
    }

    void PrepareIntro()
    {
        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileFrom.ExitPoint;
        direction = tileFrom.PathDirection;
        directionChange = DirectionChange.None;
        directionAngleFrom = directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speed;
    }

    void PrepareOutro()
    {
        positionTo = tileFrom.transform.localPosition;
        directionChange = DirectionChange.None;
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speed;
    }
    public Enemy(int hp, float speed, float lightArmor, float mediumArmor, float heavyArmor)
    {
        this.hp = hp;
        this.speed = speed;
        this.lightArmor = lightArmor;
        this.mediumArmor = mediumArmor;
        this.heavyArmor = heavyArmor;

    }
    //brakuje usuwania przy 0 hp
    public void OnHit(int dmg, DamageType dmgType)
    {
        dmg = dmg + (int)(dmg * cursePower);
        switch (dmgType)
        {
            case DamageType.Light:
                hp = hp - (int)(dmg * lightArmor);
                break;
            case DamageType.Medium:
                hp = hp - (int)(dmg * mediumArmor);
                break;
            case DamageType.Heavy:
                hp = hp - (int)(dmg * heavyArmor);
                break;
        }
    }
    public bool getCurse()
    {
        return curse;
    }
    //zrob zabezpieczenie ze jak juz ma curse to nowa na nic nie wplywa (prze serii atakow mamy ciekawa sytuacje,
    //wejdz se do mnie na scene, zbuduj wierze i odpal jak chcesz, to sie posmiejesz[chyba ze juz naprawiles blad przy hp])
    public void setCurse(float time, float power)
    {
        curse = true;
        cursePower = power;
        StartCoroutine(WaitAndExecute(time));
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    IEnumerator WaitAndExecute(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Minê³y 4 sekundy, teraz wykonujê akcjê!");
        curse = false;
        cursePower = 0;
        transform.localScale *= 2;
    }
    public float getProgress()
    {
        return tileFrom.getDistance() + progress;
    }

}