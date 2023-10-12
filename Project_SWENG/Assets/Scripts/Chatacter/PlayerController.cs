using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class PlayerController : NetworkCharacterController {

        public int maxHealth;

        [SerializeField] int atkPoint = 3;

        public static event EventHandler<IntEventArgs> EventRecover;
        public static event EventHandler<IntEventArgs> EventDamaged;

        private NetworkUnit unit;

        private void Awake()
        {
            stat.curHP = maxHealth;
            unit = GetComponent<NetworkUnit>();
            _PhotonView = GetComponent<PhotonView>();
        }

        private void OnEnable()
        {
            Hex curHex = HexGrid.Instance.GetHexFromPosition(this.gameObject.transform.position);
            curHex.Entity = this.gameObject;
            HexGrid.Instance.RemoveAtEmeptyHexTiles(curHex);
        }

        public int Recover(int val)
        {
            if (stat.curHP <= 0) return 0;

            stat.curHP += val;

            if (stat.curHP > maxHealth)
            {
                stat.curHP = maxHealth;
            }

            EventRecover?.Invoke(this, new IntEventArgs(stat.curHP));
            return stat.curHP;
        }

        public bool CanAttack()
        {
            return unit.dicePoints >= atkPoint;
        }

        public override void AttackAct()
        {
            unit.dicePoints -= atkPoint;
            _PhotonView.RPC("AttackVfx", RpcTarget.All, null);
        }

        public override int GetAttackValue()
        {
            return stat.attackPower +
                (InventoryManager.Instance.Weapon ? InventoryManager.Instance.Weapon.value : 0);
        }

        [PunRPC]
        public void AttackVfx()
        {
            EffectManager.Instance.SetTarget(gameObject);
            if (InventoryManager.Instance.Weapon)
            {
                int weaponID = InventoryManager.Instance.Weapon.id;
                EffectManager.Instance.ShowImpactVfxHandler(weaponID);
            }
        }

        public override void DamageAct()
        {
            base.DamageAct();
            EventDamaged?.Invoke(this, new IntEventArgs(stat.curHP));
        }

        public override void DieAct()
        {
            base.DieAct();
        }
    }
}