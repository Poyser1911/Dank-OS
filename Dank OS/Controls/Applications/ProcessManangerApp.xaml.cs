using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;

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
            datagrid.ContextMenu = new ContextMenu();
            MenuItem info = new MenuItem();
            info.Header = "End Process";
            info.Click += (s, e) => EndSelectedProcess();
            datagrid.ContextMenu.Items.Add(info);
        }

        private void Monitor()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        List<Application> _apps = AppM.Apps.ToList();
                        Dispatcher.Invoke(new Action(() => datagrid.ItemsSource = _apps));
                    }
                    catch (Exception) { }
                    System.Threading.Thread.Sleep(300);

                }
            });
        }

        private void AppM_OnChanged(List<Application> apps)
        {
            List<Application> _apps = apps.ToList();
            datagrid.ItemsSource = _apps;
            //mmPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("appM"));
        }
        private void EndSelectedProcess()
        {
            if (datagrid.SelectedIndex < 0)
                return;
            Application item = datagrid.SelectedItem as Application;
            if (!item.AppStates.HasFlag(AppState.Background))
            {
                AppM.AppClearUp(item);
                AppM.EndAppProcess(app: item);
            }
        }
    }
}
