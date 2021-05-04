using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourcesManagmentScript
{
    public StreamReader ReadDataFromResource(string path)
    {
        StreamReader reader = new StreamReader(path);
        return reader;
    }

    public StreamWriter WriteDataToResource(string path)
    {
        StreamWriter writer = new StreamWriter(path, false);
        return writer;
    }

    public bool GetGameVariable(string varName)
    {
        bool variableValue = false;
        StreamReader reader = new StreamReader("Assets/Resources/game_variables.txt");
        string numberLines = reader.ReadLine();
        int.TryParse(numberLines, out int numLines);
        for (int i = 0; i < numLines; i++)
        {
            string varStr = reader.ReadLine();
            GameVariable variable = GameVariable.CreateFromJSON(varStr);
            if (varName == variable.name)
            {
                variableValue = variable.value;
            }
        }
        reader.Close();
        return variableValue;
    }

    public void SetGameVariable(string varName, bool varValue)
    {
        StreamReader reader = new StreamReader("Assets/Resources/game_variables.txt");
        string numberLines = reader.ReadLine();
        int.TryParse(numberLines, out int numLines);
        GameVariable[] allVars = new GameVariable[numLines];
        for (int i = 0; i < numLines; i++)
        {
            string varStr = reader.ReadLine();
            GameVariable variable = GameVariable.CreateFromJSON(varStr);
            allVars[i] = variable;
        }
        reader.Close();

        StreamWriter writer = new StreamWriter("Assets/Resources/game_variables.txt", false);
        writer.WriteLine(allVars.Length);
        foreach (GameVariable vars in allVars)
        {
            if (vars.name == varName) vars.value = varValue;
            string varsStr = GameVariable.CreateFromObject(vars);
            writer.WriteLine(varsStr);
        }
        writer.Close();
    }
}
