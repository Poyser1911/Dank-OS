using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Dank_OS
{
    /// <summary>
    /// Interaction logic for Taskbar.xaml
    /// </summary>
    public partial class Taskbar : UserControl
    {
        public delegate void OpenTaskBarItem(int pid);
        public event OpenTaskBarItem TaskBarItemOpened;

        public delegate void CloseTaskBarItem(Application app);
        public event CloseTaskBarItem TaskBarItemClosed;

        public Taskbar()
        {
            InitializeComponent();
        }
        public void AddTaskBarItem(Application appItem)
        {
            TaskbarItem item = new TaskbarItem() { ProcessId = appItem.AppProcess.ProcessID, IconSource = appItem.AppIcon };
            item.MouseLeftButtonUp += (s, e) => TaskBarItemOpened?.Invoke(appItem.AppProcess.ProcessID);
            item.ContextMenu = new ContextMenu() { Items = { new MenuItem() { Header = "Close Window",Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Dank OS;component/Controls/Desktop/Icons/close.ico")), Height = 20} } } };
            ((MenuItem)item.ContextMenu.Items[0]).Click += (s, e) => TaskBarItemClosed?.Invoke(appItem);
            item.ToolTip = appItem.AppName;
            TaskBarStack.Children.Add(item);
        }

        public void RemoveTaskBarItem(int processId)
        {
            for (int i = 0; i < TaskBarStack.Children.Count; i++)
                if ((TaskBarStack.Children[i] as TaskbarItem).ProcessId == processId)
                {
                    TaskBarStack.Children.RemoveAt(i);
                    break;
                }
        }
    }
}
