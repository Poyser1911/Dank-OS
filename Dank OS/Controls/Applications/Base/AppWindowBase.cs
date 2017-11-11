using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dank_OS.Controls.Applications.Base;

namespace Dank_OS
{
    public class AppWindowBase : ContentControl
    {
        public Application AppData { get; set; }

        public AppWindowState WindowState { get; set; } = AppWindowState.None;

        public delegate void AppWindowAction(Application app);
        public event AppWindowAction OnWindowAction;

        public string Title { get; set; }
        public string AppIconSource { get; set; }

        public void WindowAction(AppWindowState state)
        {
            if (state == AppWindowState.Maximize && WindowState.HasFlag(AppWindowState.Maximize))
                WindowState = AppWindowState.Default;
            else if (state == AppWindowState.Maximize)
                WindowState = state;
            else if (state == AppWindowState.CloseRequest)
                WindowState = state;
            else
                WindowState |= state;

            OnWindowAction?.Invoke(AppData);
        }
        public AppWindowBase()
        {
            this.Style = System.Windows.Application.Current.FindResource("WindowBase") as Style;
        }

        #region Private - Dragable
        private double _mMouseX;
        private double _mMouseY;
        private Grid _container;
        private AppTitleBar _control;
        #endregion

        protected void EnableDragable(Grid container, AppTitleBar control)
        {

            _control = control;
            _container = container;
            _control.PreviewMouseUp += (s, e) => e.MouseDevice.Capture(null);
            _control.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(control_MouseLeftButtonUp);
            _control.PreviewMouseMove += new MouseEventHandler(control_MouseMove);
        }
        private void control_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get the Position of Window so that it will set margin from this window
            _mMouseX = e.GetPosition(this).X;
            _mMouseY = e.GetPosition(this).Y;
        }
        private void control_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Capture the mouse for border
                e.MouseDevice.Capture(_control);
                System.Windows.Thickness margin = new System.Windows.Thickness();
                int tempX = Convert.ToInt32(e.GetPosition(this).X);
                int tempY = Convert.ToInt32(e.GetPosition(this).Y);
                margin = _container.Margin;
                // when While moving _tempX get greater than m_MouseX relative to usercontrol 
                if (_mMouseX > tempX)
                {
                    // add the difference of both to Left
                    margin.Left += (tempX - _mMouseX);
                    // subtract the difference of both to Left
                    margin.Right -= (tempX - _mMouseX);
                }
                else
                {
                    margin.Left -= (_mMouseX - tempX);
                    margin.Right -= (tempX - _mMouseX);
                }
                if (_mMouseY > tempY)
                {
                    margin.Top += (tempY - _mMouseY);
                    margin.Bottom -= (tempY - _mMouseY);
                }
                else
                {
                    margin.Top -= (_mMouseY - tempY);
                    margin.Bottom -= (tempY - _mMouseY);
                }
                _container.Margin = margin;
                _mMouseX = tempX;
                _mMouseY = tempY;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Grid mainGrd = GetTemplateChild("mainGrd") as Grid;
            AppTitleBar titlebar = GetTemplateChild("titlebar") as AppTitleBar;

            titlebar.appClose.MouseLeftButtonUp += (s, e) => WindowAction(AppWindowState.CloseRequest);
            titlebar.appMaximize.MouseLeftButtonUp += (s, e) => WindowAction(AppWindowState.Maximize);
            titlebar.appMinimize.MouseLeftButtonUp += (s, e) => WindowAction(AppWindowState.Minimize);
            EnableDragable(mainGrd, titlebar);
        }
    }
}
