using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace Dank_OS
{
    public class ApplicationManager
    {

        #region Private
        private int _pidcounter = 0;
        private List<Application> _apps = new List<Application>();
        #endregion

        #region Public Properties
        public List<Application> Apps
        {
            get
            {
                return _apps;
            }
            set
            {
                _apps = value;
                //OnChanged?.Invoke();
            }
        }
        public MemoryManager mManager { get; set; } = MemoryManagerService.DefaultInstance;

        public FileManager FManager { get; set; } = FileManagerInstance.GetInstance;
        #endregion

        #region Events
        public delegate void AppAction(Application app);
        public event AppAction OnProcessStarted;
        public event AppAction OnBackGroundProcessStarted;
        public event AppAction OnAppClearUp;


        public delegate void ProcessResume(Application app);
        public event ProcessResume OnProcessResume;
        #endregion

        #region Actions
        public void AddProcess(Application app, bool IsBackGroundProcess = false)
        {
            app.AppProcess = new Process(_pidcounter);
            if (!mManager.Allocate(app))
            {
                System.Windows.MessageBox.Show($"Could not allocate enough memory for that.\nAlloation Scheme: {mManager.AllocationMode.ToString()}", "OUT OF MEMORY", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            app.AppWindowLogic.AppData = app;
            Apps.Add(app);
            if (IsBackGroundProcess)
                OnBackGroundProcessStarted?.Invoke(app);
            else
                OnProcessStarted?.Invoke(app);
            _pidcounter++;
        }

        public void AllocToStorage(Application app)
        {
            if (!FManager.Alloc(app))
                System.Windows.MessageBox.Show($"Could not allocate enough Disk Space for that.\nAlloation Scheme: {FManager.AllocationMode.ToString()}", "NO FREE Disk Space", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
        private Application GetAppProcessByID(int ProcessID)
        {
            foreach (Application p in _apps)
                if (p.AppProcess.ProcessID == ProcessID)
                    return p;
            return null;
        }
        public void AppClearUp(Application app) => OnAppClearUp?.Invoke(app);
        public void ResumeProcess(int ProcessID) => OnProcessResume?.Invoke(GetAppProcessByID(ProcessID));
        public void EndAppProcess(int ProcessID = 0, Application app = null)
        {
            if (app == null)
                app = GetAppProcessByID(ProcessID);

            if (app.AppStates.HasFlag(AppState.Background))
            {
                app.AppStates ^= AppState.Active;
                return;
            }
            app.AppProcess.Stop();
            AppendProcessHistory(app);
            mManager.DeAllocate(app);
            int index = _apps.IndexOf(app);
            _apps.RemoveAt(index);
        }

        private void AppendProcessHistory(Application app)
        {
            File.AppendAllText("ProcessHistory.txt", $"{app.AppProcess.ProcessID} {app.AppName} {(app.AppStates.HasFlag(AppState.Background) ? "System" : "User")} {app.AppProcess.StartTime.ToLongDateString()} {app.AppProcess.StartTime.ToLongTimeString()}  {app.AppProcess.EndTime.ToLongDateString()} {app.AppProcess.EndTime.ToLongTimeString()} {app.AppProcess.CPUTime}\n");
        } 
        #endregion
    }
}
