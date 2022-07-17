using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SubtitleFileCleanerGUI.Model
{
    // Basic implementation of the INotifyPropertyChanged interface
    public abstract class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string property = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
