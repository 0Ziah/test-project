using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //singleton
    public static BattleManager instance;
    void Awake()
    {
        instance = this;
    }

    [Header("Setup")]
    public float myDamage;
    public float burnDamage;
    public int burnDuration;
    public float LavaDamage;
    public GameObject playerPrefab;
    Player player;
    Transform playerTransfrom;
    public Transform enemyPrefab;
    public Transform enemyParent;
    public Transform floatingDamagePrefab;
    public Transform floatingDamagePlayerPrefab;
    public List<Enemy> enemyList;
    public float aoeRadius;
    public int skillSelect;
    public List<int> skillList;
    public enum GameState
    {
        Ready,
        Start,
        Over
    }
    public GameState CurrentGameState;

    [Header("Debug")]
    public Transform debugAoe;

    // Use this for initialization
    void Start()
    {
        enemyList = new List<Enemy>();
        player = playerPrefab.GetComponent<Player>();
        playerTransfrom = playerPrefab.transform;

        //init
        player.SetHPMax(100);
        player.SetHP(100);
        isStillHit = false;
        skillSelect = 0;
        skillList = new List<int>{1,2,3};

        CurrentGameState = GameState.Ready;
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < skillList.Count; i++)
        {
            if (Input.GetKeyDown(skillList[i].ToString()))
                SkillSelect(skillList[i]);
        }

        if (Enemy.num < 3 && (CurrentGameState != GameState.Over))
        {
            Transform clone = Instantiate(enemyPrefab);
            clone.SetParent(enemyParent);
            clone.localPosition = new Vector3(Random.Range(-10, 11), clone.localPosition.y, Random.Range(-10, 11));
            clone.localEulerAngles = new Vector3(0, Random.Range(0, 361), 0);
            clone.localScale = Vector3.one;
            Enemy.num++;

            Enemy cloneEnemy = clone.GetComponent<Enemy>();
            enemyList.Add(cloneEnemy);
        }
    }

    public void GameOver()
    {
        CurrentGameState = GameState.Over;
    }

    public void SkillSelect(int skillNum)
    {
        if (skillSelect == skillNum)
            skillSelect = 0;
        else
            skillSelect = skillNum;

        UIManager.instance.UpdateSkillText(skillNum.ToString());
    }

    bool isStillHit;
    public void EnemyHitStart(Collider col)
    {
        Enemy enemy = col.GetComponentInParent<Enemy>();
        if (isStillHit)
            return;
        else if (enemy == null)
            return;
        else
        {
            DealDamage(enemy);
            //print("Hit");
        }
    }

    public void DealDamage(Enemy enemy)
    {
        enemy.Damage(myDamage);
        isStillHit = true;
        if (skillSelect > 0)
            SkillEffect(enemy);
        }

    public void SkillEffect(Enemy enemy)
    {
        switch (skillSelect)
        {
            case 1:
                //Burn
                enemy.AddStatusAilments(Enemy.StatusAilment.Burn, burnDuration);
                StartCoroutine(enemyBurn(enemy));
                break;
            case 2:
                //Freeze
                break;
            case 3:
                // AoE
                //set debug sphere
                /*debugAoe.position = playerPrefab.GetComponent<Transform>().position;
                float targetScale = aoeRadius * 2;
                debugAoe.localScale = new Vector3(targetScale, targetScale, targetScale);

                if (false) //Hit
                {
                    //array use .Length , List use .Count
                    for (int i = enemyList.Count - 1; i >= 0; i--)
                    {
                        float distance = Vector3.Distance(debugAoe.position, enemyList[i].hitBox.position);
                        if (distance <= aoeRadius)
                        {
                            enemyList[i].Damage(myDamage);
                        }
                    }
                }*/
                break;
        }
    }

    IEnumerator enemyBurn(Enemy enemy)
    {
        if (enemy.statusEffectTime <= 0)
        {
            enemy.ResetStatusAilments();
            StopCoroutine(enemyBurn(enemy));
        }

        while (enemy.statusEffectTime > 0)
        {
            yield return new WaitForSeconds(1f);
            enemy.Damage(burnDamage);
            enemy.statusEffectTime--;
        }
    }


    public void EnemyHitStop(Collider col)
    {
        if (col.GetComponentInParent<Enemy>() == null)
            isStillHit = false;
    }

    bool isStillOnLava = false;
    public void OnLavaStart(Collider col)
    {
        if (isStillOnLava)
            return;
        else if (col.name.Contains("Player"))
        {
            isStillOnLava = true;
            player.SetHP(player.GetHP() - LavaDamage);
            CreateFloatingDamage(LavaDamage.ToString(), playerTransfrom, true);
            DelayLavaBurn(1f);
        }
    }

    public void OnLavaStop(Collider col)
    {
        if (col.name.Contains("Player"))
        {
            isStillOnLava = false;
        }
    }

    void DelayLavaBurn(float delayTime)
    {
        if (isStillOnLava)
            StartCoroutine(_DelayLavaBurn(delayTime));
    }

    private IEnumerator _DelayLavaBurn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isStillOnLava = false;
    }

    public void CreateFloatingDamage(string msg, Transform parent)
    {
        CreateFloatingDamage(msg, parent, false);
    }

    public void CreateFloatingDamage(string msg, Transform parent, bool isPlayer)
    {
        Transform clone;
        if (isPlayer)
            clone = Instantiate(floatingDamagePlayerPrefab);
        else
            clone = Instantiate(floatingDamagePrefab);
        //Always SetParent before reset transform
        clone.SetParent(parent);
        //Reset Transform , position (0,0,0) , rotation (0,0,0) , scale (1,1,1)
        clone.localPosition = Vector3.zero;
        clone.SetParent(UIManager.instance.gameObject.transform);
        clone.localScale = Vector3.one;
        clone.localEulerAngles = Vector3.zero;

        //set text

        clone.Find("Text").GetComponent<TextMesh>().text = msg;
    }
}
