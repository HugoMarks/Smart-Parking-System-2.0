using SPS.Raspberry.Logic;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SPS.Raspberry.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        public StartPageLogic Logic { get; set; }

        public StartPage()
        {
            this.InitializeComponent();
            this.Logic = new StartPageLogic();
            this.Loaded += OnLoaded;
            this.Logic.TagPresent += OnNewTag;
            this.Logic.DistanceChanged += OnDistanceChanged;
        }

        private async void OnDistanceChanged(object sender, DistanceChangedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.DistanceTextBlock.Text = e.Distance + " cm";
            });
        }

        private async void OnNewTag(object sender, TagPresentEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.TagUUIDTextBlock.Text = e.TagUid.ToString();
            });
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await Logic.StartCameraAsync(PreviewElement);
            await Logic.StartMFRC522ComponentAsync();
            Logic.StartUltrasonicDistanceSensor();
            Logic.StartServoMotor();
            MoveMotor();
        }

        private void MoveMotor()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Logic.RotateMotor(0);
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    await Logic.RotateMotor(90);
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    await Logic.RotateMotor(180);
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    await Logic.RotateMotor(90);
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            });
        }
    }
}
