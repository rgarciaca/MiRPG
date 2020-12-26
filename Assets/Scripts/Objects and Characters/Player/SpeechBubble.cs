using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpeechBubble : MonoBehaviour, IPointerClickHandler
{
    public Texture2D cursor;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
        GameManager.instance.SetDefaultCursor();
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
