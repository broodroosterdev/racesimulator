using System.Timers;
using System.Windows;
using Model;

namespace RaceSimulatorGUI
{
    public partial class CompetitionStatsWindow : Window
    {
        public CompetitionStatsWindow(CompetitionStatsData competition)
        {
            InitializeComponent();
            DataContext = competition;
        }
    }
}