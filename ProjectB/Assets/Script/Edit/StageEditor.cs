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
    public class SaveStageData
    {
        public List<StageData> dataList = new();
    }

    public class StageEditor : BaseEditor
    {
        #region Data
        private string path;
        private List<StageData> dataList = new();
        private bool isReadData;
        #endregion

        [MenuItem("Tools/StageEditor")]
        public static void Open()
        {
            GetWindow<StageEditor>().titleContent = new GUIContent("Stage Info");
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
                var type = unit.stageType;
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
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Stage")))
                {
                    CreateDataAdd();
                    ForceMenuTreeRebuild();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete Stage")))
                {
                    DeleteData();
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
                        Debug.LogError("중복 Seed존재");
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
                        if (!(menuItem.Value is StageData stage)) continue;
                        if (!dicTemp.TryGetValue(stage.Seed, out var count)) continue;

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

        private void CreateDataAdd()
        {
            StageData data = new StageData(dataList.Count, dataList.Count.ToString());
            dataList.Add(data);
        }

        private void DeleteData()
        {
            if (MenuTree == null) return;
            if (!(MenuTree.Selection.SelectedValue is StageData stage)) return;

            dataList.Remove(stage);
        }


        #region Json
        private void SaveToJson()
        {
            SaveStageData saveData = new();

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
            SaveStageData saveData = new();
            if (File.Exists(path))
            {
                string decryptString = File.ReadAllText(path);
                string jsonText = Decrypt(decryptString);
                saveData = JsonUtility.FromJson<SaveStageData>(jsonText);

                for (int i = 0; i < saveData.dataList.Count; i++)
                {
                    dataList.Add(saveData.dataList[i]);
                }
            }
            else
            {
                StageData data = new StageData(0, "0");
                dataList.Add(data);
                SaveToJson();
            }
        }
        #endregion
    }
}


