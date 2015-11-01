using SPS.Raspberry.Core.MFRC522;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SPS.Raspberry
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MFRC522 _mfrc522;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            _mfrc522 = new MFRC522(BoardConfig.MFRC522CmdPin, BoardConfig.MFRC522ResetPin);
            await _mfrc522.InitAsync(BoardConfig.MFRC522SpiControllerName, BoardConfig.MFRC522SpiSelectChipLine);
            await Task.Factory.StartNew(WaitForTag);
        }

        private async Task WaitForTag()
        {
            while (!_mfrc522.IsTagPresent())
            {
                await Task.Delay(50);
            }

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var tag = _mfrc522.ReadUid();

                TagUUIDTextBlock.Text = $"Tag found: {tag}";
            });
        }
    }
}
