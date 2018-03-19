using System.Windows.Controls;
using Project.ViewModels;

namespace Project.UserControls {
    /// <summary>
    /// Interaction logic for Conversion.xaml
    /// </summary>
    public partial class Conversion : UserControl {
        
        public Conversion()
        {
            InitializeComponent();
            this.DataContext = new ConversionViewModel();
        }
    }
}
