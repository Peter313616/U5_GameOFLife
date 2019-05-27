/* 
 * Peter McEwen
 * May 27, 2019
 * Creates and allows to the user to play the game of life
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace U5_GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Button[][] grid;
        bool[][] gridLive;
        bool?[][] newLive;
        bool gameStart = false;
        Button btnPlay, btnPreset;
        DispatcherTimer gameTimer = new DispatcherTimer();
        RadioButton rbPuffer;
        RadioButton rbOscillator;
        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            btnPlay = new Button();
            btnPlay.BorderBrush = Brushes.Black;
            btnPlay.Content = "Click me to play";
            btnPlay.Height = 50;
            btnPlay.Width = 400;
            btnPlay.FontSize = 30;
            btnPlay.Click += playGame;
            Canvas.SetTop(btnPlay, 500);
            Canvas.SetLeft(btnPlay, 0);
            canvas.Children.Add(btnPlay);

            btnPreset = new Button();
            btnPreset.BorderBrush = Brushes.Black;
            btnPreset.Content = "Click to play selected preset";
            btnPreset.Height = 50;
            btnPreset.Width = 400;
            btnPreset.FontSize = 30;
            btnPreset.Click += BtnPreset_Click;
            btnPreset.Click += playGame;
            Canvas.SetTop(btnPreset, 600);
            Canvas.SetLeft(btnPreset, 0);
            canvas.Children.Add(btnPreset);

            Label lblPresets = new Label();
            lblPresets.Content = "presets to choose from";
            Canvas.SetTop(lblPresets, 550);
            Canvas.SetLeft(lblPresets, 0);
            canvas.Children.Add(lblPresets);

            rbPuffer = new RadioButton();
            rbPuffer.GroupName = "Presets";
            Canvas.SetTop(rbPuffer, 580);
            Canvas.SetLeft(rbPuffer, 0);
            rbPuffer.Content = "Pufferfish";
            canvas.Children.Add(rbPuffer);

            rbOscillator = new RadioButton();
            rbOscillator.GroupName = "Presets";
            Canvas.SetTop(rbOscillator, 580);
            Canvas.SetLeft(rbOscillator, 100);
            rbOscillator.Content = "Oscillator";
            canvas.Children.Add(rbOscillator);

            grid = new Button[50][];
            gridLive = new bool[50][];
            newLive = new bool?[50][];
            for (int i = 0; i < 50; i++)
            {
                grid[i] = new Button[50];
                gridLive[i] = new bool[50];
                newLive[i] = new bool?[50];
            }
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    gridLive[x][y] = false;
                    grid[x][y] = new Button();
                    grid[x][y].Height = 10;
                    grid[x][y].Width = 10;
                    grid[x][y].Background = Brushes.White;
                    grid[x][y].BorderBrush = Brushes.Black;
                    grid[x][y].BorderThickness = new Thickness(1);
                    grid[x][y].Click += clickEvent;
                    Canvas.SetTop(grid[x][y], y * 10);
                    Canvas.SetLeft(grid[x][y], x * 10);
                    canvas.Children.Add(grid[x][y]);
                }
            }
        }

        private void BtnPreset_Click(object sender, RoutedEventArgs e)
        {
            StreamReader sr;
            if (rbPuffer.IsChecked == true)
            {
                sr = new StreamReader("Pufferfish.txt");
                while (!sr.EndOfStream)
                {
                    string[] tempValues = sr.ReadLine().Split(',');
                    int[] values = new int[2];
                    int.TryParse(tempValues[0], out values[0]);
                    int.TryParse(tempValues[1], out values[1]);
                    gridLive[values[0]][values[1]] = true;
                }
            }
            else if (rbOscillator.IsChecked == true)
            {
                sr = new StreamReader("Oscillator.txt");
                while (!sr.EndOfStream)
                {
                    string[] tempValues = sr.ReadLine().Split(',');
                    int[] values = new int[2];
                    int.TryParse(tempValues[0], out values[0]);
                    int.TryParse(tempValues[1], out values[1]);
                    gridLive[values[0]][values[1]] = true;
                }
            }
            /*StreamWriter sw = new StreamWriter("tempOsc.txt"); //used to create Presets
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (gridLive[x][y])
                    {
                        sw.WriteLine(x + "," + y);
                    }
                }
            }
            sw.Flush();
            sw.Close();*/
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    bool?[] neighbours = new bool?[8];
                    try
                    {
                        neighbours[0] = gridLive[x + 1][y];
                    }
                    catch
                    {
                        neighbours[0] = null;
                    }
                    try
                    {
                        neighbours[1] = gridLive[x - 1][y];
                    }
                    catch
                    {
                        neighbours[1] = null;
                    }
                    try
                    {
                        neighbours[2] = gridLive[x][y + 1];
                    }
                    catch
                    {
                        neighbours[2] = null;
                    }
                    try
                    {
                        neighbours[3] = gridLive[x][y - 1];
                    }
                    catch
                    {
                        neighbours[3] = null;
                    }
                    try
                    {
                        neighbours[4] = gridLive[x + 1][y + 1];
                    }
                    catch
                    {
                        neighbours[4] = null;
                    }
                    try
                    {
                        neighbours[5] = gridLive[x + 1][y - 1];
                    }
                    catch
                    {
                        neighbours[5] = null;
                    }
                    try
                    {
                        neighbours[6] = gridLive[x - 1][y + 1];
                    }
                    catch
                    {
                        neighbours[6] = null;
                    }
                    try
                    {
                        neighbours[7] = gridLive[x - 1][y - 1];
                    }
                    catch
                    {
                        neighbours[7] = null;
                    }
                    int neighbourCount = 0;
                    for (int i = 0; i < neighbours.Length; i++)
                    {
                        if (neighbours[i] == true)
                        {
                            neighbourCount++;
                        }
                    }
                    if (neighbourCount < 2 || neighbourCount > 3)
                    {
                        newLive[x][y] = false;
                    }
                    else if (neighbourCount == 3)
                    {
                        newLive[x][y] = true;
                    }
                }
            }
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    try
                    {
                        if (newLive[x][y] == true)
                        {

                            grid[x][y].Background = Brushes.Red;
                            grid[x][y].BorderBrush = Brushes.Black;
                        }
                        else if (newLive[x][y] == false)
                        {
                            grid[x][y].Background = Brushes.White;
                            grid[x][y].BorderBrush = Brushes.Black;
                        }
                        gridLive[x][y] = (bool)(newLive[x][y]);
                        newLive[x][y] = null;
                    }
                    catch
                    {

                    }
                }
            }
        }
        public void clickEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (gameStart == false)
            {
                for (int x = 0; x < 50; x++)
                {
                    for (int y = 0; y < 50; y++)
                    {
                        if (grid[x][y] == button)
                        {
                            if (gridLive[x][y] == false)
                            {
                                gridLive[x][y] = true;
                                grid[x][y].Background = Brushes.Black;
                                grid[x][y].BorderBrush = Brushes.White;
                            }
                            else if (gridLive[x][y])  
                            {
                                gridLive[x][y] = false;
                                grid[x][y].Background = Brushes.White;
                                grid[x][y].BorderBrush = Brushes.Black;
                            }
                            
                        }
                    }
                }
            }
        }
        public void playGame(object sender, EventArgs e)
        {
            gameStart = true;
            gameTimer.Start();
        }
    }
}
