using Microsoft.Win32;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows;

namespace CrossSystemsLimitationEditorWPF.Logic
{
    class FileUtils
    {
        public static async Task LoadExcelConfig(string filename, string typefile)
        {
            switch(typefile)
            {
                case "limitation":
                    List<Task> Tasks = new List<Task>();
                    Task t = Task.Run(() => LoadExcel(filename));
                    Tasks.Add(t);
                    await Task.WhenAll(Tasks.ToArray());
                    break;
                case "ontology":
                    List<Task> Tasks_ontology = new List<Task>();
                    Task ontology = Task.Run(() => ReloadOntology(filename));
                    Tasks_ontology.Add(ontology);
                    await Task.WhenAll(Tasks_ontology.ToArray());
                    break;
            }
        }
        public static async Task LoadExcel(string filename)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            OpenFileDialog dlg = new OpenFileDialog();
            IWorkbook workbook = application.Workbooks.Open(filename);
            //Sets workbook version.
            workbook.Version = ExcelVersion.Excel2016;
            IWorksheet worksheet = workbook.Worksheets[0];

            ProgectData.CrossSystemsLimitation_data = new BindingList<CrossSystemsLimitation>();
            ProgectData.UniClassGroup_data = new BindingList<UniClassGroup>();
            ProgectData.GlobalGroup_data = new BindingList<GlobalGroup>();
            ProgectData.Limitation_functions_data = new BindingList<Limitation_functions>();
            ProgectData.GroupsSs = new BindingList<GroupsUC>();
            ProgectData.GroupsPr = new BindingList<GroupsUC>();
            ProgectData.OntologyGroups = new BindingList<OntologyGroups>();

            for (int i = 2; i < worksheet.Rows.Count<IRange>(); i++)
            {
                if (worksheet.GetValueRowCol(i, 9) != null && worksheet.GetValueRowCol(i, 9) != "")
                {
                    CrossSystemsLimitation newLimit = new CrossSystemsLimitation();
                    newLimit.N = i;
                    newLimit.SsSource = worksheet.GetValueRowCol(i, 1).ToString();
                    newLimit.PrSource = worksheet.GetValueRowCol(i, 2).ToString();
                    newLimit.GroupSource = worksheet.GetValueRowCol(i, 3).ToString();
                    newLimit.SsTarget = worksheet.GetValueRowCol(i, 4).ToString();
                    newLimit.PrTarget = worksheet.GetValueRowCol(i, 5).ToString();
                    newLimit.GroupTarget = worksheet.GetValueRowCol(i, 6).ToString();
                    newLimit.Description = worksheet.GetValueRowCol(i, 7).ToString();
                    //string descr = ""/*Utils.CreateDescrLimitation(newLimit, ProgectData.uniClassGroups_key, ProgectData.UniSystemTable, ProgectData.UniProductTable)*/;
                    //newLimit.Description = descr;
                    newLimit.PointSource = worksheet.GetValueRowCol(i, 8).ToString();
                    newLimit.IDLimitation = worksheet.GetValueRowCol(i, 9).ToString();
                    newLimit.ConnectionWeight = worksheet.GetValueRowCol(i, 10).ToString();
                    newLimit.StrSource = worksheet.GetValueRowCol(i, 11).ToString();
                    newLimit.Category = worksheet.GetValueRowCol(i, 12).ToString();
                    ProgectData.CrossSystemsLimitation_data.Add(newLimit);
                    //colLimitation.Add(newLimit);
                }
                else
                    break;
            }

            ProgectData.UniclassGroupDescription = new Dictionary<int, string>();

            worksheet = workbook.Worksheets[1];
            for (int i = 2; i <= worksheet.Rows.Count<IRange>(); i++)
            {
                try {
                    if(worksheet.GetValueRowCol(i, 1) != null && worksheet.GetValueRowCol(i, 1) != "") {
                        int key = Convert.ToInt32(worksheet.GetValueRowCol(i, 1).ToString());
                        if (ProgectData.UniclassGroupDescription.ContainsKey(key) == false) {
                            ProgectData.UniclassGroupDescription.Add(key, worksheet.GetValueRowCol(i, 8).ToString());
                        }
                        if (worksheet.GetValueRowCol(i, 1) != null && worksheet.GetValueRowCol(i, 1) != "") {
                            UniClassGroup newData = new UniClassGroup();
                            newData.GroupID = Convert.ToInt32(worksheet.GetValueRowCol(i, 1).ToString());
                            newData.Exclude = worksheet.GetValueRowCol(i, 2).ToString();
                            newData.SystemsID = worksheet.GetValueRowCol(i, 3).ToString();
                            newData.ProductID = worksheet.GetValueRowCol(i, 4).ToString();
                            newData.GroupDescription = Utils.CreateDescrGroups(newData.SystemsID, newData.ProductID);
                            newData.GroupDescription_Note = worksheet.GetValueRowCol(i, 8).ToString();
                            newData.Function_layer = worksheet.GetValueRowCol(i, 7).ToString();
                            newData.AnalyticalModel = worksheet.GetValueRowCol(i, 6).ToString();
                            ProgectData.UniClassGroup_data.Add(newData);
                        }
                    }
                    else
                        break;
                }
                catch {
                    MessageBox.Show("Ошибка при чтении UniClassGroups");
                }
            }

