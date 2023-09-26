using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ClassFileSave
{
    public delegate string ResPathAction(string _path);
    ResPathAction m_resPathAction;

    public virtual bool Save(string _path, object _data)
    {
        if (null == _data)
        {
            Debug.LogError("null == object : " + _path);
            return false;
        }

        FileStream stream = new FileStream(_path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();

        try
        {
            formatter.Serialize(stream, _data);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Save Fail:" + _path + " ex: " + ex);
            return false;
        }
        finally
        {
            stream.Close();
        }
    }

    public void SetResPath(ResPathAction _resPathAction)
    {
        m_resPathAction = _resPathAction;
    }

    public string GetResPath(string _path)
    {
        if (m_resPathAction == null)
        {
            Debug.LogError("error : " + _path);
            return _path;
        }
        return m_resPathAction(_path);
    }

    public virtual object LoadRes(string _path)
    {
        TextAsset _asset = Resources.Load<TextAsset>(_path);
        if (null == _asset)
        {
            Debug.LogError("Load failed : " + _path);
            return null;
        }

        byte[] byteData = _asset.bytes;
        MemoryStream stream = new MemoryStream(byteData);
        try
        {
            stream.Seek(0, SeekOrigin.Begin);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }
        catch (Exception ex)
        {
            Debug.LogError("Load FAIL: " + _path + " Error: " + ex);
            return null;
        }
        finally
        {
            stream.Close();
        }
    }
}
