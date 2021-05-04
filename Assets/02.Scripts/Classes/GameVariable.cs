using System;
using UnityEngine;

[Serializable]
public class GameVariable
{
    public string name;
    public bool value;

    public GameVariable()
    {
        name = "";
        value = false;
    }

    public static GameVariable CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameVariable>(jsonString);
    }

    public static string CreateFromObject(GameVariable jsonObject)
    {
        return JsonUtility.ToJson(jsonObject);
    }
}
