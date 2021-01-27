using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using RaceSimulatorGUI.Annotations;

namespace RaceSimulatorGUI
{
    public class StatsData : INotifyPropertyChanged
    {
        private readonly Timer _timer = new Timer(1000);
        public event PropertyChangedEventHandler PropertyChanged;

        protected StatsData()
        {
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            //Updates the window
            OnPropertyChanged("");
        }


        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}