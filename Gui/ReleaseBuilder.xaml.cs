using System;
using System.Windows;
using System.Windows.Controls;
using Xan.StellarisModTools.Release;

namespace Xan.StellarisModTools
{
    /// <summary>
    /// Interaction logic for ReleaseBuilder.xaml
    /// </summary>
    public partial class ReleaseBuilder : UserControl
    {
        private readonly ReleaseCreator creator;

        public ReleaseBuilder()
        {
            InitializeComponent();

            this.textBoxInkscapePath.Text = Utils.SuggestInkscapePath();
            this.textBoxRepository.Text = Utils.SuggestRepositoryPath();
            this.textBoxTarget.Text = Utils.SuggestTargetPath();

            this.creator = new ReleaseCreator();
            this.creator.MessageReady += this.ReleaseCreatorMessageReady;
        }

        private void ReleaseCreatorMessageReady(object Sender, ReleaseCreatorEventArgs E)
        {
            this.textBoxOutput.AppendText(E.Message);
            this.textBoxOutput.AppendText(Environment.NewLine);
        }

        private void ButtonMakeReleaseClick(object Sender, RoutedEventArgs E)
        {
            Setup setup = new Setup();
            setup.CopyToTarget = this.checkBoxCopy.IsChecked == true;
            setup.CreateFlags = this.checkBoxCreateFlags.IsChecked == true;
            setup.CreatePortraits = this.checkBoxCreatePortraits.IsChecked == true;
            setup.InkscapePath = this.textBoxInkscapePath.Text;
            setup.RepositoryPath = this.textBoxRepository.Text;
            setup.TargetPath = this.textBoxTarget.Text;

            this.textBoxOutput.Text = string.Empty;
            this.creator.Setup = setup;
            this.creator.RunAsync();
        }
    }
}
