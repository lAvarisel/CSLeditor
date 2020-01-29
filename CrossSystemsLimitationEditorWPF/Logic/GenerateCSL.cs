using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CrossSystemsLimitationEditorWPF;
using CrossSystemsLimitationEditorWPF.Logic;
using Microsoft.Win32;

namespace CrossSystemsLimitationEditorWPF.Logic
{
    internal class GenerateCSL
    {
        public static BindingList<UniClassGroup> UniClassGroup_generated;
        public static BindingList<CrossSystemsLimitation> CrossSystemsLimitation_generated;
        public static BindingList<GlobalGroup> GlobalGroup_generated;

        public static async Task GenerateCrossSystemsLimitations()
        {
            ProgectData.ontology_classes = new Dictionary<string, List<string>>(
                FileUtils.JsonDeserialization<Dictionary<string, List<string>>>(@"Data\ontology_classes.json"));
            ProgectData.relations = new Dictionary<string, List<string>>(
                FileUtils.JsonDeserialization<Dictionary<string, List<string>>>(@"Data\relations.json"));

            var property = new List<string>(GetAllSubProperty("has_limitation"));
            var node_leaf_CSL = new List<string>(GetAllSubClasses("limitation"));
            var allOntologyRelations = new List<string>(GetAllRelations(node_leaf_CSL, property));
            var all_node = GetAllNodesSubClasses("limitation");
            var all_leafe = GetAllLeafSubClasses("limitation");
            var meta = GetSubClasses("limitation");
            var metas = GenerMetaNodes(meta);
            var dict_nodes = GenerMetaNodes(all_node);
            var inputData = CreateInputData(all_leafe, allOntologyRelations, meta, all_node, dict_nodes);
            var meta_data = CreateMetaData(meta, allOntologyRelations);
            GenerateCSLTable(inputData,
                ProgectData.OntologyGroups[0].keyValuePairs,
                meta,
                metas,
                node_leaf_CSL,
                meta_data);

            ProgectData.CrossSystemsLimitation_data = CrossSystemsLimitation_generated;
            ProgectData.UniClassGroup_data = UniClassGroup_generated;
            ProgectData.GlobalGroup_data = GlobalGroup_generated;

        }
        #region Table_generation
        public static void GenerateCSLTable(OntologyInputData inputData,
                                            Dictionary<string, string> ontology_id,
                                            List<string> meta,
                                            Dictionary<string,List<string>> metas,
                                            List<string> node,
                                            string[] nums)
        {
            var ucOntoList = new Dictionary<string, List<string>>();
            foreach (var ss in ProgectData.GroupsSs) {
                if (ucOntoList.ContainsKey(ss.Group))
                    ucOntoList[ss.Group].Add(ss.UC_Id);
                else {
                    var list = new List<string>();
                    list.Add(ss.UC_Id);
                    ucOntoList.Add(ss.Group, list);
                }
            }
            foreach (var pr in ProgectData.GroupsPr) {
                if (ucOntoList.ContainsKey(pr.Group))
                    ucOntoList[pr.Group].Add(pr.UC_Id);
                else {
                    var list = new List<string>();
                    list.Add(pr.UC_Id);
                    ucOntoList.Add(pr.Group, list);
                }
            }

            ProgectData.UniclassGroupDescription.Clear();
            int idGroup = 1;
            var csl_id = new Dictionary<string, int>();
            UniClassGroup_generated = new BindingList<UniClassGroup>();
            foreach(var key in inputData.ontology_UC.Keys) {
                var ss_key = new HashSet<string>();
                var pr_key = new HashSet<string>();
                if (key.Contains("csl_floor_str")) {

                }
                foreach(var value in inputData.ontology_UC[key]) {
                    ss_key.Add(value.Ss);
                    pr_key.Add(value.Pr);
                }
                foreach (var ss_item in ss_key) {
                    string ss = ontology_id[ss_item];
                    var pr_list = new HashSet<string>();
                    foreach (var pr_item in pr_key) {
                        string pr = ontology_id[pr_item];
                        foreach (var x in ucOntoList[pr])
                            pr_list.Add(x);
                    }
                    var prop = new List<string>();
                    if (inputData.prop.ContainsKey(key))
                        prop = inputData.prop[key];
                    CreateNewGroup(idGroup,
                        UniClassGroup_generated,
                        ucOntoList[ss],
                        pr_list.ToList(),
                        prop,
                        key.Split(new[] { '|' })[1]
                        );
                    if (!ProgectData.UniclassGroupDescription.ContainsKey(idGroup))
                        ProgectData.UniclassGroupDescription.Add(idGroup, key.Split(new[] { '|' })[1]);
                    if (!csl_id.ContainsKey(key))
                        csl_id.Add(key, idGroup);
                }
                idGroup++;


                //string ss = ontology_id[inputData.ontology_UC[key].Ss];
                //string pr = ontology_id[inputData.ontology_UC[key].Pr];
                //var prop = new List<string>();
                //if (inputData.prop.ContainsKey(key))
                //    prop = inputData.prop[key];
                //CreateNewGroup(idGroup,
                //    UniClassGroup_generated,
                //    ucOntoList[ss],
                //    ucOntoList[pr],
                //    prop,
                //    key.Split(new[] { '|' })[1]
                //    );
                //csl_id.Add(key, idGroup);
            }
            GenerateLimitationTable(meta, inputData, csl_id, metas, node, nums);
        }
        public static void GenerateMetaTable(Dictionary<string, List<string>> metas, Dictionary<string, int> csl_id, string[] nums)
        {
            GlobalGroup_generated = new BindingList<GlobalGroup>();
            foreach (var global in nums) {
                var newGlobalGroup = new GlobalGroup() { IDGlobalGroup = Array.IndexOf(nums, global) + 1 };
                string groups = "";
                if (metas[global].Count == 0) {
                    groups = csl_id[global].ToString();
                }
                else {
                    foreach (var item in metas[global]) {
                        if (groups == "")
                            groups = csl_id[item].ToString();
                        else {
                            groups = groups + ", " + csl_id[item].ToString();
                        }
                    }
                }
                newGlobalGroup.LocGroups = groups;
                GlobalGroup_generated.Add(newGlobalGroup);
            }
        }
        public static void GenerateLimitationTable(List<string> meta,
                                                   OntologyInputData inputData,
                                                   Dictionary<string, int> csl_id,
                                                   Dictionary<string, List<string>> metas,
                                                   List<string> node,
                                                   string[] nums)
        {
            CrossSystemsLimitation_generated = new BindingList<CrossSystemsLimitation>();
            int id = 1;
            foreach (var key in meta) {
                try {
                    foreach (var target in metas[key]) {
                        if (inputData.relation.ContainsKey(target)) {
                            foreach (var relation in inputData.relation[target]) {
                                var newLimitation = new CrossSystemsLimitation();
                                newLimitation.IDLimitation = GetFunction(relation.relation);
                                newLimitation.N = id;
                                newLimitation.GroupTarget = csl_id[target].ToString();
                                string source = CheackIsNode(relation.source, node);
                                newLimitation.GroupSource = csl_id[source].ToString();
                                newLimitation.ConnectionWeight = "2";
                                newLimitation.Description = target.Split(new[] { '|' })[1] + "--->" + relation.source.Split(new[] { '|' })[1];
                                newLimitation.Category = key.Split(new[] { '|' })[1];
                                newLimitation.StrSource = GetStrSource(source, metas);
                                CrossSystemsLimitation_generated.Add(newLimitation);
                                id++;
                            }
                        }
                    }
                    var list_A1 = new List<string>();
                    if (key.Contains("csl_substructure") || key.Contains("csl_structural_frame")) {
                        list_A1.AddRange(metas[key]);
                        foreach (var target in list_A1) {
                            if (!target.Contains("arch")) {
                                foreach (var source in list_A1) {
                                    if (!source.Contains("arch") && !source.Contains("csl_balks")) {
                                        var newLimitation = new CrossSystemsLimitation();
                                        newLimitation.IDLimitation = "A1";
                                        newLimitation.N = id;
                                        newLimitation.GroupTarget = csl_id[target].ToString();
                                        string s = CheackIsNode(source, node);
                                        newLimitation.GroupSource = csl_id[s].ToString();
                                        newLimitation.ConnectionWeight = "2";
                                        newLimitation.Description = target.Split(new[] { '|' })[1] + "--->" + s.Split(new[] { '|' })[1];
                                        newLimitation.Category = key.Split(new[] { '|' })[1];
                                        newLimitation.StrSource = GetStrSource(source, metas);
                                        CrossSystemsLimitation_generated.Add(newLimitation);
                                        id++;
                                    }
                                }
                            }
                            else if (target.Contains("arch") && !target.Contains("concrete")) {
                                foreach (var source in list_A1) {
                                    if (source.Contains("arch") && !source.Contains("concrete") && !source.Contains("csl_balks")) {
                                        var newLimitation = new CrossSystemsLimitation();
                                        newLimitation.IDLimitation = "A1";
                                        newLimitation.N = id;
                                        newLimitation.GroupTarget = csl_id[target].ToString();
                                        string s = CheackIsNode(source, node);
                                        newLimitation.GroupSource = csl_id[s].ToString();
                                        newLimitation.ConnectionWeight = "2";
                                        newLimitation.Description = target.Split(new[] { '|' })[1] + "--->" + s.Split(new[] { '|' })[1];
                                        newLimitation.Category = key.Split(new[] { '|' })[1];
                                        newLimitation.StrSource = GetStrSource(source, metas);
                                        CrossSystemsLimitation_generated.Add(newLimitation);
                                        id++;
                                    }
                                }
                            }
                        }
                    }
                }
                catch {

                }
            }
            GenerateMetaTable(metas, csl_id, nums);
            //GenerAbbreviatedTable(meta, inputData, metas, node, csl_id);
        }
        //private static void GenerAbbreviatedTable(List<string> meta,
        //                                           OntologyInputData inputData,
        //                                           Dictionary<string, List<string>> metas,
        //                                           List<string> node,
        //                                           Dictionary<string, int> csl_id)
        //{
        //    var result = new Dictionary<string, List<RelationFromOntology>>();
        //    var data = new Dictionary<string, Dictionary<string, List<RelationFromOntology>>>();

