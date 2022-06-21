using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VirtualBookshelf.Model;
using static VirtualBookshelf.Model.Books;

namespace VirtualBookshelf.Helper
{
    public class HelperClass
    {
        FirebaseClient firebasedatabase = new FirebaseClient("https://virtualbookshelf-73717-default-rtdb.firebaseio.com/");
        HttpClient client = new HttpClient();

        UserClass user = new UserClass();

        string url = "https://www.googleapis.com/books/v1/volumes?q=";

        public async Task<bool> AddUser(string email, string password)
        {
            try
            {
              
                await firebasedatabase.Child("Users").PostAsync(new UserClass() { Email = email, Password = password });

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<UserClass>> GetAllUser()
        {
            try
            {
                var userlist = (await firebasedatabase
                .Child("Users")
                .OnceAsync<UserClass>()).Select(item =>
                new UserClass
                {
                    Email = item.Object.Email,
                    Password = item.Object.Password
                }).ToList();
               
                return userlist;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        //Read     
        public async Task<UserClass> GetUser(string email)
        {
            try
            {
                var allUsers = await GetAllUser();

               /* await firebasedatabase
                .Child("Users")
                .OnceAsync<UserClass>();*/

                return allUsers.Where(a => a.Email == email).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }


        public async Task<Root> GetBooksByTitle(string search)
        {
            List<string> ls = new List<string>();
            string[] multiplewords = search.Split(' ');

            ls = multiplewords.ToList();

            var sb = new System.Text.StringBuilder();


            for (int i = 0; i < ls.Count; i++)
            {
                sb.Append(ls[i] + "+");
   
            }

            string finalsearch = sb.ToString().TrimEnd('+');


            string searchString = url + finalsearch + "&projection=full&maxResults=20";

            var content = new System.Net.WebClient().DownloadString(searchString);


            Root Jsonobject = JsonConvert.DeserializeObject<Root>(content);


            //List<Item> list = Jsonobject.items;

            // var result = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(content);

          // Dictionary<string, Books> myData = JsonConvert.DeserializeObject<Dictionary<string, Books>>(content);
           // List<Books> myData = JsonConvert.DeserializeObject<List<Books>>(content);

            return Jsonobject;
            
        }

        public async Task<Root> GetBooksByTitleAndAuthor(string title , string author)
        {
            List<string> ls = new List<string>();
            List<string> authorlist = new List<string>();
            string[] multipletitle = title.Split(' ');
            string[] multipleauthor = author.Split(' ');

            ls = multipletitle.ToList();
            authorlist = multipleauthor.ToList();

            var sb = new System.Text.StringBuilder();
            var authorSB = new System.Text.StringBuilder();


            for (int i = 0; i < ls.Count; i++)
            {
                sb.Append(ls[i] + "+");

            }

            for (int i = 0; i < authorlist.Count; i++)
            {
                authorSB.Append(authorlist[i] + "+");

            }

            string authorstring = authorSB.ToString().TrimEnd('+');

            string titlestring = sb.ToString().TrimEnd('+');


            string searchString = url + titlestring + "+inauthor:" + authorstring + "&maxResults=40";

            var content = new System.Net.WebClient().DownloadString(searchString);


            Root Jsonobject = JsonConvert.DeserializeObject<Root>(content);


            //List<Item> list = Jsonobject.items;

            // var result = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(content);

            // Dictionary<string, Books> myData = JsonConvert.DeserializeObject<Dictionary<string, Books>>(content);
            // List<Books> myData = JsonConvert.DeserializeObject<List<Books>>(content);

            return Jsonobject;

        }

        public async Task<Item> GetBook(string selflink)
        {
            var content = new System.Net.WebClient().DownloadString(selflink);

            Item JsonObject = JsonConvert.DeserializeObject<Item>(content);

            return JsonObject;
        }

        public async Task<List<UserClass>> GetAllPersons()
        {

            return (await firebasedatabase
              .Child("Users")
              .OnceAsync<UserClass>()).Select(item => new UserClass
              {
                  Email = item.Object.Email
                 // PersonId = item.Object.PersonId
              }).ToList();
        }

        public async Task<UserClass> GetPerson(string email)
        {
            var allPersons = await GetAllPersons();
            await firebasedatabase
              .Child("Users")
              .OnceAsync<UserClass>();
            return allPersons.Where(a => a.Email == email).FirstOrDefault();
        }

        public async Task UpdateUserList(string email,string title,string author , string description , string image, string link, string isOwned, string ownedImage)
        {


            var toUpdatePerson = (await firebasedatabase
              .Child("Users")
              .OnceAsync<UserClass>()).Where(a => a.Object.Email == email).FirstOrDefault();

            await firebasedatabase
              .Child("Users")
              .Child(toUpdatePerson.Key).Child("Books")
              .PostAsync(new Book { title = title, author = author, description = description, imagelink = image, selflink = link , isOwned = isOwned });

 

        }

        public async Task<List<Book>> getUserList(string email)
        {

            var getpersondata = (await firebasedatabase
              .Child("Users")
              .OnceAsync<UserClass>()).Where(a => a.Object.Email == email).FirstOrDefault();

            var reponse = (await firebasedatabase.Child("Users")
           .Child(getpersondata.Key).Child("Books").OnceAsync<Book>()).Select(item => new Book
           {
               title = item.Object.title,
               description = item.Object.description,
               author = item.Object.author,
               imagelink = item.Object.imagelink,
               selflink = item.Object.selflink,
               isOwned = item.Object.isOwned ?? "Not Owned",
               OwnedImage = item.Object.OwnedImage


           }).ToList();

            return reponse;

          
          

           
        }

        public async Task<List<Book>> getOwnedandNotOwnedData(string email, string isowned)
        {

            var getpersondata = (await firebasedatabase
             .Child("Users")
             .OnceAsync<UserClass>()).Where(a => a.Object.Email == email).FirstOrDefault();

            var response = (await firebasedatabase.Child("Users").Child(getpersondata.Key)
                .Child("Books").OnceAsync<Book>()).Where(a => a.Object.isOwned == isowned).Select(item => new Book
                {
                    title = item.Object.title,
                    description = item.Object.description,
                    author = item.Object.author,
                    imagelink = item.Object.imagelink,
                    selflink = item.Object.selflink,
                    isOwned = item.Object.isOwned,
                    OwnedImage = item.Object.OwnedImage
                }).ToList();

            return response;
        }

       /* public async Task<Book> getBookById(string email, string id , string selflink)
        {
            var userData = (await firebasedatabase
             .Child("Users")
             .OnceAsync<UserClass>()).Where(a => a.Object.Email == email).FirstOrDefault();

            var userbook = (await firebasedatabase
             .Child("Users")
             .OnceAsync<Book>()).Where(a => a.Object.selflink == selflink).FirstOrDefault();

            var response = await firebasedatabase
             .Child("Users")
             .Child(userData.Key).Child("Books").Child(userbook.Key).OnceSingleAsync<Book>();

            return response;


        }*/

        public async Task UpdateBookByID(string email, string title, string author, string description, string image, string link, string isOwned , string ownedImage)
        {
            
                var userData = (await firebasedatabase
             .Child("Users")
             .OnceAsync<UserClass>()).Where(a => a.Object.Email == email).FirstOrDefault();

            var userbook = (await firebasedatabase
             .Child("Users").Child(userData.Key).Child("Books")
             .OnceAsync<Book>()).Where(a => a.Object.selflink == link).FirstOrDefault();

            await firebasedatabase
                 .Child("Users")
                 .Child(userData.Key).Child("Books").Child(userbook.Key)
                 .PutAsync(new Book { title = title, author = author, description = description, imagelink = image, selflink = link ,isOwned = isOwned , OwnedImage = ownedImage});
           
           

        }

        public async Task DeleteBook(string email, string link)
        {
            var userData = (await firebasedatabase
           .Child("Users")
           .OnceAsync<UserClass>()).Where(a => a.Object.Email == email).FirstOrDefault();

            var userbook = (await firebasedatabase
             .Child("Users").Child(userData.Key).Child("Books")
             .OnceAsync<Book>()).Where(a => a.Object.selflink == link).FirstOrDefault();

            await firebasedatabase
                .Child("Users")
                .Child(userData.Key).Child("Books").Child(userbook.Key).DeleteAsync();
        }


    }
}
