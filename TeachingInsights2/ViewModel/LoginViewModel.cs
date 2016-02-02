using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeachingInsights2.Model;

namespace TeachingInsights2.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private ICommand _LoginUser;
        private String _username;
        public String _password { private get; set; }
        public event EventHandler LoginSuccessful;

        private void RaiseLoginSuccessfulEvent()
        {
            var handler = LoginSuccessful;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public String Username
        {
            get{ return _username; }
            set{ _username = value; }
        }

        public ICommand LoginUser
        {
            get
            { 
                if (_LoginUser == null)                
                    _LoginUser = new RelayCommand(param => this.Submit(), null);                
                return _LoginUser;
            }
        } 

        private void Submit()
        {
            Console.WriteLine(_username);
            Console.WriteLine(_password.ToString());
            Console.WriteLine("Submit action triggered");
            if (Authenticator.Authorize(_username, _password))
                RaiseLoginSuccessfulEvent();
            else
                ;
        }
        public LoginViewModel()
        {
           
        }


    }
}
