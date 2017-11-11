using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;

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
