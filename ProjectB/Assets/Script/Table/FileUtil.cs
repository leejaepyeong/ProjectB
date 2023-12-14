using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Util
{
    #region 암호화
    public static string Encrypt(string p_data, string _key)
    {
        byte[] Skey = System.Text.ASCIIEncoding.ASCII.GetBytes(_key);

        // 암호화 알고리즘중 RC2 암호화를 하려면 RC를
        // DES알고리즘을 사용하려면 DESCryptoServiceProvider 객체를 선언한다.
        //RC2 rc2 = new RC2CryptoServiceProvider();
        System.Security.Cryptography.DESCryptoServiceProvider rc2 = new System.Security.Cryptography.DESCryptoServiceProvider();

        // 대칭키 배치
        rc2.Key = Skey;
        rc2.IV = Skey;

        // 암호화는 스트림(바이트 배열)을
        // 대칭키에 의존하여 암호화 하기때문에 먼저 메모리 스트림을 생성한다.
        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //만들어진 메모리 스트림을 이용해서 암호화 스트림 생성
        System.Security.Cryptography.CryptoStream cryStream = new System.Security.Cryptography.CryptoStream(ms, rc2.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

        // 데이터를 바이트 배열로 변경
        byte[] data = System.Text.Encoding.UTF8.GetBytes(p_data.ToCharArray());

        // 암호화 스트림에 데이터 씀
        cryStream.Write(data, 0, data.Length);
        cryStream.FlushFinalBlock();

        // 암호화 완료 (string으로 컨버팅해서 반환)
        return System.Convert.ToBase64String(ms.ToArray());
    }
    public static string Decrypt(string p_data, string _key)
    {
        byte[] Skey = System.Text.ASCIIEncoding.ASCII.GetBytes(_key);
        // 암호화 알고리즘중 RC2 암호화를 하려면 RC를
        // DES알고리즘을 사용하려면 DESCryptoServiceProvider 객체를 선언한다.
        //RC2 rc2 = new RC2CryptoServiceProvider();
        System.Security.Cryptography.DESCryptoServiceProvider rc2 = new System.Security.Cryptography.DESCryptoServiceProvider();

        // 대칭키 배치
        rc2.Key = Skey;
        rc2.IV = Skey;

        // 암호화는 스트림(바이트 배열)을
        // 대칭키에 의존하여 암호화 하기때문에 먼저 메모리 스트림을 생성한다.
        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //만들어진 메모리 스트림을 이용해서 암호화 스트림 생성
        System.Security.Cryptography.CryptoStream cryStream =
                          new System.Security.Cryptography.CryptoStream(ms, rc2.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

        //데이터를 바이트배열로 변경한다.
        byte[] data = System.Convert.FromBase64String(p_data);

        //변경된 바이트배열을 암호화 한다.
        cryStream.Write(data, 0, data.Length);
        cryStream.FlushFinalBlock();

        //암호화 한 데이터를 스트링으로 변환해서 리턴
        return System.Text.Encoding.UTF8.GetString(ms.GetBuffer());
    }
    #endregion
}
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
}

public class EditorUtil
{
    static public string GetResPath_digimon(string _filename)
    {
        return string.Format("{0}/{1}.bytes", Application.dataPath, _filename);
    }
}
