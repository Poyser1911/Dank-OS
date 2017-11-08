using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dank_OS
{
    public static class FileManagerInstance
    {
        public static FileManager GetInstance { get; set; } = new FileManager();
    }
}