            worksheet = workbook.Worksheets[2];
            for (int i = 1; i < worksheet.Rows.Count<IRange>(); i++)
            {
                if (worksheet.GetValueRowCol(i, 1) != null && worksheet.GetValueRowCol(i, 1) != "")
                {
                    Limitation_functions newLimitFunc = new Limitation_functions();
                    newLimitFunc.IDLimitation = worksheet.GetValueRowCol(i, 1).ToString();
                    newLimitFunc.IDLimitDescription = worksheet.GetValueRowCol(i, 2).ToString();
                    ProgectData.Limitation_functions_data.Add(newLimitFunc);
                }
            }

            worksheet = workbook.Worksheets[3];
            for (int i = 2; i < worksheet.Rows.Count<IRange>(); i++)
            {
                var IDGlobalGroup = worksheet.GetValueRowCol(i, 1).ToString();
                if (IDGlobalGroup != null && IDGlobalGroup != "")
                {
                    GlobalGroup newGroup = new GlobalGroup();
                    newGroup.IDGlobalGroup = Convert.ToInt32(worksheet.GetValueRowCol(i, 1).ToString());
                    newGroup.SystemID = worksheet.GetValueRowCol(i, 2).ToString();
                    newGroup.ProductID = worksheet.GetValueRowCol(i, 3).ToString();
                    newGroup.LocGroups = worksheet.GetValueRowCol(i, 4).ToString();
                    newGroup.ZCoord = worksheet.GetValueRowCol(i, 5).ToString();
                    newGroup.GlobalGroupsDescription = worksheet.GetValueRowCol(i, 6).ToString();
                    ProgectData.GlobalGroup_data.Add(newGroup);
                }
            }

            worksheet = workbook.Worksheets[6];
            for (int i = 2; i <= worksheet.Rows.Count<IRange>(); i++) {
                if (worksheet.GetValueRowCol(i, 1) != null && worksheet.GetValueRowCol(i, 1).ToString() != "") {
                    var uc = new GroupsUC();
                    uc.Group = worksheet.GetValueRowCol(i, 1).ToString();
                    uc.UC_Id = worksheet.GetValueRowCol(i, 3).ToString();
                    uc.UC_Description = worksheet.GetValueRowCol(i, 4).ToString();
                    uc.GroupDescription = worksheet.GetValueRowCol(i, 2).ToString();
                    ProgectData.GroupsSs.Add(uc);
                }
            }

            worksheet = workbook.Worksheets[7];
            for (int i = 2; i <= worksheet.Rows.Count<IRange>(); i++) {
                if(worksheet.GetValueRowCol(i, 1) != null && worksheet.GetValueRowCol(i, 1).ToString() != "") {
                    var up = new GroupsUC();
                    up.Group = worksheet.GetValueRowCol(i, 1).ToString();
                    up.UC_Id = worksheet.GetValueRowCol(i, 3).ToString();
                    up.UC_Description = worksheet.GetValueRowCol(i, 4).ToString();
                    up.GroupDescription = worksheet.GetValueRowCol(i, 2).ToString();
                    ProgectData.GroupsPr.Add(up);
                }   
            }

            OntologyGroups og = new OntologyGroups();
            og.keyValuePairs = new Dictionary<string, string>();
            worksheet = workbook.Worksheets[8];
            for (int i = 2; i <= worksheet.Rows.Count<IRange>(); i++) {
                if(worksheet.GetValueRowCol(i, 1) != null && worksheet.GetValueRowCol(i, 1).ToString() != "") {
                    string group = worksheet.GetValueRowCol(i, 2).ToString();
                    string ontology = worksheet.GetValueRowCol(i, 1).ToString();
                    og.keyValuePairs.Add(group, ontology);
                }
            }
            ProgectData.OntologyGroups.Add(og);

        }
        public static async Task ReloadOntology(string filename)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            OpenFileDialog dlg = new OpenFileDialog();
            IWorkbook workbook = application.Workbooks.Open(filename);
            //Sets workbook version.
            workbook.Version = ExcelVersion.Excel2016;
            IWorksheet worksheet = workbook.Worksheets[0];

