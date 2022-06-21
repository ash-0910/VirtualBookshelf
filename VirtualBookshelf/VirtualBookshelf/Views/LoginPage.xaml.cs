using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualBookshelf.Helper;
using VirtualBookshelf.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static VirtualBookshelf.Model.Books;

namespace VirtualBookshelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        UserClass user = new UserClass();

        HelperClass helper = new HelperClass();

        string email = "";
        string password = "";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void SignInButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                email = EntryUserEmail.Text.Trim();
                password = EntryPassword.Text.Trim();

                if (string.IsNullOrEmpty(EntryUserEmail.Text))
                {
                    await DisplayAlert("Error", "Please enter an amail", "OK");
                }
                else if (string.IsNullOrEmpty(EntryPassword.Text))
                {
                    await DisplayAlert("Error", "Please enter a Password", "OK");
                }
                else
                {
                    var user = await helper.GetUser(email);

                    if (user != null)
                    {
                        if (email == user.Email && password == user.Password)
                        {
                            Preferences.Set("Email", email);

                            await Navigation.PushAsync(new HomePage());
                            Navigation.RemovePage(this);

                        }
                        else
                        {
                            await DisplayAlert("Error", "Invalid email or password", "OK");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            
          


        }

        private void forgotpass_Tapped(object sender, EventArgs e)
        {

        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignupPage());
        }
    }
}