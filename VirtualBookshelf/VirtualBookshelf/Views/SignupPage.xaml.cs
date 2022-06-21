using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualBookshelf.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirtualBookshelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {

        HelperClass helper = new HelperClass();

        string email;
        string password;

       

        public SignupPage()
        {
            InitializeComponent();
        }

       

        private async void SignUpButton_Clicked(object sender, EventArgs e)
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
                else if (string.IsNullOrEmpty(EntryConfirmPassword.Text))
                {
                    await DisplayAlert("Error", "Please confirm the Password", "OK");
                }
                else
                {
                    var usercheck = await helper.GetUser(email);

                    if (usercheck != null)
                    {
                        await DisplayAlert("User Already Exists", "Please Login", "OK");
                    }
                    else
                    {
                        var user = await helper.AddUser(email, password);

                        if (user)
                        {
                            await DisplayAlert("Success", "Account Created successfully. Please Login", "OK");
                        }

                    }


                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }


    }

}