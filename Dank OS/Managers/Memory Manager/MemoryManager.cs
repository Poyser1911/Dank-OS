using System.Collections.Generic;

namespace Dank_OS
{
    public class MemoryManager
    {
        public static readonly int Totalmemory = 1000;
        public static readonly int Totalstorage = 1000;

        public static MemoryManager GetInstance { get; set; } = new MemoryManager(MemAllowcationMode.First);
        #region Mode
        public MemAllowcationMode AllocationMode { get; set; }

        #endregion


        #region Blocks
        public List<MemoryBlock> Blocks { get; set; } = new List<MemoryBlock>() {
            new MemoryBlock() { Name = "Block A", TotalMemory = 350.0f },
            new MemoryBlock() { Name = "Block B", TotalMemory = 400.0f },
            new MemoryBlock() { Name = "Block C", TotalMemory = 250.0f },
        };

        #endregion

        #region Constructor
        public MemoryManager(MemAllowcationMode mode)
        {
            AllocationMode = mode;
        }
        #endregion

        #region Events
        public delegate void BlockChanged(int blockIndex, MemoryBlock sourceBlock);
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
        public static MemAppData GetMemAppData(Application app) => new MemAppData() { Name = app.AppName, ProcessId = app.AppProcess.ProcessID, AppProcess = app.AppProcess, MemorySize = app.AppMemorySize, BlockColour = app.BlockColour };
    }
}
