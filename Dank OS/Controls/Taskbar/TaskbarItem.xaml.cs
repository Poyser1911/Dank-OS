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
    /// Interaction logic for TaskbarItem.xaml
    /// </summary>
    public partial class TaskbarItem : UserControl
    {

        public string IconSource { get; set; }
        public int ProcessID { get; set; }

        public bool IsActive { get; set; }
        public TaskbarItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
