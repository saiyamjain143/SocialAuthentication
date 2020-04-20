using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SocialAuthentication
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        IGoogleClientManager _googleService = CrossGoogleClient.Current;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void BtnSocial_Clicked(object sender, EventArgs e)
        {
            switch ((sender as Button).StyleId)
            {
                case "btnFacebook":
                    WelcomePage.LoginType = "facebook";
                    FacebookResponse<bool> response = await CrossFacebookClient.Current.LoginAsync(new string[] { "email", "public_profile" });
                    if (response.Data) await Navigation.PushModalAsync(new WelcomePage());
                    break;

                case "btnInstagram":
                    WelcomePage.LoginType = "instagram";
                    break;

                case "btnGoogle":
                    WelcomePage.LoginType = "google";
                    await LoginGoogleAsync();
                    break;
            }
        }

        async Task LoginGoogleAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(_googleService.ActiveToken))
                {
                    //Always require user authentication
                    _googleService.Logout();
                }
                EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
                {
                    switch (e.Status)
                    {
                        case GoogleActionStatus.Completed:
                            await App.Current.MainPage.Navigation.PushModalAsync(new WelcomePage(e.Data));
                            break;
                        case GoogleActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
                            break;
                        case GoogleActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                            break;
                        case GoogleActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
                            break;
                    }
                    _googleService.OnLogin -= userLoginDelegate;
                };
                _googleService.OnLogin += userLoginDelegate;
                await _googleService.LoginAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
