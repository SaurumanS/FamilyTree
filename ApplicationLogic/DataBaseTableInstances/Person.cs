using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLogic.DataBaseTableInstances
{
    public class Person: INotifyPropertyChanged //Класс, описывающий какого либо человека
    {
        private string firstName;
        private string secondName;
        private string patronymic;
        private string dateOfBirth;
        private string dateOfDeath;
        private string gender;
        private string father;
        private string mother;
        private string wife;
        private string husband;
        private string sons;
        private string dauthers;
        private string description;
        private string pathPhoto;

        public int Id { get; set; }
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string SecondName
        {
            get { return secondName; }
            set
            {
                secondName = value;
                OnPropertyChanged("SecondName");
            }
        }
        public string Patronymic
        {
            get { return patronymic; }
            set
            {
                patronymic = value;
                OnPropertyChanged("Patronymic");
            }
        }
        public string DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }
        public string DateOfDeath
        {
            get { return dateOfDeath; }
            set
            {
                dateOfDeath = value;
                OnPropertyChanged("DateOfDeath");
            }
        }
        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }
        public string Father
        {
            get { return father; }
            set
            {
                father = value;
                OnPropertyChanged("Father");
            }
        }
        public string Mother
        {
            get { return mother; }
            set
            {
                mother = value;
                OnPropertyChanged("Mother");
            }
        }
        public string Wife
        {
            get { return wife; }
            set
            {
                wife = value;
                OnPropertyChanged("Wife");
            }
        }
        public string Husband
        {
            get { return husband; }
            set
            {
                husband = value;
                OnPropertyChanged("Husband");
            }
        }
        public string Sons
        {
            get { return sons; }
            set
            {
                sons = value;
                OnPropertyChanged("Sons");
            }
        }
        public string Dauthers
        {
            get { return dauthers; }
            set
            {
                dauthers = value;
                OnPropertyChanged("Dauthers");
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        public string PathPhoto
        {
            get { return pathPhoto; }
            set
            {
                pathPhoto = value;
                OnPropertyChanged("PathPhoto");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
