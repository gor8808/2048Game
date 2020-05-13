using Game2048;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace _2048Game
{

    public partial class MainWindow : Window
    {
        private Model _model = new Model(4);
        private int _highScore = 0;
        private string pathToJsonForScore = @"score.json";
        public MainWindow()
        {
            InitializeComponent();
            NewGameBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            RestoreScoreFromJson();

        }

        private void RestoreScoreFromJson()
        {
            using (StreamReader r = new StreamReader(pathToJsonForScore))
            {
                string json = r.ReadToEnd();
                List<ScoreDataForJson> items = JsonConvert.DeserializeObject<List<ScoreDataForJson>>(json);
                foreach (var item in items)
                {
                    BestScore.Text = item.HighScore.ToString();
                    _highScore = item.HighScore;
                }
            }
        }

        private void StartBtnClick(object sender, RoutedEventArgs e)
        {
            _model.Start();
            ShowMap();
            _model.ScoreResult = 0;
            ScoreCount.Text = _model.ScoreResult.ToString();
        }

        private void ShowMap()
        {
            for (int y = 0; y < _model.MapSize; y++)
            {
                for (int x = 0; x < _model.MapSize; x++)
                {
                    Grid grid = (Grid)FindName("b" + x + y);
                    Color bgColor = new Color();
                    Brush fgColor = Brushes.Black;
                    int numberFromMap = _model.GetMap(x, y);
                    switch (numberFromMap)
                    {
                        case 2:
                            bgColor = Color.FromRgb(238, 228, 218);
                            break;
                        case 4:
                            bgColor = Color.FromRgb(237, 224, 200);
                            break;
                        case 8:
                            bgColor = Color.FromRgb(242, 177, 121);
                            fgColor = Brushes.White;
                            break;
                        case 16:
                            bgColor = Color.FromRgb(245, 149, 99);
                            fgColor = Brushes.White;
                            break;
                        case 32:
                            bgColor = Color.FromRgb(246, 124, 95);
                            fgColor = Brushes.White;
                            break;
                        case 64:
                            bgColor = Color.FromRgb(246, 94, 59);
                            fgColor = Brushes.White;
                            break;
                        case 128:
                            bgColor = Color.FromRgb(237, 207, 114);
                            fgColor = Brushes.White;
                            break;
                        case 256:
                            bgColor = Color.FromRgb(237, 204, 97);
                            fgColor = Brushes.White;
                            break;
                    }

                    if (bgColor == new Color() && numberFromMap > 256)
                    {
                        bgColor = Color.FromRgb(237, 200, 9);
                        fgColor = Brushes.White;
                    }
                    grid.Background = new SolidColorBrush(bgColor);
                    TextBlock textblock = grid.Children.Cast<TextBlock>().First();
                    textblock.Foreground = fgColor;
                    int number = _model.GetMap(x, y);
                    if (number == 0)
                    {
                        textblock.Text = " ";
                    }
                    else
                    {
                        textblock.Text = number.ToString();
                    }
                }
            }

            if (_model.IsGameOver())
            {
                GameOverPanel.Visibility = Visibility.Visible;
            }
            else
            {
                GameOverPanel.Visibility = Visibility.Hidden;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _model.Up();
                    break;
                case Key.Down:
                    _model.Down();
                    break;
                case Key.Right:
                    _model.Right();
                    break;
                case Key.Left:
                    _model.Left();
                    break;
            }
            ShowMap();
            int score = _model.ScoreResult;
            ScoreCount.Text = score.ToString();
            if (_highScore < score)
            {
                _highScore = score;
                BestScore.Text = _highScore.ToString();
                UpdateScoreDataFromJson(score);

            }
        }

        private void UpdateScoreDataFromJson(int score)
        {
            List<ScoreDataForJson> _data = new List<ScoreDataForJson>() { new ScoreDataForJson(score) };

            string json = JsonConvert.SerializeObject(_data.ToArray());

            File.WriteAllText(pathToJsonForScore, json);
        }

        //private void BackStep_Btn(object sender, MouseButtonEventArgs e)
        //{
        //    _step++;
        //    _model = _steps[_steps.Count - _step];
        //    ShowMap();

        //}
    }
}
