using System.IO;
using System.Windows;
using System.Windows.Controls;
using Xan.StellarisModTools.Stellaris;

namespace Xan.StellarisModTools
{
    /// <summary>
    /// Interaktionslogik für Upgrader.xaml
    /// </summary>
    public partial class Upgrader : UserControl
    {
        public Upgrader()
        {
            InitializeComponent();

            this.textBoxRepository.Text = Utils.SuggestRepositoryPath();
        }

        private void buttonUpgrade182Empires_Click(object sender, RoutedEventArgs e)
        {
            string empirePath = Path.Combine(this.textBoxRepository.Text, "output", "XansSciFiMashup", "prescripted_countries");

            var upgrader = new EmpireFileUpgrader();
            string[] empireFiles = Directory.GetFiles(empirePath, "*.txt");
            foreach (var file in empireFiles)
            {
                // make backup
                File.Copy(file, string.Format("{0}.bak", file), true);

                //  upgrade
                upgrader.Upgrade(file);
            }

        }
    }
}
