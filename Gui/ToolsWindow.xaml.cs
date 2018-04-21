using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace Xan.StellarisModTools
{
    /// <summary>
    /// Interaction logic for ToolsWindow.xaml
    /// </summary>
    public partial class ToolsWindow : Window
    {
        public ToolsWindow()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetAssembly(typeof(ToolsWindow));
            AssemblyFileVersionAttribute versionAttribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            this.Title += versionAttribute.Version;
        }
    }
}
