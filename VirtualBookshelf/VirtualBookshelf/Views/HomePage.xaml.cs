using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class HomePage : ContentPage
    {

        public ObservableCollection<Book> Items { get; set; }
        public ObservableRangeCollection<Book> AllItems { get; set; }
        public ObservableRangeCollection<string> FilterOptions { get; }

        public Command TouchCommand { get; }

        List<string> pickerlist = new List<string>();

        List<Book> allList = new List<Book>();

        HelperClass helper = new HelperClass();
        public HomePage()
        {
            InitializeComponent();

            pickerlist.Add("All");
            pickerlist.Add("Owned");
            pickerlist.Add("Not Owned");

            ownedpicker.ItemsSource = pickerlist;

            getData();

            

            BooksListView.RefreshCommand = new Command(() =>{
                getData();
            });
            

        }


        public async void getData()
        {
            
            try
            {
                string email = Preferences.Get("Email", "");
                var selected = ownedpicker.SelectedIndex;

                if (selected.Equals(-1))
                {
                    allList = await helper.getUserList(email);

                    Items = new ObservableCollection<Book>(allList);

                    BooksListView.ItemsSource = Items;
                    BooksListView.IsRefreshing = false;
                }
                else if (selected.Equals(0))
                {
                    allList = await helper.getUserList(email);

                    Items = new ObservableCollection<Book>(allList);

                    BooksListView.ItemsSource = Items;
                    BooksListView.IsRefreshing = false;
                }
                else if (selected.Equals(1))
                {
                    var res = await helper.getOwnedandNotOwnedData(email, "Owned");
                    BooksListView.ItemsSource = res;
                    BooksListView.IsRefreshing = false;
                }
                else if (selected.Equals(2))
                {
                    var res = await helper.getOwnedandNotOwnedData(email, "Not Owned");
                    BooksListView.ItemsSource = res;
                    BooksListView.IsRefreshing = false;
                }


                
                
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }


      /*  private async void BooksListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                bool mylist = true;

                var details = e.Item as Book;

                await Navigation.PushAsync(new BookDetailPage(details.selflink, mylist));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }*/

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }

   

        private void notOwned_Invoked(object sender, EventArgs e)
        {
           
            string isOwned = "Not Owned";
            string ownedImage = "https://cdn-icons-png.flaticon.com/512/399/399274.png";


            swipeInvoke(sender, isOwned, ownedImage);

        }

        private void owned_Invoked(object sender, EventArgs e)
        {
            string isOwned = "Owned";
            string ownedImage = "https://cdn-icons.flaticon.com/png/512/4315/premium/4315445.png?token=exp=1655705709~hmac=abc3ceeda0aff73a6f7a51a72ad8591d";

            swipeInvoke(sender, isOwned, ownedImage);
        }

        public async void swipeInvoke(object sender, string isOwned , string ownedImage)
        {
            string email = Preferences.Get("Email", "");

            var menu = sender as MenuItem;

            var b = menu.CommandParameter as Book;

            if(b.isOwned == "Owned" && isOwned == "Owned")
            {
               await DisplayAlert("Book already owned", "You already Own this Book", "OK");
            }
            else if(b.isOwned== "Not Owned" && isOwned == "Not Owned")
            {
                await DisplayAlert("Book not owned", "You have already marked this Book as not owned. Please swipe the list item right if you wish to mark it as owned", "OK");
            }
            else
            {
                try
                {

                    await helper.UpdateBookByID(email, b.title, b.author, b.description, b.imagelink, b.selflink, isOwned, ownedImage);

                    getData();
                    await DisplayAlert("Success", "List Updated Successfully", "OK");

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
          
        }

        private void ownedpicker_SelectedIndexChanged(object sender, EventArgs e)
        {

            getData();

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            string email = Preferences.Get("Email", "");

            string link = ((TappedEventArgs)e).Parameter.ToString();

            var res = await DisplayAlert("Delete", "Do you want to delete this?", "Yes", "No");

            if (res)
            {
                await helper.DeleteBook(email, link);
                await DisplayAlert("Deleted", "Book has been deleted from your list", "OK");
                getData();
            }
            
        }

        private void searchIcon_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BooksSearchPage());
        }

        private async void summaryTapped_Tapped(object sender, EventArgs e)
        {
            try
            {
                bool mylist = true;

                string link = ((TappedEventArgs)e).Parameter.ToString();

                await Navigation.PushAsync(new BookDetailPage(link, mylist));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}