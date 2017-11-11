using System.Windows.Media;

namespace Dank_OS
{
    /// <summary>
    /// Interaction logic for TicTacToeApp.xaml
    /// </summary>
    public partial class TicTacToeApp : AppWindowBase
    {
        private readonly GameSquareItem[] _squares = new GameSquareItem[9];
        private bool _next = true;
        private bool _endOfMath = false;
        public TicTacToeApp()
        {
            InitializeComponent();
            appGrd1.Loaded += (s, e) => InitializeGame();
        }

        private void InitializeGame()
        {
            for (int i = 0; i < 9; i++)
            {
                GameSquareItem g = appGrd1.Children[i] as GameSquareItem;
                g.MouseLeftButtonUp += (s, e) => SetNext(g);
                _squares[i] = g;
            }
        }
        public void SetNext(GameSquareItem s)
        {
            if (s.IsFilled || _endOfMath)
                return;
            if (_next)
                s.SetX();
            else
                s.SetO();
            nextturn.Content = !_next ? "Player 1 Move" : "Player 3 Move";
            nextturn.Foreground = Brushes.Indigo;
            CheckWin();
            _next = !_next;
        }

        public void CheckWin()
        {
            int[,] w = new int[,] { { 1, 2, 3 }, { 3, 6, 9 }, { 9, 8, 7 }, { 7, 4, 1 }, { 1, 5, 9 }, { 3, 5, 7 }, { 4, 5, 6 }, { 7, 8, 9 }, { 2, 5, 8 } };
            for (int i = 0; i < w.GetLength(0); i++)
                if ((_squares[w[i, 0] - 1].IsX && _squares[w[i, 1] - 1].IsX && _squares[w[i, 2] - 1].IsX) || (_squares[w[i, 0] - 1].IsO && _squares[w[i, 1] - 1].IsO && _squares[w[i, 2] - 1].IsO))
                {
                    SolidColorBrush wp = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    wp.Opacity = 0.5;
                    _squares[w[i, 0] - 1].Background = wp;
                    _squares[w[i, 1] - 1].Background = wp;
                    _squares[w[i, 2] - 1].Background = wp;
                    nextturn.Content = _next ? "Player 1 Won" : "Player 2 Won";
                    nextturn.Foreground = Brushes.Green;
                    _endOfMath = true;
                    return;
                }
            CheckTie();
        }
        public void CheckTie()
        {
            for (int i = 0; i < 9; i++)
                if (!_squares[i].IsFilled)
                    return;
            nextturn.Content = "Match Tied";
            nextturn.Foreground = Brushes.Orange;
            _endOfMath = true;
        }
    }
}
