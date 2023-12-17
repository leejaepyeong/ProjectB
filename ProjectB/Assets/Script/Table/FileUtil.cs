using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Util
{
    #region ��ȣȭ
    public static string Encrypt(string p_data, string _key)
    {
        byte[] Skey = System.Text.ASCIIEncoding.ASCII.GetBytes(_key);

        // ��ȣȭ �˰����� RC2 ��ȣȭ�� �Ϸ��� RC��
        // DES�˰����� ����Ϸ��� DESCryptoServiceProvider ��ü�� �����Ѵ�.
        //RC2 rc2 = new RC2CryptoServiceProvider();
        System.Security.Cryptography.DESCryptoServiceProvider rc2 = new System.Security.Cryptography.DESCryptoServiceProvider();

        // ��ĪŰ ��ġ
        rc2.Key = Skey;
        rc2.IV = Skey;

        // ��ȣȭ�� ��Ʈ��(����Ʈ �迭)��
        // ��ĪŰ�� �����Ͽ� ��ȣȭ �ϱ⶧���� ���� �޸� ��Ʈ���� �����Ѵ�.
        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //������� �޸� ��Ʈ���� �̿��ؼ� ��ȣȭ ��Ʈ�� ����
        System.Security.Cryptography.CryptoStream cryStream = new System.Security.Cryptography.CryptoStream(ms, rc2.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

        // �����͸� ����Ʈ �迭�� ����
        byte[] data = System.Text.Encoding.UTF8.GetBytes(p_data.ToCharArray());

        // ��ȣȭ ��Ʈ���� ������ ��
        cryStream.Write(data, 0, data.Length);
        cryStream.FlushFinalBlock();

        // ��ȣȭ �Ϸ� (string���� �������ؼ� ��ȯ)
        return System.Convert.ToBase64String(ms.ToArray());
    }
    public static string Decrypt(string p_data, string _key)
    {
        byte[] Skey = System.Text.ASCIIEncoding.ASCII.GetBytes(_key);
        // ��ȣȭ �˰����� RC2 ��ȣȭ�� �Ϸ��� RC��
        // DES�˰����� ����Ϸ��� DESCryptoServiceProvider ��ü�� �����Ѵ�.
        //RC2 rc2 = new RC2CryptoServiceProvider();
        System.Security.Cryptography.DESCryptoServiceProvider rc2 = new System.Security.Cryptography.DESCryptoServiceProvider();

        // ��ĪŰ ��ġ
        rc2.Key = Skey;
        rc2.IV = Skey;

        // ��ȣȭ�� ��Ʈ��(����Ʈ �迭)��
        // ��ĪŰ�� �����Ͽ� ��ȣȭ �ϱ⶧���� ���� �޸� ��Ʈ���� �����Ѵ�.
        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //������� �޸� ��Ʈ���� �̿��ؼ� ��ȣȭ ��Ʈ�� ����
        System.Security.Cryptography.CryptoStream cryStream =
                          new System.Security.Cryptography.CryptoStream(ms, rc2.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

        //�����͸� ����Ʈ�迭�� �����Ѵ�.
        byte[] data = System.Convert.FromBase64String(p_data);

        //����� ����Ʈ�迭�� ��ȣȭ �Ѵ�.
        cryStream.Write(data, 0, data.Length);
        cryStream.FlushFinalBlock();

        //��ȣȭ �� �����͸� ��Ʈ������ ��ȯ�ؼ� ����
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
