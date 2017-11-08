using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dank_OS
{
    public static class MemoryManagerService
    {
        public static MemoryManager DefaultInstance { get; set; } = new MemoryManager(MemAllowcationMode.First);

        public static MemoryManager GetInstance(MemAllowcationMode mode) => new MemoryManager(mode);
    }
}
