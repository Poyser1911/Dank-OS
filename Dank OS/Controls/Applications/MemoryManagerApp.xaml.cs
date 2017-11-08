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
    /// Interaction logic for MemoryManagerApp.xaml
    /// </summary>
    public partial class MemoryManagerApp : AppWindowBase
    {
        private MemoryManager mManger = MemoryManagerService.DefaultInstance;
        private static SolidColorBrush UsageBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255)) { Opacity = 0.4 };
        private static SolidColorBrush BorderSolidBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)) { Opacity = 0.7 };

        private static SolidColorBrush ABrush = new SolidColorBrush(Color.FromRgb(0, 255, 0)) { Opacity = 0.1 };
        private static SolidColorBrush BBrush = new SolidColorBrush(Color.FromRgb(0, 255, 255)) { Opacity = 0.1 };
        private static SolidColorBrush CBrush = new SolidColorBrush(Color.FromRgb(255, 255, 0)) { Opacity = 0.1 };
        private Border border => new Border() { Background = BorderSolidBrush, Width = 5 };
        public MemoryManagerApp()
        {
            InitializeComponent();

            MemView.Loaded += (s, e) =>
            {
                InitializeMemoryBlocks(MemView.ActualWidth);
                mManger.OnBlockChanged += MManger_OnBlockChanged;
                MemView.SizeChanged += MemView_SizeChanged;
                RenderMemUsage(MemView.ActualWidth);
            };
            //RenderMemUsage();
        }

        private void InitializeMemoryBlocks(double width)
        {
            MemView.Children.Clear();
            for (int i = 0; i < mManger.Blocks.Count; i++)
            {
                if (i < mManger.Blocks.Count - 1)
                    MemView.Children.Add(new StackPanel() { Background = mManger.Blocks[i].BlockColour, ToolTip = $"{mManger.Blocks[i].Name}, {mManger.Blocks[i].AvaliableMemory}MB Free", Orientation = Orientation.Horizontal, Width = (((mManger.Blocks[i].TotalMemory * 100) / MemoryManager.TOTALMEMORY) * (width)) / 100 });
                else
                    MemView.Children.Add(new StackPanel() { Background = mManger.Blocks[i].BlockColour, ToolTip = $"{mManger.Blocks[i].Name}, {mManger.Blocks[i].AvaliableMemory}MB Free", Orientation = Orientation.Horizontal, Width = ((((mManger.Blocks[i].TotalMemory * 100) / MemoryManager.TOTALMEMORY) * (width)) / 100) - 10 });

                if (i < mManger.Blocks.Count - 1)
                    MemView.Children.Add(border);
            }
        }

        private SolidColorBrush GetRandomColour()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            SolidColorBrush g = new SolidColorBrush(Color.FromRgb((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255)));
            return g;
        }

        private void RenderMemUsage(double width)
        {
            if (MemView.IsLoaded)
            {
                InitializeMemoryBlocks(width);

                for (int i=0,k = 0;  i < (mManger.Blocks.Count * 2); i += 2,k++)
                {
                    StackPanel sp = (MemView.Children[i] as StackPanel);
                    foreach (MemAppData app in mManger.Blocks[k].apps)
                        sp.Children.Add(new Border() { Background = app.BlockColour, ToolTip = $"{app.Name}({app.MemorySize}MB) - {app.BlockPercentageUseage.ToString("0.00")}% Block Usage, {app.TotalPercentageUseage.ToString("0.00")}% Total Memory Usage.", Width = (app.BlockPercentageUseage / 100) * sp.Width });
                }
                double height = MemView.ActualHeight;
            
            }
        }

        private void MemView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //MessageBox.Show("Size Changed;");
            RenderMemUsage(e.NewSize.Width);
        }

        private void MManger_OnBlockChanged(int BlockIndex, MemoryBlock SourceBlock) => RenderMemUsage(MemView.ActualWidth);
    }
}
