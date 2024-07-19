using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace Editor
{
    public class BaseEditor : OdinMenuEditorWindow
    {
        protected static GUIStyle redLabel;
        public static GUIStyle RedLabel
        {
            get
            {
                if (redLabel == null)
                {
                    redLabel = new GUIStyle(EditorStyles.label) { margin = new RectOffset(0, 0, 0, 0) };
                    redLabel.fontStyle = FontStyle.Bold;
                    redLabel.normal.textColor = Color.red;
                    redLabel.onNormal.textColor = Color.red;
                }
                return redLabel;
            }
        }
        protected static OdinMenuStyle menuErrorStyle;
        public static OdinMenuStyle MenuErrorStyle
        {
            get
            {
                if (menuErrorStyle == null)
                {
                    menuErrorStyle = new OdinMenuStyle();
                    menuErrorStyle.DefaultLabelStyle = RedLabel;
                    menuErrorStyle.SelectedLabelStyle = RedLabel;
                }
                return menuErrorStyle;
            }
        }
        protected static OdinMenuStyle menuDefaultStyle;

        public static OdinMenuStyle MenuDefaultStyle
        {
            get
            {
                if (menuDefaultStyle == null)
                {
                    menuDefaultStyle = new OdinMenuStyle();
                }

                return menuDefaultStyle;
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 50f;
            tree.Config.DrawSearchToolbar = true;

            return tree;
        }
        #region Encryption

        protected static string Encrypt(string data)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            RijndaelManaged rm = CreateRijndaelManaged();
            ICryptoTransform ct = rm.CreateEncryptor();
            byte[] result = ct.TransformFinalBlock(bytes, 0, bytes.Length);
            return System.Convert.ToBase64String(result, 0, result.Length);
        }

        protected static string Decrypt(string data)
        {
            byte[] bytes = System.Convert.FromBase64String(data);
            RijndaelManaged rm = CreateRijndaelManaged();
            ICryptoTransform ct = rm.CreateDecryptor();
            byte[] result = ct.TransformFinalBlock(bytes, 0, bytes.Length);
            return System.Text.Encoding.UTF8.GetString(result);
        }

        protected static RijndaelManaged CreateRijndaelManaged()
        {
            byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(Define.privateKey);
            RijndaelManaged result = new();

            byte[] newKeysArray = new byte[16];
            System.Array.Copy(keyArray, 0, newKeysArray, 0, 16);

            result.Key = newKeysArray;
            result.Mode = CipherMode.ECB;
            result.Padding = PaddingMode.PKCS7;
            return result;
        }

        #endregion
    }
}
