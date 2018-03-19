using System.Windows.Controls;
using Project.ViewModels;

namespace Project.UserControls {
    /// <summary>
    /// Interaction logic for BannerCard.xaml
    /// </summary>
    public partial class BannerCard : UserControl {
        public BannerCard()
        {
            InitializeComponent();
            this.DataContext = new LiveViewModel();
        }
    }
}
