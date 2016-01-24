using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TeachingInsights2.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand _LoginUser;
        
        public ICommand LoginUser
        {
            get
            {
                Console.WriteLine("Login User triggered");
                if (_LoginUser == null)
                {
                    _LoginUser = new RelayCommand(param => this.Submit(), null);
                }
                return _LoginUser;
            }
        } 

        private void Submit()
        {
            Console.WriteLine("Submit action triggered");
        }


    }
}
