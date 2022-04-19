using Imenik.Model;
using Imenik.PomocneKlase;
using System.Windows.Input;

namespace Imenik.ViewModels
{
    public class IzmenaKontaktaViewModel : ObservableObject
    {
        private Kontakt _izabraniKontakt;
        public Kontakt IzabraniKontakt
        {
            get { return _izabraniKontakt; }
            set { OnPropertyChanged(ref _izabraniKontakt, value);
                   }
        }

        private readonly IImenikViewModel _sender;


        public ICommand SacuvajKomanda { get; private set; }
        public ICommand ImgKomanda { get; private set; }
        public ICommand OmiljeniKomanda { get; private set; }
        public object TrenutniPrikaz { get; private set; }

        public IzmenaKontaktaViewModel(Kontakt kontaktZaIzmenu, IImenikViewModel sender)
        {
            IzabraniKontakt = kontaktZaIzmenu;
            _sender = sender;


            SacuvajKomanda = new RelayCommand(Sacuvaj);
            ImgKomanda = new RelayCommand(PretraziSliku); // ove komande su povezane sa sa korisnickim interfejsom
            OmiljeniKomanda = new RelayCommand(Omiljeni);
        }

        // Biramo da li je izabrani kontakt omiljen ili ne 
        private void Omiljeni()
        {
            IzabraniKontakt.JeOmiljeni = !IzabraniKontakt.JeOmiljeni;
        }

        //ubacivanje slike
        private void PretraziSliku()
        {
            var putanjaDatoteke = _sender.OtvoriDatoteku("Image files|*.bmp;*.jpg;*.jpeg;*.png|All files");
            IzabraniKontakt.ImgPutanja = putanjaDatoteke;
        }

        // usnimavanje podataka pozivanjem metode u ImenikViewModel
        private void Sacuvaj()
        {
            _sender.SacuvajKontakt(IzabraniKontakt);
        }
    }
}