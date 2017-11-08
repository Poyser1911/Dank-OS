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
    /// Interaction logic for AppTitleBar.xaml
    /// </summary>
    public partial class AppTitleBar : UserControl
    {
        public static readonly DependencyProperty AppTitleDependency = DependencyProperty.Register("AppTitle", typeof(string), typeof(AppTitleBar));
        public string AppTitle
        {
            get { return (string)GetValue(AppTitleDependency); }
            set { SetValue(AppTitleDependency, value); }
        }

        public static readonly DependencyProperty AppIconSourceDependency = DependencyProperty.Register("AppIconSource", typeof(string), typeof(AppTitleBar));
        public string AppIconSource
        {
            get { return (string)GetValue(AppIconSourceDependency); }
            set { SetValue(AppIconSourceDependency, value); }
        }
        public AppTitleBar()
        {
            InitializeComponent();
            this.DataContext = this;
            //AppIconSource = "memory.png";
        }
    }
}
