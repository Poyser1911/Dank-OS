using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dank_OS
{
    /// <summary>
    /// Interaction logic for FileManagerApp.xaml
    /// </summary>
    public partial class FileManagerApp : AppWindowBase
    {
        public FileManager FManager { get; set; } = FileManagerInstance.GetInstance;
        public FileManagerApp()
        {
            InitializeComponent();
            FManager.OnAllocationCompelted += () => RenderFileManagerBlocks();
        }

        public void RenderFileManagerBlocks()
        {
            DiskView.Children.Clear();
            foreach (LinkedList<StorageBlock> list in FManager.Blocks)
            {
                var temp = list.First;
                do
                {
                    temp.Value.RenderBlock();
                    int row = temp.Value.BlockIndex/ 10;
                    int col = temp.Value.BlockIndex % 10;
                    Grid.SetColumn(temp.Value, col);
                    Grid.SetRow(temp.Value, row);
                    DiskView.Children.Add(temp.Value);
                    temp = temp.Next;
                } while (temp != null);
            }
        }
    }
}
