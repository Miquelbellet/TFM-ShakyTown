using System;
using UnityEngine;

[Serializable]
public class Tool
{
    public int toolbarIndex;
    public bool empty;
    public string name;
    public bool isCountable;
    public int countItems;
    public string spriteName;
    public bool isWeapon;
    public bool isBow;

    public Tool()
    {
        toolbarIndex = 0;
        empty = true;
        name = "";
        isCountable = false;
        countItems = 0;
        spriteName = "";
        isWeapon = false;
        isBow = false;
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
