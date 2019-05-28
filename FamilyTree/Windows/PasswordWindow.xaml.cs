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
using System.Windows.Shapes;

namespace FamilyTree.Windows
{
    /// <summary>
    /// Логика взаимодействия для PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
            ModelView.PasswordWindowModelView passwordWindowModelView = new ModelView.PasswordWindowModelView();
            DataContext = passwordWindowModelView;
            if (passwordWindowModelView.CloseAction == null)
            {
                passwordWindowModelView.CloseAction = new Action(this.Close); // Реализация закрытия окна 
            }

        }
    }
}
