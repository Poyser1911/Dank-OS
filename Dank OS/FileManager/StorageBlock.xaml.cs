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
    /// Interaction logic for StorageBlock.xaml
    /// </summary>
    public partial class StorageBlock : UserControl
    {
        public Application AppData { get; set; }
        public int BlockIndex { get; set; }
        public int ActualIndex { get; set; }
        public double AvaliableStorage { get; set; } = MaxStorage;
        public static double MaxStorage = 10.0f;

        public StorageBlock(int index,int ActualIndex,Application app)
        {
            InitializeComponent();
            AppData = app;
            Block.Background = app.BlockColour;
            //Container.Loaded += (s, e) => RenderBlock();
            BlockIndex = index;
            this.ActualIndex = ActualIndex;
        }

        public void RenderBlock()
        {
            //Block.Width = Container.Width;
            ToolTip = $"Disk Block #{BlockIndex}/{FileManager.MaxBlocks} - {AppData.AppName} - Block # {ActualIndex}/{AppData.AppStorageSize / FileManager.SizePerBlock} - Free Space: {AvaliableStorage.ToString("0.00")}MB";
        }

        public void Alloc(double StorageSize)
        {
            AvaliableStorage -= StorageSize;
            RenderBlock();
        }
    }
}
