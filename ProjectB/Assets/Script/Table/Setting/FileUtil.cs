using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using ICSharpCode.SharpZipLib.Zip;


public class FileUtil
{
    public static T Get<T>(Dictionary<string, string> _data, string _key, T def = default(T))
    {
        _key = _key.ToLower();

        if (false == _data.ContainsKey(_key))
        {
            Debug.LogError("Get<" + typeof(T).ToString() + ">  [not find] : " + _key);
            return def;
        }

        string _temp = _data[_key];
        if (null == _temp)
        {
            Debug.LogError("Get[null == _temp]");
            return def;
        }

        if (_temp.Length <= 0)
        {
            if (typeof(string) == typeof(T))
                return (T)((object)_temp);

            return def;
        }

        try
        {
            if (true == typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), _temp, true);
            }
            else
            {
                if (typeof(int) == typeof(T))
                {
                    return (T)((object)int.Parse(_temp));
                }
                else if (typeof(long) == typeof(T))
                {
                    return (T)((object)long.Parse(_temp));
                }
                else if (typeof(float) == typeof(T))
                {
                    return (T)((object)float.Parse(_temp));
                }
                else if (typeof(bool) == typeof(T))
                {
                    return (T)((object)bool.Parse(_temp));
                }
                else if (typeof(string) == typeof(T))
                {
                    //return (T)((object)_temp);
                    return _temp == "None" ? default(T) : (T)((object)_temp);
                }
                else
                {
                    Debug.LogError("Get<" + typeof(T).ToString() + "> - [" + _key + "] : " + _temp);
                }
            }
        }
        catch
        {
            Debug.LogError("Get< catch : " + typeof(T).ToString() + "> - [" + _key + "] : " + _temp);
        }

        return def;
    }
    public static List<T> GetList<T>(Dictionary<string, string> _data, string _key)
    {
        List<T> value = new List<T>();
        string strLoadData = FileUtil.Get<string>(_data, _key);
        string[] arr_strSplitedData = strLoadData.Split(',');

        if (arr_strSplitedData.Length <= 0)
            return value;

        for (int i = 0; i < arr_strSplitedData.Length; i++)
        {
            if (null == arr_strSplitedData[i])
            {
                Debug.LogError("Get[null == arr_strSplitedData[i]]");
                return value;
            }

            if (arr_strSplitedData[i].Length <= 0)
            {
                if (typeof(string) == typeof(T))
                    value.Add((T)((object)arr_strSplitedData[i]));
                return value;
            }

            try
            {
                if (true == typeof(T).IsEnum)
                {
                    value.Add((T)Enum.Parse(typeof(T), arr_strSplitedData[i], true));
                }
                else
                {
                    if (typeof(int) == typeof(T))
                    {
                        value.Add((T)((object)int.Parse(arr_strSplitedData[i])));
                    }
                    else if (typeof(long) == typeof(T))
                    {
                        value.Add((T)((object)long.Parse(arr_strSplitedData[i])));
                    }
                    else if (typeof(float) == typeof(T))
                    {
                        value.Add((T)((object)float.Parse(arr_strSplitedData[i])));
                    }
                    else if (typeof(bool) == typeof(T))
                    {
                        value.Add((T)((object)bool.Parse(arr_strSplitedData[i])));
                    }
                    else if (typeof(string) == typeof(T))
                    {
                        value.Add((T)((object)arr_strSplitedData[i]));
                    }
                    else
                    {
                        Debug.LogError("Get<" + typeof(T).ToString() + "> - [" + _key + "] : " + arr_strSplitedData[i]);
                    }
                }
            }
            catch
            {
                Debug.LogError("Get< catch : " + typeof(T).ToString() + "> - [" + _key + "] : " + arr_strSplitedData[i]);
            }
        }

        return value;
    }


    #region - file path
    static public string GetResPath(string _filename, string _ext = "bytes")
    {
        return string.Format("{0}/Resources/{1}.{2}", Application.dataPath, _filename, _ext);
    }

    static public string GetFilePath(string _filename, string _ext = "bytes")
    {
        return string.Format("{0}/{1}.{2}", Application.persistentDataPath, _filename, _ext);
    }
    #endregion

    static public bool SaveJson(string _path, string _data)
    {
        try
        {
            string _strTemp = _data;
            FileStream fs = new FileStream(_path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(_strTemp);
            sw.Close();
            fs.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveJson() path : " + _path + " exception : " + e.ToString());
            return false;
        }

        return true;
    }

    static public string LoadJson(string _path)
    {
        if (System.IO.File.Exists(_path) == false)
        {
            Debug.LogError("LoadJson() [file no exists] path : " + _path);
            return null;
        }

        try
        {
            FileStream fs = new FileStream(_path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string _strTemp = sr.ReadLine();
            sr.Close();
            fs.Close();
            return _strTemp;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadClass() path : " + _path + " exception : " + e.ToString());
            return null;
        }
    }


    #region - json file save&load
    static public bool SaveJson<T>(T t, string _path, string _cry = null)
    {
        try
        {
            string _strTemp = JsonUtility.ToJson(t);
            if (null != _cry)
            {
                _strTemp = Util.Encrypt(_strTemp, _cry);
            }
            FileStream fs = new FileStream(_path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(_strTemp);
            sw.Close();
            fs.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveJson() path : " + _path + " exception : " + e.ToString());
            return false;
        }

        return true;
    }

    static public T LoadJson<T>(string _path, string _cry = null)
    {
        if (System.IO.File.Exists(_path) == false)
        {
            Debug.LogError("LoadJson() [file no exists] path : " + _path);
            return default(T);
        }

        try
        {
            FileStream fs = new FileStream(_path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string _strTemp = sr.ReadLine();
            if (null != _cry)
            {
                _strTemp = Util.Decrypt(_strTemp, _cry);
            }
            sr.Close();
            fs.Close();
            return JsonUtility.FromJson<T>(_strTemp);
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadClass() path : " + _path + " exception : " + e.ToString());
            return default(T);
        }
    }
    #endregion

    #region - Serialization & Deserialization
    static public void SaveClass(string _path, object _obj)
    {
        using (FileStream stream = new FileStream(_path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, _obj);
            stream.Close();
        }
    }

    static public object LoadClassRes(string _path)
    {
        TextAsset _asset = Resources.Load<TextAsset>(_path);
        if (null == _asset)
            return null;

        byte[] byteData = _asset.bytes;
        using (MemoryStream stream = new MemoryStream(byteData))
        {
            BinaryFormatter bf = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            return bf.Deserialize(stream);
        }
    }

    static public T LoadClass<T>(string _path)
    {
        if (System.IO.File.Exists(_path) == false)
        {
            Debug.LogError("LoadClass() [file no exists] path : " + _path);
            return default(T);
        }

        using (FileStream stream = new FileStream(_path, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            T t = (T)formatter.Deserialize(stream);
            stream.Close();
            return t;
        }
    }
    #endregion
}
