using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Dank_OS
{
    public class AppWindowBase : ContentControl
    {
        public Application AppData { get; set; }


        public delegate void AppClose(int ProcessID);
        public event AppClose OnAppClose;

        public string Title { get; set; }
        public string AppIconSource { get; set; }

        public void CloseApp()
        {
            OnAppClose?.Invoke(AppData.AppProcess.ProcessID);
        }
        public AppWindowBase()
        {
            this.Style = System.Windows.Application.Current.FindResource("WindowBase") as Style;
        }

        #region Private - Dragable
        private double m_MouseX;
        private double m_MouseY;
        private Grid _container;
        private AppTitleBar _control;
        #endregion

        protected void EnableDragable(Grid Container, AppTitleBar control)
        {

            _control = control;
            _container = Container;
            _control.PreviewMouseUp += (s, e) => e.MouseDevice.Capture(null);
            _control.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(control_MouseLeftButtonUp);
            _control.PreviewMouseMove += new MouseEventHandler(control_MouseMove);
        }
        private void control_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get the Position of Window so that it will set margin from this window
            m_MouseX = e.GetPosition(this).X;
            m_MouseY = e.GetPosition(this).Y;
        }
        private void control_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Capture the mouse for border
                e.MouseDevice.Capture(_control);
                System.Windows.Thickness _margin = new System.Windows.Thickness();
                int _tempX = Convert.ToInt32(e.GetPosition(this).X);
                int _tempY = Convert.ToInt32(e.GetPosition(this).Y);
                _margin = _container.Margin;
                // when While moving _tempX get greater than m_MouseX relative to usercontrol 
                if (m_MouseX > _tempX)
                {
                    // add the difference of both to Left
                    _margin.Left += (_tempX - m_MouseX);
                    // subtract the difference of both to Left
                    _margin.Right -= (_tempX - m_MouseX);
                }
                else
                {
                    _margin.Left -= (m_MouseX - _tempX);
                    _margin.Right -= (_tempX - m_MouseX);
                }
                if (m_MouseY > _tempY)
                {
                    _margin.Top += (_tempY - m_MouseY);
                    _margin.Bottom -= (_tempY - m_MouseY);
                }
                else
                {
                    _margin.Top -= (m_MouseY - _tempY);
                    _margin.Bottom -= (_tempY - m_MouseY);
                }
                _container.Margin = _margin;
                m_MouseX = _tempX;
                m_MouseY = _tempY;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Grid mainGrd = GetTemplateChild("mainGrd") as Grid;
            AppTitleBar titlebar = GetTemplateChild("titlebar") as AppTitleBar;
            //titlebar.AppIconSource = AppIconSource;
            titlebar.appClose.MouseLeftButtonUp += (s, e) => CloseApp();
            EnableDragable(mainGrd, titlebar);
        }
    }
}
