using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Xan.StellarisModTools
{
    /// <summary>
    /// Interaction logic for Extractor.xaml
    /// </summary>
    public partial class Extractor : UserControl
    {
        private readonly List<string> lines;
        private readonly List<string> result;

        public Extractor()
        {
            InitializeComponent();

            this.lines = new List<string>();
            this.result = new List<string>();
        }

        private void ButtonExtractClick(object Sender, RoutedEventArgs E)
        {
            this.lines.Clear();
            this.lines.AddRange(Utils.ParseLines(this.textBoxInput.Text));


            string keyword = this.textBoxKeyword.Text;
            this.result.Clear();
            this.result.AddRange(this.lines.Where(X => X.ToLower().Contains(keyword.ToLower())));
            this.textBoxResult.Text = Utils.MakeLines(this.result);
        }

        private void ButtonCopyResultToClipboardClick(object Sender, RoutedEventArgs E)
        {
            Clipboard.SetText(Utils.MakeLines(this.result), TextDataFormat.Text);
        }

        private void ButtonCopyCorrectedSourceClipboardClick(object Sender, RoutedEventArgs E)
        {
            Clipboard.SetText(Utils.MakeLines(this.lines.Where( X => !this.result.Contains(X))), TextDataFormat.Text);
        }
    }
}
