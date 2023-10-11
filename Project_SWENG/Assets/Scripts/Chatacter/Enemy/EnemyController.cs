using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class EnemyController : ControllerOfCharacter {

        [SerializeField] GUI_EnemyHealth healthGUI;
        [SerializeField] LayerMask playerLayerMask;
        public EnemyStat enemyStat;

        public Hex curHex;

        private void OnEnable()
        {
            if (enemyStat == null)
                enemyStat = GetComponent<EnemyStat>();

            if (healthGUI == null)
                healthGUI = GetComponentInChildren<GUI_EnemyHealth>();

            stat.curHP = enemyStat.maxHp;
            curHex = HexGrid.Instance.GetHexFromPosition(this.gameObject.transform.position);
            curHex.Entity = this.gameObject;
            EnemySpawner.Instance.enemyList.Add(gameObject);
        }

        public override int GetAttackValue()
        {
            Debug.Log(enemyStat.atk);
            return enemyStat.atk;
        }

        protected override void DamageAct()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, playerLayerMask);
            if (colliders.Length > 0)
                gameObject.transform.LookAt(colliders[0].transform);

            healthGUI.UpdateGUI((float)stat.curHP / enemyStat.maxHp);
        }

        protected override void DieAct()
        {
            curHex = HexGrid.Instance.GetHexFromPosition(this.gameObject.transform.position);
            curHex.Entity = null;
            EnemySpawner.Instance.enemyList.Remove(this.gameObject);

            healthGUI.UpdateGUI(0);
            DropItem();
            Destroy(this.gameObject, 1f);
        }

        private void DropItem()
        {
            if (enemyStat.dropItem.Count == 0) return;
            Item dropitem = enemyStat.dropItem[Random.Range(0, enemyStat.dropItem.Count)];
            curHex.Item = dropitem;
            dropitem.itemHex = this.curHex;
        }
    }
}
