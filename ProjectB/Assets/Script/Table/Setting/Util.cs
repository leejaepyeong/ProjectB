using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Util
{
    #region - gold text
    static System.Text.StringBuilder s_sb = new System.Text.StringBuilder();
    static string[] s_strs = new string[] {
        "", "K", "M", "G", "T", "P", "E", "Z", "Y" };


    public static string GetGoldText_T2(long _iGold)
    {
        s_sb.Length = 0;
        double tmpDb = (double)_iGold;
        int count = 0;
        while (tmpDb >= 1000)
        {
            tmpDb *= 0.001;
            count++;
        }
        if (count >= s_strs.Length)
            count = s_strs.Length - 1;

        if (count == 0)
        {
            s_sb.AppendFormat("{0}", _iGold);
        }
        else
        {
            int lastInt = (int)tmpDb;
            double lastPreDouble = (tmpDb - lastInt) * 1000;
            int lastPreInt = Mathf.RoundToInt((float)lastPreDouble);
            if (lastPreInt < 100)
            {
                s_sb.AppendFormat("{0}{1}", lastInt, s_strs[count]);
            }
            else
            {
                int lastPreInt100 = (int)(lastPreInt * 0.01);
                s_sb.AppendFormat("{0}.{1}{2}", lastInt, lastPreInt100, s_strs[count]);
            }
        }

        return s_sb.ToString();
    }

    public static string GetGoldText_T1(long _iGold)
    {
        return _iGold.ToString("#,#0", CultureInfo.InvariantCulture);
    }

    public static string GetCurrencyText(int _iCurrency, string locale = "")
    {
        if (string.IsNullOrEmpty(locale))
            return _iCurrency.ToString("C");
        return _iCurrency.ToString("C", new CultureInfo(locale));
    }

    #endregion

    #region -spline
    static public Vector3 GetBezier3(Vector3 p1, Vector3 p2, Vector3 p3, float mu)
    {
        float mum1, mum12, mu2;
        Vector3 p;
        mu2 = mu * mu;
        mum1 = 1f - mu;
        mum12 = mum1 * mum1;
        p.x = p1.x * mum12 + 2 * p2.x * mum1 * mu + p3.x * mu2;
        p.y = p1.y * mum12 + 2 * p2.y * mum1 * mu + p3.y * mu2;
        p.z = p1.z * mum12 + 2 * p2.z * mum1 * mu + p3.z * mu2;
        return p;
    }

    static public Vector3 GetBezier4(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float mu)
    {
        float mum1, mum13, mu3;
        Vector3 p;
        mum1 = 1 - mu;
        mum13 = mum1 * mum1 * mum1;
        mu3 = mu * mu * mu;
        p.x = mum13 * p1.x + 3 * mu * mum1 * mum1 * p2.x + 3 * mu * mu * mum1 * p3.x + mu3 * p4.x;
        p.y = mum13 * p1.y + 3 * mu * mum1 * mum1 * p2.y + 3 * mu * mu * mum1 * p3.y + mu3 * p4.y;
        p.z = mum13 * p1.z + 3 * mu * mum1 * mum1 * p2.z + 3 * mu * mu * mum1 * p3.z + mu3 * p4.z;
        return (p);
    }

    // 0f <= t <= 1f ( t = 0f -> p1, t = 1f ->p2 )
    static public Vector3 GetCatmullRomSpline(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 ret = Vector3.zero;

        float t2 = t * t;
        float t3 = t2 * t;

        ret.x = 0.5f * ((2.0f * p1.x) +
        (-p0.x + p2.x) * t +
        (2.0f * p0.x - 5.0f * p1.x + 4 * p2.x - p3.x) * t2 +
        (-p0.x + 3.0f * p1.x - 3.0f * p2.x + p3.x) * t3);

        ret.y = 0.5f * ((2.0f * p1.y) +
        (-p0.y + p2.y) * t +
        (2.0f * p0.y - 5.0f * p1.y + 4 * p2.y - p3.y) * t2 +
        (-p0.y + 3.0f * p1.y - 3.0f * p2.y + p3.y) * t3);

        ret.z = 0.5f * ((2.0f * p1.z) +
        (-p0.z + p2.z) * t +
        (2.0f * p0.z - 5.0f * p1.z + 4 * p2.z - p3.z) * t2 +
        (-p0.z + 3.0f * p1.z - 3.0f * p2.z + p3.z) * t3);

        return ret;
    }
    #endregion

    static public bool CheckLangPattern(string _msg)
    {
        string reg = ".*[a-zA-Z0-9°¡-ÆR¤¡-¤¾¤¿-¤Ó\u2e80-\u2eff\u31c0-\u31ef\u3200-\u32ff\u3400-\u4dbf\u4e00-\u9fbf\uf900-\ufaff].*";
        bool isCheck = Regex.IsMatch(_msg, reg);
        return isCheck;
    }

    static public bool IsValidEmail(string _email)
    {
        if (string.IsNullOrWhiteSpace(_email))
            return false;
        return Regex.IsMatch(_email,
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    }

    public static string Encrypt(string p_data, string _key)
    {
        byte[] Skey = System.Text.ASCIIEncoding.ASCII.GetBytes(_key);

        // ¾ÏÈ£È­ ¾Ë°í¸®ÁòÁß RC2 ¾ÏÈ£È­¸¦ ÇÏ·Á¸é RC¸¦
        // DES¾Ë°í¸®ÁòÀ» »ç¿ëÇÏ·Á¸é DESCryptoServiceProvider °´Ã¼¸¦ ¼±¾ðÇÑ´Ù.
        //RC2 rc2 = new RC2CryptoServiceProvider();
        System.Security.Cryptography.DESCryptoServiceProvider rc2 = new System.Security.Cryptography.DESCryptoServiceProvider();

        // ´ëÄªÅ° ¹èÄ¡
        rc2.Key = Skey;
        rc2.IV = Skey;

        // ¾ÏÈ£È­´Â ½ºÆ®¸²(¹ÙÀÌÆ® ¹è¿­)À»
        // ´ëÄªÅ°¿¡ ÀÇÁ¸ÇÏ¿© ¾ÏÈ£È­ ÇÏ±â¶§¹®¿¡ ¸ÕÀú ¸Þ¸ð¸® ½ºÆ®¸²À» »ý¼ºÇÑ´Ù.
        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //¸¸µé¾îÁø ¸Þ¸ð¸® ½ºÆ®¸²À» ÀÌ¿ëÇØ¼­ ¾ÏÈ£È­ ½ºÆ®¸² »ý¼º
        System.Security.Cryptography.CryptoStream cryStream = new System.Security.Cryptography.CryptoStream(ms, rc2.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

        // µ¥ÀÌÅÍ¸¦ ¹ÙÀÌÆ® ¹è¿­·Î º¯°æ
        byte[] data = System.Text.Encoding.UTF8.GetBytes(p_data.ToCharArray());

        // ¾ÏÈ£È­ ½ºÆ®¸²¿¡ µ¥ÀÌÅÍ ¾¸
        cryStream.Write(data, 0, data.Length);
        cryStream.FlushFinalBlock();

        // ¾ÏÈ£È­ ¿Ï·á (stringÀ¸·Î ÄÁ¹öÆÃÇØ¼­ ¹ÝÈ¯)
        return System.Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string p_data, string _key)
    {
        byte[] Skey = System.Text.ASCIIEncoding.ASCII.GetBytes(_key);
        // ¾ÏÈ£È­ ¾Ë°í¸®ÁòÁß RC2 ¾ÏÈ£È­¸¦ ÇÏ·Á¸é RC¸¦
        // DES¾Ë°í¸®ÁòÀ» »ç¿ëÇÏ·Á¸é DESCryptoServiceProvider °´Ã¼¸¦ ¼±¾ðÇÑ´Ù.
        //RC2 rc2 = new RC2CryptoServiceProvider();
        System.Security.Cryptography.DESCryptoServiceProvider rc2 = new System.Security.Cryptography.DESCryptoServiceProvider();

        // ´ëÄªÅ° ¹èÄ¡
        rc2.Key = Skey;
        rc2.IV = Skey;

        // ¾ÏÈ£È­´Â ½ºÆ®¸²(¹ÙÀÌÆ® ¹è¿­)À»
        // ´ëÄªÅ°¿¡ ÀÇÁ¸ÇÏ¿© ¾ÏÈ£È­ ÇÏ±â¶§¹®¿¡ ¸ÕÀú ¸Þ¸ð¸® ½ºÆ®¸²À» »ý¼ºÇÑ´Ù.
        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //¸¸µé¾îÁø ¸Þ¸ð¸® ½ºÆ®¸²À» ÀÌ¿ëÇØ¼­ ¾ÏÈ£È­ ½ºÆ®¸² »ý¼º
        System.Security.Cryptography.CryptoStream cryStream =
                          new System.Security.Cryptography.CryptoStream(ms, rc2.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

        //µ¥ÀÌÅÍ¸¦ ¹ÙÀÌÆ®¹è¿­·Î º¯°æÇÑ´Ù.
        byte[] data = System.Convert.FromBase64String(p_data);

        //º¯°æµÈ ¹ÙÀÌÆ®¹è¿­À» ¾ÏÈ£È­ ÇÑ´Ù.
        cryStream.Write(data, 0, data.Length);
        cryStream.FlushFinalBlock();

        //¾ÏÈ£È­ ÇÑ µ¥ÀÌÅÍ¸¦ ½ºÆ®¸µÀ¸·Î º¯È¯ÇØ¼­ ¸®ÅÏ
        return System.Text.Encoding.UTF8.GetString(ms.GetBuffer());
    }
}