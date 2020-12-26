using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryCell : MonoBehaviour, IPointerClickHandler
{
    public int stockQuantity;
    public Item storedItem;
    public Text stockQuantityText;

    public Image imageObject;
    public Image imageInactive;


    private void Awake()
    {
        //imageObject = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (storedItem == null)
        {
            //ChangeCellState();
            imageObject.enabled = false;
            SetStockQuantityLabel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCellState()
    {
        imageInactive.enabled = !imageInactive.enabled;

    }

    public void AddObject(Item item, int quantity)
    {
        storedItem = item;

        imageObject.enabled = true;
        imageObject.sprite = item.sprite;
        stockQuantity += quantity;

        SetStockQuantityLabel();
    }

    public void RemoveInventoryObject()
    {
        Inventory.instance.RemoveObject(storedItem);

        RemoveCellObject();
    }

    public void RemoveCellObject()
    {
        storedItem = null;
        //ChangeCellState();
        imageObject.sprite = null;
        stockQuantity = 0;
        imageObject.enabled = false;

        SetStockQuantityLabel();
    }

    protected void UseObject()
    {
        if (storedItem)
        {
            if (storedItem.UseItem())
            {
                if (!(storedItem is Equipment))
                    ReduceStock(1);
            }
        }
    }

    private void ReduceStock(int quantity)
    {
        stockQuantity -= quantity;
        if (stockQuantity <= 0)
        {
            stockQuantity = 0;
            RemoveInventoryObject();
        }
        else
        {
            SetStockQuantityLabel();
        }
    }

    public void SetStockQuantityLabel()
    {
        if (stockQuantityText != null)
        {
            CanvasGroup panel = stockQuantityText.GetComponentInParent<CanvasGroup>();
            if (storedItem)
            {
                if (storedItem.stackable)
                {
                    panel.alpha = 1;
                    stockQuantityText.text = stockQuantity.ToString();
                }
                else
                {
                    panel.alpha = 0;
                }
            }
            else
            {
                panel.alpha = 0;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UseObject();
    }
}
