using SPS.Raspberry.Core.MFRC522;
using SPS.Raspberry.Logic;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SPS.Raspberry.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {

        private const double CloseAngle = 160;
        private const double OpenAngle = 70;
        private bool _isProcessing = false;
        private bool _isCloseToSensor = false;

        public StartPageLogic Logic { get; set; }

        public StartPage()
        {
            this.InitializeComponent();
            this.Logic = new StartPageLogic();
            this.Loaded += OnLoaded;
            this.Logic.NewResponse += OnNewResponse;
            this.Logic.DistanceChanged += OnDistanceChanged;
        }

        private async void OnNewResponse(object sender, AuthResponseEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                var status = e.Reponse.StatusCode;

                if (status == HttpStatusCode.Ok)
                {
                    if (e.Reponse.Control == 1)
                    {
                        MessageTextBlock.Text = "Bem vindo, " + e.Reponse.UserName;
                        await Logic.RotateMotor(OpenAngle);
                    }
                    else
                    {
                        MessageTextBlock.Text = "Até logo, " + e.Reponse.UserName;
                        await Logic.RotateMotor(OpenAngle);
                    }
                }
                else if (status == HttpStatusCode.BadRequest)
                {
                    MessageTextBlock.Text = "Não há mais vagas disponíveis";
                }
                else if (status == HttpStatusCode.Unauthorized)
                {
                    if (e.Reponse.Message.Contains("tag"))
                    {
                        MessageTextBlock.Text = "Tag inválida";
                    }
                    else
                    {
                        MessageTextBlock.Text = "Placa não reconhecida.";
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        await RequestTagAsync();
                    }
                }
                else
                {
                    MessageTextBlock.Text = "Oops, um erro aconteceu";
                }
                
                await Task.Delay(TimeSpan.FromSeconds(3));
                MessageTextBlock.Text = "Smart Parking System";
            });
        }

        private async void OnDistanceChanged(object sender, DistanceChangedEventArgs e)
        {
            if (e.Distance < 4d || e.Distance > 13d)
            {
                if (_isCloseToSensor)
                {
                    await ProcessVehicleFarFromSensorAsync();
                }

                _isCloseToSensor = false;
            }
            else
            {
                if (_isProcessing)
                {
                    return;
                }

                if (!_isCloseToSensor)
                {
                    await ProcessVehicleNearToSensorAsync();
                }

                _isCloseToSensor = true;
            }
        }

        private async Task ProcessVehicleNearToSensorAsync()
        {
            _isProcessing = true;
            await SetMessageTextAsync("Verificando sua placa...");

            var tcs = new TaskCompletionSource<bool>();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                var result = await Logic.GetPlateNumberAsync();

                tcs.SetResult(result);
            });

            var responseResult = await tcs.Task;

            if (!responseResult)
            {
                await RequestTagAsync();
            }
        }

        private async Task RequestTagAsync()
        {
            await SetMessageTextAsync("Aguardando sua tag...");

            var tag = await Logic.WaitForTagAsync();

            await SetMessageTextAsync("Verificando sua tag no sistema. Aguarde.");
            await Logic.SendTagToServerAsync(tag);
        }

        private async Task ProcessVehicleFarFromSensorAsync()
        {
            await Logic.RotateMotor(CloseAngle);
            await SetMessageTextAsync("Smart Parking System");
            _isProcessing = false;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await Logic.StartCameraAsync(PreviewElement);//(new CaptureElement());
            await Logic.StartMFRC522ComponentAsync();
            Logic.StartUltrasonicDistanceSensor();
            Logic.StartServoMotor();
            await Logic.RotateMotor(CloseAngle);
        }

        private async Task SetMessageTextAsync(string text)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                MessageTextBlock.Text = text;
            });
        }
    }
}
