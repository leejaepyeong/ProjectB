using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager
{
    static public TableManager Instance
    {
        get
        {
            return Manager.Instance.getTableManager;
        }
    }

    ClassFileSave m_fileSave = new ClassFileSave();
    List<TableBase> m_tableList = new List<TableBase>();

    public ClassFileSave getFileSave
    {
        get
        {
            return m_fileSave;
        }
    }
    public bool isLoad = false;

    // excel

    public StringTable stringTable;
    public SkillTable skillTable;
    public SkillEffectTable skillEffectTable;
    public RuneTable runeTable;

    public void Load(ClassFileSave _fileSave = null)
    {
        if (null != _fileSave)
        {
            m_fileSave = _fileSave;
        }
        m_tableList.Clear();
        m_tableList.Add(skillTable = new SkillTable(m_fileSave, "Table/Skill_Info"));
        m_tableList.Add(runeTable = new RuneTable(m_fileSave, "Table/Rune_Effect_Info"));
        m_tableList.Add(skillEffectTable = new SkillEffectTable(m_fileSave, "Table/Skill_Effect_Info"));
        m_tableList.Add(stringTable = new StringTable(m_fileSave, "Table/String_Info"));

        for (int i = 0; i < m_tableList.Count; ++i)
        {
            m_tableList[i].Load();
        }

        isLoad = true;
    }
}
