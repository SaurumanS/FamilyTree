using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Person = ApplicationLogic.DataBaseTableInstances.Person;
using PersonTable = ApplicationLogic.OperationsWithDataBase.PersonTable;
using FamilyTable = ApplicationLogic.OperationsWithDataBase.FamilyTable;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;

namespace FamilyTree.ModelView
{
    class AddOrEditInfoAboutPersonWindowModelView: Person, INotifyPropertyChanged
    {
        PersonTable dataBase;
        FamilyTable familyesTable;
        public AddOrEditInfoAboutPersonWindowModelView()
        {
            Title = "Добавление профиля";
            Initialization();
        }
        public AddOrEditInfoAboutPersonWindowModelView(Person person)
        {
            Title = "Изменение профиля";
            Initialization();
        }
        public Action CloseAction { get; set; } // Действие на закрытие окна
        private void Initialization()//Инициализация списков, начальные состояний объектов и тд.
        {
            CheckBoxFatherIsChecking = true;
            ComboBoxFatherFirstNameIsEnable = false;
            ComboBoxFatherPatronymicsIsEnable = false;
            CheckBoxMotherIsChecking = true;
            ComboBoxMotherFirstNameIsEnable = false;
            ComboBoxMotherPatronymicsIsEnable = false;
            CheckBoxSpouseIsChecking = true;
            ComboBoxSpouseFirstNameIsEnable = false;
            ComboBoxSpousePatronymicsIsEnable = false;
            deleteSonFromComboBoxButtonIsEnable = true;
            ComboBoxSonsFirstNameIsEnable = false;
            ComboBoxSonsPatronymicsIsEnable = false;
            CheckBoxSonsIsChecking = true;
            deleteDautherFromComboBoxButtonIsEnable = true;
            ComboBoxDauthersFirstNameIsEnable = false;
            ComboBoxDauthersPatronymicsIsEnable = false;
            CheckBoxDauthersIsChecking = true;
            spouse = "Муж/Жена";
            dataBase = new PersonTable();
            familyesTable = new FamilyTable();
            comboBoxFatherSecondNamesList = new ObservableCollection<string>();
            ComboBoxFatherFirstNamesList = new ObservableCollection<string>();
            ComboBoxFatherPatronymicsList = new ObservableCollection<string>();
            comboBoxMotherSecondNamesList = new ObservableCollection<string>();
            ComboBoxMotherFirstNamesList = new ObservableCollection<string>();
            ComboBoxMotherPatronymicsList = new ObservableCollection<string>();
            comboBoxSpouseSecondNamesList = new ObservableCollection<string>();
            ComboBoxSpouseFirstNamesList = new ObservableCollection<string>();
            ComboBoxSpousePatronymicsList = new ObservableCollection<string>();
            comboBoxSonsSecondNamesList = new ObservableCollection<string>();
            ComboBoxSonsFirstNamesList = new ObservableCollection<string>();
            ComboBoxSonsPatronymicsList = new ObservableCollection<string>();
            CheckBoxSonsList = new ObservableCollection<string>();
            comboBoxDauthersSecondNamesList = new ObservableCollection<string>();
            ComboBoxDauthersFirstNamesList = new ObservableCollection<string>();
            ComboBoxDauthersPatronymicsList = new ObservableCollection<string>();
            CheckBoxDauthersList = new ObservableCollection<string>();
        }
        public string Title { get; set; }
        private List<string> GetPatronymics(string secondName, string firstName, bool isMan)
        {
            return dataBase.ReturnPatronymicsMembersFamily(secondName, firstName, isMan);
        }
        private List<string> GetFirstNames(string secondName, bool isMan)
        {
            return dataBase.ReturnFirstNameMembersFamily(secondName, isMan);
        }
        private List<string> ReturnAllSecondNamesInDataBase
        {
            get { return dataBase.ReturnAllSecondNamesInDataBase; }
        }
        public IEnumerable<string> ReturnAllFamilyesInDataBase
        {
            get { return familyesTable.RerurnListFamilyes.Select(x=>x.Name); }
        }
        #region MenuCommand
        private Person FromThisToPerson()
        {
            Person newPerson = new Person();
            newPerson.DateOfBirth = this.DateOfBirth;
            newPerson.DateOfDeath = this.DateOfDeath;
            newPerson.Dauthers = this.Dauthers;
            newPerson.Description = this.Description;
            newPerson.Father = this.Father;
            newPerson.FirstName = this.FirstName;
            newPerson.Gender = this.Gender;
            newPerson.Husband = this.Husband;
            newPerson.Mother = this.Mother;
            newPerson.PathPhoto = this.PathPhoto;
            newPerson.Patronymic = this.Patronymic;
            newPerson.SecondName = this.SecondName;
            newPerson.Sons = this.Sons;
            newPerson.Wife = this.Wife;
            return newPerson;
        }
        private bool SaveMainPhotoInNewFolder(string path)
        {
            if (String.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return false;
            }
            string currentDirectory = Environment.CurrentDirectory + "\\PhotoBase\\" + Family;
            int counter = 1;
            bool folderIsCreated = Directory.Exists(currentDirectory);//Создана ли общая семейная папка
            if (!folderIsCreated) Directory.CreateDirectory(currentDirectory);
            currentDirectory += $"\\{SecondName} {FirstName} {Patronymic}";
            folderIsCreated = Directory.Exists(currentDirectory);//Содана ли папка с таким именем, если да то добавляем цифры
            while (folderIsCreated)
            {
                string currentDirectoryTemp = currentDirectory + $" ({counter})";
                folderIsCreated = Directory.Exists(currentDirectoryTemp);
                if (!folderIsCreated) currentDirectory = currentDirectoryTemp;
                counter++;
            }
            currentDirectory += "\\Icon";
            Directory.CreateDirectory(currentDirectory);
            string photoPath = currentDirectory + "\\icon.jpg";
            File.Copy(path, photoPath, true);
            this.PathPhoto = "PhotoBase\\" + Family + "\\Icon\\icon.jpg";
            return true;
        }

