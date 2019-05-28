using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApplicationLogic;

namespace FamilyTree.ModelView
{
    class PasswordWindowModelView: INotifyPropertyChanged
    {
        ApplicationLogic.OperationsWithDataBase.FamilyTable familyTable;
        ApplicationLogic.OperationsWithDataBase.AccountTable accountTable;
        public Action CloseAction { get; set; } // Действие на закрытие окна
        public PasswordWindowModelView()
        {
            familyTable = new ApplicationLogic.OperationsWithDataBase.FamilyTable();
            accountTable = new ApplicationLogic.OperationsWithDataBase.AccountTable();
        }
        public IEnumerable<ApplicationLogic.DataBaseTableInstances.Family> Families //Передача списка фамилий в comboBox
        {
            get
            {
                return familyTable.RerurnListFamilyes;
            }
        }
        private string comboBoxSelected;
        public string ComboBoxSelected
        {
            get { return comboBoxSelected; }
            set
            {
                comboBoxSelected = value;
                OnPropertyChanged("ComboBoxSelected");
            }
        }
        private string textBoxLogin;
        public string TextBoxLogin //Хранит текст, введенный в поле для ввода логина
        {
            get { return textBoxLogin; }
            set
            {
                textBoxLogin = value;
                OnPropertyChanged("TextBoxLogin");
            }
        }
        private string textBoxPassword;
        public string TextBoxPassword //Хранит текст, введенный в поле для ввода пароля
        {
            get { return textBoxPassword; }
            set
            {
                textBoxPassword = value;
                OnPropertyChanged("TextBoxPassword");
            }
        }
        private RelayCommand Command;
        public RelayCommand LoginCommand
        {
            get
            {
                return Command ??
                    (Command = new RelayCommand(obj =>
                    {
                        bool result = accountTable.IsCorrectAccount(TextBoxLogin, TextBoxPassword, ComboBoxSelected);
                        if (result)
                        {
                            MessageBox.Show("Вход выполнен\nДобро пожаловать", "Приветствие", MessageBoxButton.OK, MessageBoxImage.Information);
                            CloseAction();
                        }
                        else MessageBox.Show("Были введены неверные данные\nПовторите попытку", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
