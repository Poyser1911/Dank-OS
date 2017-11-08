using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dank_OS
{
    [Flags]
    public enum AppState
    {
        None = 0,
        Active = 1,
        Background = 2
    }
    public class Application
    {
        private string _appicon;
        public string AppName { get; set; }
        public string AppIcon
        {
            get
            {
                return "/Dank OS;component/Controls/Desktop/Icons/" + _appicon;
            }
            set
            {
                _appicon = value;
            }
        }
        public double AppMemorySize { get; set; }
        public double AppStorageSize { get; set; }
        public int MemBlockIndex { get; set; }
        public AppState AppStates { get; set; }
        public AppWindowBase AppWindowLogic { get; set; }
        public Process AppProcess { get; set; }

        public SolidColorBrush BlockColour { get; set; }

        public Application()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            System.Threading.Thread.Sleep(5);
            SolidColorBrush g = new SolidColorBrush(Color.FromRgb((byte)r.Next(0, 256), (byte)r.Next(0, 256), (byte)r.Next(0, 256)));
            BlockColour = g;
        }
    }
}
