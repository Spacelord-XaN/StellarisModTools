using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Xan.StellarisModTools
{
    /// <summary>
    /// Interaction logic for Collector.xaml
    /// </summary>
    public partial class Collector : UserControl
    {
        private readonly List<char> invalidCharacters;
        private readonly DispatcherTimer timer;
        private readonly List<string> lines;
        private string lastContent;

        public Collector()
        {
            InitializeComponent();

            this.lines = new List<string>();
            this.lastContent = Clipboard.GetText();
            this.invalidCharacters = new List<char>();
            this.invalidCharacters.Add('(');
            this.invalidCharacters.Add(')');
            this.invalidCharacters.Add('0');
            this.invalidCharacters.Add('1');
            this.invalidCharacters.Add('2');
            this.invalidCharacters.Add('3');
            this.invalidCharacters.Add('4');
            this.invalidCharacters.Add('5');
            this.invalidCharacters.Add('6');
            this.invalidCharacters.Add('7');
            this.invalidCharacters.Add('8');
            this.invalidCharacters.Add('9');

            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(300);
            this.timer.Tick += this.TimerTick;

            this.UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            this.buttonStart.IsEnabled = !this.timer.IsEnabled;
            this.buttonStop.IsEnabled = this.timer.IsEnabled;
        }

        private void ButtonStartClick(object sender, RoutedEventArgs e)
        {
            this.timer.Start();
            this.UpdateButtonStates();
        }

        private void ButtonStopClick(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
            this.UpdateButtonStates();
        }

        private void ButtonClearClick(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
            Clipboard.Clear();
            this.lastContent = null;
            this.lines.Clear();
            this.textBoxResult.Text = "";
            this.timer.Start();
        }

        private void ButtonCopyResultToClipboardClick(object sender, RoutedEventArgs e)
        {
            this.lines.Sort();

            StringBuilder sb = new StringBuilder();
            foreach (string line in this.lines)
            {
                if (line.Contains(' '))
                {
                    sb.AppendFormat("\"{0}\"", line);
                }
                else
                {
                    sb.Append(line);
                }
                sb.Append(' ');
            }

            Clipboard.SetText(sb.ToString(), TextDataFormat.Text);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            string content = Clipboard.GetText();
            if (content != this.lastContent)
            {
                this.lastContent = content;
                this.AppendText(content);
            }
        }

        private void AppendText(string Text)
        {
            string[] lineTokens = Text.Split('\n', '\r');
            StringBuilder sb = new StringBuilder();
            foreach (string line in lineTokens)
            {
                string name = line;
                int first = name.IndexOf('(');
                first--;
                if (first >= 0)
                {
                    name = name.Substring(0, first);
                }
                name = name.Replace("\"", "");
                name = name.Replace("\t", "");
                if (this.lines.Contains(name))
                {
                    continue;
                }
                this.lines.Add(name);
                sb.Append(name);
                sb.Append(" ");
            }

            this.textBoxResult.Text += sb.ToString();
        }
    }
}
