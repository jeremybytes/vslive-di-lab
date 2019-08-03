using Microsoft.FSharp.Core;
using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DigitDisplay
{
    public partial class MainWindow : Window
    {
        SolidColorBrush redBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 150, 150));
        SolidColorBrush whiteBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));

        public MainWindow()
        {
            InitializeComponent();
            Offset.Text = 8000.ToString();
            RecordCount.Text = 242.ToString();
        }

        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPanel.Children.Clear();
            RightPanel.Children.Clear();

            string fileName = AppDomain.CurrentDomain.BaseDirectory + "train.csv";

            int offset = int.Parse(Offset.Text);
            int recordCount = int.Parse(RecordCount.Text);

            string[] rawTrain = await Task.Run(() => Loader.trainingReader(fileName, offset, recordCount));
            string[] rawValidation = await Task.Run(() => Loader.validationReader(fileName, offset, recordCount));

            var manhattanClassifier = Recognizers.manhattanClassifier(rawTrain);
            var euclideanClassifier = Recognizers.euclideanClassifier(rawTrain);

            var manhattanRecognizer = new ParallelRecognizerControl(
                "Manhattan Classifier", manhattanClassifier, rawValidation);
            LeftPanel.Children.Add(manhattanRecognizer);

            var euclideanRecognizer = new ParallelRecognizerControl(
                "Euclidean Classifier", euclideanClassifier, rawValidation);
            RightPanel.Children.Add(euclideanRecognizer);

            await manhattanRecognizer.Start();

            await euclideanRecognizer.Start();
        }
    }
}
