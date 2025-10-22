using CMCS_WPF.Services;
using CMCS_WPF.ViewModels;
using System.Windows;

namespace CMCS_WPF.Views
{
    public partial class CoordinatorView : Window
    {
        private readonly ClaimService _service;
        private readonly FileLogger _logger;

        public CoordinatorView(ClaimService service, FileLogger logger)
        {
            InitializeComponent();
            _service = service;
            _logger = logger;
            DataContext = new CoordinatorViewModel(_service);
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            if (ClaimsGrid.SelectedItem is Models.Claim claim)
            {
                var w = new DetailsView(claim);
                w.ShowDialog();
            }
        }
    }
}