        //    foreach (var category in meta) {
        //        foreach (var target in metas[category]) {
        //            if (inputData.relation.ContainsKey(target)) {
        //                foreach (var relation in inputData.relation[target]) {
        //                    string s = CheackIsNode(relation.source, node);
        //                    if (!data.ContainsKey(category)) {
        //                        var source = new Dictionary<string, List<RelationFromOntology>>();
        //                        var list_rela = new List<RelationFromOntology>();
        //                        list_rela.Add(relation);
        //                        if (!source.ContainsKey(relation.relation))
        //                            source.Add(relation.relation, list_rela);
        //                        else {
        //                            source = new Dictionary<string, List<RelationFromOntology>>();
        //                            source[relation.relation].Add(relation);
        //                        }
        //                        data.Add(category, source);
        //                    }
        //                    else {
        //                        if (data[category].ContainsKey(relation.relation)) {
        //                            data[category][relation.relation].Add(relation);
        //                        }
        //                        else {
        //                            var source = new Dictionary<string, List<RelationFromOntology>>();
        //                            var list_rela = new List<RelationFromOntology>();
        //                            list_rela.Add(relation);
        //                            data[category].Add(relation.relation, list_rela);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    foreach(var category in data.Keys) {
        //        foreach(var id_limitation in data[category].Keys) {
        //            var limitations = GenerateLimit(data[category][id_limitation], node, csl_id);
        //        }
        //    }
        //}
        #endregion

