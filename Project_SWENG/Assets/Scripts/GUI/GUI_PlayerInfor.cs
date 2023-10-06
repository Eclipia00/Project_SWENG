using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Character;

public class GUI_PlayerInfor : GUIFullScreen
{
    [SerializeField] private GameObject _target;
    [SerializeField] private TextMeshProUGUI _dicePoint;
    [SerializeField] private GUI_PlayerHealth _playerHealth;

    Unit _targetUnit;
    PlayerController _targetPlayer;

    protected override void Open(Vector2 openPos)
    {
        base.Open(openPos);
    }

    public void SetPlayer(GameObject target) {

        _target = target;

        _targetUnit = target.GetComponent<Unit>();
        _targetPlayer = target.GetComponent<PlayerController>();
        _playerHealth.SetPlayerHealth(target);

    }

    protected override void Update()
    {
        base.Update();
        _dicePoint.text = _targetUnit.dicePoints.ToString();

    }

    public override void HexSelect(Vector3Int selectGridPos)
    {
        Hex selected = HexGrid.Instance.GetTileAt(selectGridPos);

        if (selected && selected.Entity == _target)
            UIManager.OpenGUI<GUI_ActionSelect>("ActionSelect").Set(_target);
    }

    public void TurnEndButton() {
        GameManager.Instance.PlayerTurnEnd();
    }

    public void AttackButton() {
        if (!_targetPlayer.CanAttack()) return;
    }

}  
