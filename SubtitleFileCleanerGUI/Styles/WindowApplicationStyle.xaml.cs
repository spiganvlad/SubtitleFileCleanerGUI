using System;
using System.Windows;
using System.Windows.Controls;

namespace SubtitleFileCleanerGUI.Styles
{
    public partial class WindowApplicationStyle : ResourceDictionary
    {
        public WindowApplicationStyle() =>
            InitializeComponent();

        // Hide the MaximizeButton and show the RestoreButton
        private void MaximizeButton_Click(object sender, RoutedEventArgs e) =>
            TrySwitchButtonVisibility(e, "RestoreButton");

        // Hide the RestoreButton and show the MaximizeButton
        private void RestoreButton_Click(object sender, RoutedEventArgs e) =>
            TrySwitchButtonVisibility(e, "MaximizeButton");

        // Hide the button that triggered the event and show the button whose
        // name matches the parameter (buttons must be in the same stackpanel)
        private void TrySwitchButtonVisibility(RoutedEventArgs e, string buttonToVisible)
        {
            if (e.OriginalSource is not Button button)
                throw new InvalidOperationException("Called element is not a button");

            if (button.Parent is not StackPanel stackPanel)
                throw new InvalidOperationException("The button's parent is not StackPanel");
            
            foreach (var item in stackPanel.Children)
            {
                if (item is not Button b)
                    continue;

                if (b.Name == buttonToVisible)
                {
                    button.Visibility = Visibility.Collapsed;
                    b.Visibility = Visibility.Visible;
                    return;
                }
            }

            throw new ArgumentException($"Could not find Button element with name {buttonToVisible}", nameof(buttonToVisible));
        }
    }
}
