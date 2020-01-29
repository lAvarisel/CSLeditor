using CrossSystemsLimitationEditorWPF.Data;
using CrossSystemsLimitationEditorWPF.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrossSystemsLimitationEditorWPF.Tests
{
    class TestMetaGroups
    {
        private static Dictionary<int, GlobalGroupForTest> globalGroupTest;

        //public static async Task LoadTestsConfig()
        //{
        //    List<Task> Tasks = new List<Task>();
        //    Task t1 = Task.Run(() => Test_1());
        //    Tasks.Add(t1);
        //    Task t2 = Task.Run(() => Test_2());
        //    Tasks.Add(t2);
        //    await Task.WhenAll(Tasks.ToArray());
        //}
        public static async Task Test_1()//Отсутствует привязка группы к глобальным группам
        {

        }
        public static async Task Test_2()//Конфликт - UC попадает в несколько глобальных групп
        {
            getGlobalGrTests();
            TestData.errorListMetaGroups = new List<Error>();
            var id = 1;
            foreach (var NG_target in globalGroupTest.Keys)
            {
                foreach (var NG_source in globalGroupTest.Keys)
                {
                    if (NG_target != NG_source)
                    {
                        if(NG_source > id)
                            Check_GlobalGroups(globalGroupTest[NG_target], globalGroupTest[NG_source]);
                    }
                }
                id++;
            }
            HashSet<string> er = new HashSet<string>();
            foreach (var error in TestData.errorListMetaGroups)
            {
                string item = "Пересечение GlobalGroup " + error.Globalgroup1 + " с группой " + error.Globalgroup2 + " - (" + error.Ss1 + " / " + error.Pr1 + ") - (" + error.Ss2 + " / " + error.Pr2 + ") - локальные группы (" + error.group1 + " " + error.group2 + ")";
                er.Add(item);
            }

            //CorrectErrors.CorrectError(errorList, uniPair);

           if(er.Count() != 0)
            {
                ProgectData.meta_error = er.Count();
                var path = System.IO.Path.Combine(App.OutputDirectory, "ErrorLogsMetaGroupTest.txt");
                System.IO.StreamWriter textFile = System.IO.File.CreateText(path);
                textFile.WriteLine("Количество ошибок = " + er.Count());
                foreach (var error in er)
                {
                    textFile.WriteLine(error);
                }
                textFile.Close();
                Process.Start(path);
            }

        }
        private static void getGlobalGrTests()
        {
            globalGroupTest = new Dictionary<int, GlobalGroupForTest>();
            foreach (var value in ProgectData.GlobalGroup_data) {
                string NgrTarget = value.IDGlobalGroup.ToString();
                UniPair newPair = new UniPair();
                newPair.Ss = value.SystemID;
                newPair.Pr = value.ProductID;
                newPair.Analitic = "";
                //newPair.Exclude = "";
                newPair.Function_layer = "";
                char[] separatorsTarget = new char[] { ',' };
                List<string> GlListTarget = value.LocGroups.Split(separatorsTarget, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

                if (globalGroupTest.ContainsKey(value.IDGlobalGroup)) {
                    if (newPair.Ss != "" || newPair.Pr != "") {
                        globalGroupTest[value.IDGlobalGroup].UniPairsForTest.Add(newPair);
                    }
                    else {
                        globalGroupTest[value.IDGlobalGroup].LockGroups.AddRange(GlListTarget);
                    }
                }
                else {
                    GlobalGroupForTest newList = new GlobalGroupForTest();
                    newList.LockGroups = new List<string>();
                    newList.UniPairsForTest = new List<UniPair>();
                    newList.N = value.IDGlobalGroup;
                    if ((newPair.Ss != "" && newPair.Ss != null) || (newPair.Pr != "" && newPair.Pr != null)) {
                        newList.UniPairsForTest.Add(newPair);
                    }
                    else {
                        newList.LockGroups.AddRange(GlListTarget);
                    }
                    globalGroupTest.Add(value.IDGlobalGroup, newList);
                }
            }
        }
        private static List<Error> Check_GlobalGroups(GlobalGroupForTest g1, GlobalGroupForTest g2)
        {
            if (g1.UniPairsForTest.Count != 0)
            {
                foreach (var up1 in g1.UniPairsForTest)
                {
                    if (g2.UniPairsForTest.Count != 0)
                    {
                        foreach (var up2 in g2.UniPairsForTest)
                        {
                            bool Ss = Utils.Ss_equals(up1.Ss, up2.Ss);
                            bool Pr = Utils.Pr_equals(up1.Pr, up2.Pr);
                            if (Ss == true && Pr == true)
                            {
                                Error newError = new Error();
                                newError.Globalgroup1 = g1.N.ToString();
                                newError.Globalgroup2 = g2.N.ToString();
                                newError.Ss1 = up1.Ss;
                                newError.Pr1 = up1.Pr;
                                newError.Ss2 = up2.Ss;
                                newError.Pr2 = up2.Pr;
                                newError.Analitic_1 = up1.Analitic;
                                newError.Analitic_2 = up2.Analitic;
                                newError.Function_layer_1 = up1.Function_layer;
                                newError.Function_layer_2 = up2.Function_layer;
                                TestData.errorListMetaGroups.Add(newError);
                            }
                        }
                    }
                    else if (g2.LockGroups.Count != 0)
                    {
                        foreach (var key in g2.LockGroups)
                            foreach (var up2 in TestData.uniPair[Convert.ToInt32(key)].uniPair)
                            {
                                bool Ss = Utils.Ss_equals(up1.Ss, up2.Ss);
                                bool Pr = Utils.Pr_equals(up1.Pr, up2.Pr);
                                if (Ss == true && Pr == true)
                                {
                                    Error newError = new Error();
                                    newError.Globalgroup1 = g1.N.ToString();
                                    newError.Globalgroup2 = g2.N.ToString();
                                    newError.group2 = key;
                                    newError.Ss1 = up1.Ss;
                                    newError.Pr1 = up1.Pr;
                                    newError.Ss2 = up2.Ss;
                                    newError.Pr2 = up2.Pr;
                                    newError.Analitic_1 = up1.Analitic;
                                    newError.Analitic_2 = up2.Analitic;
                                    newError.Function_layer_1 = up1.Function_layer;
                                    newError.Function_layer_2 = up2.Function_layer;
                                    TestData.errorListMetaGroups.Add(newError);
                                }
                            }
                    }
                }
            }
            else if (g1.LockGroups.Count != 0)
            {
                foreach (var key1 in g1.LockGroups)
                    foreach (var up1 in TestData.uniPair[Convert.ToInt32(key1)].uniPair)
                    {
                        if (g2.UniPairsForTest.Count != 0)
                        {
                            foreach (var up2 in g2.UniPairsForTest)
                            {
                                bool Ss = Utils.Ss_equals(up1.Ss, up2.Ss);
                                bool Pr = Utils.Pr_equals(up1.Pr, up2.Pr);
                                if (Ss == true && Pr == true)
                                {
                                    Error newError = new Error();
                                    newError.Globalgroup1 = g1.N.ToString();
                                    newError.Globalgroup2 = g2.N.ToString();
                                    newError.group1 = key1;
                                    newError.Ss1 = up1.Ss;
                                    newError.Pr1 = up1.Pr;
                                    newError.Ss2 = up2.Ss;
                                    newError.Pr2 = up2.Pr;
                                    newError.Analitic_1 = up1.Analitic;
                                    newError.Analitic_2 = up2.Analitic;
                                    newError.Function_layer_1 = up1.Function_layer;
                                    newError.Function_layer_2 = up2.Function_layer;
                                    TestData.errorListMetaGroups.Add(newError);
                                }
                            }
                        }
                        else if (g2.LockGroups.Count != 0)
                            foreach (var key2 in g2.LockGroups)
                                foreach (var up2 in TestData.uniPair[Convert.ToInt32(key2)].uniPair)
                                {
                                    bool Ss = Utils.Ss_equals(up1.Ss, up2.Ss);
                                    bool Pr = Utils.Pr_equals(up1.Pr, up2.Pr);
                                    bool analitic = Utils.Analitics_equals(up1.Analitic, up2.Analitic);
                                    bool Fl = Utils.FuncLayer_equals(up1.Function_layer, up2.Function_layer);
                                    if (Ss == true &&
                                        Pr == true &&
                                        analitic == true &&
                                        Fl == true)
                                    {
                                        bool ex1 = false;
                                        bool ex2 = false;
                                        foreach (var up1Exclude in TestData.uniPair[Convert.ToInt32(key2)].Exclude)
                                        {
                                            bool Ss1 = Utils.Ss_equals_exclude(up1.Ss, up1Exclude.Ss);
                                            bool Pr1 = Utils.Pr_equals_exclude(up1.Pr, up1Exclude.Pr);
                                            bool analitic1 = Utils.Analitics_equals(up1.Analitic, up1Exclude.Analitic);
                                            bool Fl1 = Utils.FuncLayer_equals(up1.Function_layer, up1Exclude.Function_layer);
                                            if (Ss1 == true &&
                                                Pr1 == true &&
                                                analitic1 == true &&
                                                Fl1 == true)
                                            {
                                                ex1 = true;
                                            }
                                        }
                                        foreach (var up2Exclude in TestData.uniPair[Convert.ToInt32(key1)].Exclude)
                                        {
                                            bool Ss2 = Utils.Ss_equals_exclude(up2.Ss, up2Exclude.Ss);
                                            bool Pr2 = Utils.Pr_equals_exclude(up2.Pr, up2Exclude.Pr);
                                            bool analitic2 = Utils.Analitics_equals(up1.Analitic, up2Exclude.Analitic);
                                            bool Fl2 = Utils.FuncLayer_equals(up1.Function_layer, up2Exclude.Function_layer);
                                            if (Ss2 == true &&
                                                Pr2 == true &&
                                                analitic2 == true &&
                                                Fl2 == true)
                                            {
                                                ex2 = true;
                                            }
                                        }
                                        if (ex1 == false && ex2 == false)
                                        {
                                            Error newError = new Error();
                                            newError.Globalgroup1 = g1.N.ToString();
                                            newError.Globalgroup2 = g2.N.ToString();
                                            newError.group1 = key1;
                                            newError.group2 = key2;
                                            newError.Ss1 = up1.Ss;
                                            newError.Pr1 = up1.Pr;
                                            newError.Ss2 = up2.Ss;
                                            newError.Pr2 = up2.Pr;
                                            newError.Analitic_1 = up1.Analitic;
                                            newError.Analitic_2 = up2.Analitic;
                                            newError.Function_layer_1 = up1.Function_layer;
                                            newError.Function_layer_2 = up2.Function_layer;
                                            TestData.errorListMetaGroups.Add(newError);
                                        }
                                    }
                                }
                    }
            }
            return TestData.errorListMetaGroups;
        }
    }
}
