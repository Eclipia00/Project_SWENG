using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item/Creat New Item")]
public class Item : ScriptableObject
{
    [Tooltip("if itemType is helmet : item Code is 100 * helmetTyp + helmetIndex\n" +
             "helmet Type 0 : headCovering Base\n" + 
             "helmet Type 1 : headCovering No FacialHair\n" +
             "helmetType 2 : headCovering No Hair\n" +
             "helmetType 3 : no head\n")]
    public int id;
    public bool isEquip = false;
    
    public string itemName;
    public int value;
    public int cost;
    public Sprite icon;
    public Sprite skillIcon;
    public GameObject itemObject;
    public Hex itemHex;

    public enum ItemTier
    {
        Common,
        UnCommon,
        Rare,
        Unique,
        Legendary,
        Mythic,
    }

    public ItemTier tier;
    
    public enum ItemType
    {
        Helmet,
        Armor,
        Weapon,
        Shield,
        Consumables
    }

    public ItemType type;
    
    public void Pick() {
        UIManager.OpenGUI<GUI_ItemInterAction>("ItemInterAction").SetItem(this);    
    }
}