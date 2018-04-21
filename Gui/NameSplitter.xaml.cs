using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Xan.StellarisModTools
{
    /// <summary>
    /// Interaction logic for NameSplitter.xaml
    /// </summary>
    public partial class NameSplitter : UserControl
    {
        private string firstNamesResult;
        private string lastNamesResult;

        public NameSplitter()
        {
            InitializeComponent();
        }

        private void ButtonSplitClick(object Sender, RoutedEventArgs E)
        {
            List<string> lines = new List<string>(Utils.ParseLines(this.textBoxInput.Text));

            List<string> firstNames = new List<string>();
            List<string> lastNames = new List<string>();
            foreach (string line in lines)
            {
                string firstName = line;
                string lastName = string.Empty;

                int firstSpace = line.IndexOf(' ');
                if (firstSpace > 0)
                {
                    firstName = line.Substring(0, firstSpace);
                    lastName = line.Substring(firstSpace + 1);
                }

                if (!string.IsNullOrEmpty(firstName))
                {
                    firstNames.Add(firstName);
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    lastNames.Add(lastName);
                }
            }


            this.firstNamesResult = Utils.MakeLines(firstNames);
            this.textBoxFirstNames.Text = firstNamesResult;
            this.lastNamesResult = Utils.MakeLines(lastNames);
            this.textBoxLastNames.Text = this.lastNamesResult;
        }

        private void ButtonCopyFirstNamesToClipboardClick(object Sender, RoutedEventArgs E)
        {
            Clipboard.SetText(this.firstNamesResult, TextDataFormat.Text);
        }

        private void ButtonCopyLastNamesToClipboardClick(object Sender, RoutedEventArgs E)
        {
            Clipboard.SetText(this.lastNamesResult, TextDataFormat.Text);
        }
    }
}
