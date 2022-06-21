using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VirtualBookshelf.Helper;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static VirtualBookshelf.Model.Books;

namespace VirtualBookshelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookDetailPage : ContentPage
    {

        HelperClass helper = new HelperClass();

        string imageURL, detailLink;
        public BookDetailPage(string selflink ,[Optional] bool mylist)
        {
            InitializeComponent();

            if (mylist)
            {
                addToListBtn.IsVisible = false;
                addToListBtn.IsEnabled = false;
            }

            /*  string image = imagelinks.thumbnail;


              bookTitle.Text = title;
              bookDescription.Text = description;
              bookAuthors.Text = String.Join(",", authors.ToArray());
              bookimage.Source = image;*/

            bookDetails(selflink);

 
        }


        public async void bookDetails(string selflink)
        {
            try
            {
                Item book = await helper.GetBook(selflink);

                //VolumeInfo bookdetails = new VolumeInfo();


                bookTitle.Text = book.volumeInfo.title;
                string s = Regex.Replace(book.volumeInfo.description??"No Description available", "<.*?>|&.*?;", String.Empty);
                bookDescription.Text = s;
                if(book.volumeInfo.authors == null)
                {
                    bookAuthors.Text = "No author information";
                }
                else
                {
                    bookAuthors.Text = String.Join(",", book.volumeInfo.authors.ToArray());
                }

                imageURL = book.volumeInfo.imageLinks?.thumbnail ?? "https://icon-library.com/images/no-image-icon/no-image-icon-6.jpg";

                bookimage.Source = imageURL;
                detailLink = book.selfLink;
                
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

        }

        private async void addToListBtn_Clicked(object sender, EventArgs e)
        {

            string email = Preferences.Get("Email", "");
            string isOwned = "Not Owned";
            string ownedImage = "https://img.icons8.com/fluency/48/undefined/cancel.png";
            try
            {
                await helper.UpdateUserList(email, bookTitle.Text, bookAuthors.Text, bookDescription.Text, imageURL , detailLink, isOwned , ownedImage);

                await DisplayAlert("Success", "Book added to your wishlist. Please refresh your list to mark it as owned or not owned ", "OK");

            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }



        }
    }
}