using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Interactive : MonoBehaviour, IPointerClickHandler
{   
    public Texture2D cursor;
    public UnityEvent OnInteraccion;

    protected BoxCollider2D colliderInt;

    private void Start()
    {
        colliderInt = GetComponent<BoxCollider2D>();
        //playerController = GameManager.instance.jugador.GetComponent<PlayerController>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    OnInteraccion?.Invoke();
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log("Haciendo click" + eventData);
    //    Interactuar();
    //}

    private void Interacting()
    {
        RaycastHit2D[] items = GameManager.instance.player.GetComponentInParent<PlayerController>().InteractingItems();
        foreach (RaycastHit2D item in items)
        {
            if (item.collider.gameObject == gameObject)
            {
                Interaction();
                break;
            }
        }
    }

    public virtual void Interaction()
    {
        //Debug.Log("Interactuando con " + gameObject.name);
        OnInteraccion?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interacting();    
        //Interaction();
    }

    public virtual void OnMouseEnter()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnMouseExit()
    {
        GameManager.instance.SetDefaultCursor();
    }
}
