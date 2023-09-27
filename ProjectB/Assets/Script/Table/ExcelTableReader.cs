using UnityEngine;
using UnityEditor;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Text;
public class ExcelFileSheet
{
    public string sheetName;
    public UnityAction<string, List<Dictionary<string, string>>> excelLoad;
    public bool isLoad;

    public ExcelFileSheet(string _sheetName, UnityAction<string, List<Dictionary<string, string>>> _excelLoad)
    {
        sheetName = _sheetName;
        excelLoad = _excelLoad;
        isLoad = false;
    }

    public void ExcelLoadAction(string _sheetName, List<Dictionary<string, string>> _data)
    {
        if (excelLoad != null)
            excelLoad(_sheetName, _data);
        isLoad = true;
    }
}

public class ExcelFile
{
    public string path;
    public List<ExcelFileSheet> m_sheetList = new List<ExcelFileSheet>();

    public ExcelFile(string _path)
    {
        path = string.Format("{0}{1}", Application.dataPath, _path);
    }

    public ExcelFile(string _path, string _sheet, UnityAction<string, List<Dictionary<string, string>>> _excelLoad)
    {
        path = string.Format("{0}{1}", Application.dataPath, _path);
        m_sheetList.Add(new ExcelFileSheet(_sheet, _excelLoad));
    }

    public void AddSheet(ExcelFileSheet _sheet)
    {
        m_sheetList.Add(_sheet);
    }

    public void Load(string _sheetName, List<Dictionary<string, string>> _data)
    {
        for (int i = 0; i < m_sheetList.Count; ++i)
        {
            if (string.Compare(m_sheetList[i].sheetName, _sheetName, true) == 0)
            {
                m_sheetList[i].ExcelLoadAction(_sheetName, _data);
            }
            else
            {
                if (m_sheetList[i].isLoad == false)
                    Debug.LogError("NotFound : " + path + ", NotFound SheetName :" + m_sheetList[i].sheetName);
            }
        }
    }
}



public class ExcelFileGroup
{
    private TableBase m_tablebase;
    private List<ExcelFile> m_excelFileList = new List<ExcelFile>();

    public TableBase getTable
    {
        get
        {
            return m_tablebase;
        }
    }


    public ExcelFileGroup(TableBase _tableBase)
    {
        m_tablebase = _tableBase;
    }

    public void AddExcelFile(ExcelFile _file)
    {
        m_excelFileList.Add(_file);
    }
    public bool LoadExcel(UnityAction<ExcelFile> _loadExcel)
    {
        for (int i = 0; i < m_excelFileList.Count; ++i)
        {
            _loadExcel(m_excelFileList[i]);
        }

        m_tablebase.Write();
        AssetDatabase.Refresh();
        return true;
    }
}


public class ExcelTableReader : EditorWindow
{
    [MenuItem("Tools/ExcelTable")]
    static void Init()
    {
        EditorWindow.GetWindow<ExcelTableReader>(false, "ExcelTable");
    }

    string m_textSucc;
    bool m_succ = false;
    List<ExcelFileGroup> m_excel = null;
    public Vector2 scrollPosition = Vector2.zero;

    public void AddLoadExcelGroup(string _path, TableBase _tableBase, string _sheet)
    {
        ExcelFileGroup _table = new ExcelFileGroup(_tableBase);
        ExcelFile _excelFile = new ExcelFile(_path);
        _excelFile.AddSheet(new ExcelFileSheet(_sheet, _table.getTable.LoadExcel));
        _table.AddExcelFile(_excelFile);
        m_excel.Add(_table);
    }

    public void AddLoadExcelGroup(string _path, TableBase _tableBase, string _sheet, UnityAction<string, List<Dictionary<string, string>>> _loadExcel)
    {
        ExcelFileGroup _table = new ExcelFileGroup(_tableBase);
        ExcelFile _excelFile = new ExcelFile(_path);
        _excelFile.AddSheet(new ExcelFileSheet(_sheet, _loadExcel));
        _table.AddExcelFile(_excelFile);
        m_excel.Add(_table);
    }

    private void OnEnable()
    {
        m_excel = null;
    }

