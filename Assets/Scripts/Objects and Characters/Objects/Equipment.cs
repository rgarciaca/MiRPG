using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Armor,
    Belt,
    Boots,
    Weapon, 
    Shield, 
    Bag
}

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Equipment")]
public class Equipment : Item
{
    public EquipmentType type;
    public int armorChange;  //defensa
    public int strengthChange; //ataque
    public int inventoryCells;


    public override bool UseItem()
    {
        Inventory.instance.RemoveObjectCell(this);
        Equipment currentEquipment = EquipmentPanel.instance.EquipObject(this);
        if (currentEquipment != null)
        {
            //StatsPanel.instance.RemoveEquipment(currentEquipment);
            EquipmentPanel.instance.RemoveEquipment(currentEquipment);
            Inventory.instance.AddObject(currentEquipment, 1);

        }

        return true;
    }
}
