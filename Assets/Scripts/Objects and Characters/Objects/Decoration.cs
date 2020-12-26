using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Decoration")]
public class Decoration : ScriptableObject
{
    public Sprite sprite;
    public string nombre;
    public int experienceChange;

    [TextArea(1, 3)]
    public string commentary;


    public virtual bool UseItem()
    {
        return true;
    }
}
