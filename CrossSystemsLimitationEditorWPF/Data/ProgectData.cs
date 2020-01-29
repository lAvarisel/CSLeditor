using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CrossSystemsLimitationEditorWPF
{
    public class ProgectData
    {
        public static BindingList<CrossSystemsLimitation> CrossSystemsLimitation_data;
        public static BindingList<UniClassGroup> UniClassGroup_data;
        public static BindingList<GlobalGroup> GlobalGroup_data;
        public static BindingList<Limitation_functions> Limitation_functions_data;
        public static BindingList<GroupsUC> GroupsSs;
        public static BindingList<GroupsUC> GroupsPr;
        public static BindingList<OntologyGroups> OntologyGroups;

        public static BindingList<UniClassGroup> UniClassGroup_data_generate;
        public static BindingList<CrossSystemsLimitation> CrossSystemsLimitation_data_generate;

        public static Dictionary<string, string> UniSystemTable;
        public static Dictionary<string, string> UniProductTable;

        public static Dictionary<string, List<string>> Pr1_lvl;
        public static Dictionary<string, List<string>> Pr2_lvl;
        public static Dictionary<string, List<string>> Pr3_lvl;
        public static Dictionary<string, string> Pr4_lvl;

        public static Dictionary<string, List<string>> Ss1_lvl;
        public static Dictionary<string, List<string>> Ss2_lvl;
        public static Dictionary<string, List<string>> Ss3_lvl;
        public static Dictionary<string, string> Ss4_lvl;

        public static Dictionary<int, UniPairs> uniPair_Correct;//исправленные данные после теста metagroups

        public static Dictionary<int, string> UniclassGroupDescription;

        public static Dictionary<string, List<string>> relations;
        public static Dictionary<string, List<string>> ontology_classes;

        public static List<SecExtractor> tableExtractor;

        public static int uc_error;
        public static int meta_error;
        public static int ontology_error;
       
    }
}
