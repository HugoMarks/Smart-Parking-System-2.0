using SPS.Raspberry.Core.Camera;
using SPS.Raspberry.Core.MFRC522;
using SPS.Raspberry.Core.ServoMotor;
using SPS.Raspberry.Core.UltrasonicSensor;
using SPS.Raspberry.DataObject;
using SPS.Raspberry.Extensions;
using SPS.Raspberry.Recognition;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace SPS.Raspberry.Logic
{
    /// <summary>
    /// Logic for the <see cref="Pages.StartPage"/> page.
    /// </summary>
    public class StartPageLogic
    {
        private const string ServerUrl = "http://" + Constants.ServerAddress + "/api/authorize";

        private MFRC522 _mfrc522;
        private UltrasonicDistanceSensor _distanceSensor;
        private PhotoCam _camera;
        private ServoMotor _motor;
        private HttpClient _httpClient;

        /// <summary>
        /// Fires when a new <see cref="AuthResponse"/> is received.
        /// </summary>
        public event EventHandler<AuthResponseEventArgs> NewResponse;

        /// <summary>
        /// Fires when a new <see cref="TagUid"/> is received.
        /// </summary>
        public event EventHandler<TagPresentEventArgs> TagPresent;

        public event EventHandler<DistanceChangedEventArgs> DistanceChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartPageLogic"/> class.
        /// </summary>
        public StartPageLogic()
        {
            _mfrc522 = new MFRC522(BoardConfig.MFRC522CmdPin, BoardConfig.MFRC522ResetPin);
            _distanceSensor = new UltrasonicDistanceSensor(BoardConfig.UltrasonicSensorTriggerPin, BoardConfig.UltrasonicSensorEchoPin);
            _camera = new PhotoCam();
            _motor = new ServoMotor(BoardConfig.ServoMotorPin);
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Starts the <see cref="MFRC522"/> component.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public async Task StartMFRC522ComponentAsync()
        {
            await _mfrc522.InitAsync(BoardConfig.MFRC522SpiControllerName, BoardConfig.MFRC522SpiSelectChipLine);
            StartTagListener();
        }

        public void StartUltrasonicDistanceSensor()
        {
            Task.Factory.StartNew(() =>
            {
                while (_distanceSensor != null)
                {
                    var distance = _distanceSensor.GetDistance();

                    if (DistanceChanged != null)
                    {
                        DistanceChanged(this, new DistanceChangedEventArgs(distance));
                    }
                }
            });
        }

        public void StartServoMotor()
        {
            _motor.Init();
        }

        public async Task StartCameraAsync(CaptureElement captureElement)
        {
            await _camera.InitAsync(captureElement);
        }

        public async Task<bool> GetPlateNumberAsync()
        {
            var plateRecognizer = new PlateRecognizer();

            using (var stream = new InMemoryRandomAccessStream())
            {
                await _camera.TakePhotoToStreamAsync(stream);

                var plateImage =  await plateRecognizer.RecognizePlateAsync(stream);

                if (plateImage == null)
                {
                    return false;
                }

                var base64Image = await plateImage.ToBase64StringAsync();

                if (base64Image == null)
                {
                    return false;
                }

                var request = new AuthRequest
                {
                    CarPlate = base64Image,
                    ParkingCNPJ = "46.451.585/0001-67"
                };

                await SendAuthRequestAsync(request);
            }

            return true;
        }

        public async Task RotateMotor(double angle)
        {
            await _motor.RotateAsync(angle);
        }

        /// <summary>
        /// Starts listening for present tags.
        /// </summary>
        private async void StartTagListener()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    while (!_mfrc522.IsTagPresent())
                    {
                        await Task.Delay(50);
                    }

                    var tagUid = _mfrc522.ReadUid();
                    var request = new AuthRequest();

                    request.TagId = tagUid.ToString();
                    RaiseNewTagEvent(tagUid);
                }
            });
        }

        public async Task<TagUid> WaitForTagAsync()
        {
            TaskCompletionSource<TagUid> _tagTask = new TaskCompletionSource<TagUid>();

            TagPresent += (s, e) =>
            {
                _tagTask.TrySetResult(e.TagUid);
            };

            return await _tagTask.Task;
        }

        public async Task SendTagToServerAsync(TagUid tag)
        {
            await SendAuthRequestAsync(new AuthRequest() { TagId = tag.ToString(), CarPlate = "", ParkingCNPJ = "46.451.585/0001-67" });
        }

        /// <summary>
        /// Sends an authorization request to the server.
        /// </summary>
        /// <param name="request">The request to send.</param>
        /// <returns><see cref="Task"/></returns>
        private async Task SendAuthRequestAsync(IRequest request)
        {
            try
            {
                var content = new HttpStringContent(request.Serialize());

                content.Headers["Content-Type"] = "application/json";

                var httpResponse = await _httpClient.PostAsync(new Uri(ServerUrl, UriKind.Absolute), content);
                var response = new AuthResponse() { StatusCode = httpResponse.StatusCode };
                var buffer = await httpResponse.Content.ReadAsBufferAsync();
                var bytes = buffer.ToArray();

                response.Deserialize(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                RaiseNewResponseEvent(response);
            }
            catch
            {
                var response = new AuthResponse { StatusCode = HttpStatusCode.InternalServerError };

                RaiseNewResponseEvent(response);
            }
        }

        /// <summary>
        /// Raises the <see cref="StartPageLogic.NewResponse"/> event.
        /// </summary>
        /// <param name="response">The response to be included in the event data.</param>
        private void RaiseNewResponseEvent(AuthResponse response)
        {
            if (NewResponse != null)
            {
                NewResponse(this, new AuthResponseEventArgs(response));
            }
        }

        /// <summary>
        /// Raises the <see cref="StartPageLogic.NewResponse"/> event.
        /// </summary>
        /// <param name="tagUid">The response to be included in the event data.</param>
        private void RaiseNewTagEvent(TagUid tagUid)
        {
            if (TagPresent != null)
            {
                TagPresent(this, new TagPresentEventArgs(tagUid));
            }
        }
    }
}
