using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Objects")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public string nombre;
    public bool stackable;
    public int value;
    public int experienceChange;
    public int healthChange;

    [TextArea(1, 3)]
    public string description;

    [TextArea(1, 3)]
    public string commentary;


    public virtual bool UseItem()
    {
        return true;
    }
}
