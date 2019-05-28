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
    /// Логика взаимодействия для PersonWindow.xaml
    /// </summary>
    public partial class PersonWindow : Window
    {
        public PersonWindow(ApplicationLogic.DataBaseTableInstances.Person person, int isAdministrator)
        {
            InitializeComponent();
            ModelView.PersonWindowModelView modelView = new ModelView.PersonWindowModelView(person,isAdministrator);
            DataContext = modelView;
            FlowDocument myFlowDoc = new FlowDocument();
            // Add paragraphs to the FlowDocument.
            myFlowDoc.Blocks.Add(new Paragraph(new Run(person.Description)));
            _richTextBox.Document = myFlowDoc;
            var test = new TextRange(myFlowDoc.ContentStart, myFlowDoc.ContentEnd).Text;
        }

    }
}
