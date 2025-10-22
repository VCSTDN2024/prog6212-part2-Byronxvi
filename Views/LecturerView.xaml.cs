using CMCS_WPF.Services;
using CMCS_WPF.ViewModels;
using System.Windows;

namespace CMCS_WPF.Views
{
    public partial class LecturerView : Window
    {
        public LecturerView(ClaimService service, FileLogger logger)
        {
            InitializeComponent();
            var vm = new LecturerViewModel(service, logger);
            DataContext = vm;
        }
    }
}
