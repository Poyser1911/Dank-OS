using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dank_OS
{
    public class MemoryManager
    {
        public readonly static int TOTALMEMORY = 1000;
        public readonly static int TOTALSTORAGE = 1000;

        #region Mode
        public MemAllowcationMode AllocationMode { get; set; }

        #endregion

        private static SolidColorBrush ABrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)) { Opacity = 0.0 };
        private static SolidColorBrush BBrush = new SolidColorBrush(Color.FromRgb(0, 255, 255)) { Opacity = 0.6 };
        private static SolidColorBrush CBrush = new SolidColorBrush(Color.FromRgb(255, 255, 0)) { Opacity = 0.6 };

        #region Blocks
        public List<MemoryBlock> Blocks { get; set; } = new List<MemoryBlock>() {
            new MemoryBlock() { Name = "Block A", TotalMemory = 350.0f, BlockColour = ABrush },
            new MemoryBlock() { Name = "Block B", TotalMemory = 400.0f, BlockColour = ABrush },
            new MemoryBlock() { Name = "Block C", TotalMemory = 250.0f, BlockColour = ABrush },
        };

        #endregion

        #region Constructor
        public MemoryManager(MemAllowcationMode mode)
        {
            AllocationMode = mode;
        }
        #endregion

        #region Events
        public delegate void BlockChanged(int BlockIndex, MemoryBlock SourceBlock);
        public event BlockChanged OnBlockChanged;
        #endregion
        public bool Allocate(Application app)
        {
            MemAppData memdata = GetMemAppData(app);
            switch (AllocationMode)
            {
                case MemAllowcationMode.First:
                    #region Alloc First
                    for (int i = 0; i < Blocks.Count; i++)
                        if (Blocks[i].AvaliableMemory >= memdata.MemorySize)
                        {
                            app.MemBlockIndex = i;
                            Blocks[i].Alloc(memdata);
                            OnBlockChanged?.Invoke(i, Blocks[i]);
                            return true;
                        }
                    #endregion

                    break;
                case MemAllowcationMode.Best:
                    break;
                case MemAllowcationMode.Next:
                    break;
                case MemAllowcationMode.Worst:
                    break;
                default:
                    break;
            }
            return false;
        }
        public void DeAllocate(Application app)
        {
            Blocks[app.MemBlockIndex].DeAlloc(app.AppProcess.ProcessID);
            OnBlockChanged?.Invoke(app.MemBlockIndex, Blocks[app.MemBlockIndex]);
        }
        public static MemAppData GetMemAppData(Application app) => new MemAppData() { Name = app.AppName, ProcessID = app.AppProcess.ProcessID, AppProcess = app.AppProcess, MemorySize = app.AppMemorySize, BlockColour = app.BlockColour };
    }
}
