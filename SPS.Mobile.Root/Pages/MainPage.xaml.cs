using SPS.Mobile.Root.Common;
using SPS.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SPS.Mobile.Root.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int TokenExpirationTimeSeconds = 30;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private DispatcherTimer cleanTimer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();

            this.cleanTimer.Interval = TimeSpan.FromSeconds(TokenExpirationTimeSeconds);
            this.cleanTimer.Tick += OnCleanTimerTicked;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void OnGenerateTokenButtonClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.CPFTextBox.Text))
            {
                await new MessageDialog("Você precisa digitar seu CPF!", "CPF inválido").ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(this.PassTextBox.Password))
            {
                await new MessageDialog("Você precisa digitar sua senha!", "Senha inválida").ShowAsync();
                return;
            }

            string token;
            string cpf = this.CPFTextBox.Text;
            string rawPass = this.PassTextBox.Password;

            token = await Task.Run(() =>
            {
                string pass = HashServices.HashPassword(rawPass, cpf);

                return TokenGeneratorService.GenerateToken(pass);
            });

            this.TokenTextBlock.Text = token;
            this.TokenExpirationTextBlock.Text = "Válido até " + DateTime.Now.AddSeconds(TokenExpirationTimeSeconds).ToString("hh:mm:ss dd/MM/yyyy");
            this.cleanTimer.Start();
        }

        public void OnCleanTimerTicked(object sender, object e)
        {
            this.CPFTextBox.Text = 
            this.PassTextBox.Password =
            this.TokenTextBlock.Text = 
            this.TokenExpirationTextBlock.Text = string.Empty;
            this.cleanTimer.Stop();
        }
    }
}
