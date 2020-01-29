using CrossSystemsLimitationEditorWPF.Logic;
using Microsoft.Win32;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossSystemsLimitationEditorWPF.Tests
{
    class TestOntology
    {
        public static async Task Test_1()
        {
            Data.TestData.OntologyUP = FileUtils.JsonDeserialization<DataFromOntology>(@"Data\OntologyData.json");
            Data.TestData.erroListOntology = new List<string>();
            foreach (var Ss_ontology in Data.TestData.OntologyUP.Ss) {
                int sscount = 0;
                foreach (var key in Data.TestData.uniPair.Keys) {
                    foreach (var unipaer in Data.TestData.uniPair[key].uniPair) {
                        bool ss_equelse = Utils.Ss_equals_exclude(Ss_ontology, unipaer.Ss);
                        if (ss_equelse == true) {
                            sscount++;
                            break;
                        }

                    }
                    if (sscount > 0)
                        break;
                }
                if (sscount == 0)
                    Data.TestData.erroListOntology.Add(Ss_ontology);
            }
            foreach (var Pr_ontology in Data.TestData.OntologyUP.Pr) {
                int prcount = 0;
                foreach (var key in Data.TestData.uniPair.Keys) {
                    foreach (var unipaer in Data.TestData.uniPair[key].uniPair) {
                        if (unipaer.Pr != "") {
                            bool pr_equelse = Utils.Pr_equals_exclude(Pr_ontology, unipaer.Pr);
                            if (pr_equelse == true) {
                                prcount++;
                                break;
                            }
                        }
                    }
                    if (prcount > 0)
                        break;
                }
                if (prcount == 0)
                    Data.TestData.erroListOntology.Add(Pr_ontology);
            }
            if (Data.TestData.erroListOntology.Count != 0) {
                var path = System.IO.Path.Combine(App.OutputDirectory, "ErrorLogsOntologyTest.txt");
                System.IO.StreamWriter textFile = System.IO.File.CreateText(path);
                textFile.WriteLine("Количество ошибок = " + Data.TestData.erroListOntology.Count());
                foreach (var error in Data.TestData.erroListOntology) {
                    string text = error;
                    if (ProgectData.UniSystemTable.ContainsKey(error))
                        textFile.WriteLine(error + " " + ProgectData.UniSystemTable[error]);
                    else if (ProgectData.UniProductTable.ContainsKey(error))
                        textFile.WriteLine(error + " " + ProgectData.UniProductTable[error]);
                    else
                        textFile.WriteLine(error);
                }
                textFile.Close();
                Process.Start(path);
            }
        }

        #region Test_Mace
        //public static async Task Test_Mace()
        //{
        //    Data.TestData.erroListOntology = new List<string>();
        //    OpenFileDialog dlg = new OpenFileDialog();
        //    bool isload = false;
        //    // Set filter options and filter index.
        //    dlg.FileName = "documetn";//по умолчанию имя файла
        //    dlg.DefaultExt = ".xlsx";//расширение файла по умолчанию
        //    dlg.Filter = "Microsoft Excel (.xlsx)|*.xlsx";//фильтра разширения
        //    bool? userClickedOK = dlg.ShowDialog();
        //    if (userClickedOK == true)
        //    {
        //        ExcelEngine excelEngine = new ExcelEngine();
        //        IApplication application = excelEngine.Excel;
        //        IWorkbook workbook = application.Workbooks.Open(dlg.FileName);
        //        //Sets workbook version.
        //        workbook.Version = ExcelVersion.Excel2016;
        //        IWorksheet worksheet = workbook.Worksheets[8];

        //        Dictionary<string, List<string>> uniclass_model = new Dictionary<string, List<string>>();

        //        for (int i = 2; i < worksheet.Rows.Count<IRange>(); i++)
        //        {
        //            try
        //            {
        //                string cell_ss = worksheet.GetValueRowCol(i, 1).ToString();
        //                string cell_pr = worksheet.GetValueRowCol(i, 2).ToString();
        //                if (cell_ss.Contains("Ss_") && cell_pr.Contains("Pr_") && cell_ss.Length > 14)
        //                {
        //                    string sd = cell_ss;
        //                    int count_ss = sd.ToCharArray().Where(ss => ss == '_').Count();
        //                    string sp = cell_pr;
        //                    int count_pr = sp.ToCharArray().Where(pr => pr == '_').Count();
        //                    string Ss = cell_ss.Substring(0, 14);
        //                    string Pr = cell_pr.Substring(0, 14);
        //                    if (uniclass_model.ContainsKey(Ss) == true)
        //                    {
        //                        int count = 0;
        //                        foreach (var value in uniclass_model[Ss])
        //                        {
        //                            if(value == Pr)
        //                            {
        //                                count++;
        //                                break;
        //                            }
        //                        }
        //                        if (count == 0)
        //                            uniclass_model[Ss].Add(Pr);
        //                    }
        //                    else
        //                    {
        //                        List<string> list_pr = new List<string>();
        //                        list_pr.Add(Pr);
        //                        uniclass_model.Add(Ss, list_pr);
        //                    }
        //                }
        //                if (cell_ss.Contains("Ss_") && cell_pr.Contains("Pr_") && cell_ss.Length == 14)
        //                {
        //                    if (uniclass_model.ContainsKey(cell_ss) == true)
        //                    {
        //                        int count = 0;
        //                        foreach (var value in uniclass_model[cell_ss])
        //                        {
        //                            if (value == cell_pr)
        //                            {
        //                                count++;
        //                                break;
        //                            }
        //                        }
        //                        if (count == 0)
        //                            uniclass_model[cell_ss].Add(cell_pr);
        //                    }
        //                    else
        //                    {
        //                        List<string> list_pr = new List<string>();
        //                        list_pr.Add(cell_pr);
        //                        uniclass_model.Add(cell_ss, list_pr);
        //                    }
        //                }
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        List<string> errors = new List<string>();
        //        foreach(var ss_mace in uniclass_model.Keys)
        //        {
        //            bool ss_eq = false;
        //            foreach (var pr_mace in uniclass_model[ss_mace])
        //            {
        //                bool pr_eq = false;
        //                foreach (var value in ProgectData.UniClassGroup_data)
        //                {
        //                    if (value.SystemsID != "" && value.ProductID != "")
        //                    {
        //                        if (ss_mace.Length == value.SystemsID.Length)
        //                        {
        //                            if (ss_mace == value.SystemsID)
        //                                ss_eq = true;
        //                        }
        //                        else if (ss_mace.Length > value.SystemsID.Length)
        //                        {
        //                            if (ss_mace.Contains(value.SystemsID))
        //                                ss_eq = true;
        //                        }
        //                        if (pr_mace.Length == value.ProductID.Length)
        //                        {
        //                            if (pr_mace == value.ProductID)
        //                                pr_eq = true;
        //                        }
        //                        else if (pr_mace.Length > value.ProductID.Length)
        //                        {
        //                            if(pr_mace.Contains(value.ProductID))
        //                                pr_eq = true;
        //                        }
        //                    }
        //                    if (ss_eq == true && pr_eq == true)
        //                        break;      
        //                }
        //                if (pr_eq == false)
        //                    errors.Add(ss_mace + " " + pr_mace);
        //            }
        //            if (ss_eq == false)
        //                errors.Add(ss_mace + " all products");
        //        }              
        //        if(errors.Count != 0)
        //        {
        //            System.IO.StreamWriter textFile = new System.IO.StreamWriter(@"Data\ErrorLogsOntologyTest.txt");
        //            textFile.WriteLine("Количество ошибок = " + errors.Count());
        //            foreach (var error in errors)
        //            {
        //                string text = error;
        //                textFile.WriteLine(error);
        //            }
        //            textFile.Close();
        //            Process.Start(@"Data\ErrorLogsOntologyTest.txt");
        //        }
        //    }
        //}
        #endregion
    }
}
