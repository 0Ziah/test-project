using UnityEngine;

public class HitTriggerCheck : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        BattleManager.instance.EnemyHitStart(col);
        //print("<color=green>" + col.name + " hit me</color>");
    }

    void OnTriggerStay(Collider col)
    {
        /*if (col.GetComponentInParent<Enemy>() != null)
            print("<color=yellow>" + col.transform.name + " still touching me</color>");*/
    }

    void OnTriggerExit(Collider col)
    {
        BattleManager.instance.EnemyHitStop(col);
        //print("<color=red>" + col.transform.name + " no longer touch me</color>");
    }
}
