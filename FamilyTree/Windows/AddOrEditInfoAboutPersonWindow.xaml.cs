﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Person = ApplicationLogic.DataBaseTableInstances.Person;

namespace FamilyTree.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditInfoAboutPersonWindow.xaml
    /// </summary>
    public partial class AddOrEditInfoAboutPersonWindow : Window
    {
        public AddOrEditInfoAboutPersonWindow()
        {
            InitializeComponent();
            ModelView.AddOrEditInfoAboutPersonWindowModelView modelView = new ModelView.AddOrEditInfoAboutPersonWindowModelView();
            DataContext = modelView;
            if (modelView.CloseAction == null)
            {
                modelView.CloseAction = new Action(this.Close); // Реализация закрытия окна 
            }
        }
        public AddOrEditInfoAboutPersonWindow(Person person)
        {
            InitializeComponent();
            ModelView.AddOrEditInfoAboutPersonWindowModelView modelView = new ModelView.AddOrEditInfoAboutPersonWindowModelView(person);
            DataContext = modelView;
            if (modelView.CloseAction == null)
            {
                modelView.CloseAction = new Action(this.Close); // Реализация закрытия окна 
            }
        }
    }
}
