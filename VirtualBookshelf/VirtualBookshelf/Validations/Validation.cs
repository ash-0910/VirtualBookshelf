using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace VirtualBookshelf.Validations
{
    public class Validation
    {



        public bool CheckConPassword(Entry password, Entry ConfirmPass,Label ConfirmPasslbl)
        {
            var length = (ConfirmPass.Text ?? "").Length;
            bool isValid = length >= 6;

           
            var pass = password.Text;
            string equalstate = "invalid";
            if (pass.Equals(ConfirmPass.Text) && length >= 6)
            {
                equalstate = "equal";
            }

            
            return isValid;
        }

        public bool CheckEmailValid( Entry Email , Label emailmsg)
        {
            bool isValid = IsValidEmail(Email.Text);



            return isValid;
        }

        /* private bool CheckPasswordValid()
         {
             var pwdLength = (EntPassword.Text ?? "").Length;
             bool isValid = pwdLength >= 6;


             //var pwd = @"(?=^.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$";

             var state = isValid ? "valid" : "invalid";
             VisualStateManager.GoToState(EntPassword, state);

             //check strength
             string strengthstate = "invalid";
             if (pwdLength >= 10)
             {
                 strengthstate = "strong";
             }
             else if(pwdLength >= 6 )
             {
                 strengthstate = "weak";
             }
             VisualStateManager.GoToState(passwordmsg, strengthstate);

             return isValid;
         }*/

        public bool ValidatePassword( Entry password , Label passwordmsg)
        {

            var input = password.Text;


            string strengthstate = "invalid";

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                strengthstate = "lowercase";

            }
            else if (!hasUpperChar.IsMatch(input))
            {
                strengthstate = "uppercase";

            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                strengthstate = "greater";

            }
            else if (!hasNumber.IsMatch(input))
            {
                strengthstate = "numeric";

            }

            else if (!hasSymbols.IsMatch(input))
            {
                strengthstate = "specialchar";

            }
            else
            {
                strengthstate = "valid";
            }


            bool isValid = strengthstate.Equals("valid");
           

            return isValid;



        }

      
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
