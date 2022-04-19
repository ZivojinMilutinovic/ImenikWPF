using Imenik.ViewModels;
using System.Windows;
using System.Diagnostics;

namespace Imenik.Views
{
    /// <summary>
    /// Interaction logic for ImenikView.xaml
    /// </summary>
    public partial class ImenikView : Window
    {
        public ImenikView()
        {
            InitializeComponent();
            this.DataContext = new ImenikViewModel();
        }
    }
}
