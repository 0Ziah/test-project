using UnityEngine;

public class LavaTriggerCheck : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        BattleManager.instance.OnLavaStart(col);
        //print("<color=green>" + col.name + " hit me</color>");
    }


    void OnTriggerStay(Collider col)
    {
        BattleManager.instance.OnLavaStart(col);
    }

    void OnTriggerExit(Collider col)
    {
        BattleManager.instance.OnLavaStop(col);
    }
}
