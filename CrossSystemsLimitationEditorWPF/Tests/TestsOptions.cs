using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossSystemsLimitationEditorWPF.Tests
{
    class TestsOptions
    {
        public static async Task LoadTestsConfig(string test)
        {
            if (!System.IO.Directory.Exists(App.OutputDirectory))
                System.IO.Directory.CreateDirectory(App.OutputDirectory);
            switch (test) {
                case "all":
                    var Tasks = new List<Task>();
                    var t1 = Task.Run(() => TestMetaGroups.Test_2());
                    Tasks.Add(t1);
                    var t2 = Task.Run(() => TestOntology.Test_1());
                    Tasks.Add(t2);
                    var t3 = Task.Run(() => TestUniClass.Test_1());
                    Tasks.Add(t3);
                    //Task t2 = Task.Run(() => TestOntology.Test_Mace());
                    //Tasks.Add(t2);
                    await Task.WhenAll(Tasks.ToArray());
                    break;
                case "meta":
                    Tasks = new List<Task>();
                    t1 = Task.Run(() => TestMetaGroups.Test_2());
                    Tasks.Add(t1);
                    await Task.WhenAll(Tasks.ToArray());
                    break;
                case "uc_groups":
                    Tasks = new List<Task>();
                    t1 = Task.Run(() => TestUniClass.Test_1());
                    Tasks.Add(t1);
                    await Task.WhenAll(Tasks.ToArray());
                    break;
            }
        }
    }
}
