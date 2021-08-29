using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static int num;
    [Header("Setup")]
    public string monsterName;
    public float hp;
    public int statusEffectTime;
    public TextMesh hpText;
    public Vector2 randomSpeed;
    public Vector2 randomYPos;
    public Transform sprite;
    public Transform hitBox;
    public enum StatusAilment
    {
        Normal,
        Burn,
        Freeze
    }
    public StatusAilment CurrentStatusAilment;

    [Header("Debug")]
    public float speed;

    Transform t; //cache
    int direction;

    void Start()
    {
        //random start direction
        int rand = Random.Range(0, 2); // 0 1
        if (rand == 0)
        {
            rand = -1; //-1 , 1
        }
        direction = rand;
        UpdateFacing();

        //random speed
        speed = Random.Range(randomSpeed.x, randomSpeed.y);

        UpdateHp();
        statusEffectTime = 0;
        CurrentStatusAilment = StatusAilment.Normal;

        //cache Transform into t
        t = this.transform;

        //random start y
        //copy
        Vector3 pos = t.localPosition;
        //modify pos
        pos.y = Random.Range(randomYPos.x, randomYPos.y);
        //paste
        t.localPosition = pos;
    }

    void Update()
    {
        if (BattleManager.instance.CurrentGameState == BattleManager.GameState.Over)
        {
            //early return
            return;
        }
        //copy localPosition to pos
        /*Vector3 pos = t.localPosition;
        //modify pos
        if (pos.x > 2.5f)
        {
            direction = -1;
            UpdateFacing();
        }
        if (pos.x < -2.5f)
        {
            direction = 1;
            UpdateFacing();
        }
        pos.x = pos.x + (direction * speed * Time.deltaTime);
        //paste pos back to localPosition
        t.localPosition = pos;*/
    }

    void UpdateFacing()
    {
        if (sprite != null)
        {
            Vector3 scale = sprite.localScale;
            scale.x = -1 * direction;
            sprite.localScale = scale;
        }
    }

    void UpdateHp()
    {
        if (hpText != null)
        {
            hpText.text = hp + "";
        }
    }

    void Death()
    {
        BattleManager.instance.enemyList.Remove(this);
        Destroy(this.gameObject);
        Enemy.num--;
    }

    public void Damage(float dmg)
    {
        hp = hp - dmg;
        BattleManager.instance.CreateFloatingDamage(dmg.ToString(), this.transform);
        UpdateHp();

        if (hp <= 0)
        {
            Death();
        }
    }

    public void ResetStatusAilments()
    {
        AddStatusAilments(StatusAilment.Normal,0);
    }

    public void AddStatusAilments(StatusAilment status, int time)
    {
        statusEffectTime = time;
        CurrentStatusAilment = status;
    }
}
