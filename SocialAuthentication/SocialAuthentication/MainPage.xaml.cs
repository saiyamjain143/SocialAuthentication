using Plugin.FacebookClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnFacebook_Clicked(object sender, EventArgs e)
        {
            switch ((sender as Button).StyleId)
            {
                case "btnFacebook":
                    WelcomePage.LoginType = "facebook";
                    FacebookResponse<bool> response = await CrossFacebookClient.Current.LoginAsync(new string[] { "email", "public_profile" });
                    if (response.Data) await Navigation.PushModalAsync(new WelcomePage());
                    break;
            }
        }
    }
}