        #region Utils
        private static string[] CreateMetaData(List<string> meta, List<string> relation)
        {
            string[] nums = new string[meta.Count()];
            foreach (var node in meta) {
                var list = relation.Where(x => x.Contains(node + " has_meta_connection")).ToList();
                nums[list.Count] = node;
            }
            return nums;
        }
        private static string GetStrSource(string source, Dictionary<string, List<string>> metas)
        {
            var result = "";
            var listKeys = new List<string>();
            foreach (var key in metas.Keys) {
                if(key.Contains("csl_substructure") || key.Contains("csl_structural_frame")) {
                    listKeys.Add(key);
                }
            }
            foreach (var value in listKeys) {
                if (metas[value].Contains(source)) {
                    result = "1";
                    return result;
                }
            }
            return result;
        }
        private static string GetFunction(string key)
        {
            string limit_func = "";
            switch (key) {
                case "has_touching_from_above":
                    limit_func = "A1";
                    break;
                case "has_touching_with_side":
                    limit_func = "A2";
                    break;
                case "has_any_touch":
                    limit_func = "A3";
                    break;
                case "has_touching_from_below":
                    limit_func = "A4";
                    break;
                case "has_object_bottom":
                    limit_func = "C1";
                    break;
                case "has_3_object_on_top":
                    limit_func = "C2";
                    break;
                case "has_any_object_in_room":
                    limit_func = "D1";
                    break;
                case "has_over_all":
                    limit_func = "E1";
                    break;
                case "has_leaning_or_touching":
                    limit_func = "A12";
                    break;
            }
            return limit_func;
        }
        private static List<string> GetAllRelations(List<string> node_leaf_CSL, List<string> property)
        {
            var result_list = new List<string>();
            var listSubProperty = new List<string> {
                "has_object_bearing_function",
                "has_location"
            };
            listSubProperty.AddRange(property.Select(i => i.Split(new[] { '|' })[1]));
            var list = new List<string>();
            foreach (var value in node_leaf_CSL) {
                list.AddRange(ProgectData.ontology_classes["edges"].Where(i => i.Contains(value + " ")));
            }
            foreach(var value in listSubProperty) {
                result_list.AddRange(list.Where(i => i.Contains(value)));
            }

            return result_list;
        }
        private static List<string> GetSubProperty(string prorerty)//получить список property n+1
        {
            var result = new List<string>();
            var relation = ProgectData.relations["edges"]
                .Where(i => i.Contains("subproperty_of " + prorerty))
                .Select(i => i.Split(new[] { ' ' })[0]);
            result = relation.ToList();
            return result;
        }
        private static List<string> GetAllSubProperty(string prorerty)//получить все property
        {
            var result = new List<string>();
            var id = ProgectData.relations["nodes"].Where(x => x.Contains(prorerty));
            var list = new List<string>();
            foreach (var value in id) {
                list = GetSubProperty(value);
                result.AddRange(list);
            }
            //var list = GetSubProperty(id.First());
            //result.AddRange(list);
            foreach (var value in list) {
                var list_1 = GetSubProperty(value);
                if (list_1.Count != 0) {
                    result.AddRange(GetAllSubProperty(value));
                }
            }
            return result;
        }
        private static List<string> GetSubClasses(string ontology_class)//получить список классов n+1
        {
            var result = new List<string>();
            if (ontology_class.Contains("|")) {
                var selectItems = ProgectData.ontology_classes["edges"]
                    .Where(i => i.Contains("subclass_of " + ontology_class))
                    .Select(i => i.Split(new[] { ' ' })[0]);

                result = selectItems.ToList();
            }
            else {
                var id = ProgectData.ontology_classes["nodes"]
                    .Where(x => x.Contains(ontology_class))
                    .First();

                var selectItems = ProgectData.ontology_classes["edges"]
                    .Where(i => i.Contains("subclass_of " + id))
                    .Select(i => i.Split(new[] { ' ' })[0]);

                result = selectItems.ToList();
            }
            return result;
        }
        private static List<string> GetAllSubClasses(string ontology_class)//получить все классы
        {
            var result = new List<string>();
            var id = ProgectData.ontology_classes["nodes"].Where(x => x.Contains(ontology_class));
            var list = GetSubClasses(id.First());
            result.AddRange(list);
            foreach (var value in list) {
                var list_1 = GetSubClasses(value);
                if (list_1.Count != 0) {
                    result.AddRange(GetAllSubClasses(value));
                }
            }
            return result;
        }
        private static List<string> GetAllLeafSubClasses(string ontology_class)
        {
            var result = new List<string>();
            var node_leaf_CSL = new List<string>(GetAllSubClasses(ontology_class));
            foreach(var value in node_leaf_CSL) {
                var subClasses = GetSubClasses(value);
                if (subClasses.Count == 0)
                    result.Add(value);
            }
            return result;
        }
        private static List<string> GetAllNodesSubClasses(string ontology_class)
        {
            var result = new List<string>();
            var node_leaf_CSL = new List<string>(GetAllSubClasses(ontology_class));
            foreach (var value in node_leaf_CSL) {
                var subClasses = GetSubClasses(value);
                if (subClasses.Count != 0)
                    result.Add(value);
            }
            return result;
        }
        private static string CheackIsNode(string key, List<string> nodes)
        {
            var result = "";
            var dict_nodes = new Dictionary<string, string>();
            var item_key = key.Split(new[] { '|' })[1];
            foreach(var value in nodes) {
                dict_nodes.Add(value.Split(new[] { '|' })[1], value);
            }
            if (dict_nodes.ContainsKey(item_key))
                result = dict_nodes[item_key];
            return result;
        }
        private static Dictionary<string, List<string>> GenerMetaNodes(List<string> meta)
        {
            var result = new Dictionary<string, List<string>>();
            foreach (var item in meta) {
                var list = new List<string>();
                list.AddRange(GetAllLeafSubClasses(item));
                result.Add(item, list);
            }
            return result;
        }
        private static OntologyInputData CreateInputData(List<string> obj, List<string> relation, List<string> meta, List<string> node, Dictionary<string, List<string>> nodes)
        {
            var result = new OntologyInputData();
            result.ontology_UC = new Dictionary<string, List<UniPairOntology>>();
            result.relation = new Dictionary<string, List<RelationFromOntology>>();
            result.prop = new Dictionary<string, List<string>>();
            var prop = new Dictionary<string, List<string>>();

            var listSubProperty = new List<string> {
                "has_object_bearing_function",
                "has_location"
            };

            foreach (var value in obj) {
                var list_ss = new List<string>();
                list_ss.AddRange(relation.Where(i => i.Contains(value + " has_csl_object")));
                var list_pr = new List<string>();
                list_pr.AddRange(relation.Where(i => i.Contains(value + " has_csl_material")));
                foreach(var item_ss in list_ss) {
                    string ss = item_ss.Split(new[] { '|' })[2];
                    foreach (var item_pr in list_pr) {
                        string pr = item_pr.Split(new[] { '|' })[2];
                        var uniclass = new UniPairOntology() {
                            Ss = ss,
                            Pr = pr
                        };
                        if (result.ontology_UC.ContainsKey(value))
                            result.ontology_UC[value].Add(uniclass);
                        else {
                            var list = new List<UniPairOntology>();
                            list.Add(uniclass);
                            result.ontology_UC.Add(value, list);
                        }
                    }
                }

                //var uniclass = new UniPairOntology() {
                //    Ss = relation
                //    .Where(i => i.Contains(value + " has_csl_object"))
                //    .First()
                //    .Split(new[] { '|' })[2],

                //    Pr = relation
                //    .Where(i => i.Contains(value + " has_csl_material"))
                //    .First()
                //    .Split(new[] { '|' })[2]
                //};
                

                var list_prop = new List<string>();
                list_prop.AddRange(relation.Where(x => x.Contains(value) && listSubProperty.Contains(x.Split(new[] { ' ' })[1])));
                if(list_prop.Count != 0) {
                    prop.Add(value, list_prop);
                    result.prop.Add(value, list_prop);
                }

                var listrel = new List<string>();
                listrel.AddRange(relation.Where(x =>
                x.Contains(value)
                && !x.Contains("subclass_of")
                && !x.Contains("has_meta_connection")
                && !x.Contains("has_csl_object")
                && !x.Contains("has_csl_material")
                && !listSubProperty.Contains(x.Split(new[] { ' ' })[1])));

                if (listrel.Count != 0) {   
                    var result_list = new List<RelationFromOntology>();
                    foreach (var item in listrel) {
                        string source = item.Split(new[] { ' ' })[2];
                        var checkMNode = CheackIsNode(source, node);
                        if (checkMNode != "") {
                            var listSource = nodes[checkMNode];
                            if(listSource.Count != 0) {
                                foreach(var x in listSource) {
                                    var newRelation = new RelationFromOntology();
                                    newRelation.target = value;
                                    if (list_prop.Count != 0) {
                                        newRelation.property = new List<string>();
                                        newRelation.property.AddRange(list_prop);
                                    }
                                    newRelation.source = x;
                                    newRelation.relation = item.Split(new[] { ' ' })[1];
                                    result_list.Add(newRelation);
                                }
                            }
                        }
                        else {
                            var newRelation = new RelationFromOntology();
                            newRelation.target = value;
                            if (list_prop.Count != 0) {
                                newRelation.property = new List<string>();
                                newRelation.property.AddRange(list_prop);
                            }
                            newRelation.source = source;
                            newRelation.relation = item.Split(new[] { ' ' })[1];
                            result_list.Add(newRelation);
                        }
                    }
                    result.relation.Add(value, result_list);
                }
                    


                //if(meta.Contains(value))
                //    result.meta_description.Add(value, value.Split(new[] { '|' })[1]);
                //else {
                //    foreach (var item in meta) {
                //        var list = new List<string>();
                //        list.AddRange(GetAllLeafSubClasses(item));
                //        if (list.Contains(value)) {
                //            result.meta_description.Add(value, item.Split(new[] { '|' })[1]);
                //            break;
                //        }
                //    }
                //}
            }
            return result;
        }
        private static void CreateNewGroup(int id,
           BindingList<UniClassGroup> listUC_group,
           List<string> listSs,
           List<string> listPr,
           List<string> prop,
           string group_description
           )
        {
            string analitic = "";
            string layer = "";
            foreach (var item in prop) {
                if (item.Contains("has_object_bearing_function")) {
                    switch (item.Split(new[] { '|' })[2]) {
                        case "structure":
                            analitic = "1";
                            break;
                        case "non_structure":
                            analitic = "0";
                            break;
                    }
                }
                else if (item.Contains("has_location")) {
                    switch (item.Split(new[] { '|' })[2]) {
                        case "external":
                            layer = "ext";
                            break;
                        case "internal":
                            layer = "int";
                            break;
                    }
                }
            }
            foreach (var pr in listPr) {
                foreach (var ss in listSs) {
                    var ug = new UniClassGroup() {
                        GroupID = id,
                        Exclude = "",
                        SystemsID = ss,
                        ProductID = pr,
                        Function_layer = layer,
                        AnalyticalModel = analitic,
                        GroupDescription_Note = group_description
                    };
                    if (ProgectData.UniSystemTable.ContainsKey(ss) && ProgectData.UniProductTable.ContainsKey(pr))
                        ug.GroupDescription = ProgectData.UniSystemTable[ss] + "---" + ProgectData.UniProductTable[pr];
                    listUC_group.Add(ug);
                }
            }
        }
        //private static List<RelationFromOntology> GenerateLimit(List<RelationFromOntology> data, List<string> node, Dictionary<string, int> csl_id)
        //{
        //    var result = new List<RelationFromOntology>();

