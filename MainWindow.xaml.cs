using CMCS_WPF.Views;
using System.Windows;

namespace CMCS_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenLecturer_Click(object sender, RoutedEventArgs e)
        {
            var w = new LecturerView(App.ClaimService, App.Logger);
            w.Show();
        }

        private void OpenCoordinator_Click(object sender, RoutedEventArgs e)
        {
            var w = new CoordinatorView(App.ClaimService, App.Logger);
            w.Show();
        }

        private void OpenManager_Click(object sender, RoutedEventArgs e)
        {
            // Manager uses same coordinator view but will pass role to approve
            var w = new CoordinatorView(App.ClaimService, App.Logger);
            w.Show();
        }
    }
}
