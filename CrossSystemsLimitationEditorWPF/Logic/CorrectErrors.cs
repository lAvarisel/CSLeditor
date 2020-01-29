using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CrossSystemsLimitationEditorWPF.Logic
{
    class CorrectErrors
    {
        //public static void CorrectErrorUcGroups(List<Error> errorList, Dictionary<int, UniPairs> uniPair)
        //{
        //    ProgectData.uniPair_Correct = new Dictionary<int, UniPairs>();
        //    ProgectData.uniPair_Correct = uniPair;
        //    foreach (var error in errorList) {
        //        bool canfix = false;
        //        if (error.group1 != null && error.group2 != null) {

        //        }
        //    }
        //}
        public static void CorrectError(List<Error> errorList, Dictionary<int, UniPairs> uniPair, string metod)
        {
            ProgectData.uniPair_Correct = new Dictionary<int, UniPairs>();
            ProgectData.uniPair_Correct = uniPair;
            foreach (var error in errorList)
            {
                bool canfix = false;
                if(error.group1 != null && error.group2 != null)
                {
                    foreach (var excl in uniPair[Convert.ToInt32(error.group2)].Exclude)
                    {
                        bool Ss1 = Utils.Ss_equals_exclude(error.Ss1, excl.Ss);
                        bool Ss2 = Utils.Ss_equals_exclude(error.Ss2, excl.Ss);
                        bool Pr1 = Utils.Pr_equals_exclude(error.Pr1, excl.Pr);
                        bool Pr2 = Utils.Pr_equals_exclude(error.Pr2, excl.Pr);
                        if (Ss1 == true
                            && Pr1 == true
                            && error.Analitic_1 == excl.Analitic
                            && error.Function_layer_1 == excl.Function_layer)
                        {
                            canfix = true;
                            break;
                        }
                        if (Ss2 == true
                            && Pr2 == true
                            && error.Analitic_2 == excl.Analitic
                            && error.Function_layer_2 == excl.Function_layer)
                        {
                            canfix = true;
                            break;
                        }
                    }
                    foreach (var excl in uniPair[Convert.ToInt32(error.group1)].Exclude)
                    {
                        bool Ss1 = Utils.Ss_equals_exclude(error.Ss1, excl.Ss);
                        bool Ss2 = Utils.Ss_equals_exclude(error.Ss2, excl.Ss);
                        bool Pr1 = Utils.Pr_equals_exclude(error.Pr1, excl.Pr);
                        bool Pr2 = Utils.Pr_equals_exclude(error.Pr2, excl.Pr);
                        if (Ss1 == true
                            && Pr1 == true
                            && error.Analitic_1 == excl.Analitic
                            && error.Function_layer_1 == excl.Function_layer)
                        {
                            canfix = true;
                            break;
                        }
                        if (Ss2 == true
                            && Pr2 == true
                            && error.Analitic_2 == excl.Analitic
                            && error.Function_layer_2 == excl.Function_layer)
                        {
                            canfix = true;
                            break;
                        }
                    }
                }

                if (canfix == false)
                {
                    if(error.Pr1.Length > error.Pr2.Length)
                        CreateUniPaerExclude(ProgectData.uniPair_Correct, "2", error);
                    else if(error.Pr1.Length < error.Pr2.Length)
                        CreateUniPaerExclude(ProgectData.uniPair_Correct, "1", error);
                    else
                    {
                        if (error.Ss1.Length > error.Ss2.Length)
                            CreateUniPaerExclude(ProgectData.uniPair_Correct, "2", error);
                        else if (error.Ss1.Length < error.Ss2.Length)
                            CreateUniPaerExclude(ProgectData.uniPair_Correct, "1", error);
                        else
                        {
                            if(error.Analitic_1 != "" && error.Analitic_1 != error.Analitic_2 
                                || error.Function_layer_1 != "" && error.Function_layer_1 != error.Function_layer_2)
                            {
                                CreateUniPaerExclude(ProgectData.uniPair_Correct, "2", error);
                            }
                            else if(error.Analitic_2 != "" && error.Analitic_2 != error.Analitic_1
                                || error.Function_layer_2 != "" && error.Function_layer_2 != error.Function_layer_1)
                            {
                                CreateUniPaerExclude(ProgectData.uniPair_Correct, "1", error);
                            }
                        }
                    }
                }
            }
            string sMessageBoxText = "";
            switch (metod) {
                case "UC_Groups":
                    sMessageBoxText = "Внести изменения в CrossSystemsLimitation добавив exclude? Исправление по UniClassGroups";
                    break;
                case "MetaGroups":
                    sMessageBoxText = "Внести изменения в CrossSystemsLimitation добавив exclude? Исправление по MetaGroups";
                    break;
            }
            string sCaption = "CrossSystemsLimitationEditor";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    ProgectData.UniClassGroup_data.Clear();
                    foreach(var key in ProgectData.uniPair_Correct.Keys)
                    {
                        if(ProgectData.uniPair_Correct[key].Exclude.Count != 0)
                        {
                            foreach (var exclude in ProgectData.uniPair_Correct[key].Exclude)
                            {
                                UniClassGroup newData = new UniClassGroup();
                                newData.GroupID = key;
                                newData.Exclude = "exclude";
                                newData.SystemsID = exclude.Ss;
                                newData.ProductID = exclude.Pr;
                                newData.GroupDescription = Utils.CreateDescrGroups(newData.SystemsID, newData.ProductID);
                                newData.Function_layer = exclude.Function_layer;
                                newData.AnalyticalModel = exclude.Analitic;
                                newData.GroupDescription_Note = ProgectData.UniclassGroupDescription[key];
                                ProgectData.UniClassGroup_data.Add(newData);
                            }
                        }
                        if (ProgectData.uniPair_Correct[key].uniPair.Count != 0)
                        {
                            foreach (var unipaer in ProgectData.uniPair_Correct[key].uniPair)
                            {
                                if(key == 33)
                                {

                                }
                                UniClassGroup newData = new UniClassGroup();
                                newData.GroupID = key;
                                newData.Exclude = "";
                                newData.SystemsID = unipaer.Ss;
                                newData.ProductID = unipaer.Pr;
                                newData.GroupDescription = Utils.CreateDescrGroups(newData.SystemsID, newData.ProductID);
                                newData.Function_layer = unipaer.Function_layer;
                                newData.AnalyticalModel = unipaer.Analitic;
                                newData.GroupDescription_Note = ProgectData.UniclassGroupDescription[key];
                                ProgectData.UniClassGroup_data.Add(newData);
                            }
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    /* ... */
                    break;
            }
        }
        private static void CreateUniPaerExclude (Dictionary<int, UniPairs> uniPair, string N_excludeGroup, Error error)
        {
            UniPair newExcludePair = new UniPair();
            switch (N_excludeGroup)
            {
                case "1":               
                    newExcludePair.Ss = error.Ss2;
                    newExcludePair.Pr = error.Pr2;
                    newExcludePair.Analitic = error.Analitic_2;
                    newExcludePair.Function_layer = error.Function_layer_2;
                    uniPair[Convert.ToInt32(error.group1)].Exclude.Add(newExcludePair);
                    break;
                case "2":
                    newExcludePair = new UniPair();
                    newExcludePair.Ss = error.Ss1;
                    newExcludePair.Pr = error.Pr1;
                    newExcludePair.Analitic = error.Analitic_1;
                    newExcludePair.Function_layer = error.Function_layer_1;
                    uniPair[Convert.ToInt32(error.group2)].Exclude.Add(newExcludePair);
                    break;
            }
            
        }
    }
}
