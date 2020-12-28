using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller;
using Model;
using RaceSimulatorGUI.Annotations;

namespace RaceSimulatorGUI
{
    public class MainWindowData : INotifyPropertyChanged
    {
        public string TrackName => Data.CurrentRace.Track.Name;

        public MainWindowData()
        {
            Data.CurrentRace.DriversChanged += (o,e) => OnPropertyChanged("");
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}