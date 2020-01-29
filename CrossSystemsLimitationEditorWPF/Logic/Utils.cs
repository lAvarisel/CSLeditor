using Microsoft.Win32;
using Syncfusion.XlsIO;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.ComponentModel;
using CrossSystemsLimitationEditorWPF.Data;

namespace CrossSystemsLimitationEditorWPF
{
    class Utils
    {      
        public static void CreatePrLevels(Dictionary<string, string> UniProductTable)
        {
            ProgectData.Pr1_lvl = new Dictionary<string, List<string>>();
            ProgectData.Pr2_lvl = new Dictionary<string, List<string>>();
            ProgectData.Pr3_lvl = new Dictionary<string, List<string>>();
            ProgectData.Pr4_lvl = new Dictionary<string, string>();

            foreach(var pr in UniProductTable.Keys)
            {
                int lenght = pr.Length;
                switch(lenght)
                {
                    case 5:
                        if(ProgectData.Pr1_lvl.ContainsKey(pr) == false)
                        {
                            List<string> newList = new List<string>();
                            ProgectData.Pr1_lvl.Add(pr, newList);
                        }
                        break;
                    case 8:
                        if (ProgectData.Pr2_lvl.ContainsKey(pr) == false)
                        {
                            List<string> newList = new List<string>();
                            ProgectData.Pr2_lvl.Add(pr, newList);
                        }
                        string Pr_xx = pr.Substring(0, 5);
                        ProgectData.Pr1_lvl[Pr_xx].Add(pr);
                        break;
                    case 11:
                        if (ProgectData.Pr3_lvl.ContainsKey(pr) == false)
                        {
                            List<string> newList = new List<string>();
                            ProgectData.Pr3_lvl.Add(pr, newList);
                        }
                        Pr_xx = pr.Substring(0, 8);
                        ProgectData.Pr2_lvl[Pr_xx].Add(pr);
                        break;
                    case 14:
                        if (ProgectData.Pr4_lvl.ContainsKey(pr) == false)
                        {
                            ProgectData.Pr4_lvl.Add(pr, ProgectData.UniProductTable[pr]);
                        }
                        Pr_xx = pr.Substring(0, 11);
                        ProgectData.Pr3_lvl[Pr_xx].Add(pr);
                        break;
                }
            }
        }
        public static void CreateSsLevels(Dictionary<string, string> UnisystemTable)
        {
            ProgectData.Ss1_lvl = new Dictionary<string, List<string>>();
            ProgectData.Ss2_lvl = new Dictionary<string, List<string>>();
            ProgectData.Ss3_lvl = new Dictionary<string, List<string>>();
            ProgectData.Ss4_lvl = new Dictionary<string, string>();

            foreach (var ss in UnisystemTable.Keys)
            {
                //int lenght = ss.Length;
                switch (ss.Length)
                {
                    case 5:
                        if (ProgectData.Ss1_lvl.ContainsKey(ss) == false)
                        {
                            List<string> newList = new List<string>();
                            ProgectData.Ss1_lvl.Add(ss, newList);
                        }
                        break;
                    case 8:
                        if (ProgectData.Ss2_lvl.ContainsKey(ss) == false)
                        {
                            List<string> newList = new List<string>();
                            ProgectData.Ss2_lvl.Add(ss, newList);
                        }
                        string Ss_xx = ss.Substring(0, 5);
                        ProgectData.Ss1_lvl[Ss_xx].Add(ss);
                        break;
                    case 11:
                        if (ProgectData.Ss3_lvl.ContainsKey(ss) == false)
                        {
                            List<string> newList = new List<string>();
                            ProgectData.Ss3_lvl.Add(ss, newList);
                        }
                        Ss_xx = ss.Substring(0, 8);
                        ProgectData.Ss2_lvl[Ss_xx].Add(ss);
                        break;
                    case 14:
                        if (ProgectData.Ss4_lvl.ContainsKey(ss) == false)
                        {
                            ProgectData.Ss4_lvl.Add(ss, ProgectData.UniSystemTable[ss]);
                        }
                        Ss_xx = ss.Substring(0, 11);
                        ProgectData.Ss3_lvl[Ss_xx].Add(ss);
                        break;
                }
            }
        }
        public static string CreateDescrGroups(string Ss, string Pr)
        {
            string description = "";
            if (ProgectData.UniSystemTable.ContainsKey(Ss))
                description = ProgectData.UniSystemTable[Ss];
            if (ProgectData.UniProductTable.ContainsKey(Pr))
                description = description + "---" + ProgectData.UniProductTable[Pr];
            return description;
        }
        public static void getUniPair()
        {
            TestData.uniPair = new Dictionary<int, UniPairs>();
            foreach (var element in ProgectData.UniClassGroup_data)
            {
                UniPairs unipairs = new UniPairs();
                unipairs.Exclude = new List<UniPair>();
                unipairs.uniPair = new List<UniPair>();

                UniPair newPair = new UniPair();
                newPair.Ss = element.SystemsID;
                newPair.Pr = element.ProductID;
                newPair.Analitic = element.AnalyticalModel;
                newPair.Function_layer = element.Function_layer;

                if (TestData.uniPair.ContainsKey(element.GroupID))
                {
                    if (element.Exclude == "exclude")
                        TestData.uniPair[element.GroupID].Exclude.Add(newPair);
                    else
                        TestData.uniPair[element.GroupID].uniPair.Add(newPair);
                }
                else
                {
                    UniPairs newuniPairs = new UniPairs();
                    newuniPairs.Exclude = new List<UniPair>();
                    newuniPairs.uniPair = new List<UniPair>();
                    if (element.Exclude == "exclude")
                        newuniPairs.Exclude.Add(newPair);
                    else
                        newuniPairs.uniPair.Add(newPair);
                    TestData.uniPair.Add(element.GroupID, newuniPairs);
                }
            }
        }
        public static bool Analitics_equals(string an1, string an2)
        {
            bool result = false;
            if (an1 == an2)
                return true;
            else if (an1 == "" || an2 == "")
                return true;
            return result;
        }
        public static bool FuncLayer_equals(string fl1, string fl2)
        {
            bool result = false;
            if (fl1 == fl2)
                return true;
            else if (fl1 == "" || fl2 == "")
                return true;
            return result;
        }
        public static bool Ss_equals_exclude(string ss, string ssSource)
        {
            bool result = false;
            if (ss.Length > ssSource.Length)
            {
                if (ss.Contains(ssSource))
                    return true;
            }
            else if (ss.Length < ssSource.Length)
            {
                return false;
            }
            else if (ss.Length == ssSource.Length)
            {
                if (ss == ssSource)
                    return true;
            }
            return result;
        }
        public static bool Pr_equals_exclude(string pr, string prSource)
        {
            bool result = false;
            if (pr.Length > prSource.Length)
            {
                if (pr.Contains(prSource))
                    return true;
            }
            else if (pr.Length < prSource.Length)
            {
                return false;
            }
            else if (pr.Length == prSource.Length)
            {
                if (pr == prSource)
                    return true;
            }
            return result;
        }
        public static bool Ss_equals(string ss, string ssSource)
        {
            bool result = false;
            if (ss.Length > ssSource.Length)
            {
                if (ss.Contains(ssSource))
                    return true;
            }
            else if (ss.Length < ssSource.Length)
            {
                if (ssSource.Contains(ss))
                    return true;
            }
            else if (ss.Length == ssSource.Length)
            {
                if (ss == ssSource)
                    return true;
            }
            return result;
        }
        public static bool Pr_equals(string pr, string prSource)
        {
            bool result = false;
            if (pr.Length > prSource.Length)
            {
                if (pr.Contains(prSource))
                    return true;
            }
            else if (pr.Length < prSource.Length)
            {
                if (prSource.Contains(pr))
                    return true;
            }
            else if (pr.Length == prSource.Length)
            {
                if (pr == prSource)
                    return true;
            }
            return result;
        }
    }
}
