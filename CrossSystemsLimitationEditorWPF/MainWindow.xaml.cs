using CrossSystemsLimitationEditorWPF.Logic;
using CrossSystemsLimitationEditorWPF.Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrossSystemsLimitationEditorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string jsonInputPatch_Ss = @"Data\UniSystemsTable.json";
            string jsonInputPatch_Pr = @"Data\UniProductTable.json";
            ProgectData.UniSystemTable = new Dictionary<string, string>();
            ProgectData.UniSystemTable = FileUtils.JsonDeserialization<Dictionary<string, string>>(jsonInputPatch_Ss);
            ProgectData.UniProductTable = new Dictionary<string, string>();
            ProgectData.UniProductTable = FileUtils.JsonDeserialization<Dictionary<string, string>>(jsonInputPatch_Pr);

        }
        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = sender as TabControl; // e.Source could have been used instead of sender as well
            TabItem item = tabControl.SelectedValue as TabItem;

            if (item != null)
            {
                switch (item.Name)
                {
                    case "CrossSystemsLimitations":

                        break;
                    case "UniClassGroups":
                       
                        break;
                    case "Limitation functions":

                        break;
                    case "GlobalGroup":

                        break;
                    case "UniSystem_v_1_8":
                        //await SetSsTable(ProgectData.UniSystemTable);
                        break;
                    case "UniProduct_v_1_8":
                        //await SetPrTable(ProgectData.UniProductTable);
                        break;
                    case "GroupsSs":

                        break;
                    case "GroupsPr":

                        break;
                }
            }
        }
        private async void LoadExcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            bool isload = false;
            // Set filter options and filter index.
            dlg.FileName = "documetn";//по умолчанию имя файла
            dlg.DefaultExt = ".xlsx";//расширение файла по умолчанию
            dlg.Filter = "Microsoft Excel (.xlsx)|*.xlsx";//фильтра разширения
            bool? userClickedOK = dlg.ShowDialog();
            if (userClickedOK == true)
            {
                try
                {
                    await FileUtils.LoadExcelConfig(dlg.FileName, "limitation");
                    dataGridLimitation.ItemsSource = ProgectData.CrossSystemsLimitation_data;
                    dataGridLimitation.GroupColumnDescriptions.Clear();
                    dataGridLimitation.GroupColumnDescriptions.Add(new Syncfusion.UI.Xaml.Grid.GroupColumnDescription() { ColumnName = "Category" });
                    dataGridLimitation.Columns[0].Width = 45;
                    dataGridLimitation.Columns[1].Width = 100;
                    dataGridLimitation.Columns[2].Width = 100;
                    dataGridLimitation.Columns[3].Width = 200;
                    dataGridLimitation.Columns[4].Width = 100;
                    dataGridLimitation.Columns[5].Width = 100;
                    dataGridLimitation.Columns[6].Width = 200;
                    dataGridLimitation.Columns[8].Width = 45;
                    dataGridLimitation.Columns[9].Width = 45;
                    dataGridLimitation.Columns[10].Width = 45;
                    dataGridLimitation.Columns[11].Width = 45;
                    dataGridLimitation.Columns[12].Width = 150;


                    dataGridUniclassGroups.ItemsSource = ProgectData.UniClassGroup_data;
                    dataGridUniclassGroups.GroupColumnDescriptions.Clear();
                    dataGridUniclassGroups.GroupColumnDescriptions.Add(new Syncfusion.UI.Xaml.Grid.GroupColumnDescription() { ColumnName = "GroupID" });
                    //dataGridUniclassGroups.Columns[0].Width = 45;
                    //dataGridUniclassGroups.Columns[1].Width = 70;
                    //dataGridUniclassGroups.Columns[2].Width = 100;
                    //dataGridUniclassGroups.Columns[3].Width = 100;
                    //dataGridUniclassGroups.Columns[5].Width = 45;
                    //dataGridUniclassGroups.Columns[6].Width = 45;
                    //dataGridUniclassGroups.Columns[7].Width = 300;
                    RunTests.IsEnabled = true;
                    SaveExcel.IsEnabled = true;

                    isload = true;

                    MessageBox.Show("Файл загружен");
                }
                catch
                {
                    if (isload == false)
                        MessageBox.Show("Невозможно открыть файл");
                }
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private async void RunTests_Click(object sender, RoutedEventArgs e)
        {
            result_t1.Fill = new SolidColorBrush(Color.FromRgb(70, 77, 255));
            result_t2.Fill = new SolidColorBrush(Color.FromRgb(70, 77, 255));
            result_t3.Fill = new SolidColorBrush(Color.FromRgb(70, 77, 255));
            Progress_label.Content = "Тесты запущены...";
            Utils.getUniPair();
            await Tests.TestsOptions.LoadTestsConfig("all");
            ShowResult.IsEnabled = true;
            RefreshStatusError();
            MessageBox.Show("Тесты завершены");
        }
        private async void CorrectErrors_button_Click(object sender, RoutedEventArgs e)
        {
            //CorrectErrors.CorrectErrorUcGroups(TestData.erroListUniClassGroups, TestData.uniPair);
            //Utils.getUniPair();
            CorrectErrors.CorrectError(TestData.erroListUniClassGroups, TestData.uniPair, "UC_Groups");
            Utils.getUniPair();
            await Tests.TestsOptions.LoadTestsConfig("all");
            RefreshStatusError();
            CorrectErrors.CorrectError(TestData.errorListMetaGroups, TestData.uniPair, "MetaGroups");
            Utils.getUniPair();
            await Tests.TestsOptions.LoadTestsConfig("all");
            RefreshStatusError();
        }
        private async void SaveExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            // Set filter options and filter index.
            dlg.FileName = "documetn";//по умолчанию имя файла
            dlg.DefaultExt = ".xlsx";//расширение файла по умолчанию
            dlg.Filter = "Microsoft Excel (.xlsx)|*.xlsx";//фильтра разширения
            bool? userClickedOK = dlg.ShowDialog();
            if (userClickedOK == true)
            {
                await FileUtils.SaveExcel(dlg.FileName, "full" /*"error"*/);
                MessageBox.Show("Файл сохранен");
            }
        }
        private async void ReloadOntology_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            bool isload = false;
            // Set filter options and filter index.
            dlg.FileName = "documetn";//по умолчанию имя файла
            dlg.DefaultExt = ".xlsx";//расширение файла по умолчанию
            dlg.Filter = "Microsoft Excel (.xlsx)|*.xlsx";//фильтра разширения
            bool? userClickedOK = dlg.ShowDialog();
            if (userClickedOK == true)
            {
                await FileUtils.LoadExcelConfig(dlg.FileName, "ontology");
                MessageBox.Show("Ss+Pr из онтологии обновлены");
            }
        }
        private async void LoadOntology_Click(object sender, RoutedEventArgs e)
        {
            await OntologyUtils.ReadOntology();
        }
        private async void Generate_CSL_Click(object sender, RoutedEventArgs e)
        {
            await GenerateCSL.GenerateCrossSystemsLimitations();
            dataGridLimitation.ItemsSource = ProgectData.CrossSystemsLimitation_data;
            dataGridLimitation.GroupColumnDescriptions.Clear();
            dataGridLimitation.GroupColumnDescriptions.Add(new Syncfusion.UI.Xaml.Grid.GroupColumnDescription() { ColumnName = "Category" });
            dataGridUniclassGroups.ItemsSource = ProgectData.UniClassGroup_data;
            dataGridUniclassGroups.GroupColumnDescriptions.Clear();
            dataGridUniclassGroups.GroupColumnDescriptions.Add(new Syncfusion.UI.Xaml.Grid.GroupColumnDescription() { ColumnName = "GroupID" });

            //GenerateCSL.GenereteTableObj();
        }
        private async void RefreshStatusError()
        {
            int errorsCount = 0;
            if (TestData.errorListMetaGroups.Count != 0) {
                error_t1.Visibility = Visibility.Visible;
                result_t1.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                error_t1.ToolTip = "Кол-во ошибок " + ProgectData.meta_error /*TestData.errorListMetaGroups.Count*/;
                CorrectErrors_button.IsEnabled = true;
                errorsCount = errorsCount + ProgectData.meta_error;
            }
            else if (TestData.errorListMetaGroups.Count == 0) {
                result_t1.Fill = new SolidColorBrush(Color.FromRgb(17, 142, 5));
                error_t1.Visibility = Visibility.Hidden;
            }

            if (TestData.erroListOntology.Count != 0) {
                error_t2.Visibility = Visibility.Visible;
                result_t2.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                error_t2.ToolTip = "Кол-во ошибок " + TestData.erroListOntology.Count;
                CorrectErrors_button.IsEnabled = true;
                errorsCount = errorsCount + TestData.erroListOntology.Count;
            }
            else if (TestData.erroListOntology.Count == 0) {
                result_t2.Fill = new SolidColorBrush(Color.FromRgb(17, 142, 5));
                error_t2.Visibility = Visibility.Hidden;
            }

            if (TestData.erroListUniClassGroups.Count != 0) {
                error_t3.Visibility = Visibility.Visible;
                result_t3.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                error_t3.ToolTip = "Кол-во ошибок " + ProgectData.uc_error /*TestData.erroListUniClassGroups.Count*/;
                CorrectErrors_button.IsEnabled = true;
                errorsCount = errorsCount + ProgectData.uc_error;
            }
            else if (TestData.erroListOntology.Count == 0) {
                result_t3.Fill = new SolidColorBrush(Color.FromRgb(17, 142, 5));
                error_t3.Visibility = Visibility.Hidden;
            }

            if (errorsCount == 0)
                Progress_label.Content = "Ошибок нет";
            else
                Progress_label.Content = "Кол-во ошибок: " + errorsCount;
        }
    }
}
