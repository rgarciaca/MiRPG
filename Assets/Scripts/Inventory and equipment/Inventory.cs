using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    
    public bool inventoryFull;
    public Text coinsText;
    public int availableCells;


    public static Inventory instance;

    //private InventoryCell[] cells;
    private List<InventoryCell> cells;
    private List<Item> items = new List<Item>();
    private int voidCell = 0;


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
        SetAvailableCells();

        SetCoins();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAvailableCells()
    {
        if (cells == null)
            cells = new List<InventoryCell>();

        int numCells = availableCells;

        var tempCells = GetComponentsInChildren<InventoryCell>();
        for (int i = 0; i < tempCells.Length; i++)
        {
            if (numCells > 0)
            {
                if (tempCells[i].imageInactive.enabled )
                {
                    cells.Add(tempCells[i]);
                    tempCells[i].ChangeCellState();
                }

                numCells--;
            }
            else 
            {
                if (!tempCells[i].imageInactive.enabled)
                {
                    cells.Remove(tempCells[i]);
                    tempCells[i].ChangeCellState();
                }
            }
        }
    }

    private void SetCoins()
    {
        coinsText.text = GameManager.instance.gameData.playerData.coins.ToString(); ;
    }

    public  int GetOccupiedCellsCount()
    {
        int res = 0;

        var tempCells = GetComponentsInChildren<InventoryCell>();
        for (int i = 0; i < tempCells.Length; i++)
        {
            
            if (!tempCells[i].imageInactive.enabled && tempCells[i].storedItem != null)
            {
                res++;               
            }
        }

        return res;
    }


    void NextVoidCell()
    {
        voidCell = 0;

        foreach (InventoryCell cell in cells)
        {
            if (cell.storedItem)
            {
                voidCell++;
            }
            else
            {
                break;
            }
        }

        if (voidCell >= Inventory.instance.availableCells)
        {
            inventoryFull = true;
        }
        else
        {
            inventoryFull = false;
        }
    }


    public bool AddObject(Item item, int quantity)
    {
        NextVoidCell();

        if (!inventoryFull)
        {
            if ((item.stackable && !items.Contains(item)) || (!item.stackable))
            {
                InventoryCell newCell = cells[voidCell];
                items.Add(item);
                newCell.AddObject(item, quantity);
            }
            else
            {
                int index = items.IndexOf(item);
                cells[index].stockQuantity += quantity;
                cells[index].SetStockQuantityLabel();
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public void RemoveObject(Item item)
    {
        items.Remove(item);
    }

    public void RemoveObjectCell(Item item)
    {
        int indice = 0;
        while (!cells[indice].storedItem || !cells[indice].storedItem.Equals(item))
        {
            indice++;
        }

        cells[indice].RemoveInventoryObject();

    }

    public void ReorganizeInventory()
    {
        int currentCell = 0;
        foreach (InventoryCell cell in cells)
        {
            if (!cell.storedItem)
            {
                for (int i = currentCell + 1; i < cells.Count; i++)
                {
                    if (cells[i].storedItem)
                    {
                        cell.AddObject(cells[i].storedItem, cells[i].stockQuantity);
                        cells[i].RemoveCellObject();

                        break;
                    }
                }
            }

            currentCell++;
        }
    }
}
