using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dank_OS.Controls.Applications.Base
{
    [Flags]
    public enum AppWindowState
    {
        None = 0,
        Default = 1,
        Minimize = 2,
        Maximize = 4,
        CloseRequest = 8,
    }
}
