using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CrossSystemsLimitationEditorWPF
{
    public class CrossSystemsLimitation
    {
        public int N { get; set; }
        public string SsSource { get; set; }
        public string PrSource { get; set; }
        public string GroupSource { get; set; }
        public string SsTarget { get; set; }
        public string PrTarget { get; set; }
        public string GroupTarget { get; set; }
        public string Description { get; set; }
        public string PointSource { get; set; }
        public string IDLimitation { get; set; }
        public string ConnectionWeight { get; set; }
        public string StrSource { get; set; }
        public string Category { get; set; }
    }

    public class UniClassGroup
    {
        public int GroupID { get; set; }
        public string Exclude { get; set; }
        public string SystemsID { get; set; }
        public string ProductID { get; set; }
        public string GroupDescription { get; set; }
        public string AnalyticalModel { get; set; }
        public string Function_layer { get; set; }
        public string GroupDescription_Note { get; set; }
    }

    public class Limitation_functions
    {
        public string IDLimitation { get; set; }
        public string IDLimitDescription { get; set; }
    }

    public class GlobalGroup
    {
        public int IDGlobalGroup { get; set; }
        public string SystemID { get; set; }
        public string ProductID { get; set; }
        public string LocGroups { get; set; }
        public string ZCoord { get; set; }
        public string GlobalGroupsDescription { get; set; }
    }

    public class GroupsSs
    {
        public string Group { get; set; }
        public string GroupDescription { get; set; }
        public string UniSystem { get; set; }
        public string UniSystemDescription { get; set; }
    }

    public class GroupsPr
    {
        public string Group { get; set; }
        public string GroupDescription { get; set; }
        public string UniProduct { get; set; }
        public string UniProductDescription { get; set; }
    }

    public class SetupLimitation
    {
        public string Group { get; set; }
        public string UniSystem { get; set; }
        public string UniSystemDescription { get; set; }
        public string UniProduct { get; set; }
        public string UniProductDescription { get; set; }
        public string GroupDescription { get; set; }
        public string Function_layer { get; set; }
        public string AnalyticalModel { get; set; }
        public string Category { get; set; }
        public string Element { get; set; }
        public string Material { get; set; }
        public string Orientation { get; set; }
    }

    public class ListGroups
    {
        public string Group { get; set; }
        public string Description { get; set; }
        public string AnalyticalModel { get; set; }
        public string Function_layer { get; set; }
        public string Orientation { get; set; }
    }

    public class UC_KeyDescription
    {
        public int GroupID { get; set; }
        public string GroupDescription_Note { get; set; }
    }
    public class UC_UniClass
    {
        public string SystemsID { get; set; }
        public string ProductID { get; set; }
        public string GroupDescription { get; set; }
    }

    public class UniPairs
    {
        public List<UniPair> Exclude { get; set; }
        public List<UniPair> uniPair { get; set; }
    }

    public class UniPair
    {
        public string Ss { get; set; }
        public string Pr { get; set; }
        public string Analitic { get; set; }
        public string Function_layer { get; set; }
    }

    public class Error
    {
        public string Ss1 { get; set; }
        public string Pr1 { get; set; }
        public string Ss2 { get; set; }
        public string Pr2 { get; set; }
        public string group1 { get; set; }
        public string group2 { get; set; }
        public string Globalgroup1 { get; set; }
        public string Globalgroup2 { get; set; }
        public string Analitic_1 { get; set; }
        public string Analitic_2 { get; set; }
        public string Function_layer_1 { get; set; }
        public string Function_layer_2 { get; set; }
    }

    public class GlobalGroupForTest
    {
        public int N { get; set; }
        public List<UniPair> UniPairsForTest { get; set; }
        public List<string> LockGroups { get; set; }
    }

    public class DataFromOntology
    {
        public List<string> Ss { get; set; }
        public List<string> Pr { get; set; }
    }

    public class UniPairOntology
    {
        public string Ss { get; set; }
        public string Pr { get; set; }
    }

    public class RelationFromOntology
    {
        public string source { get; set; }
        public string target { get; set; }
        public string relation { get; set; }
        public List<string> property { get; set; }
       
    }

    public class GroupsUC
    {
        public string Group { get; set; }
        public string UC_Id { get; set; }
        public string UC_Description { get; set; }
        public string GroupDescription { get; set; }
    }

    public class OntologyGroups
    {
        public Dictionary<string,string> keyValuePairs { get; set; }
    }

    public class LimitationPropery
    {
        public string relation { get; set; }
        public string value { get; set; }
    }

    public class SecExtractor
    {
        public string item { get; set; }
        public string type { get; set; }
        public string extractor { get; set; }
    }
    public class OntologyInputData
    {
        public Dictionary<string, List<UniPairOntology>> ontology_UC { get; set; }
        public Dictionary<string, List<RelationFromOntology>> relation { get; set; }
        public Dictionary<string, List<string>> prop { get; set; }
    }
    public class LimitationRange
    {
        public string id_limitation { get; set; }
        public string source { get; set; }
        public List<string> list_target { get; set; }
    }

}
