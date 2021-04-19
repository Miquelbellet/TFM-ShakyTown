using System;
using UnityEngine;

[Serializable]
public class Tool
{
    public int toolbarIndex;
    public bool empty;
    public string name;
    public string spriteName;
    public bool isCountable;
    public int countItems;
    public bool isWeapon;
    public bool isBow;
    public bool isArrow;
    public bool isPotion;
    public int potionLevel;
    public int damage;
    public int goldValue;

    public Tool()
    {
        toolbarIndex = 0;
        empty = true;
        name = "";
        spriteName = "";
        isCountable = false;
        countItems = 0;
        isWeapon = false;
        isBow = false;
        isArrow = false;
        isPotion = false;
        potionLevel = 0;
        damage = 0;
        goldValue = 1;
    }

    public static Tool CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Tool>(jsonString);
    }

    public static string CreateFromObject(Tool toolObject)
    {
        return JsonUtility.ToJson(toolObject);
    }
}
