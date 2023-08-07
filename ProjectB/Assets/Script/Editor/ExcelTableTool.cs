using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;

public class ExcelFileSheet
{
    public string sheetName;
    public UnityAction<string, List<Dictionary<string, string>>> excelLoad;
    public bool isLoad;

    public ExcelFileSheet(string sheetName, UnityAction<string, List<Dictionary<string, string>>> action)
    {
        this.sheetName = sheetName;
        excelLoad = action;
    }

    public void ExcelLoadAction(string sheetName, List<Dictionary<string, string>> data)
    {
        if (excelLoad != null)
            excelLoad(sheetName, data);
        isLoad = true;
    }
}

public class ExcelFile
{
    public string path;
    public List<ExcelFileSheet> sheetList = new List<ExcelFileSheet>();

    public ExcelFile(string path)
    {
        this.path = string.Format("{0}{1}", Application.dataPath, path);
    }

    public ExcelFile(string path, string sheet, UnityAction<string, List<Dictionary<string, string>>> action)
    {
        this.path = string.Format("{0}{1}", Application.dataPath, path);
        sheetList.Add(new ExcelFileSheet(sheet, action));
    }

    public void AddSheet(ExcelFileSheet sheet)
    {
        sheetList.Add(sheet);
    }

    public void Load(string sheetName, List<Dictionary<string, string>> data)
    {
        for (int i = 0; i < sheetList.Count; i++)
        {
            if (string.Compare(sheetList[i].sheetName, sheetName, true) == 0)
                sheetList[i].ExcelLoadAction(sheetName, data);
            else if(sheetList[i].isLoad == false)
                Debug.LogError("NotFound : " + path + ", NotFound SheetName :" + sheetList[i].sheetName);
        }
    }
}

public class ExcelFileGroup
{
    private TableBase tableBase;
    private List<ExcelFile> excelFileList = new List<ExcelFile>();

    public TableBase getTable { get { return tableBase; } }

    public ExcelFileGroup(TableBase table)
    {
        tableBase = table;
    }

    public void AddExcelFile(ExcelFile file)
    {
        excelFileList.Add(file);
    }

    public bool LoadExcel(UnityAction<ExcelFile> action)
    {
        for (int i = 0; i < excelFileList.Count; i++)
        {
            action(excelFileList[i]);
        }

        tableBase.Write();
        AssetDatabase.Refresh();
        return true;
    }
}

public class ExcelTableTool : EditorWindow
{
    [MenuItem("Tools/ExcelTable")]
    private static void Init()
    {
        EditorWindow.GetWindow<ExcelTableTool>(false, "ExcelTable");
    }

    List<ExcelFileGroup> excelGroup;
    public Vector2 scrollPos = Vector2.zero;

    public void AddLoadExcelGroup(string path, TableBase tableBase, string sheet)
    {
        ExcelFileGroup fileGroup = new ExcelFileGroup(tableBase);
        ExcelFile file = new ExcelFile(path);
        file.AddSheet(new ExcelFileSheet(sheet, fileGroup.getTable.LoadExcel));
        fileGroup.AddExcelFile(file);
        excelGroup.Add(fileGroup);
    }

    private void OnEnable()
    {
        excelGroup = null;
    }

    private void InitExcelFileList()
    {
        ClassFileSave fileSave = new ClassFileSave();
        fileSave.SetResPath(EditorUtil.GetResPath);

        excelGroup = new List<ExcelFileGroup>();

        AddLoadExcelGroup("/../Table/Test_Info.csv", new TestTable("Table/Test_Info", fileSave), "Test_Info");
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Load : All Table", GUI.skin.button))
        {
            InitExcelFileList();
            for (int i = 0; i < excelGroup.Count; i++)
            {
                if (excelGroup[i].LoadExcel(LoadExcel) == false)
                    GUILayout.TextArea("Load Failed");

                excelGroup = null;
            }
        }

        if (excelGroup == null)
            InitExcelFileList();

        GUILayout.Space(20f);

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        BeginWindows();

        for (int i = 0; i < excelGroup.Count; i++)
        {
            InitExcelFileList();
            if(GUILayout.Button(excelGroup[i].getTable.getPath, GUI.skin.button))
            {
                if (excelGroup[i].LoadExcel(LoadExcel) == false)
                    GUILayout.TextArea("Load Failed");
            }
        }

        EndWindows();
        GUILayout.EndScrollView();
    }

    public void LoadExcel(ExcelFile data)
    {
        using (FileStream stream = File.Open(data.path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            IWorkbook book = null;
            if (Path.GetExtension(data.path) == ".xls")
                book = new HSSFWorkbook(stream);
            else if (Path.GetExtension(data.path) == ".xlsx")
                book = new XSSFWorkbook(stream);

            if(book != null)
            {
                for (int i = 0; i < book.NumberOfSheets; i++)
                {
                    ISheet s = book.GetSheetAt(i);
                    data.Load(s.SheetName, CreateExcelData(s));
                }
            }
            else
            {
                if(Path.GetExtension(data.path) == ".csv")
                {
                    data.Load(data.sheetList[0].sheetName, CreateExcelDataCSV(data.path));
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
                        switch (_cell.CellType)
                        {
                            case CellType.Boolean:
                                _data = _cell.BooleanCellValue.ToString();
                                break;

                            case CellType.Numeric:
                                _data = _cell.NumericCellValue.ToString();
                                break;

                            case CellType.String:
                                _data = _cell.StringCellValue;
                                break;
                        }

                    }
                }
                _rowList.Add(_key.ToLower(), _data);
            }
            _create.Add(_rowList);
        }
        return _create;
    }

    public List<Dictionary<string, string>> CreateExcelDataCSV(string csvFile)
    {
        List<Dictionary<string, string>> create = new List<Dictionary<string, string>>();
        StreamReader sr = new StreamReader(csvFile, Encoding.UTF8);
        string readTop = sr.ReadLine();
        string[] column = readTop.Split(',');
        while(!sr.EndOfStream)
        {
            string temp = sr.ReadLine();
            string[] data = temp.Split(',');
            Dictionary<string, string> rowList = new Dictionary<string, string>();

            for (int i = 0; i < column.Length; i++)
            {
                rowList.Add(column[i].ToLower(), data[i]);
            }
            create.Add(rowList);
        }

        return create;
    }
}
