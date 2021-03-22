using System;
using UnityEngine;

[Serializable]
public class Note
{
    public int index;
    public string name;
    public string text;

    public Note()
    {
        index = 0;
        name = "";
        text = "";
    }

    public static Note CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Note>(jsonString);
    }

    public static string CreateFromObject(Note toolObject)
    {
        return JsonUtility.ToJson(toolObject);
    }
}