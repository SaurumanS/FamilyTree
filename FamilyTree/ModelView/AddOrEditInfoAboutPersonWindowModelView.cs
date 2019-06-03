using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Person = ApplicationLogic.DataBaseTableInstances.Person;

namespace FamilyTree.ModelView
{
    class AddOrEditInfoAboutPersonWindowModelView: Person, INotifyPropertyChanged
    {
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
        private void Initialization()
        {
            CheckBoxFatherIsChecking = true;
            spouce = "Муж/Жена";
        }
        public string Title { get; set; }
        public TextBlock TransitionalGender//Получает выбранный textBlock из поля выбора пола, изымает текст и передаёт его Spouce
        {
            set
            {
                TextBlock textBlock = (TextBlock) value;
                Spouce = textBlock.Text;
            }
        }
        private string spouce;
        public string Spouce//Изменяет несколько свойств и передаёт Муж или Жена в соотвествющее поле
        {
            get { return spouce; }
            set
            {
                if (value == "Мужской")
                {
                    Gender = "Мужской";
                    spouce = "Жена";
                }
                else
                {
                    Gender = "Женский";
                    spouce = "Муж";
                }
                OnPropertyChanged("Spouce");
            }
        }


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
                OnPropertyChanged("PanelFatherAccountIs");
            }
        }
        #endregion

    }
}
