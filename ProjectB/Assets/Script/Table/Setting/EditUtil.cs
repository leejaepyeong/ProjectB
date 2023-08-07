using UnityEngine;
using UnityEditor;

using System.IO;
using System.Collections;
using System.Collections.Generic;


public class EditorUtil
{

    public static Vector3 DrawVector2(Vector3 vec3Value)
    {
        GUILayoutOption opt = GUILayout.MinWidth(30f);
        vec3Value.x = EditorGUILayout.FloatField("X", vec3Value.x, opt);
        vec3Value.y = EditorGUILayout.FloatField("Y", vec3Value.y, opt);
        vec3Value.z = EditorGUILayout.FloatField("Z", vec3Value.z, opt);
        return vec3Value;
    }

    public static Vector2 DrawVector2(Vector2 vec2Value)
    {
        GUILayoutOption opt = GUILayout.MinWidth(30f);
        vec2Value.x = EditorGUILayout.FloatField("X", vec2Value.x, opt);
        vec2Value.y = EditorGUILayout.FloatField("Y", vec2Value.y, opt);
        return vec2Value;
    }

    public static string GetSelectedResPath()
    {
        string sourcePath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        string strName = Selection.activeObject.name;

        string strPath = string.Empty;
        sourcePath = sourcePath.ToLower();
        string[] words = sourcePath.Split('\\');
        bool sw = false;
        foreach (string word in words)
        {
            if (sw)
            {
                strPath = strPath + word + "/";
            }

            if (word.CompareTo("resources") == 0)
                sw = true;
        }
        strPath += strName;
        return strPath;
    }

    public static string GetSelectResPath_Hierachy(string _tag)
    {
        Transform _trans = Selection.activeTransform;
        string _path = null;
        if (_trans != null)
        {
            _path = _trans.name;
            while (_trans.parent != null)
            {
                _trans = _trans.parent;
                if (_trans.tag == _tag)
                    break;
                _path = _trans.name + "/" + _path;
            }
        }
        return _path;
    }

    public static string GetSelectedName()
    {
        return Selection.activeObject.name;
    }

    public static int GetConvertInt(string _strData, string strShowErrorMsg)
    {
        try
        {
            return int.Parse(_strData);
        }
        catch
        {
            EditorUtility.DisplayDialog("error GetConvertInt", strShowErrorMsg, "Ok");
        }

        return int.MaxValue;
    }

    public static float GetConvertFloat(string _strData, string strShowErrorMsg)
    {
        try
        {
            return float.Parse(_strData);
        }
        catch
        {
            EditorUtility.DisplayDialog("error GetConvertFloat", strShowErrorMsg, "Ok");
        }

        return float.MaxValue;
    }

    public static void OpenSceneInEdit(string _strSceneName)
    {
        string strScene = string.Format("Assets/Scenes/{0}.unity", _strSceneName);
        string _curSceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
        if (0 == string.Compare(strScene, _curSceneName))
        {
            return;
        }

        Debug.Log("Load Scene : " + strScene);
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(strScene);
    }

    static public string GetHierachyPath(string _path, string _title, string _tag)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply", GUILayout.Width(80f)))
        {
            GUIUtility.keyboardControl = 0;
            _path = GetSelectResPath_Hierachy(_tag);
        }
        _path = EditorGUILayout.TextField(_title, _path);
        GUILayout.EndHorizontal();
        return _path;
    }

    static public string GetAssetFilePath(string _path, string _title)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply", GUILayout.Width(80f)))
        {
            GUIUtility.keyboardControl = 0;
            _path = EditorUtil.GetSelectedResPath();
        }

        _path = EditorGUILayout.TextField(_title, _path);
        GUILayout.EndHorizontal();

        return _path;
    }

    static public string[] GetMaxString(string _title, int _max)
    {
        string[] _str = new string[_max];
        for (int i = -0; i < _max; ++i)
        {
            if (null != _title)
            {
                _str[i] = _title + (i + 1).ToString();
            }
            else
            {
                _str[i] = (i + 1).ToString();
            }

        }

        return _str;
    }

    static public string GetResPath(string _filename)
    {
        return string.Format("{0}/Resources/{1}.bytes", Application.dataPath, _filename);
    }
}
