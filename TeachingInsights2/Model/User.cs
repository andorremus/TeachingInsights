using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TeachingInsights2.Model
{
    public class User : INotifyPropertyChanged
    {
        private String username;
        private String name;
        private String password;
        
        public String Username
        {
            get { return username;}
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public String Name
        {
            get { return name;}
            set 
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }



        #region INotifyPropertyChanged Members
       public event PropertyChangedEventHandler PropertyChanged;
       private void OnPropertyChanged(string propertyName)
       {
           if (PropertyChanged != null)
           {
               PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
           }
       }
       #endregion
    }

    
}
