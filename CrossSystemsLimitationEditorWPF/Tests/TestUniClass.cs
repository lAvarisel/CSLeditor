using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossSystemsLimitationEditorWPF.Tests
{
    class TestUniClass
    {
        public static async Task Test_1()//Отсутствует привязка группы к глобальным группам
        {
            Data.TestData.erroListUniClassGroups = new List<Error>();
            var id = 1;
            foreach (var id_group_target in Data.TestData.uniPair.Keys) {
                foreach(var id_group_source in Data.TestData.uniPair.Keys) {
                    if(id_group_target != id_group_source) {
                        if (id_group_source > id) {
                            var error = CheackUCGroups(Data.TestData.uniPair[id_group_target],
                            Data.TestData.uniPair[id_group_source],
                            id_group_target,
                            id_group_source);
                            Data.TestData.erroListUniClassGroups.AddRange(error);
                        }
                    }
                }
                id++;
            }
            HashSet<string> er = new HashSet<string>();
            foreach (var error in Data.TestData.erroListUniClassGroups) {
                string item = "Пересечение UniClassGroups " + error.group1 + " с группой " + error.group2 + " - (" + error.Ss1 + " / " + error.Pr1 + ") - (" + error.Ss2 + " / " + error.Pr2 + ")";
                er.Add(item);
            }

            //CorrectErrors.CorrectError(errorList, uniPair);

            if (er.Count() != 0) {
                ProgectData.uc_error = er.Count();
                var path = System.IO.Path.Combine(App.OutputDirectory, "ErrorLogsUniClassGroupsTest.txt");
                System.IO.StreamWriter textFile = System.IO.File.CreateText(path);
                textFile.WriteLine("Количество ошибок = " + er.Count());
                foreach (var error in er) {
                    textFile.WriteLine(error);
                }
                textFile.Close();
                Process.Start(path);
            }
        }
        private static List<Error> CheackUCGroups(UniPairs target, UniPairs source, int id_1, int id_2)
        {
            var result = new List<Error>();
            foreach(var up1 in target.uniPair) {
                foreach(var up2 in source.uniPair) {
                    bool Ss = Utils.Ss_equals(up1.Ss, up2.Ss);
                    bool Pr = Utils.Pr_equals(up1.Pr, up2.Pr);
                    bool analitic = Utils.Analitics_equals(up1.Analitic, up2.Analitic);
                    bool func_lauer = Utils.FuncLayer_equals(up1.Function_layer, up2.Function_layer);
                    if(Ss == true && Pr == true && analitic == true && func_lauer == true) {
                        bool ex1 = cheak_equals(up1, target.Exclude, source.Exclude);
                        bool ex2 = cheak_equals(up2, target.Exclude, source.Exclude);
                        if (ex1 == false && ex2 == false) {
                            Error newError = new Error();
                            newError.group1 = id_1.ToString();
                            newError.group2 = id_2.ToString();
                            newError.Ss1 = up1.Ss;
                            newError.Pr1 = up1.Pr;
                            newError.Ss2 = up2.Ss;
                            newError.Pr2 = up2.Pr;
                            newError.Analitic_1 = up1.Analitic;
                            newError.Analitic_2 = up2.Analitic;
                            newError.Function_layer_1 = up1.Function_layer;
                            newError.Function_layer_2 = up2.Function_layer;
                            result.Add(newError);
                        }
                    }
                }
            }
            return result;
        }
        private static bool cheak_equals(UniPair up, List<UniPair> excl_1, List<UniPair> excl_2)
        {
            var result = false;
            foreach (var exclude in excl_1) {
                bool Ss1 = Utils.Ss_equals_exclude(up.Ss, exclude.Ss);
                bool Pr1 = Utils.Pr_equals_exclude(up.Pr, exclude.Pr);
                bool analitic1 = Utils.Analitics_equals(up.Analitic, exclude.Analitic);
                bool Fl1 = Utils.FuncLayer_equals(up.Function_layer, exclude.Function_layer);
                if (Ss1 == true &&
                    Pr1 == true &&
                    analitic1 == true &&
                    Fl1 == true) {
                    result = true;
                    break;
                }
            }
            foreach (var exclude in excl_2) {
                bool Ss1 = Utils.Ss_equals_exclude(up.Ss, exclude.Ss);
                bool Pr1 = Utils.Pr_equals_exclude(up.Pr, exclude.Pr);
                bool analitic1 = Utils.Analitics_equals(up.Analitic, exclude.Analitic);
                bool Fl1 = Utils.FuncLayer_equals(up.Function_layer, exclude.Function_layer);
                if (Ss1 == true &&
                    Pr1 == true &&
                    analitic1 == true &&
                    Fl1 == true) {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
