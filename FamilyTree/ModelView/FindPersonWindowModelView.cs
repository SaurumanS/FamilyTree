using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Person = ApplicationLogic.DataBaseTableInstances.Person;

namespace FamilyTree.ModelView
{
    class FindPersonWindowModelView:INotifyPropertyChanged
    {
        ApplicationLogic.OperationsWithDataBase.PersonTable operationWithDataBase;
        public FindPersonWindowModelView()
        {
            ReturnSearchingResult = new ObservableCollection<Person>();
            operationWithDataBase = new ApplicationLogic.OperationsWithDataBase.PersonTable();
            
        }

        public ObservableCollection<Person> ReturnSearchingResult { get; set; }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        private string secondName;
        public string SecondName
        {
            get { return secondName; }
            set
            {
                secondName = value;
                OnPropertyChanged("SecondName");
            }
        }
        private string patronymic;
        public string Patronymic
        {
            get { return patronymic; }
            set
            {
                patronymic = value;
                OnPropertyChanged("Patronymic");
            }
        }

        public Person SelectedItem { get; set; }


        private RelayCommand sendDataCommand;
        public RelayCommand SendDataCommand
        {
            get
            {
                return sendDataCommand ??
                    (sendDataCommand = new RelayCommand(obj =>
                    {
                        List<Person> result = operationWithDataBase.ReturnSearchingResult(SecondName, FirstName, Patronymic);
                        ReturnSearchingResult.Clear();
                        if (result.Count == 0)
                        {
                            MessageBox.Show("Ничего не найдено\nИзмените условия поиска или заведите новый профиль", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        foreach (var temp in result) ReturnSearchingResult.Add(temp);

                    }));
            }
        }
        private RelayCommand goToProfileCommand;
        public RelayCommand GoToProfileCommand
        {
            get
            {
                return goToProfileCommand ??
                    (goToProfileCommand = new RelayCommand(obj =>
                    {
                        Windows.PersonWindow newPersonWindow = new Windows.PersonWindow(SelectedItem,1);
                        newPersonWindow.Show();
                    }));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
