using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dank_OS
{
    public static class ApplicationManagerInstance
    {
        public static ApplicationManager GetInstance { get; set; } = new ApplicationManager();
    }
}
