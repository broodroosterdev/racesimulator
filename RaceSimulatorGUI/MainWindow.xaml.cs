using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Controller;
using Model;
using Section = System.Windows.Documents.Section;

namespace RaceSimulatorGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CompetitionStatsWindow competitionWindow;
        private RaceStatsWindow raceWindow; 
        public MainWindow()
        {
            Data.Initialize();
            Data.NextRace();
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            ImageCache.Clear();
            Data.CurrentRace.DriversChanged += CurrentRaceOnDriversChanged;
            Data.CurrentRace.RaceEnded += CurrentRaceOnRaceEnded;
            Data.CurrentRace.Start();
        }
        

        private void CurrentRaceOnRaceEnded(object model)
        {
            Data.Competition.GivePoints();    
            Data.NextRace();
            Initialize();
        }

        private void CurrentRaceOnDriversChanged(object model, DriversChangedEventArgs e)
        {
            this.Screen.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    Screen.Source = null;
                    Screen.Source = Renderer.DrawTrack(Data.CurrentRace.Track);
                }));
            
        }


        private void MenuItem_Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Race_OnClick(object sender, RoutedEventArgs e)
        {
            raceWindow = new RaceStatsWindow();
            raceWindow.Show();
        }
        
        private void MenuItem_Competition_OnClick(object sender, RoutedEventArgs e)
        {
            competitionWindow = new CompetitionStatsWindow();
            competitionWindow.Show();
        }
    }
}