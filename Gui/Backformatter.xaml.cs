using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xan.StellarisModTools
{
    /// <summary>
    /// Interaction logic for Backformatter.xaml
    /// </summary>
    public partial class Backformatter : UserControl
    {
        private string result;

        public Backformatter()
        {
            InitializeComponent();
        }

        private List<string> ParseLine(string Line)
        {
            List<string> names = new List<string>();
            int start = 0;
            do
            {
                int nextSpace = Line.IndexOf(' ', start);
                int nextQuote = Line.IndexOf('"', start);
                if (nextQuote < 0  && nextSpace < 0)
                {
                    names.Add(Line.Substring(start));
                    break;
                }
                if (nextSpace < nextQuote || nextQuote < 0)
                {
                    names.Add(Line.Substring(start, nextSpace - start));
                    start = nextSpace + 1;
                }
                else
                {
                    int secondNextQuote = Line.IndexOf('"', nextQuote + 1);
                    names.Add(Line.Substring(nextQuote + 1, secondNextQuote - nextQuote - 1));
                    start = secondNextQuote + 2;
                }
            }
            while (start < Line.Length);
            return names;
        }

        private void ButtonFormatClick(object Sender, RoutedEventArgs E)
        {
            string[] lines = this.textBoxInput.Text.Split('\n', '\r');
            List<string> names = new List<string>();
            foreach (string line in lines.Where(X => !string.IsNullOrEmpty(X)))
            {
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                string trimmedLine = regex.Replace(line, " ");

                names.AddRange(this.ParseLine(trimmedLine.Trim()));
            }

            this.result = Utils.MakeLines(names);
            this.textBoxResult.Text = this.result;
        }

        private void ButtonCopyResultToClipboardClick(object Sender, RoutedEventArgs E)
        {
            Clipboard.SetText(this.result, TextDataFormat.Text);
        }
    }
}
