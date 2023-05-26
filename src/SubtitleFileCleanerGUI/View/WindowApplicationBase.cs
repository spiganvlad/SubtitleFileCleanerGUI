using System;
using System.Windows;
using System.Windows.Input;

namespace SubtitleFileCleanerGUI.View
{
    // The main class for all windows that will have a changed title bar
    public class WindowApplicationBase : Window
    {
        public WindowApplicationBase() : base()
        {
            DefaultStyleKey = typeof(WindowApplicationBase);
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindows));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (SizeToContent == SizeToContent.WidthAndHeight)
                InvalidateMeasure();
        }

        #region Windows Command

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = ResizeMode != ResizeMode.NoResize;

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e) => Close();

        private void MaximizeWindows(object sender, ExecutedRoutedEventArgs e) =>
            SystemCommands.MaximizeWindow(this);

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e) =>
            SystemCommands.MinimizeWindow(this);

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e) =>
            SystemCommands.RestoreWindow(this);

        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is not FrameworkElement element)
                return;

            var point = WindowState == WindowState.Minimized ? new Point(0, element.ActualHeight) :
                new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            point = element.TransformToAncestor(this).Transform(point);
            SystemCommands.ShowSystemMenu(this, point);
        }

        #endregion
    }
}
