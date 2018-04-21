using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Xan.StellarisModTools
{
    /// <summary>
    /// Interaction logic for Subtractor.xaml
    /// </summary>
    public partial class Subtractor : UserControl
    {
        private string result;

        public Subtractor()
        {
            InitializeComponent();
        }

        private void ButtonSubtractClick(object Sender, RoutedEventArgs E)
        {
            List<string> main = new List<string>(Utils.ParseLines(this.textBoxInputMain.Text));
            List<string> sutract = new List<string>(Utils.ParseLines(this.textBoxInputSubtract.Text));

            List<string> subtracted = new List<string>();
            foreach (string name in main)
            {
                if (sutract.Contains(name))
                {
                    continue;
                }
                subtracted.Add(name);
            }

            this.result = Utils.MakeLines(subtracted);
            this.textBoxInputMain.Text = this.result;
        }

        private void ButtonCopyResultToClipboardClick(object Sender, RoutedEventArgs E)
        {
            Clipboard.SetText(this.result, TextDataFormat.Text);
        }
    }
}
