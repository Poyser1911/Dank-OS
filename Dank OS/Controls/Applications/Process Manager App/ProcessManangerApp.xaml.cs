using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dank_OS
{
    /// <summary>
    /// Interaction logic for ProcessManangerApp.xaml
    /// </summary>
    public partial class ProcessManangerApp : AppWindowBase
    {
        public ApplicationManager AppM { get; set; } = ApplicationManagerInstance.GetInstance;
        public ProcessManangerApp()
        {
            InitializeComponent();

            Monitor();
            EndProcess.Click += (s,e) => EndSelectedProcess();
        }

        private void Monitor()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        List<Application> apps = AppM.Apps.ToList();
                        Dispatcher.Invoke(new Action(() => ProcessGrid.ItemsSource = apps));
                    }
                    catch (Exception) { }
                    System.Threading.Thread.Sleep(1000);

                }
            });
        }

        private void EndSelectedProcess()
        {
            if (ProcessGrid.SelectedIndex < 0)
                return;
            Application item = ProcessGrid.SelectedItem as Application;
            if (item.IsSystemApp)
                return;

            AppM.AppClearUp(item);
            AppM.EndAppProcess(app: item);
        }
    }
}
