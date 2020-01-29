using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Builder;

namespace CrossSystemsLimitationEditorWPF.Logic
{
    class OntologyUtils
    {
        public static async Task ReadOntology()
        {
            string sMessageBoxText = "Обновить правила формирования Limitations из онтологии?";
            string sCaption = "CrossSystemsLimitationEditor";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox) {
                case MessageBoxResult.Yes:
                    try {
                        WebClient client = new WebClient();
                        client.Headers.Set("Authorization", "Basic a3JlbzprcmVv");
                        client.DownloadFile("http://192.168.7.5:9000/internal/ontology/ontology_classes/", "ontology_classes.json");
                        client.DownloadFile("http://192.168.7.5:9000/internal/ontology/relations/", "relations.json");
                        MessageBox.Show("Данные загружены успешно");
                    }
                    catch {
                        MessageBox.Show("Ошибка при загрузке");
                    }
                    break;

                case MessageBoxResult.No:
                    MessageBox.Show("Правила не обновлены, версия текущего файла от " + File.GetLastWriteTime(@"Data\ontology_classes.json"));
                    break;
            }
        }
    }
}
