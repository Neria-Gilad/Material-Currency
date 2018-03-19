using System.Windows.Controls;
using Project.ViewModels;

namespace Project.UserControls {
    /// <summary>
    /// Interaction logic for Historychart.xaml
    /// </summary>
    public partial class Historychart : UserControl {
        public Historychart()
        {
            InitializeComponent();
            DataContext = new GraphViewModel();
        }
    }
}
