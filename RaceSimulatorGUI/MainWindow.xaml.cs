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
        public MainWindow()
        {
            InitializeComponent();
            Data.Initialize();
            Data.NextRace();
            Data.CurrentRace.DriversChanged += CurrentRaceOnDriversChanged;
            Data.CurrentRace.Start();
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
    }
}