using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PasswordWindow = FamilyTree.Windows.PasswordWindow;
using PersonWindow = FamilyTree.Windows.PersonWindow;

namespace FamilyTree
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //PasswordWindow passwordWindow = new PasswordWindow();
            //passwordWindow.ShowDialog();
            //ApplicationLogic.OperationsWithDataBase.PersonTable personTable = new ApplicationLogic.OperationsWithDataBase.PersonTable();
            //personTable.ReturnSearchingResult("Салий", "", "");
            //PersonWindow personWindow = new PersonWindow(personTable.ReturnPersonInFullName("Saliy  Alex Romanovyich"), 1);
            //personWindow.ShowDialog();

            //Windows.FindPersonWindow findPersonWindow = new Windows.FindPersonWindow();
            //findPersonWindow.ShowDialog();
            Windows.AddOrEditInfoAboutPersonWindow addOrEdit = new Windows.AddOrEditInfoAboutPersonWindow();
            addOrEdit.ShowDialog();
            InitializeComponent();
        }
    }
}
