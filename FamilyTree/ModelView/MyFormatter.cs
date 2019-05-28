using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Xceed.Wpf.Toolkit;

namespace FamilyTree.ModelView
{
    public class MyFormatter : ITextFormatter
    {
        public string GetText(System.Windows.Documents.FlowDocument document)
        {
            return new TextRange(document.ContentStart, document.ContentEnd).Text;
        }

        public void SetText(System.Windows.Documents.FlowDocument document, string text)
        {
            new TextRange(document.ContentStart, document.ContentEnd).Text = text;
        }
    }
}
