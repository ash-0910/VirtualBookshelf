using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualBookshelf.Helper;
using VirtualBookshelf.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static VirtualBookshelf.Model.Books;

namespace VirtualBookshelf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BooksSearchPage : ContentPage
    {

        List<VolumeInfo> listinfo = new List<VolumeInfo>();
        HelperClass helper = new HelperClass();
        public BooksSearchPage()
        {
            InitializeComponent();
        }

        private async void searchBtn_Clicked(object sender, EventArgs e)
        {

            listinfo.Clear();
            BooksListView.ItemsSource = null;

            if (string.IsNullOrEmpty(searchentry.Text))
            {
                await DisplayAlert("Error", "search value is empty", "OK");
            }
            else
            {
                if (searchentry.Text.Length > 0 && string.IsNullOrEmpty(authorSearchEntry.Text))
                {
                    string search = searchentry.Text.Trim();

                    Root books = await helper.GetBooksByTitle(search);

                    List<Item> list = books.items;

                    foreach (var n in list)
                    {

                        try
                        {

                            // Item i = new Item();
                            VolumeInfo info = new VolumeInfo();
                            //ImageLinks image = new ImageLinks();

                            info.title = n.volumeInfo.title ?? "No title available";
                            // info.authors = n.volumeInfo.authors;

                            if (n.volumeInfo.authors == null)
                            {
                                info.authorString = "No author information";
                            }
                            else
                            {
                                info.authorString = String.Join(" , ", n.volumeInfo.authors?.ToArray());
                            }

                            // info.authorString = String.Join(" , ", n.volumeInfo.authors?.ToArray()) ?? "No author information";
                            // info.imageLinks = n.volumeInfo.imageLinks;
                            info.description = n.volumeInfo.description ?? "No description available";
                            info.publisher = n.volumeInfo.publisher ?? "No publisher information";
                            info.selfLink = n.selfLink ?? "no link available";
                            info.smallThumbnail = n.volumeInfo.imageLinks?.smallThumbnail ?? "https://icon-library.com/images/no-image-icon/no-image-icon-6.jpg";

                            //info.imageLinks.smallThumbnail = image.smallThumbnail;

                            listinfo.Add(info);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", ex.Message, "OK");
                        }



                    }
                }
                else if (searchentry.Text.Length > 0 && authorSearchEntry.Text.Length > 0)
                {
                    string title = searchentry.Text.Trim();
                    string authorName = authorSearchEntry.Text.Trim();

                    Root books = await helper.GetBooksByTitleAndAuthor(title,authorName);

                    List<Item> list = books.items;

                    foreach (var n in list)
                    {

                        try
                        {

                            // Item i = new Item();
                            VolumeInfo info = new VolumeInfo();
                            //ImageLinks image = new ImageLinks();

                            info.title = n.volumeInfo.title ?? "No title available";
                            // info.authors = n.volumeInfo.authors;

                            if (n.volumeInfo.authors == null)
                            {
                                info.authorString = "No author information";
                            }
                            else
                            {
                                info.authorString = String.Join(" , ", n.volumeInfo.authors?.ToArray());
                            }

                            // info.authorString = String.Join(" , ", n.volumeInfo.authors?.ToArray()) ?? "No author information";
                            // info.imageLinks = n.volumeInfo.imageLinks;
                            info.description = n.volumeInfo.description ?? "No description available";
                            info.publisher = n.volumeInfo.publisher ?? "No publisher information";
                            info.selfLink = n.selfLink ?? "no link available";
                            info.smallThumbnail = n.volumeInfo.imageLinks?.smallThumbnail ?? "https://icon-library.com/images/no-image-icon/no-image-icon-6.jpg";

                            //info.imageLinks.smallThumbnail = image.smallThumbnail;

                            listinfo.Add(info);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", ex.Message, "OK");
                        }



                    }

                }
                

                BooksListView.ItemsSource = listinfo;
            }

   
        }

        private async void BooksListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var details = e.Item as VolumeInfo;

                await Navigation.PushAsync(new BookDetailPage(details.selfLink));
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

        }

        public async void UpdateList()
        {
            
        }

       
    }
}