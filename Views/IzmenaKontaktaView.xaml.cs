using Imenik.Model;
using Imenik.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Imenik.Views
{
    /// <summary>
    /// Interaction logic for IzmenaKontaktaView.xaml
    /// </summary>
    public partial class IzmenaKontaktaView : UserControl, IKontaktView
    {
        private IzmenaKontaktaViewModel _viewModel;
        public IzmenaKontaktaView()
        {
            InitializeComponent();
        }
        public IzmenaKontaktaView(Kontakt izabraniKontakt, IImenikViewModel sender)
        {
            InitializeComponent();
            _viewModel = new IzmenaKontaktaViewModel(izabraniKontakt, sender);
            this.DataContext = _viewModel;
        }

        public void postaviKontakt(Kontakt kontakt)
        {
            Trace.WriteLine("Zove");
            _viewModel.IzabraniKontakt = kontakt;
        }
    }
}
