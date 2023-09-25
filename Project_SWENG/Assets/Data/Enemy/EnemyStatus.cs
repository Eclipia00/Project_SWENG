using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatus", menuName = "ScriptableObjects/EnemyBaseStatsData", order = 2)]

public class EnemyStatus : ScriptableObject
{
    public string monsterName = "Normal";
    public int atk = 10;
    public int def = 10;
    public int maxHp = 60;
    public float percent = 70;
    
    public int speed = 50;
    
    public int Lv = 1;
    public int Exp = 10;
    public Sprite UIImage; 
    public List<Item> dropItem;
}
