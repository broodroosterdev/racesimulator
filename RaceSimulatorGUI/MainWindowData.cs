using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller;

namespace RaceSimulatorGUI
{
    public class MainWindowData : INotifyPropertyChanged
    {
        public string TrackName => Data.CurrentRace.Track.Name;

        public MainWindowData()
        {
            Data.CurrentRace.DriversChanged += (o, e) => OnPropertyChanged("");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}