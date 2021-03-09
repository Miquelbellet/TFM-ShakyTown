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

    public void WriteDataToResource(string path)
    {
        StreamWriter writer = new StreamWriter(path, false);
    }
}
