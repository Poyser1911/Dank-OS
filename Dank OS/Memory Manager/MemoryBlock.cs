using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dank_OS
{
    public class MemAppData
    {
        public int ProcessID { get; set; }

        public string Name { get; set; }
        public double MemorySize { get; set; }
        public double BlockPercentageUseage { get; set; }

        public SolidColorBrush BlockColour { get; set; }
        public double TotalPercentageUseage { get; set; }

        public Process AppProcess { get; set; }

    }

    public class MemoryBlock
    {
        public string Name { get; set; }
        public double AvaliableMemory { get; set; }
        public SolidColorBrush BlockColour { get; set; }
        private double _totalmemory;
        public double TotalMemory
        {
            get
            {
                return _totalmemory;
            }
            set
            {
                AvaliableMemory = value;
                _totalmemory = value;
            }
        }
        public List<MemAppData> apps { get; set; } = new List<MemAppData>();

        public void Alloc(MemAppData app)
        {
            app.BlockPercentageUseage = (app.MemorySize * 100) / _totalmemory;
            app.TotalPercentageUseage = (app.MemorySize * 100) / MemoryManager.TOTALMEMORY;
            apps.Add(app);
            AvaliableMemory -= app.MemorySize;
        }
        public void DeAlloc(int ProcessID)
        {
            for (int i = 0; i < apps.Count; i++)
                if (apps[i].ProcessID == ProcessID)
                {
                    AvaliableMemory += apps[i].MemorySize;
                    apps.RemoveAt(i);
                }
        }
    }
}
