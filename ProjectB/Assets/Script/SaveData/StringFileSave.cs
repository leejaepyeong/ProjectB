using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StringFileSave
{
    public virtual string GetPath(string name)
    {
        return string.Format("{0}/{1}.bytes",Application.persistentDataPath, name);
    }
    public virtual void Save(string path, string data)
    {
        if(data == null)
        {
            Debug.LogError("data is Null :" + path);
            return;
        }

        File.WriteAllText(path, data);
    }

    public virtual string Load(string path)
    {
        if(File.Exists(path) == false)
        {
            Debug.LogError("File No Exists :" + path);
            return null;
        }

        return File.ReadAllText(path);
    }
}
