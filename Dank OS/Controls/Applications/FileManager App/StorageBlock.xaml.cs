using System.Windows.Controls;

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

        public StorageBlock(int index,int actualIndex,Application app)
        {
            InitializeComponent();
            AppData = app;
            Block.Background = app.BlockColour;
            //Container.Loaded += (s, e) => RenderBlock();
            BlockIndex = index;
            this.ActualIndex = actualIndex;
        }

        public void RenderBlock()
        {
            //Block.Width = Container.Width;
            ToolTip = $"Disk Block #{BlockIndex}/{FileManager.MaxBlocks} - {AppData.AppName} - Block # {ActualIndex}/{AppData.AppStorageSize / FileManager.SizePerBlock} - Free Space: {AvaliableStorage:0.00}MB";
            BlockLabel.Content = AvaliableStorage.ToString("0");
            LeftSubLabel.Content = ActualIndex;
            RightSubLabel.Content = BlockIndex;
        }

        public void Alloc(double storageSize)
        {
            AvaliableStorage -= storageSize;
            RenderBlock();
        }
    }
}
