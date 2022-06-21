using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualBookshelf.Model
{
    public class UserClass
    {
        public string Email { get; set; }
        public string Password { get; set; }

        
    }

    public class Book
    {
        public string ID { get; set; }
        public string selflink { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string imagelink { get; set; }
        public string author { get; set; }
        public string isOwned { get; set; }

        public string OwnedImage { get; set; }


    }

    public class root
    {
        public Book books { get; set; }
    }
}
