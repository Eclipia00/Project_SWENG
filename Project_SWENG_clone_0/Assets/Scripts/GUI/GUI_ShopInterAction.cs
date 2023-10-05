using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_ShopInterAction : GUIPopUp
{
    public int itemCounts;
    [SerializeField] Transform itemSlot;
    [SerializeField] GameObject itemInfo;
    protected override void Open(Vector2 openPos)
    {
        base.Open(new Vector2(-1000,0));

        DisplayItem();
    }

    private void DisplayItem()
    {
        foreach (Item item in ShopManager.Instance.GetRandomItemList(itemCounts))
        {
            SetItem(item);
        }

    }

    public void ExitShop()
    {
        Destroy(gameObject);
    }

    private void SetItem(Item item)
    {
        GameObject itemInstance = Instantiate(itemInfo, itemSlot);
        itemInstance.GetComponent<ShopItemController>().SetItem(item);
    }
}