        private RelayCommand saveAndCloseWindowCommand;
        public RelayCommand SaveAndCloseWindowCommand
        {
            get
            {
                return saveAndCloseWindowCommand ??
                    (saveAndCloseWindowCommand = new RelayCommand(obj =>
                      {
                          if (Save()) CloseAction();
                      }));
            }
        }
        private RelayCommand saveAndClearWindowCommand;
        public RelayCommand SaveAndClearWindowCommand
        {
            get
            {
                return saveAndClearWindowCommand ??
                    (saveAndClearWindowCommand = new RelayCommand(obj =>
                    {
                        if (Save())
                        {
                            Windows.AddOrEditInfoAboutPersonWindow newWindow = new Windows.AddOrEditInfoAboutPersonWindow();
                            newWindow.Show();
                            CloseAction();
                        }
                    }));
            }
        }
        private bool Save()//Сохранить данные в базе
        {
            if (!SaveMainPhotoInNewFolder(this.PathPhoto))
            {
                MessageBox.Show("По введённому пути фото отсутствует", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if(String.IsNullOrEmpty(Family) || String.IsNullOrEmpty(SecondName) || String.IsNullOrEmpty(FirstName) || String.IsNullOrEmpty(Patronymic))
            {
                MessageBox.Show("Не все поля для ФИО заполнены", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            Person newPerson = FromThisToPerson();
            dataBase.SaveNewPerson(newPerson);
            return true;
        }
        private RelayCommand closeWindowCommand;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return closeWindowCommand ??
                    (closeWindowCommand = new RelayCommand(obj =>
                    {
                        CloseAction();
                    }));
            }
        }
        #endregion

        #region GenderPanel
        public TextBlock TransitionalGender//Получает выбранный textBlock из поля выбора пола, изымает текст и передаёт его Spouce
        {
            set
            {
                TextBlock textBlock = (TextBlock)value;
                Spouse = textBlock.Text;
            }
        }
        private string spouse;
        public string Spouse//Изменяет несколько свойств и передаёт Муж или Жена в соотвествющее поле
        {
            get { return spouse; }
            set
            {
                Gender = value;
                if (value == "Мужской")
                {
                    spouse = "Жена";
                    SpouseIsMan = true;
                }
                else
                {
                    spouse = "Муж";
                    SpouseIsMan = false;
                }
                OnPropertyChanged("Spouse");
            }
        }
        #endregion

        #region FatherPanel
        private bool checkBoxFatherIsChecking;//Занесён ли аккаунт отца в систему
        public bool CheckBoxFatherIsChecking
        {
            get { return checkBoxFatherIsChecking; }
            set
            {
                checkBoxFatherIsChecking = value;
                PanelFatherAccountIsNot = !value;
                OnPropertyChanged("CheckBoxFatherIsChecking");
            }
        }
        private bool panelFatherAccountIsNot;
        public bool PanelFatherAccountIsNot
        {
            get { return panelFatherAccountIsNot; }
            set
            {
                panelFatherAccountIsNot = value;
                PanelFatherAccountIs = !value;
                OnPropertyChanged("PanelFatherAccountIsNot");
            }
        }
        private bool panelFatherAccountIs;
        public bool PanelFatherAccountIs
        {
            get { return panelFatherAccountIs; }
            set
            {
                panelFatherAccountIs = value;
                Father = "";
                OnPropertyChanged("PanelFatherAccountIs");
            }
        }

        #region SecondName
        private ObservableCollection<string> comboBoxFatherSecondNamesList; //Будет содержать все фамилии в базе
        public ObservableCollection<string> ComboBoxFatherSecondNamesList
        {
            get
            {
                foreach (var item in dataBase.ReturnAllSecondNamesInDataBase)
                {
                    comboBoxFatherSecondNamesList.Add(item);
                }
                return comboBoxFatherSecondNamesList;
            }
            
        }
        private string comboBoxFatherSecondNamesSelectedItem;
        public string ComboBoxFatherSecondNamesSelectedItem//Получает выделенную фамилию и передаёт её для нахождения всех имён
        {
            get { return comboBoxFatherSecondNamesSelectedItem; }
            set
            {
                comboBoxFatherSecondNamesSelectedItem = value;
                ComboBoxFatherFirstNameIsEnable = true;
                var result = GetFirstNames(value,true);
                ComboBoxFatherPatronymicsList.Clear();
                ComboBoxFatherFirstNamesList.Clear();
                ComboBoxFatherPatronymicsIsEnable = false;
                foreach (var item in result)
                {
                    ComboBoxFatherFirstNamesList.Add(item);
                }
            }
        }
        #endregion
        #region FirstNames
        public ObservableCollection<string> ComboBoxFatherFirstNamesList { get; set; }//Будет содержать все фамилии в базе

        private bool comboBoxFatherFirstNameIsEnable;
        public bool ComboBoxFatherFirstNameIsEnable
        {
            get { return comboBoxFatherFirstNameIsEnable; }
            set
            {
                comboBoxFatherFirstNameIsEnable = value;
                OnPropertyChanged("ComboBoxFatherFirstNameIsEnable");
            }
        }
        private string comboBoxFatherFirstNamesSelectedItem;
        public string ComboBoxFatherFirstNamesSelectedItem//Получает выделенное имя и передаёт её для нахождения всех отчеств
        {
            get { return comboBoxFatherFirstNamesSelectedItem; }
            set
            {
                comboBoxFatherFirstNamesSelectedItem = value;
                ComboBoxFatherPatronymicsIsEnable = true;
                List<string> result = GetPatronymics(ComboBoxFatherSecondNamesSelectedItem, value, true);
                ComboBoxFatherPatronymicsList.Clear();
                foreach (var item in result)
                {
                    ComboBoxFatherPatronymicsList.Add(item);
                }
            }
        }
        #endregion
        #region Patronymic
        public ObservableCollection<string> ComboBoxFatherPatronymicsList { get; set; }//Будет содержать все фамилии в базе

        private string comboBoxFatherPatronymicsSelectedItem;
        public string ComboBoxFatherPatronymicsSelectedItem//Получает выделенное отчество
        {
            get { return comboBoxFatherPatronymicsSelectedItem; }
            set
            {
                comboBoxFatherPatronymicsSelectedItem = value;
                Father = String.Join(" ", ComboBoxFatherSecondNamesSelectedItem, ComboBoxFatherFirstNamesSelectedItem, ComboBoxFatherPatronymicsSelectedItem);
            }
        } 
        private bool comboBoxFatherPatronymicsIsEnable;
        public bool ComboBoxFatherPatronymicsIsEnable
        {
            get { return comboBoxFatherPatronymicsIsEnable; }
            set
            {
                comboBoxFatherPatronymicsIsEnable = value;
                OnPropertyChanged("ComboBoxFatherPatronymicsIsEnable");
            }
        }

        #endregion
        #endregion

        #region MotherPanel
        private bool checkBoxMotherIsChecking;//Занесён ли аккаунт матери в систему
        public bool CheckBoxMotherIsChecking
        {
            get { return checkBoxMotherIsChecking; }
            set
            {
                checkBoxMotherIsChecking = value;
                PanelMotherAccountIsNot = !value;
                OnPropertyChanged("CheckBoxMotherIsChecking");
            }
        }
        private bool panelMotherAccountIsNot;
        public bool PanelMotherAccountIsNot
        {
            get { return panelMotherAccountIsNot; }
            set
            {
                panelMotherAccountIsNot = value;
                PanelMotherAccountIs = !value;
                OnPropertyChanged("PanelMotherAccountIsNot");
            }
        }
        private bool panelMotherAccountIs;
        public bool PanelMotherAccountIs
        {
            get { return panelMotherAccountIs; }
            set
            {
                panelMotherAccountIs = value;
                Mother = "";
                OnPropertyChanged("PanelMotherAccountIs");
            }
        }

        #region SecondName
        private ObservableCollection<string> comboBoxMotherSecondNamesList; //Будет содержать все фамилии в базе
        public ObservableCollection<string> ComboBoxMotherSecondNamesList
        {
            get
            {
                foreach (var item in dataBase.ReturnAllSecondNamesInDataBase)
                {
                    comboBoxMotherSecondNamesList.Add(item);
                }
                return comboBoxMotherSecondNamesList;
            }

        }
        private string comboBoxMotherSecondNamesSelectedItem;
        public string ComboBoxMotherSecondNamesSelectedItem//Получает выделенную фамилию и передаёт её для нахождения всех имён
        {
            get { return comboBoxMotherSecondNamesSelectedItem; }
            set
            {
                comboBoxMotherSecondNamesSelectedItem = value;
                ComboBoxMotherFirstNameIsEnable = true;
                var result = GetFirstNames(value, false);
                ComboBoxMotherPatronymicsList.Clear();
                ComboBoxMotherFirstNamesList.Clear();
                ComboBoxMotherPatronymicsIsEnable = false;
                foreach (var item in result)
                {
                    ComboBoxMotherFirstNamesList.Add(item);
                }
            }
        }
        #endregion
        #region FirstNames
        public ObservableCollection<string> ComboBoxMotherFirstNamesList { get; set; }//Будет содержать все фамилии в базе

        private bool comboBoxMotherFirstNameIsEnable;
        public bool ComboBoxMotherFirstNameIsEnable
        {
            get { return comboBoxMotherFirstNameIsEnable; }
            set
            {
                comboBoxMotherFirstNameIsEnable = value;
                OnPropertyChanged("ComboBoxMotherFirstNameIsEnable");
            }
        }
        private string comboBoxMotherFirstNamesSelectedItem;
        public string ComboBoxMotherFirstNamesSelectedItem//Получает выделенное имя и передаёт её для нахождения всех отчеств
        {
            get { return comboBoxMotherFirstNamesSelectedItem; }
            set
            {
                comboBoxMotherFirstNamesSelectedItem = value;
                ComboBoxMotherPatronymicsIsEnable = true;
                List<string> result = GetPatronymics(ComboBoxMotherSecondNamesSelectedItem, value, false);
                ComboBoxMotherPatronymicsList.Clear();
                foreach (var item in result)
                {
                    ComboBoxMotherPatronymicsList.Add(item);
                }
            }
        }
        #endregion
        #region Patronymic
        public ObservableCollection<string> ComboBoxMotherPatronymicsList { get; set; }//Будет содержать все фамилии в базе

        private string comboBoxMotherPatronymicsSelectedItem;
        public string ComboBoxMotherPatronymicsSelectedItem//Получает выделенное отчество
        {
            get { return comboBoxMotherPatronymicsSelectedItem; }
            set
            {
                comboBoxMotherPatronymicsSelectedItem = value;
                Mother = String.Join(" ", ComboBoxMotherSecondNamesSelectedItem, ComboBoxMotherFirstNamesSelectedItem, ComboBoxMotherPatronymicsSelectedItem);
            }
        }
        private bool comboBoxMotherPatronymicsIsEnable;
        public bool ComboBoxMotherPatronymicsIsEnable
        {
            get { return comboBoxMotherPatronymicsIsEnable; }
            set
            {
                comboBoxMotherPatronymicsIsEnable = value;
                OnPropertyChanged("ComboBoxMotherPatronymicsIsEnable");
            }
        }

        #endregion
        #endregion

        #region SpousePanel
        private bool SpouseIsMan { get; set; }//Какого пола супруг(а)
        private bool checkBoxSpouseIsChecking;//Занесён ли аккаунт в систему
        public bool CheckBoxSpouseIsChecking
        {
            get { return checkBoxSpouseIsChecking; }
            set
            {
                checkBoxSpouseIsChecking = value;
                PanelSpouseAccountIsNot = !value;
                OnPropertyChanged("CheckBoxSpouseIsChecking");
            }
        }
        private bool panelSpouseAccountIsNot;
        public bool PanelSpouseAccountIsNot
        {
            get { return panelSpouseAccountIsNot; }
            set
            {
                panelSpouseAccountIsNot = value;
                PanelSpouseAccountIs = !value;
                OnPropertyChanged("PanelSpouseAccountIsNot");
            }
        }
        private bool panelSpouseAccountIs;
        public bool PanelSpouseAccountIs
        {
            get { return panelSpouseAccountIs; }
            set
            {
                panelSpouseAccountIs = value;
                SpouseName = "";
                OnPropertyChanged("PanelSpouseAccountIs");
            }
        }

        private string spouseName;
        public string SpouseName
        {
            get { return spouseName; }
            set
            {
                spouseName = value;
                if (SpouseIsMan) Wife = value;
                else Husband = value;
                OnPropertyChanged("SpouseName");
            }
        }

        #region SecondName
        private ObservableCollection<string> comboBoxSpouseSecondNamesList; //Будет содержать все фамилии в базе
        public ObservableCollection<string> ComboBoxSpouseSecondNamesList
        {
            get
            {
                foreach (var item in dataBase.ReturnAllSecondNamesInDataBase)
                {
                    comboBoxSpouseSecondNamesList.Add(item);
                }
                return comboBoxSpouseSecondNamesList;
            }

        }
        private string comboBoxSpouseSecondNamesSelectedItem;
        public string ComboBoxSpouseSecondNamesSelectedItem//Получает выделенную фамилию и передаёт её для нахождения всех имён
        {
            get { return comboBoxSpouseSecondNamesSelectedItem; }
            set
            {
                if (String.IsNullOrEmpty(Gender))
                {
                    MessageBox.Show("Выберите пол", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
                comboBoxSpouseSecondNamesSelectedItem = value;
                ComboBoxSpouseFirstNameIsEnable = true;
                var result = GetFirstNames(value, SpouseIsMan);
                ComboBoxSpousePatronymicsList.Clear();
                ComboBoxSpouseFirstNamesList.Clear();
                ComboBoxSpousePatronymicsIsEnable = false;
                foreach (var item in result)
                {
                    ComboBoxSpouseFirstNamesList.Add(item);
                }
            }
        }
        #endregion
        #region FirstNames
        public ObservableCollection<string> ComboBoxSpouseFirstNamesList { get; set; }//Будет содержать все фамилии в базе

        private bool comboBoxSpouseFirstNameIsEnable;
        public bool ComboBoxSpouseFirstNameIsEnable
        {
            get { return comboBoxSpouseFirstNameIsEnable; }
            set
            {
                comboBoxSpouseFirstNameIsEnable = value;
                OnPropertyChanged("ComboBoxSpouseFirstNameIsEnable");
            }
        }
        private string comboBoxSpouseFirstNamesSelectedItem;
        public string ComboBoxSpouseFirstNamesSelectedItem//Получает выделенное имя и передаёт её для нахождения всех отчеств
        {
            get { return comboBoxSpouseFirstNamesSelectedItem; }
            set
            {
                comboBoxSpouseFirstNamesSelectedItem = value;
                ComboBoxSpousePatronymicsIsEnable = true;
                List<string> result = GetPatronymics(ComboBoxSpouseSecondNamesSelectedItem, value, SpouseIsMan);
                ComboBoxSpousePatronymicsList.Clear();
                foreach (var item in result)
                {
                    ComboBoxSpousePatronymicsList.Add(item);
                }
            }
        }
        #endregion
        #region Patronymic
        public ObservableCollection<string> ComboBoxSpousePatronymicsList { get; set; }//Будет содержать все фамилии в базе

        private string comboBoxSpousePatronymicsSelectedItem;
        public string ComboBoxSpousePatronymicsSelectedItem//Получает выделенное отчество
        {
            get { return comboBoxSpousePatronymicsSelectedItem; }
            set
            {
                comboBoxSpousePatronymicsSelectedItem = value;
                SpouseName = String.Join(" ", ComboBoxSpouseSecondNamesSelectedItem, ComboBoxSpouseFirstNamesSelectedItem, ComboBoxSpousePatronymicsSelectedItem);
            }
        }
        private bool comboBoxSpousePatronymicsIsEnable;
        public bool ComboBoxSpousePatronymicsIsEnable
        {
            get { return comboBoxSpousePatronymicsIsEnable; }
            set
            {
                comboBoxSpousePatronymicsIsEnable = value;
                OnPropertyChanged("ComboBoxSpousePatronymicsIsEnable");
            }
        }

        #endregion
        #endregion

        #region SonsPanel
        private void ClearSonsPanel()
        {
            try
            {
                ComboBoxSonsFirstNamesList.Clear();
                ComboBoxSonsPatronymicsList.Clear();
            }
            catch (NullReferenceException) { }
            ComboBoxSonsFirstNameIsEnable = false;
            ComboBoxSonsPatronymicsIsEnable = false;
            ComboBoxSonsSecondNamesSelectedItem = ComboBoxSonsSecondNamesSelectedItem;
            SonName = "";
        }
        private bool checkBoxSonsIsChecking;//Занесён ли аккаунт сына в систему
        public bool CheckBoxSonsIsChecking
        {
            get { return checkBoxSonsIsChecking; }
            set
            {
                checkBoxSonsIsChecking = value;
                PanelSonsAccountIsNot = !value;
                OnPropertyChanged("CheckBoxSonsIsChecking");
            }
        }
        private bool panelSonsAccountIsNot;
        public bool PanelSonsAccountIsNot
        {
            get { return panelSonsAccountIsNot; }
            set
            {
                panelSonsAccountIsNot = value;
                PanelSonsAccountIs = !value;
                ClearSonsPanel();
                OnPropertyChanged("PanelSonsAccountIsNot");
            }
        }
        private bool panelSonsAccountIs;
        public bool PanelSonsAccountIs
        {
            get { return panelSonsAccountIs; }
            set
            {
                panelSonsAccountIs = value;
                ClearSonsPanel();
                OnPropertyChanged("PanelSonsAccountIs");
            }
        }
        private string sonName;//ФИО, которое пользователь вводит вручную (в textBox)
        public string SonName
        {
            get { return sonName; }
            set
            {
                sonName = value;
                OnPropertyChanged("SonName");
            }
        }
        #region SonsList

        private ObservableCollection<string> checkBoxSonsList;
        public ObservableCollection<string> CheckBoxSonsList
        {
            get { return checkBoxSonsList; }
            set
            {
                checkBoxSonsList = value;
                Sons = String.Join(", ", checkBoxSonsList);
                if (CheckBoxSonsList.Count == 0 ) DeleteSonFromComboBoxButtonIsEnable = false;
            }
        }
        private string checkBoxSonsSelectedItem;
        public string CheckBoxSonsSelectedItem//Выбранный элемент из списка
        {
            get { return checkBoxSonsSelectedItem; }
            set
            {
                checkBoxSonsSelectedItem = value;
                if (String.IsNullOrEmpty(checkBoxSonsSelectedItem)) DeleteSonFromComboBoxButtonIsEnable = false;
                else DeleteSonFromComboBoxButtonIsEnable = true;
            }
        }

        private RelayCommand deleteSonFromComboBoxCommand;
        public RelayCommand DeleteSonFromComboBoxCommand
        {
            get
            {
                return deleteSonFromComboBoxCommand ??
                    (deleteSonFromComboBoxCommand = new RelayCommand(obj =>
                    {
                        
                        CheckBoxSonsList.Remove(CheckBoxSonsSelectedItem);
                        Sons = String.Join(", ", CheckBoxSonsList);
                    }));
            }
        }
        private RelayCommand addSonCommand;
        public  RelayCommand AddSonCommand
        {
            get
            {
                return addSonCommand ??
                    (addSonCommand = new RelayCommand(obj =>
                    {
                        CheckBoxSonsList.Add(SonName);
                        Sons = String.Join(", ", CheckBoxSonsList);
                        ClearSonsPanel();
                    }));
            }
        }
        private bool deleteSonFromComboBoxButtonIsEnable;
        public bool DeleteSonFromComboBoxButtonIsEnable
        {
            get { return deleteSonFromComboBoxButtonIsEnable; }
            set
            {
                deleteSonFromComboBoxButtonIsEnable = value;
                OnPropertyChanged("DeleteSonFromComboBoxButtonIsEnable");
            }
        }


        #endregion
        #region SecondName
        private ObservableCollection<string> comboBoxSonsSecondNamesList; //Будет содержать все фамилии в базе
        public ObservableCollection<string> ComboBoxSonsSecondNamesList
        {
            get
            {
                comboBoxSonsSecondNamesList.Clear();
                foreach (var item in ReturnAllSecondNamesInDataBase)
                {
                    comboBoxSonsSecondNamesList.Add(item);
                }
                return comboBoxSonsSecondNamesList;
            }

        }
        private string comboBoxSonsSecondNamesSelectedItem;
        public string ComboBoxSonsSecondNamesSelectedItem//Получает выделенную фамилию и передаёт её для нахождения всех имён
        {
            get { return comboBoxSonsSecondNamesSelectedItem; }
            set
            {
                comboBoxSonsSecondNamesSelectedItem = value;
                if (value != null)
                {
                    ComboBoxSonsFirstNameIsEnable = true;
                    var result = GetFirstNames(value, true);
                    ComboBoxSonsPatronymicsList.Clear();
                    ComboBoxSonsFirstNamesList.Clear();
                    ComboBoxSonsPatronymicsIsEnable = false;
                    foreach (var item in result)
                    {
                        ComboBoxSonsFirstNamesList.Add(item);
                    }
                }
            }
        }
        #endregion
        #region FirstNames
        public ObservableCollection<string> ComboBoxSonsFirstNamesList { get; set; }//Будет содержать все фамилии в базе

        private bool comboBoxSonsFirstNameIsEnable;
        public bool ComboBoxSonsFirstNameIsEnable
        {
            get { return comboBoxSonsFirstNameIsEnable; }
            set
            {
                comboBoxSonsFirstNameIsEnable = value;
                OnPropertyChanged("ComboBoxSonsFirstNameIsEnable");
            }
        }
        private string comboBoxSonsFirstNamesSelectedItem;
        public string ComboBoxSonsFirstNamesSelectedItem//Получает выделенное имя и передаёт её для нахождения всех отчеств
        {
            get { return comboBoxSonsFirstNamesSelectedItem; }
            set
            {
                comboBoxSonsFirstNamesSelectedItem = value;
                ComboBoxSonsPatronymicsIsEnable = true;
                List<string> result = GetPatronymics(ComboBoxSonsSecondNamesSelectedItem, value, true);
                ComboBoxSonsPatronymicsList.Clear();
                foreach (var item in result)
                {
                    ComboBoxSonsPatronymicsList.Add(item);
                }
            }
        }
        #endregion
        #region Patronymic
        public ObservableCollection<string> ComboBoxSonsPatronymicsList { get; set; }//Будет содержать все фамилии в базе

        private string comboBoxSonsPatronymicsSelectedItem;
        public string ComboBoxSonsPatronymicsSelectedItem//Получает выделенное отчество
        {
            get { return comboBoxSonsPatronymicsSelectedItem; }
            set
            {
                comboBoxSonsPatronymicsSelectedItem = value;
                SonName = String.Join(" ", ComboBoxSonsSecondNamesSelectedItem, ComboBoxSonsFirstNamesSelectedItem, ComboBoxSonsPatronymicsSelectedItem);
            }
        }
        private bool comboBoxSonsPatronymicsIsEnable;
        public bool ComboBoxSonsPatronymicsIsEnable
        {
            get { return comboBoxSonsPatronymicsIsEnable; }
            set
            {
                comboBoxSonsPatronymicsIsEnable = value;
                OnPropertyChanged("ComboBoxSonsPatronymicsIsEnable");
            }
        }

        #endregion
        #endregion

        #region DauthersPanel
        private void ClearDauthersPanel()
        {
            try
            {
                ComboBoxDauthersFirstNamesList.Clear();
                ComboBoxDauthersPatronymicsList.Clear();
            }
            catch (NullReferenceException) { }
            ComboBoxDauthersFirstNameIsEnable = false;
            ComboBoxDauthersPatronymicsIsEnable = false;
            ComboBoxDauthersSecondNamesSelectedItem = ComboBoxDauthersSecondNamesSelectedItem;
            DautherName = "";
        }
        private bool checkBoxDauthersIsChecking;//Занесён ли аккаунт сына в систему
        public bool CheckBoxDauthersIsChecking
        {
            get { return checkBoxDauthersIsChecking; }
            set
            {
                checkBoxDauthersIsChecking = value;
                PanelDauthersAccountIsNot = !value;
                OnPropertyChanged("CheckBoxDauthersIsChecking");
            }
        }
        private bool panelDauthersAccountIsNot;
        public bool PanelDauthersAccountIsNot
        {
            get { return panelDauthersAccountIsNot; }
            set
            {
                panelDauthersAccountIsNot = value;
                PanelDauthersAccountIs = !value;
                ClearDauthersPanel();
                OnPropertyChanged("PanelDauthersAccountIsNot");
            }
        }
        private bool panelDauthersAccountIs;
        public bool PanelDauthersAccountIs
        {
            get { return panelDauthersAccountIs; }
            set
            {
                panelDauthersAccountIs = value;
                ClearDauthersPanel();
                OnPropertyChanged("PanelDauthersAccountIs");
            }
        }
        private string dautherName;//ФИО, которое пользователь вводит вручную (в textBox)
        public string DautherName
        {
            get { return dautherName; }
            set
            {
                dautherName = value;
                OnPropertyChanged("DautherName");
            }
        }
        #region DauthersList

        private ObservableCollection<string> checkBoxDauthersList;
        public ObservableCollection<string> CheckBoxDauthersList
        {
            get { return checkBoxDauthersList; }
            set
            {
                checkBoxDauthersList = value;
                if (CheckBoxDauthersList.Count == 0) DeleteDautherFromComboBoxButtonIsEnable = false;
            }
        }
        private string checkBoxDauthersSelectedItem;
        public string CheckBoxDauthersSelectedItem//Выбранный элемент из списка
        {
            get { return checkBoxDauthersSelectedItem; }
            set
            {
                checkBoxDauthersSelectedItem = value;
                if (String.IsNullOrEmpty(checkBoxDauthersSelectedItem)) DeleteDautherFromComboBoxButtonIsEnable = false;
                else DeleteDautherFromComboBoxButtonIsEnable = true;
            }
        }

        private RelayCommand deleteDautherFromComboBoxCommand;
        public RelayCommand DeleteDautherFromComboBoxCommand
        {
            get
            {
                return deleteDautherFromComboBoxCommand ??
                    (deleteDautherFromComboBoxCommand = new RelayCommand(obj =>
                    {
                        CheckBoxDauthersList.Remove(CheckBoxDauthersSelectedItem);
                        Dauthers = String.Join(", ", checkBoxDauthersList);
                    }));
            }
        }
        private RelayCommand addDautherCommand;
        public RelayCommand AddDautherCommand
        {
            get
            {
                return addDautherCommand ??
                    (addDautherCommand = new RelayCommand(obj =>
                    {
                        CheckBoxDauthersList.Add(DautherName);
                        Dauthers = String.Join(", ", checkBoxDauthersList);
                        ClearDauthersPanel();
                    }));
            }
        }
        private bool deleteDautherFromComboBoxButtonIsEnable;
        public bool DeleteDautherFromComboBoxButtonIsEnable
        {
            get { return deleteDautherFromComboBoxButtonIsEnable; }
            set
            {
                deleteDautherFromComboBoxButtonIsEnable = value;
                OnPropertyChanged("DeleteDautherFromComboBoxButtonIsEnable");
            }
        }


        #endregion
        #region SecondName
        private ObservableCollection<string> comboBoxDauthersSecondNamesList; //Будет содержать все фамилии в базе
        public ObservableCollection<string> ComboBoxDauthersSecondNamesList
        {
            get
            {
                comboBoxDauthersSecondNamesList.Clear();
                foreach (var item in ReturnAllSecondNamesInDataBase)
                {
                    comboBoxDauthersSecondNamesList.Add(item);
                }
                return comboBoxDauthersSecondNamesList;
            }

        }
        private string comboBoxDauthersSecondNamesSelectedItem;
        public string ComboBoxDauthersSecondNamesSelectedItem//Получает выделенную фамилию и передаёт её для нахождения всех имён
        {
            get { return comboBoxDauthersSecondNamesSelectedItem; }
            set
            {
                comboBoxDauthersSecondNamesSelectedItem = value;
                if (value != null)
                {
                    ComboBoxDauthersFirstNameIsEnable = true;
                    var result = GetFirstNames(value, false);
                    ComboBoxDauthersPatronymicsList.Clear();
                    ComboBoxDauthersFirstNamesList.Clear();
                    ComboBoxDauthersPatronymicsIsEnable = false;
                    foreach (var item in result)
                    {
                        ComboBoxDauthersFirstNamesList.Add(item);
                    }
                }
            }
        }
        #endregion
        #region FirstNames
        public ObservableCollection<string> ComboBoxDauthersFirstNamesList { get; set; }//Будет содержать все фамилии в базе

        private bool comboBoxDauthersFirstNameIsEnable;
        public bool ComboBoxDauthersFirstNameIsEnable
        {
            get { return comboBoxDauthersFirstNameIsEnable; }
            set
            {
                comboBoxDauthersFirstNameIsEnable = value;
                OnPropertyChanged("ComboBoxDauthersFirstNameIsEnable");
            }
        }
        private string comboBoxDauthersFirstNamesSelectedItem;
        public string ComboBoxDauthersFirstNamesSelectedItem//Получает выделенное имя и передаёт её для нахождения всех отчеств
        {
            get { return comboBoxDauthersFirstNamesSelectedItem; }
            set
            {
                comboBoxDauthersFirstNamesSelectedItem = value;
                ComboBoxDauthersPatronymicsIsEnable = true;
                List<string> result = GetPatronymics(ComboBoxDauthersSecondNamesSelectedItem, value, false);
                ComboBoxDauthersPatronymicsList.Clear();
                foreach (var item in result)
                {
                    ComboBoxDauthersPatronymicsList.Add(item);
                }
            }
        }
        #endregion
        #region Patronymic
        public ObservableCollection<string> ComboBoxDauthersPatronymicsList { get; set; }//Будет содержать все фамилии в базе

        private string comboBoxDauthersPatronymicsSelectedItem;
        public string ComboBoxDauthersPatronymicsSelectedItem//Получает выделенное отчество
        {
            get { return comboBoxDauthersPatronymicsSelectedItem; }
            set
            {
                comboBoxDauthersPatronymicsSelectedItem = value;
                DautherName = String.Join(" ", ComboBoxDauthersSecondNamesSelectedItem, ComboBoxDauthersFirstNamesSelectedItem, ComboBoxDauthersPatronymicsSelectedItem);
            }
        }
        private bool comboBoxDauthersPatronymicsIsEnable;
        public bool ComboBoxDauthersPatronymicsIsEnable
        {
            get { return comboBoxDauthersPatronymicsIsEnable; }
            set
            {
                comboBoxDauthersPatronymicsIsEnable = value;
                OnPropertyChanged("ComboBoxDauthersPatronymicsIsEnable");
            }
        }

        #endregion
        #endregion

        #region PathPhotoPanel

        private RelayCommand addMainPhotoPathCommand;
        public RelayCommand AddMainPhotoPathCommand//Открытие проводника для выбора главного изображения профиля
        {
            get
            {
                return addMainPhotoPathCommand ??
                    (addMainPhotoPathCommand = new RelayCommand(obj =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.InitialDirectory = "c:\\";
                        openFileDialog.Filter = "Image Files(*.PNG; *.JPG)| *.PNG; *.JPG;";
                        openFileDialog.Multiselect = false;
                        Nullable<bool> result = openFileDialog.ShowDialog();
                        if (result == true)
                        {
                            PathPhoto = openFileDialog.FileName;
                        }
                    }));
            }
        }

        #endregion

       



    }
}
