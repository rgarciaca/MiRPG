using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    public static EquipmentPanel instance;

    private EquipmentCell[] equipmentCells;
    public List<Equipment> items = new List<Equipment>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        equipmentCells = GetComponentsInChildren<EquipmentCell>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Equipment EquipObject(Equipment equipment)
    {
        Equipment result = null;

        foreach (EquipmentCell cell in equipmentCells)
        {
            if (equipment.type == cell.cellType)
            {
                if (cell.storedItem)
                {
                    result = (Equipment)cell.storedItem;
                }

                AddEquipment(equipment, cell);
            }
        }

        return result;
    }

    private void AddEquipment(Equipment equipment, EquipmentCell cell)
    {
        cell.AddObject(equipment);
        items.Add(equipment);
        StatsPanel.instance.UpdateEquipmentValues(equipment);

        if (equipment.type == EquipmentType.Bag)
        {
            Inventory.instance.availableCells += equipment.inventoryCells;
            Inventory.instance.SetAvailableCells();
        }
    }

    public void RemoveEquipment(Equipment equipment)
    {

        items.Remove(equipment);
        StatsPanel.instance.RemoveEquipment(equipment);

        if (equipment.type == EquipmentType.Bag)
        {
            Inventory.instance.availableCells -= equipment.inventoryCells;
            Inventory.instance.SetAvailableCells();
        }

    }
}
