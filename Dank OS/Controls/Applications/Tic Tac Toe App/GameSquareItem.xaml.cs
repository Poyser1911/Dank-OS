using System.Windows.Controls;

namespace Dank_OS
{
    /// <summary>
    /// Interaction logic for GameSquareItem.xaml
    /// </summary>
    public partial class GameSquareItem : UserControl
    {
        public bool IsX { get; set; }
        public bool IsO { get; set; }
        public bool IsFilled => IsX || IsO;

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
