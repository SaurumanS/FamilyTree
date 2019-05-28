using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationLogic.DataBaseTableInstances
{
    public class Account: INotifyPropertyChanged //Представление таблицы базы данных, содержащее логин, пароль и привелегии аккаунта 
    {
        private string login;
        private string password;
        private string nameFamily;
        private int isAdministrator; //Имеет ли аккаунт статус администратора?

        public int Id { get; set; }
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public string NameFamily
        {
            get { return nameFamily; }
            set
            {
                nameFamily = value;
                OnPropertyChanged("NameFamily");
            }
        }
        public int IsAdministrator
        {
            get { return isAdministrator; }
            set
            {
                isAdministrator = value;
                OnPropertyChanged("IsAdministrator");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
