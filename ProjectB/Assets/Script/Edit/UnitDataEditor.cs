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
    [System.Serializable]
    public class SaveUnitData
    {
        public List<UnitData> dataList = new();
    }

    public class UnitInfo
    {
        public string Name;
        public int Seed;

        public UnitInfo(string Name, int seed)
        {
            this.Name = Name;
            Seed = seed;
        }
    }


    public class UnitDataEditor : BaseEditor
    {
        #region Data
        private string path;
        private List<UnitData> dataList = new();
        private bool isReadData;
        #endregion

        [MenuItem("Tools/UnitInfoEditor")]
        public static void Open()
        {
            GetWindow<UnitDataEditor>().titleContent = new GUIContent("Unit Info");
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 50f;
            tree.Config.DrawSearchToolbar = true;

            path = "./Assets/Data/GameResources/DataJsons/";
            path = Path.Combine(Application.dataPath + "/Data/GameResources/DataJsons/", this.GetType().Name + ".json");
            if (isReadData == false)
                GetSaveData();

            foreach (var unit in dataList)
            {
                var name = unit.Name;
                var type = unit.Type;
                var mainSubIdText = unit.Seed;

                var menuItem = new OdinMenuItem(tree, name, unit);
                menuItem.OnRightClick += item => EditorGUIUtility.PingObject(item.Value as Object);
                tree.AddMenuItemAtPath(type.ToString(), menuItem);
            }

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            if (MenuTree == null) return;
            if (MenuTree.Config == null) return;
            var toolbarHeiight = MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeiight);
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Unit")))
                {
                    CreateUnitAdd();
                    ForceMenuTreeRebuild();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete Unit")))
                {
                    DeleteUnit();
                    ForceMenuTreeRebuild();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Refresh")))
                {
                    ForceMenuTreeRebuild();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save")))
                {
                    if (isExistDuplicated)
                    {
                        Debug.LogError("�ߺ� Seed����");
                        return;
                    }
                    SaveToJson();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private int n = 5;
        private bool isExistDuplicated;
        private Dictionary<int, int> dicTemp = new Dictionary<int, int>();
        private void OnInspectorUpdate()
        {
            --n;
            if (n > 0) return;
            n = 5;

            isExistDuplicated = false;
            dicTemp.Clear();
            foreach (var unit in dataList)
            {
                if (!dicTemp.TryGetValue(unit.Seed, out var count))
                    dicTemp[unit.Seed] = 1;
                else
                    ++dicTemp[unit.Seed];
                if (dicTemp[unit.Seed] > 1)
                    isExistDuplicated = true;
            }

            if (MenuTree != null)
            {
                foreach (var treeMenuItem in MenuTree.MenuItems)
                {
                    foreach (var menuItem in treeMenuItem.ChildMenuItems)
                    {
                        if (!(menuItem.Value is UnitData unit)) continue;
                        if (!dicTemp.TryGetValue(unit.Seed, out var count)) continue;

                        if (count > 1)
                        {
                            menuItem.Style = MenuErrorStyle;
                            menuItem.Parent.Style = MenuErrorStyle;
                        }
                        else
                        {
                            menuItem.Style = MenuDefaultStyle;
                            menuItem.Parent.Style = MenuDefaultStyle;
                        }
                    }
                }
            }
        }

        private void GetSaveData()
        {
            isReadData = true;
            LoadJson();
        }

        private void CreateUnitAdd()
        {
            UnitData data = new UnitData(dataList.Count, dataList.Count.ToString());
            dataList.Add(data);
        }

        private void DeleteUnit()
        {
            if (MenuTree == null) return;
            if (!(MenuTree.Selection.SelectedValue is UnitData unit)) return;

            dataList.Remove(unit);
        }


        #region Json
        private void SaveToJson()
        {
            SaveUnitData saveData = new();

            for (int i = 0; i < dataList.Count; i++)
            {
                saveData.dataList = dataList;
            }

            string jsonText = JsonUtility.ToJson(saveData, true);
            string encryptString = Encrypt(jsonText);
            File.WriteAllText(path, encryptString);
        }

        private void LoadJson()
        {
            SaveUnitData saveData = new();
            if (File.Exists(path))
            {
                string decryptString = File.ReadAllText(path);
                string jsonText = Decrypt(decryptString);
                saveData = JsonUtility.FromJson<SaveUnitData>(jsonText);

                for (int i = 0; i < saveData.dataList.Count; i++)
                {
                    dataList.Add(saveData.dataList[i]);
                }
            }
            else
            {
                UnitData data = new UnitData(0, "0");
                dataList.Add(data);
                SaveToJson();
            }
        }
        #endregion
    }
}


