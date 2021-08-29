using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    void Awake()
    {
        instance = this;
    }

    public TextMesh TextHP;
    public TextMesh SkillHP;

    public void UpdateHPText(string hp, string hpMax)
    {
        TextHP.text = "HP:" + hp + "/" + hpMax;
    }

    public void UpdateSkillText(string skillNum)
    {
        if (skillNum == "0")
            skillNum = "-";
        SkillHP.text = "Skill:" + skillNum;
    }
}
