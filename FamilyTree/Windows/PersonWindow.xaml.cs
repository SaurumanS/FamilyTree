using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace FamilyTree.Windows
{
    /// <summary>
    /// Логика взаимодействия для PersonWindow.xaml
    /// </summary>
    public partial class PersonWindow : Window
    {
        ApplicationLogic.OperationsWithDataBase.PersonTable personTable;
        public PersonWindow(ApplicationLogic.DataBaseTableInstances.Person person, int isAdministrator)
        {
            InitializeComponent();
            personTable = new ApplicationLogic.OperationsWithDataBase.PersonTable();
            ModelView.PersonWindowModelView modelView = new ModelView.PersonWindowModelView(person,isAdministrator);
            DataContext = modelView;
            
            textBoxMother.Text = person.Mother;
            textBoxFather.Text = person.Father;
            textBoxSpouse.Text = person.Gender == "Мужчина" ? person.Wife : person.Husband;
            textBoxChildren.Text = String.Join(", ", person.Sons, person.Dauthers);
            TransformationTextInTextBlockToLink(textBoxFather,textBoxMother,textBoxSpouse,textBoxChildren);
            if (modelView.CloseAction == null)
            {
                modelView.CloseAction = new Action(this.Close); // Реализация закрытия окна 
            }
        }
        private void TransformationTextInTextBlockToLink(params TextBlock[] textBlock)
        {
            for (int i = 0; i < textBlock.Length; i++)
            {
                textBlock[i].Inlines.Clear();
                string text = textBlock[i].Text;
                if (String.IsNullOrEmpty(text)) continue;
                string pattern = @"\s+";
                string target = " ";
                Regex regex = new Regex(pattern);
                text = regex.Replace(text, target);
                string[] textSplit = text.Split(',');
                for (int counter = 0; counter < textSplit.Length; counter++)
                {
                    Hyperlink hyperlink = new Hyperlink(new Run(textSplit[counter])) { NavigateUri = new Uri("https://www.google.ru/?gws_rd=ssl") };
                    string fullName = textSplit[counter];
                    hyperlink.RequestNavigate += (sender, args) =>
                    {
                        var person = personTable.ReturnPersonInFullName(fullName);
                        PersonWindow newPersonWindow = new PersonWindow(person, 1);
                        newPersonWindow.Show();
                    };
                    //hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
                    textBlock[i].Inlines.Add(hyperlink);
                    if (counter != textSplit.Length - 1)
                    {
                        Run run = new Run();
                        run.Text = " , ";
                        textBlock[i].Inlines.Add(run);
                    }
                }
            }
        }//MVVM SORRY

    }
}
