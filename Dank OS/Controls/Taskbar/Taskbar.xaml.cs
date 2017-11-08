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
    /// Interaction logic for Taskbar.xaml
    /// </summary>
    public partial class Taskbar : UserControl
    {
        public delegate void OpenTaskBarItem(int pid);
        public event OpenTaskBarItem TaskBarItemOpened;

        public delegate void CloseTaskBarItem(int pid);
        public event CloseTaskBarItem TaskBarItemClosed;

        public Taskbar()
        {
            InitializeComponent();
        }
        public void AddTaskBarItem(Application appItem)
        {
            TaskbarItem item = new TaskbarItem() { ProcessID = appItem.AppProcess.ProcessID, IconSource = appItem.AppIcon };
            item.MouseLeftButtonUp += (s, e) => TaskBarItemOpened?.Invoke(appItem.AppProcess.ProcessID);
            item.ContextMenu = new ContextMenu() { Items = { new MenuItem() { Header = "Close Window",Icon = new System.Windows.Controls.Image{ Source = new BitmapImage(new Uri("pack://application:,,,/Dank OS;component/Controls/Desktop/Icons/close.ico"))} } } };
            ((MenuItem)item.ContextMenu.Items[0]).Click += (s, e) => TaskBarItemClosed?.Invoke(item.ProcessID);
            item.ToolTip = appItem.AppName;
            TaskBarStack.Children.Add(item);
        }

        public void RemoveTaskBarItem(int ProcessID)
        {
            for (int i = 0; i < TaskBarStack.Children.Count; i++)
                if ((TaskBarStack.Children[i] as TaskbarItem).ProcessID == ProcessID)
                {
                    TaskBarStack.Children.RemoveAt(i);
                    break;
                }
        }
    }
}
