using System;
using UnityEngine;

[Serializable]
public class Note
{
    public int index;
    public string name;
    public string text;
    public bool shown;

    public Note()
    {
        index = 0;
        name = "";
        text = "";
        shown = false;
    }

    public static Note CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Note>(jsonString);
    }

    public static string CreateFromObject(Note noteObject)
    {
        return JsonUtility.ToJson(noteObject);
    }
}
