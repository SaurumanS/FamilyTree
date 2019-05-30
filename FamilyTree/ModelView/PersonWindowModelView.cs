using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FamilyTree.ModelView
{
    class PersonWindowModelView: INotifyPropertyChanged
    {
        public PersonWindowModelView(ApplicationLogic.DataBaseTableInstances.Person person, int isAdministrator)
        {
            FullName = String.Join(" ", person.FirstName, person.SecondName, person.Patronymic);
            Title = FullName;
            if (isAdministrator == 1) ChangeIsEnable = true;
            else ChangeIsEnable = false;
            PhotoPath = person.PathPhoto;
            DateOfBirth = person.DateOfBirth;
            DateOfDeath = person.DateOfDeath;
            Gender = person.Gender;
            Father = person.Father;
            Mother = person.Mother;
            if (Gender == "Мужчина")
            {
                WifeOrHusband = "Жена: ";
                Spouse = person.Wife;
            }
            else
            {
                WifeOrHusband = "Муж: ";
                Spouse = person.Husband;
            }
            Children = String.Join(",", person.Sons, person.Dauthers);
            Description = person.Description;
        }
        public Action CloseAction { get; set; } // Действие на закрытие окна
        private RelayCommand closeWindowCommand;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return closeWindowCommand ??
                       (closeWindowCommand = new RelayCommand(obj => CloseAction()));
            }
        }
        public string Title { get; set; }
        public bool ChangeIsEnable { get; set; }
        private string fullName;
        private string photoPath;
        private string dateOfBirth;
        private string dateOfDeath;
        private string gender;
        private string father;
        private string mother;
        private string spouse;//Муж или жена
        private string children;
        private string description;
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }
        public string PhotoPath
        {
            get { return photoPath; }
            set
            {
                photoPath = value;
                OnPropertyChanged("PhotoPath");
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
            get
            {

                return mother;
            }
            set
            {
                mother = value;
                OnPropertyChanged("Mother");
            }
        }
        public string Spouse
        {
            get { return spouse; }
            set
            {
                spouse = value;
                OnPropertyChanged("Spouse");
            }
        }
        public string Children
        {
            get { return children; }
            set
            {
                children = value;
                OnPropertyChanged("Children");
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

        private string wifeOrHusband;//В поле для супруга вписывает (жена или муж)
        public string WifeOrHusband
        {
            get { return wifeOrHusband; }
            set
            {
                wifeOrHusband = value;
                OnPropertyChanged("WifeOrHusband");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
