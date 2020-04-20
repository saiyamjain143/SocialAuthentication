using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialAuthentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public static string LoginType;
        GoogleUser _googleUser;
        public WelcomePage(GoogleUser data = null)
        {
            InitializeComponent();
            gdBusy.IsVisible = true;
            _googleUser = data;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            switch (LoginType)
            {
                case "facebook":
                    var data = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name" }, new string[] { "email"});
                    JObject json = JObject.Parse(data.Data);
                    lblName.Text = json["first_name"] + " " + json["last_name"];
                    lblEmail.Text = json["email"].ToString();
                    break;

                case "instagram":
                    break;

                case "google":
                    lblName.Text = _googleUser.Name;
                    lblEmail.Text = _googleUser.Email;
                    imgProfile.Source = _googleUser.Picture;
                    break;
            }
            gdBusy.IsVisible = false;
        }

        private void LogOut_Clicked(object sender, EventArgs e)
        {
            switch (LoginType)
            {
                case "facebook":
                    CrossFacebookClient.Current.Logout();
                    break;

                case "instagram":
                    CrossFacebookClient.Current.Logout();
                    break;

                case "google":
                    CrossGoogleClient.Current.Logout();
                    break;
            }
            Navigation.PopModalAsync();
        }
    }
}