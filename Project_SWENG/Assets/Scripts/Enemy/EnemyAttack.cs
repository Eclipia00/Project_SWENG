using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;

    [SerializeField] int attackPower = 5;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        attackPower = enemyController.enemyStat.atk;
    }

    public void EnemyAttackHandler()
    {
        foreach(var neighbours in HexGrid.Instance.GetNeighboursDoubleFor(enemyController.curHex.HexCoords))
        {
            Hex curHex = HexGrid.Instance.GetTileAt(neighbours);
            GameObject entity = curHex.Entity;
            if (entity != null && entity.CompareTag("Player"))
            {
                this.gameObject.transform.LookAt(entity.transform);
                enemyController.ani.SetTrigger("Attack");
                curHex.DamageToEntity(attackPower);
            }
        }
    }
}
