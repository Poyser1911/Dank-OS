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

namespace Dank_OS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ApplicationManager _appM = ApplicationManagerInstance.GetInstance;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeOS();
        }
        /// <summary>
        /// Setup and Monitor OS Operations
        /// </summary>
        private void InitializeOS()
        {
            System.IO.File.WriteAllText("ProcessHistory.txt", "");
            AllocAppStorages();
            RegisterEvents();
            AllocAppMemory();
        }
        /// <summary>
        /// Auto Starts background processes
        /// </summary>
        private void AllocAppMemory()
        {
            _appM.AddProcess(memory.AppData, true);
            _appM.AddProcess(process.AppData, true);
            _appM.AddProcess(disk.AppData, true);
        }
        /// <summary>
        /// Allocates App Storages to Disk
        /// </summary>
        private void AllocAppStorages()
        {
            _appM.AllocToStorage(memory.AppData);
            _appM.AllocToStorage(process.AppData);
            _appM.AllocToStorage(disk.AppData);
            _appM.AllocToStorage(Applications.TicTacToeApp);
        }
        /// <summary>
        /// Register Callbacks on Processs Start, Resume, TaskBar Items and Desktop Icon Clicks/Request
        /// </summary>
        private void RegisterEvents()
        {
            _appM.OnProcessStarted += AppM_OnProcessStarted;
            _appM.OnBackGroundProcessStarted += AppM_OnBackGroundProcessStarted;
            _appM.OnAppClearUp += (app) => AppTaskbar.RemoveTaskBarItem(app.AppProcess.ProcessID);
            _appM.OnProcessResume += _appM_OnProcessResume;


            AppTaskbar.TaskBarItemOpened += (pid) => _appM.ResumeProcess(pid);
            AppTaskbar.TaskBarItemClosed += (pid) => AppWindowLogic_OnAppClose(pid);

            memory.OnAppLaunchRequest += (app) => _appM.ResumeProcess(0);
            process.OnAppLaunchRequest += (app) => _appM.ResumeProcess(1);
            disk.OnAppLaunchRequest += (app) => _appM.ResumeProcess(2);

            tick.OnAppLaunchRequest += (app) => _appM.AddProcess(Applications.TicTacToeApp);
        }

        private void _appM_OnProcessResume(Application app)
        {
            //Check if System App Being Launched and Attach attach Active State
            if (app.AppStates == AppState.Background)
            {
                app.AppStates |= AppState.Active;
                AppTaskbar.AddTaskBarItem(app);
            }
            //Display App Refernece Window
            ShowWindow(app);
        }

        private void AppM_OnProcessStarted(Application app)
        {
            AppTaskbar.AddTaskBarItem(app);
            ShowWindow(app);
            app.AppWindowLogic.OnAppClose += AppWindowLogic_OnAppClose;
        }
        private void AppM_OnBackGroundProcessStarted(Application app)
        {
            app.AppStates = AppState.Background;
            app.AppWindowLogic.OnAppClose += (pid) =>
            {
                AppView.Children.Clear();
                AppTaskbar.RemoveTaskBarItem(pid);
                app.AppStates ^= AppState.Active;
            };
        }
        private void AppWindowLogic_OnAppClose(int ProcessID)
        {
            AppView.Children.Clear();
            _appM.EndAppProcess(ProcessID);
            AppTaskbar.RemoveTaskBarItem(ProcessID);
        }

        private void ShowWindow(Application app)
        {
            if (AppView.Children.Count != 0)
                if ((AppView.Children[0] as AppWindowBase).AppData.AppProcess.ProcessID == app.AppProcess.ProcessID)
                    return;
            AppView.Children.Clear();
            AppView.Children.Add(app.AppWindowLogic);
        }
    }
}