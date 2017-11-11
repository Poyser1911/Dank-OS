using System.Windows.Controls;
using System.Windows.Media;

namespace Dank_OS
{
    /// <summary>
    /// Interaction logic for MemoryManagerApp.xaml
    /// </summary>
    public partial class MemoryManagerApp : AppWindowBase
    {
        private readonly MemoryManager _mManger = MemoryManager.GetInstance;

        private Border Border => new Border() { Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)) { Opacity = 0.7 }, Width = 5 };
        public MemoryManagerApp()
        {
            InitializeComponent();

            MemView.Loaded += (s, e) =>
            {
                InitializeMemoryBlocks(MemView.ActualWidth);
                _mManger.OnBlockChanged += (i, src) => RenderMemUsage(MemView.ActualWidth);
                MainGrid.SizeChanged += (sender, args) => RenderMemUsage(args.NewSize.Width);
                RenderMemUsage(MemView.ActualWidth);
            };
        }

        private void InitializeMemoryBlocks(double width)
        {
            MemView.Children.Clear();
            for (int i = 0; i < _mManger.Blocks.Count; i++)
            {
                if (i < _mManger.Blocks.Count - 1)
                    MemView.Children.Add(new StackPanel() { Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)) { Opacity = 0.1 }, ToolTip = $"{_mManger.Blocks[i].Name}, {_mManger.Blocks[i].AvaliableMemory}MB Free", Orientation = Orientation.Horizontal, Width = (((_mManger.Blocks[i].TotalMemory * 100) / MemoryManager.Totalmemory) * (width)) / 100 });
                else
                    MemView.Children.Add(new StackPanel() { Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)) { Opacity = 0.1 }, ToolTip = $"{_mManger.Blocks[i].Name}, {_mManger.Blocks[i].AvaliableMemory}MB Free", Orientation = Orientation.Horizontal, Width = ((((_mManger.Blocks[i].TotalMemory * 100) / MemoryManager.Totalmemory) * (width)) / 100) - 10 });

                if (i < _mManger.Blocks.Count - 1)
                    MemView.Children.Add(Border);
            }
        }
        private void RenderMemUsage(double width)
        {
            if (!MemView.IsLoaded) return;
            InitializeMemoryBlocks(width);

            for (int i = 0, k = 0; i < (_mManger.Blocks.Count * 2); i += 2, k++)
            {
                StackPanel sp = (MemView.Children[i] as StackPanel);
                foreach (MemAppData app in _mManger.Blocks[k].Apps)
                    sp.Children.Add(new Border() { Background = app.BlockColour, ToolTip = $"{app.Name}({app.MemorySize}MB) - {app.BlockPercentageUseage.ToString("0.00")}% Block Usage, {app.TotalPercentageUseage.ToString("0.00")}% Total Memory Usage.", Width = (app.BlockPercentageUseage / 100) * sp.Width });
            }
        }
    }
}
