using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Dank_OS
{
    /// <summary>
    /// Interaction logic for DesktopIcon.xaml
    /// </summary>
    public partial class DesktopIcon : UserControl
    {
        #region DependencyProperties
        public static readonly DependencyProperty AppDataDependency = DependencyProperty.Register("AppData", typeof(Application), typeof(DesktopIcon));
        public Application AppData
        {
            get { return (Application)GetValue(AppDataDependency); }
            set { SetValue(AppDataDependency, value); }
        }
        #endregion

        #region Dragable UserControl
        private System.Windows.Point _anchorPoint;
        private System.Windows.Point _currentPoint;
        private bool _isInDrag;
        private TranslateTransform _transform;

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            _anchorPoint = e.GetPosition(null);
            if (element != null) element.CaptureMouse();
            _isInDrag = true;
            e.Handled = true;
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isInDrag) return;
            var element = sender as FrameworkElement;
            if (element != null) element.ReleaseMouseCapture();
            _isInDrag = false;
            e.Handled = true;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isInDrag) return;
            _currentPoint = e.GetPosition(null);

            UIElement container = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(_parentGrid)) as UIElement;
            System.Windows.Point relativeLocation = _parentGrid.TranslatePoint(new System.Windows.Point(0, 0), container);

            if (relativeLocation.X <= 0)
                _transform.X += (_currentPoint.X - _anchorPoint.X) + Math.Abs(relativeLocation.X);
            if (relativeLocation.Y < 0)
                _transform.Y += (_currentPoint.Y - _anchorPoint.Y) + Math.Abs(relativeLocation.Y);

            _transform.X += _currentPoint.X - _anchorPoint.X;
            _transform.Y += (_currentPoint.Y - _anchorPoint.Y);
            RenderTransform = _transform;
            _anchorPoint = _currentPoint;
        }

        #endregion

        #region Events
        public delegate void Launched(Application app);
        public event Launched OnAppLaunchRequest;
        #endregion


        #region Constructor
        public DesktopIcon()
        {
            InitializeComponent();
            _transform = new TranslateTransform();
            this.DataContext = this;

            this.MouseDoubleClick += (s, e) => OnAppLaunchRequest?.Invoke(AppData);
        }
        #endregion

    }
}
