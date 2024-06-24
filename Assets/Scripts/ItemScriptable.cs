using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class ItemScriptable : ScriptableObject
{
    public Sprite image;
    public ItemType type;
    public bool stackable;
    public int itemID;

    public enum ItemType
    {
        Tool,
        Material
    }
}
