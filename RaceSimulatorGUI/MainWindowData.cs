using System.ComponentModel;
using System.Runtime.CompilerServices;
using RaceSimulatorGUI.Annotations;

namespace RaceSimulatorGUI
{
    public class MainWindowData : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}