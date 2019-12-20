using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SlotMachine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private string[] charArr = { "A", "B", "C" };
        private bool isPlaying = false;
        private bool isModeChanged = false;
        private DispatcherTimer timer;
        private int roundCounter = 1;
        private double mode = 1.5;
        private SolidColorBrush winColor = new SolidColorBrush(Windows.UI.Colors.Green);
        private SolidColorBrush loseColor = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush neutralColor = new SolidColorBrush(Windows.UI.Colors.Transparent);
        private bool resetTrigger = false;

        public GamePage()
        {
            this.InitializeComponent();
            ResetTimer(mode);
        }


        private string GetRandomLetter()
        {
            Random rnd = new Random();
            int r = rnd.Next(charArr.Length);
            return charArr[r];
        }

        private void SubscribeButton_Click(object sender, RoutedEventArgs e)
        {

            if (!isPlaying)
            {
                StartGame();
            }
            else
            {
                StopGame(IsWin);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }


        private void AlignmentMenuFlyoutItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            var option = ((MenuFlyoutItem)sender).Tag.ToString();
            var currentText = ((MenuFlyoutItem)sender).Text.ToString();
            System.Diagnostics.Debug.WriteLine("Current text " + currentText);
            System.Diagnostics.Debug.WriteLine("Current option " + option);
            if (option != null)
            {
                if (option == "easy")
                {
                    mode = 1.5;
                    isModeChanged = true;
                    StopGame(resetTrigger);
                }
                else if (option == "medium")
                {
                    mode = 0.9;
                    isModeChanged = true;
                    StopGame(resetTrigger);
                }
                else if (option == "hard")
                {
                    mode = 0.1;
                    isModeChanged = true;
                    StopGame(resetTrigger);
                }
            }
        }

        // game logic methods 

        private void ResetTimer(double newMode)
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(newMode)
            };
        }

        private void StartGame()
        {
            isPlaying = true;
            timer.Tick += (o, ex) =>
            {
                firstText.Text = GetRandomLetter();
                secondText.Text = GetRandomLetter();
                thirdText.Text = GetRandomLetter();
            };

            timer.Start();
            roundText.Text = "Round " + roundCounter;
            spinButton.Content = "Stop";
        }

        private void StopGame(bool isWin)
        {
            if (isWin)
            {
                MakeScoreboardWin();
                ++roundCounter;
                StartGame();
            }
            else
            {
                ResetTimer(mode);
                isPlaying = false;
                MakeScoreboardLose();
                timer.Stop();
                Reset();
            }
        }

        private void Reset()
        {
            timer.Stop();
            isPlaying = false;
            roundCounter = 1;
            if (isModeChanged)
            {
                isModeChanged = false;
                roundText.Text = "Welcome to my spinning game!";
            }
            else
            {
                roundText.Text = "You lose. Wanna try again?";
            }
            firstText.Text = "A";
            secondText.Text = "B";
            thirdText.Text = "C";
            spinButton.Content = "Spin";
            MakeScoreboardLose();
        }

        private void ResetWin()
        {
            timer.Stop();
            isPlaying = false;
            roundCounter = 1;
            roundText.Text = "You won, Congratulations !!! 🎉 🎊 🎈🎈🎈";
            firstText.Text = "A";
            secondText.Text = "B";
            thirdText.Text = "C";
            spinButton.Content = "Spin Again";
            MakeScoreboardLose();
        }

        private bool IsWin
        {
            get
            {
                if (firstText.Text == secondText.Text && secondText.Text == thirdText.Text)
                {
                    return true;
                }
                return false;
            }
        }

        private void MakeScoreboardWin()
        {
            switch (roundCounter)
            {
                case 1:
                    scoreOne.Fill = winColor;
                    break;
                case 2:
                    scoreTwo.Fill = winColor;
                    break;
                case 3:
                    scoreThree.Fill = winColor;
                    ResetWin();
                    break;
                default:
                    ResetScoreboard();
                    break;
            }
        }

        private void MakeScoreboardLose()
        {
            scoreOne.Fill = loseColor;
            scoreTwo.Fill = loseColor;
            scoreThree.Fill = loseColor;
        }

        private void ResetScoreboard()
        {
            scoreOne.Fill = neutralColor;
            scoreTwo.Fill = neutralColor;
            scoreThree.Fill = neutralColor;
        }

    }
}