            Data.TestData.OntologyUP = new DataFromOntology();
            Data.TestData.OntologyUP.Ss = new List<string>();
            Data.TestData.OntologyUP.Pr = new List<string>();
            for (int i = 2; i < worksheet.Rows.Count<IRange>(); i++)
            {
                if (worksheet.GetValueRowCol(i, 1) != null && worksheet.GetValueRowCol(i, 1).ToString() != "")
                {
                    Data.TestData.OntologyUP.Ss.Add(worksheet.GetValueRowCol(i, 1).ToString());
                }
                else
                    break;
            }

            worksheet = workbook.Worksheets[1];
            for (int i = 2; i < worksheet.Rows.Count<IRange>(); i++)
            {
                if (worksheet.GetValueRowCol(i, 1) != null && worksheet.GetValueRowCol(i, 1).ToString() != "")
                {
                    Data.TestData.OntologyUP.Pr.Add(worksheet.GetValueRowCol(i, 1).ToString());
                }
                else
                    break;
            }
            FileUtils.JsonSerialization(@"Data\OntologyData.json", Data.TestData.OntologyUP);
        }
        public static async Task SaveExcel(string filename, string mode)
        {
            switch (mode) {
                case "full":
                    ExcelEngine excelEngine = new ExcelEngine();
                    IApplication application = excelEngine.Excel;
                    OpenFileDialog dlg = new OpenFileDialog();
                    IWorkbook workbook = application.Workbooks.Open(filename);
                    //Sets workbook version.
                    workbook.Version = ExcelVersion.Excel2016;
                    IWorksheet worksheet = workbook.Worksheets[1];

                    worksheet.Range[1, 1].Text = "Group ID";
                    worksheet.Range[1, 3].Text = "Systems ID";
                    worksheet.Range[1, 4].Text = "Product ID";
                    worksheet.Range[1, 5].Text = "Group Description";
                    worksheet.Range[1, 6].Text = "Analytical model";
                    worksheet.Range[1, 7].Text = "Function layer";
                    worksheet.Range[1, 8].Text = "Note";

                    int currentRow = 2;
                    foreach (var keyGroup in ProgectData.UniClassGroup_data) {
                        worksheet.Range[currentRow, 1].Text = keyGroup.GroupID.ToString();
                        worksheet.Range[currentRow, 2].Text = keyGroup.Exclude;
                        worksheet.Range[currentRow, 3].Text = keyGroup.SystemsID;
                        worksheet.Range[currentRow, 4].Text = keyGroup.ProductID;
                        string Ss = "";
                        string Pr = "";
                        if (keyGroup.SystemsID != "" && ProgectData.UniSystemTable.ContainsKey(keyGroup.SystemsID))
                            Ss = ProgectData.UniSystemTable[keyGroup.SystemsID];
                        if (keyGroup.ProductID != "" && ProgectData.UniProductTable.ContainsKey(keyGroup.ProductID))
                            Pr = ProgectData.UniProductTable[keyGroup.ProductID];
                        string descr = Ss + "---" + Pr;
                        worksheet.Range[currentRow, 5].Text = descr;
                        worksheet.Range[currentRow, 6].Text = keyGroup.AnalyticalModel;
                        worksheet.Range[currentRow, 7].Text = keyGroup.Function_layer;
                        worksheet.Range[currentRow, 8].Text = keyGroup.GroupDescription_Note;
                        currentRow++;
                    }

                    currentRow = 2;
                    worksheet = workbook.Worksheets[0];
                    worksheet.Range[1, 1].Text = "SS source";
                    worksheet.Range[1, 2].Text = "Pr source";
                    worksheet.Range[1, 3].Text = "Group source";
                    worksheet.Range[1, 4].Text = "SS target";
                    worksheet.Range[1, 5].Text = "Pr target";
                    worksheet.Range[1, 6].Text = "Group target";
                    worksheet.Range[1, 7].Text = "Description";
                    worksheet.Range[1, 8].Text = "Source Point";
                    worksheet.Range[1, 9].Text = "ID Limitation";
                    worksheet.Range[1, 10].Text = "Сonnection weight";
                    worksheet.Range[1, 11].Text = "Str.Source";
                    foreach (var limit in ProgectData.CrossSystemsLimitation_data) {
                        worksheet.Range[currentRow, 1].Text = limit.SsSource;
                        worksheet.Range[currentRow, 2].Text = limit.PrSource;
                        worksheet.Range[currentRow, 3].Text = limit.GroupSource;
                        worksheet.Range[currentRow, 4].Text = limit.SsTarget;
                        worksheet.Range[currentRow, 5].Text = limit.PrTarget;
                        worksheet.Range[currentRow, 6].Text = limit.GroupTarget;
                        worksheet.Range[currentRow, 7].Text = limit.Description;
                        worksheet.Range[currentRow, 8].Text = limit.PointSource;
                        worksheet.Range[currentRow, 9].Text = limit.IDLimitation;
                        worksheet.Range[currentRow, 10].Text = limit.ConnectionWeight;
                        worksheet.Range[currentRow, 11].Text = limit.StrSource;
                        worksheet.Range[currentRow, 12].Text = limit.Category;
                        currentRow++;
                    }

                    currentRow = 2;
                    worksheet = workbook.Worksheets[3];
                    worksheet.Range[1, 1].Text = "№Group";
                    worksheet.Range[1, 2].Text = "Systems ID";
                    worksheet.Range[1, 3].Text = "Product ID";
                    worksheet.Range[1, 4].Text = "Loc. Group";
                    worksheet.Range[1, 5].Text = "Z";
                    worksheet.Range[1, 6].Text = "Description";
                    foreach (var group in ProgectData.GlobalGroup_data) {
                        worksheet.Range[currentRow, 1].Text = group.IDGlobalGroup.ToString();
                        worksheet.Range[currentRow, 2].Text = group.SystemID;
                        worksheet.Range[currentRow, 3].Text = group.ProductID;
                        worksheet.Range[currentRow, 4].Text = group.LocGroups;
                        worksheet.Range[currentRow, 5].Text = group.ZCoord;
                        currentRow++;
                    }

                    workbook.Version = ExcelVersion.Excel2013;
                    //Save the workbook in file system as XLSX format
                    workbook.SaveAs(filename);
                    break;

                case "error":
                    excelEngine = new ExcelEngine();
                    application = excelEngine.Excel;
                    dlg = new OpenFileDialog();
                    workbook = application.Workbooks.Open(filename);
                    //Sets workbook version.
                    workbook.Version = ExcelVersion.Excel2016;
                    worksheet = workbook.Worksheets[1];

                    worksheet.Range[1, 1].Text = "Group ID";
                    worksheet.Range[1, 3].Text = "Systems ID";
                    worksheet.Range[1, 4].Text = "Product ID";
                    worksheet.Range[1, 5].Text = "Group Description";
                    worksheet.Range[1, 6].Text = "Analytical model";
                    worksheet.Range[1, 7].Text = "Function layer";

                    currentRow = 2;
                    foreach (var row in ProgectData.UniClassGroup_data) {
                        worksheet.Range[currentRow, 1].Text = row.GroupID.ToString();
                        worksheet.Range[currentRow, 2].Text = row.Exclude;
                        worksheet.Range[currentRow, 3].Text = row.SystemsID;
                        worksheet.Range[currentRow, 4].Text = row.ProductID;
                        worksheet.Range[currentRow, 5].Text = row.GroupDescription;
                        worksheet.Range[currentRow, 6].Text = row.AnalyticalModel;
                        worksheet.Range[currentRow, 7].Text = row.Function_layer;
                        worksheet.Range[currentRow, 8].Text = row.GroupDescription_Note;
                        currentRow++;
                    }
                    workbook.Version = ExcelVersion.Excel2013;
                    //Save the workbook in file system as XLSX format
                    workbook.SaveAs(filename);
                    break;
                case "ontology":
                    excelEngine = new ExcelEngine();
                    application = excelEngine.Excel;
                    dlg = new OpenFileDialog();
                    workbook = application.Workbooks.Open(filename);
                    //Sets workbook version.
                    workbook.Version = ExcelVersion.Excel2016;
                    worksheet = workbook.Worksheets[0];

                    currentRow = 1;
                    foreach (var row in ProgectData.tableExtractor) {
                        worksheet.Range[currentRow, 1].Text = row.item;
                        worksheet.Range[currentRow, 2].Text = row.type;
                        worksheet.Range[currentRow, 3].Text = row.extractor;
                        
                        currentRow++;
                    }
                    workbook.Version = ExcelVersion.Excel2013;
                    //Save the workbook in file system as XLSX format
                    workbook.SaveAs(filename);
                    break;
            }
        }
        public static T JsonDeserialization<T>(string pathFileJson)
           where T : class
        {
            using (StreamReader r = new StreamReader(pathFileJson))
            {
                string json = r.ReadToEnd();
                T item = JsonConvert.DeserializeObject<T>(json);
                return item;
            }
        }
        public static void JsonSerialization(string filePath, object data)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.Create)))
            {
                var ser = JsonConvert.SerializeObject(data);
                sw.Write(ser);
                sw.Close();
            }
        }
    }
}