        //    var dict = new Dictionary<string, HashSet<string>>();
        //    foreach (var item in data) {
        //        var source = item.source;
        //        var target_list = new List<string>();
        //        var source_list = new List<string>();
        //        target_list.AddRange(data.Where(x => x.source == source).Select(i => i.target));
        //        var heash_target = new HashSet<string>();
        //        var heash_source = new HashSet<string>();
        //        foreach (var value in target_list)
        //            heash_target.Add(value);
        //        var k = CheackIsNode(item.source, node);
        //        if (!dict.ContainsKey(k))
        //            dict.Add(k, heash_target);
        //        else {
        //            foreach (var value in heash_target) {
        //                dict[k].Add(value);
        //            }
        //        }
        //    }
        //    var target_source = new Dictionary<string, HashSet<string>>();
        //    foreach (var source in dict.Keys) {
        //        var target = "";
        //        foreach (var value in dict[source]) {
        //            if (target == "")
        //                target = csl_id[value].ToString();
        //            else
        //                target = target + ", " + csl_id[value].ToString();
        //        }
        //        foreach (var item in dict.Keys) {
        //            if (target_source.ContainsKey(target))
        //                target_source[target].Add(csl_id[item].ToString());
        //            else {
        //                var new_hash = new HashSet<string>();
        //                new_hash.Add(csl_id[item].ToString());
        //                target_source.Add(target, new_hash);
        //            }
        //        }
        //    }
        //    return result;
        //}
        #endregion

