using SPS.Raspberry.Core.MFRC522;
using SPS.Raspberry.Logic;
using System;
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

        private const double Close = 160;
        private const double Open = 70;
        private bool _rotateMotor = false;
        private bool _isProcessing = false;

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
                        await Logic.RotateMotor(Open);
                    }
                    else
                    {
                        MessageTextBlock.Text = "Até logo, " + e.Reponse.UserName;
                        await Logic.RotateMotor(Close);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
                else if (status == HttpStatusCode.BadRequest)
                {
                    MessageTextBlock.Text = "Não há mais vagas disponíveis";
                    _rotateMotor = false;
                }
                else if (status == HttpStatusCode.Unauthorized)
                {
                    MessageTextBlock.Text = "Falha na autenticação";
                    _rotateMotor = false;
                }
                else
                {
                    MessageTextBlock.Text = "Oops, um erro aconteceu";
                    _rotateMotor = false;
                }
            });
        }

        private async void OnDistanceChanged(object sender, DistanceChangedEventArgs e)
        {
            if (_rotateMotor)
            {
                if (e.Distance < 4d || e.Distance > 13d)
                {
                    await Logic.RotateMotor(Close);
                    _rotateMotor = false;
                    _isProcessing = false;
                }

                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    MessageTextBlock.Text = "Aguarde a leitura de sua placa...";
                });

                return;
            }

            if (e.Distance >= 4d && e.Distance <= 13d && !_isProcessing)
            {
                _isProcessing = true;

                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    MessageTextBlock.Text = "Aguarde a leitura de sua placa...";
                    //Simulates the plate reading...
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    MessageTextBlock.Text = "Esperando por uma tag...";

                    var tag = await Logic.WaitForTag();

                    _rotateMotor = true;
                    MessageTextBlock.Text = "Consultando sua tag no sistema. Aguarde";
                    await Logic.SendTagToServerAsync(tag);
                });

                _isProcessing = false;
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            //await Logic.StartCameraAsync(PreviewElement);
            await Logic.StartMFRC522ComponentAsync();
            Logic.StartUltrasonicDistanceSensor();
            Logic.StartServoMotor();
            await Logic.RotateMotor(Close);
        }

    }
}
