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
    /// Interaction logic for TicTacToeApp.xaml
    /// </summary>
    public partial class TicTacToeApp : AppWindowBase
    {
        GameSquareItem[] Squares = new GameSquareItem[9];
        private bool next = true;
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
                Squares[i] = g;
            }
        }
        public void SetNext(GameSquareItem s)
        {
            if (s.IsFilled || _endOfMath)
                return;
            if (next)
                s.SetX();
            else
                s.SetO();
            nextturn.Content = !next ? "Player 1 Move" : "Player 3 Move";
            nextturn.Foreground = Brushes.Indigo;
            CheckWin();
            next = !next;
        }

        public void CheckWin()
        {
            int[,] w = new int[,] { { 1, 2, 3 }, { 3, 6, 9 }, { 9, 8, 7 }, { 7, 4, 1 }, { 1, 5, 9 }, { 3, 5, 7 }, { 4, 5, 6 }, { 7, 8, 9 }, { 2, 5, 8 } };
            for (int i = 0; i < w.GetLength(0); i++)
                if ((Squares[w[i, 0] - 1].IsX && Squares[w[i, 1] - 1].IsX && Squares[w[i, 2] - 1].IsX) || (Squares[w[i, 0] - 1].IsO && Squares[w[i, 1] - 1].IsO && Squares[w[i, 2] - 1].IsO))
                {
                    SolidColorBrush wp = new SolidColorBrush(Color.FromRgb(0,255,0));
                    wp.Opacity = 0.5;
                    Squares[w[i, 0] - 1].Background = wp;
                    Squares[w[i, 1] - 1].Background = wp;
                    Squares[w[i, 2] - 1].Background = wp;
                    nextturn.Content = next ? "Player 1 Won" : "Player 2 Won";
                    nextturn.Foreground = Brushes.Green;
                    _endOfMath = true;
                    return;
                }
            CheckTie();
        }
        public void CheckTie()
        {
            for (int i = 0; i < 9; i++)
                if (!Squares[i].IsFilled)
                    return;
            nextturn.Content = "Match Tied";
            nextturn.Foreground = Brushes.Orange;
            _endOfMath = true;
        }
    }
}
