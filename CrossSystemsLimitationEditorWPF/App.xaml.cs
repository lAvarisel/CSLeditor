using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CrossSystemsLimitationEditorWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string OutputDirectory { get; } = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjc2NkAzMTM2MmUzMjJlMzBNS0k4NHNMaUJSWkphWHh2Z3pBQS9tR0VLU0c4aWo4WUUyRTBEOG1TZG1JPQ==;Mjc2N0AzMTM2MmUzMjJlMzBIeTcyYjdIM0pWMlBvdHIrK3FKQ216RzJ4M0xKYmJmZThKWDlES3dmQlBrPQ==");
        }
    }
}
