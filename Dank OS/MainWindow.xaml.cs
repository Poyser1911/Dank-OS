using System;
using System.Windows;
using System.Windows.Controls;
using static Dank_OS.Controls.Applications.Base.AppWindowState;

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
            InitializeOs();
        }
        /// <summary>
        /// Setup and Monitor OS Operations
        /// </summary>
        private void InitializeOs()
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
            _appM.OnAppClearUp += (app) => AppTaskbar.RemoveTaskBarItem(app.AppProcess.ProcessID);
            _appM.OnProcessResume += _appM_OnProcessResume;


            AppTaskbar.TaskBarItemOpened += (pid) => _appM.ResumeProcess(pid);
            AppTaskbar.TaskBarItemClosed += CloseApp;

            memory.OnAppLaunchRequest += (app) => _appM.ResumeProcess(0);
            process.OnAppLaunchRequest += (app) => _appM.ResumeProcess(1);
            disk.OnAppLaunchRequest += (app) => _appM.ResumeProcess(2);

            tick.OnAppLaunchRequest += (app) => _appM.AddProcess(Applications.TicTacToeApp);
        }

        private void _appM_OnProcessResume(Application app)
        {
            if (app.IsSystemApp && app.AppWindowLogic.WindowState == None)
            {
                AppTaskbar.AddTaskBarItem(app);
                app.AppWindowLogic.WindowState = Default;
            }
            app.AppWindowLogic.WindowState &= ~Minimize;

            //Display App Refernece Window
            ShowWindow(app);
        }

        private void AppM_OnProcessStarted(Application app)
        {
            if (!app.IsSystemApp)
            {
                AppTaskbar.AddTaskBarItem(app);
                app.AppWindowLogic.WindowState = Default;
                ShowWindow(app);
            }
            app.AppWindowLogic.OnWindowAction += AppWindowLogic_OnWindowAction;

        }

        private void AppWindowLogic_OnWindowAction(Application app)
        {
            if (app.AppWindowLogic.WindowState == CloseRequest)
                CloseApp(app);
            else
                ShowWindow(app);
        }

        private void CloseApp(Application app)
        {
            AppView.Children.Clear();
            AppTaskbar.RemoveTaskBarItem(app.AppProcess.ProcessID);
            if (app.IsSystemApp)
                app.AppWindowLogic.WindowState = None;
            else
                _appM.EndAppProcess(app.AppProcess.ProcessID);
        }
        private void ShowWindow(Application app)
        {

            if (AppView.Children.Count != 0)
            {
                AppWindowBase child = (AppView.Children[0] as AppWindowBase);
                if (child.AppData.AppProcess.ProcessID != app.AppProcess.ProcessID)
                    child.WindowState |= Minimize;
            }
            switch (app.AppWindowLogic.WindowState)
            {
                case Default:
                    SetDefaultAppView();
                    break;
                case Maximize:
                    SetMaximizedAppView();
                    break;
            }
            AppView.Children.Clear();
            if (!app.AppWindowLogic.WindowState.HasFlag(Minimize))
                AppView.Children.Add(app.AppWindowLogic);
        }

        private void SetDefaultAppView()
        {
            Grid.SetColumn(AppView, 1);
            Grid.SetRow(AppView, 1);
            Grid.SetRowSpan(AppView, 4);
            Grid.SetColumnSpan(AppView, 1);
        }
        private void SetMaximizedAppView()
        {
            Grid.SetColumn(AppView, 0);
            Grid.SetRow(AppView, 0);
            Grid.SetRowSpan(AppView, 6);
            Grid.SetColumnSpan(AppView, 3);
        }
    }
}