    void InitExcelFileList()
    {
        ClassFileSave _fileSave = new ClassFileSave();

        _fileSave.SetResPath(EditorUtil.GetResPath_digimon);

        m_excel = new List<ExcelFileGroup>();

        AddLoadExcelGroup("/../../Table/Skill.xlsx", new SkillTable(_fileSave, "Table/Skill"), "Skill");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Load : All Table", GUI.skin.button))
        {
            InitExcelFileList();
            m_succ = true;
            for (int i = 0; i < m_excel.Count; ++i)
            {
                if (false == m_excel[i].LoadExcel(LoadExcel))
                {
                    m_succ = false;
                }
            }

            if (true == m_succ)
            {
                m_textSucc = System.DateTime.Now.ToString();
            }
            else
            {
                m_textSucc = "load failed";
            }

            m_excel = null;
        }

        GUILayout.TextArea(m_textSucc);

        if (null == m_excel)
        {
            InitExcelFileList();
            m_excel.Sort((a, b) => string.Compare(a.getTable.getPath, b.getTable.getPath, System.StringComparison.OrdinalIgnoreCase));
        }

        GUILayout.Space(30f);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        BeginWindows();

        for (int i = 0; i < m_excel.Count; ++i)
        {
            if (GUILayout.Button(m_excel[i].getTable.getPath, GUI.skin.button))
            {
                m_succ = true;
                if (false == m_excel[i].LoadExcel(LoadExcel))
                {
                    m_succ = false;
                }

                if (true == m_succ)
                {
                    m_textSucc = string.Format("sucess:{0} ->{1}", m_excel[i].getTable.getPath, System.DateTime.Now.ToString());
                }
                else
                {
                    m_textSucc = "load failed";
                }
            }
        }

        EndWindows();
        GUILayout.EndScrollView();
    }

    public void LoadExcel(ExcelFile _data)
    {
        using (FileStream stream = File.Open(_data.path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            IWorkbook book = null;
            if (Path.GetExtension(_data.path) == ".xls")
            {
                book = new HSSFWorkbook(stream);
            }
            else if (Path.GetExtension(_data.path) == ".xlsx")
            {
                book = new XSSFWorkbook(stream);
            }

            if (book != null)
            {
                for (int i = 0; i < book.NumberOfSheets; ++i)
                {
                    ISheet s = book.GetSheetAt(i);
                    if (_data.m_sheetList.Find(item => string.Compare(item.sheetName, s.SheetName) == 0) != null)
                        _data.Load(s.SheetName, CreateExcelData(s));
                }
            }
            else
            {
                if (Path.GetExtension(_data.path) == ".csv")
                {
                    _data.Load(_data.m_sheetList[0].sheetName, CreateExcelDataCSV(_data.path));
                }
            }
        }
    }

    public List<Dictionary<string, string>> CreateExcelData(ISheet sheet)
    {
        List<Dictionary<string, string>> _create = new List<Dictionary<string, string>>();
        IRow titleRow = sheet.GetRow(0);

        var _var = sheet.GetRowEnumerator();
        _var.MoveNext();
        while (_var.MoveNext())
        {
            if (titleRow == null)
                continue;

            bool hasValue = false;
            IRow dataRow = (IRow)_var.Current;
            Dictionary<string, string> _rowList = new Dictionary<string, string>();
            for (int i = 0; i < titleRow.LastCellNum; i++)
            {
                ICell _rowCell = titleRow.GetCell(i);
                if (null == _rowCell)
                    continue;

                string _key = _rowCell.StringCellValue;
                if (_key == null || _key.Length <= 0)
                    continue;
                string _data = string.Empty;

                if (dataRow != null)
                {
                    ICell _cell = dataRow.GetCell(i);
                    if (null != _cell)
                    {
                        CellType type = _cell.CellType;
                        if (type == CellType.Formula)
                            type = _cell.CachedFormulaResultType;

                        switch (type)
                        {
                            case CellType.Boolean:
                                _data = _cell.BooleanCellValue.ToString();
                                break;

                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(_cell))
                                    _data = _cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                                else
                                    _data = _cell.NumericCellValue.ToString();
                                break;

                            case CellType.String:
                                _data = _cell.StringCellValue;
                                break;
                        }
                        if (!hasValue && !string.IsNullOrEmpty(_data))
                            hasValue = true;
                    }
                }
                _rowList.Add(_key.ToLower(), _data);
            }
            if (hasValue)
                _create.Add(_rowList);
        }
        return _create;
    }

    public List<Dictionary<string, string>> CreateExcelDataCSV(string csvFile)
    {
        List<Dictionary<string, string>> _create = new List<Dictionary<string, string>>();
        StreamReader sr = new StreamReader(csvFile, Encoding.UTF8);
        string readTop = sr.ReadLine();
        string[] column = readTop.Split(',');
        int count = 0;
        while (!sr.EndOfStream)
        {
            string temp = sr.ReadLine();
            string[] data = temp.Split(',');
            Dictionary<string, string> _rowList = new Dictionary<string, string>();
            if (data.Length < 3)
                count++;
            for (int i = 0; i < column.Length; ++i)
            {
                _rowList.Add(column[i].ToLower(), data[i]);
            }
            _create.Add(_rowList);
        }

        return _create;
    }

}
