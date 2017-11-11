using System.Windows.Controls;

namespace Dank_OS
{
    /// <summary>
    /// Interaction logic for TaskbarItem.xaml
    /// </summary>
    public partial class TaskbarItem : UserControl
    {

        public string IconSource { get; set; }
        public int ProcessId { get; set; }

        public bool IsActive { get; set; }
        public TaskbarItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
