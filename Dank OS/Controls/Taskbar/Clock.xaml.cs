using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl,INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public string Time { get; set; } = DateTime.Now.ToShortTimeString();
        public string Date { get; set; } = DateTime.Now.ToShortDateString();
        public Clock()
        {
            InitializeComponent();
            this.DataContext = this;
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            string lasttime = "";
            Task.Run(() =>
            {
                while (true)
                {
                    Time = DateTime.Now.ToShortTimeString();
                    if (Time != lasttime)
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
                        lasttime = Time;
                    }
                    
                }
            });
        }
    }
}
