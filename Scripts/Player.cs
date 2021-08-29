using UnityEngine;

public class Player : MonoBehaviour
{
    int Hp;
    int HpMax;
    RPGCharacterAnimsFREE.RPGCharacterController characterController;

    private void Start()
    {
        characterController = gameObject.GetComponent<RPGCharacterAnimsFREE.RPGCharacterController>();
    }

    public int GetHP() { return Hp; }
    public void SetHP(float newHp)
    {
        SetHP(Mathf.FloorToInt(newHp));
    }
    public void SetHP(int newHp) {
        if (newHp <= 0)
        {
            newHp = 0;
            BattleManager.instance.GameOver();
        }
        else if (newHp > HpMax)
            newHp = HpMax;
        else
            Hp = newHp;
        UpdateHPui();
    }

    public int GetHPMax() { return HpMax; }
    public void SetHPMax(float newHpMax)
    {
        SetHPMax(Mathf.FloorToInt(newHpMax));
    }
    public void SetHPMax(int newHpMax)
    {
        if (newHpMax < 0)
            newHpMax = 1;
        else
            HpMax = newHpMax;
        UpdateHPui();
    }

    private void UpdateHPui()
    {
        UIManager.instance.UpdateHPText(Hp.ToString(), HpMax.ToString());
    }
}
