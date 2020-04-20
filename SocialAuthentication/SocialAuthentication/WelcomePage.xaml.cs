using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
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
        public WelcomePage()
        {
            InitializeComponent();
            gdBusy.IsVisible = true;
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
            }
            gdBusy.IsVisible = false;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            switch (LoginType)
            {
                case "facebook":
                    CrossFacebookClient.Current.Logout();
                    break;
            }
            Navigation.PopModalAsync();
        }
    }
}