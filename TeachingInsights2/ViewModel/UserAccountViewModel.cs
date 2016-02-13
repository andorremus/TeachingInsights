using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingInsights2.ViewModel
{
    public class UserAccountViewModel : ViewModelBase
    {

        public UserAccountViewModel()
        {       
        }

        public User CurrentUser
        {
            get
            {
                return (User)App.Current.Properties["currentUser"];
            }
        }
    }
}
