using System;
using System.Collections.Generic;

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
            bool allocComplete = false;
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
                            allocComplete = true;
                        }
                        appblocks.AddLast(block);
                        AvaliableBlocks[index] = true;
                        if (allocComplete) break;
                        misses = 0;
                        remaining -= 10;
                        actualindex++;
                    }
                #endregion

                #region Alloc Seq
                if (allocSeq)
                {
                    for (int avaliableIndex = 0; avaliableIndex < AvaliableBlocks.Count; avaliableIndex++)
                    {
                        if (AvaliableBlocks[avaliableIndex])
                            continue;
                        else
                        {
                            block = new StorageBlock(avaliableIndex, actualindex, app);
                            if (remaining > 10)
                                block.Alloc(10);
                            else
                            {
                                block.Alloc(remaining);
                                allocComplete = true;
                            }
                            appblocks.AddLast(block);
                            AvaliableBlocks[avaliableIndex] = true;
                            if (allocComplete) break;
                            remaining -= 10;
                            actualindex++;
                        }
                    }
                    break;
                }
                #endregion
            }
            if (allocComplete)
            {
                Blocks.Add(appblocks);
                OnAllocationCompelted?.Invoke();
            }
            return allocComplete;
        }

        public bool DeAlloc(Application app)
        {
            if (app.IsSystemApp)
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
