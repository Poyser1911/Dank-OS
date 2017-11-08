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
    /// Interaction logic for GameSquareItem.xaml
    /// </summary>
    public partial class GameSquareItem : UserControl
    {
        public bool IsX { get; set; } = false;
        public bool IsO { get; set; } = false;
        public bool IsFilled { get { return IsX || IsO; } }
        public GameSquareItem()
        {
            InitializeComponent();
        }

        public void SetX()
        {
            IsX = true;
            content.Content = "X";
        }
        public void SetO()
        {
            IsO = true;
            content.Content = "O";
        }
    }
}