        #region таблица Наташе
        public static void GenereteTableObj()//таблица Наташе
        {
            ProgectData.ontology_classes = new Dictionary<string, List<string>>(
                FileUtils.JsonDeserialization<Dictionary<string, List<string>>>(@"Data\ontology_classes.json"));
            ProgectData.relations = new Dictionary<string, List<string>>(
                FileUtils.JsonDeserialization<Dictionary<string, List<string>>>(@"Data\relations.json"));


            var key = ProgectData.ontology_classes["nodes"].Where(x => x.Split(new[] { '|' })[1] == "object");
            var node_leaf = new List<string>(GetAllSubClasses(key.First().ToString()));
            var result = new Dictionary<string, HashSet<string>>();
            var list_sec_extr_data = new List<string>(ProgectData.ontology_classes["edges"].Where(x => x.Contains("has_sec_extractor")));
            foreach (var value in node_leaf) {
                var list = ProgectData.ontology_classes["edges"].Where(x => x.Split(new[] { ' ' })[0] == value && x.Contains(value + " has_extractor_function"));
                if (list.Count() != 0) {
                    foreach (var item in list) {
                        var list_sec_extr = list_sec_extr_data.Where(x => x.Split(new[] { ' ' })[0] == item.Split(new[] { ' ' })[2] && x.Contains(item.Split(new[] { ' ' })[2]));
                        if (list_sec_extr.Count() != 0) {
                            foreach (var sec_extr in list_sec_extr) {
                                if (result.ContainsKey(value)) {
                                    result[value].Add(sec_extr);
                                }
                                else {
                                    var newlist = new HashSet<string>();
                                    newlist.Add(sec_extr);
                                    result.Add(value, newlist);
                                }
                            }
                        }
                    }
                }
            }
            ProgectData.tableExtractor = new List<SecExtractor>();

            foreach (var obj in result.Keys) {
                string s1 = "";
                string s2 = "";
                var newSecextr = new SecExtractor() { item = obj.Split(new[] { '|' })[1] };
                foreach (var data in result[obj]) {
                    if (s1 == "")
                        s1 = data.Split(new[] { '|', ' ' })[1];
                    else
                        s1 = s1 + ", " + data.Split(new[] { '|', ' ' })[1];
                    if (s2 == "")
                        s2 = data.Split(new[] { '|', ' ' })[4];
                    else
                        s2 = s2 + ", " + data.Split(new[] { '|', ' ' })[4];
                }
                newSecextr.type = s1;
                newSecextr.extractor = s2;
                ProgectData.tableExtractor.Add(newSecextr);
            }
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter options and filter index.
            dlg.FileName = "documetn";//по умолчанию имя файла
            dlg.DefaultExt = ".xlsx";//расширение файла по умолчанию
            dlg.Filter = "Microsoft Excel (.xlsx)|*.xlsx";//фильтра разширения
            bool? userClickedOK = dlg.ShowDialog();
            if (userClickedOK == true) {
                FileUtils.SaveExcel(dlg.FileName, "ontology" /*"error"*/);
                MessageBox.Show("Файл сохранен");
            }
        }
        #endregion
    }

}
