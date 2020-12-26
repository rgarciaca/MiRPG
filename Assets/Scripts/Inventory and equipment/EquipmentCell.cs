using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentCell : MonoBehaviour, IPointerClickHandler
{
    public EquipmentType cellType;
    public Item storedItem;
    public int stockQuantity = 1;

    public Image imageObject;

    // Start is called before the first frame update
    void Start()
    {
        if (storedItem == null)
        {
            imageObject.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddObject(Item item)
    {
        storedItem = item;

        imageObject.enabled = true;
        imageObject.sprite = item.sprite;
    }

    public void RemoveObject()
    {
        Inventory.instance.RemoveObject(storedItem);

        storedItem = null;
        imageObject.sprite = null;
        imageObject.enabled = false;
    }

    protected  void UseObject()
    {        
        RemoveEquipment();
    }

    private void RemoveEquipment()
    {
        if (storedItem)
        {
            bool canRemove = true;

            if (((Equipment)storedItem).type == EquipmentType.Bag)
            {
                if (((Equipment)storedItem).inventoryCells >= (Inventory.instance.availableCells - Inventory.instance.GetOccupiedCellsCount()))
                {
                    canRemove = false;
                }
                else
                {
                    Inventory.instance.ReorganizeInventory();
                }
            }

            if (canRemove)
            {

                if (Inventory.instance.AddObject(storedItem, 1))
                {
                    EquipmentPanel.instance.RemoveEquipment((Equipment)storedItem);

                    //StatsPanel.instance.RemoveEquipment((Equipment)storedItem);

                    RemoveObject();
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UseObject();
    }
}
