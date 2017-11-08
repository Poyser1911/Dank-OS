using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dank_OS
{
    public enum FileAllocationMode
    {
        Sequential,
        Linked,
        Indexed
    }
    public class FileManager
    {
        public const int MaxStorage = 1000;
        public const int SizePerBlock = 10;
        public const int MaxBlocks = MaxStorage / SizePerBlock;
        public List<LinkedList<StorageBlock>> Blocks { get; set; } = new List<LinkedList<StorageBlock>>();
        public List<bool> AvaliableBlocks = new List<bool>();
        public FileAllocationMode AllocationMode = FileAllocationMode.Linked;

        public delegate void AllocationCompelted();
        public event AllocationCompelted OnAllocationCompelted;
        public FileManager()
        {
            for (int i = 0; i < MaxBlocks; i++)
                AvaliableBlocks.Add(false);
        }
        public bool Alloc(Application app)
        {
            bool allocSeq = false;
            bool alloc_complete = false;
            int misses = 0;

            LinkedList<StorageBlock> appblocks = new LinkedList<StorageBlock>();

            double remaining = app.AppStorageSize;
            int actualindex = 0;
            while (true)
            {
                System.Threading.Thread.Sleep(1);
                int index = (new Random(DateTime.Now.Millisecond)).Next(0, MaxBlocks);
                #region Alloc Random
                StorageBlock block;
                if (!allocSeq)
                    if (AvaliableBlocks[index])
                    {
                        misses++;
                        if (misses >= 5)
                            allocSeq = true;
                    }
                    else
                    {
                        block = new StorageBlock(index, actualindex, app);
                        if (remaining > 10)
                            block.Alloc(10);
                        else
                        {
                            block.Alloc(remaining);
                            alloc_complete = true;
                        }
                        appblocks.AddLast(block);
                        AvaliableBlocks[index] = true;
                        if (alloc_complete) break;
                        misses = 0;
                        remaining -= 10;
                        actualindex++;
                    }
                #endregion

                #region Alloc Seq
                if (allocSeq)
                {
                    for (int AvaliableIndex = 0; AvaliableIndex < AvaliableBlocks.Count; AvaliableIndex++)
                    {
                        if (AvaliableBlocks[AvaliableIndex])
                            continue;
                        else
                        {
                            block = new StorageBlock(AvaliableIndex, actualindex, app);
                            if (remaining > 10)
                                block.Alloc(10);
                            else
                            {
                                block.Alloc(remaining);
                                alloc_complete = true;
                            }
                            appblocks.AddLast(block);
                            AvaliableBlocks[AvaliableIndex] = true;
                            if (alloc_complete) break;
                            remaining -= 10;
                            actualindex++;
                        }
                    }
                    break;
                }
                #endregion
            }
            if (alloc_complete)
            {
                Blocks.Add(appblocks);
                OnAllocationCompelted?.Invoke();
            }
            return alloc_complete;
        }

        public bool DeAlloc(Application app)
        {
            if (app.AppStates.HasFlag(AppState.Background))
                return false;
            for (int i = 0; i < Blocks.Count; i++)
            {
                if (Blocks[i].First == null)
                    continue;
                else if (Blocks[i].First.Value.AppData.AppProcess.ProcessID == app.AppProcess.ProcessID)
                {
                    Blocks.RemoveAt(i);
                    OnAllocationCompelted?.Invoke();
                    return true;
                }
            }
            return false;
        }
    }
}
