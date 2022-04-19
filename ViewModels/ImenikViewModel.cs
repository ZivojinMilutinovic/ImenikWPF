using Imenik.Model;
using Imenik.PomocneKlase;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.Generic;
using Imenik.Views;
using System;

namespace Imenik.ViewModels
{

    public class ImenikViewModel : ObservableObject, IImenikViewModel
    {
        private readonly string _imeDatoteke = "Json/imenik.json";

        public ICommand UcitajKontakteKomanda { get; set; }
        public ICommand UcitajOmiljeneKomanda { get; set; }
        public ICommand DodajKomanda { get; set; }

        public ObservableCollection<Kontakt> Kontakti { get; private set; } = new ObservableCollection<Kontakt>();

        private Kontakt _izabraniKontakt;

        public Kontakt IzabraniKontakt
        {
            get { return _izabraniKontakt; }
            set
            {
                OnPropertyChanged(ref _izabraniKontakt, value);
                if (_trenutniPrikaz != null)
                {
                    _trenutniPrikaz.postaviKontakt(value);
                    PrikaziDetalje();
                }
            }
        }
        private IKontaktView _trenutniPrikaz;
        public IKontaktView TrenutniPrikaz
        {
            get { return _trenutniPrikaz; }

            set { OnPropertyChanged(ref _trenutniPrikaz, value); }
        }

        public ImenikViewModel()
        {
            UcitajKontakteKomanda = new RelayCommand(UcitajKontakte);
            UcitajOmiljeneKomanda = new RelayCommand(UcitajOmiljene);
            DodajKomanda = new RelayCommand(DodajKontakt);
            UcitajKontakte();
            var kontakt = Kontakti.FirstOrDefault();
            IzabraniKontakt = kontakt;
            PrikaziDetalje();
        }
        /* Ucitavanje kontakta - metoda definisana u interfejsu IServisPodataka*/
        public void UcitajKontakte()
        {
            if (!File.Exists(_imeDatoteke))
            {
                File.Create(_imeDatoteke).Close();
            }
            var sKontakti = File.ReadAllText(_imeDatoteke);
            var rezKontakti = JsonSerializer.Deserialize<ObservableCollection<Kontakt>>(sKontakti);
            Kontakti.Clear();
            foreach (var kontakti in rezKontakti)
                Kontakti.Add(kontakti);

        }
        /* Ucitavanje omiljenih */
        private void UcitajOmiljene()
        {
            List<Kontakt> kontaktiTemp = Kontakti.Where(k => k.JeOmiljeni).ToList();
            Trace.WriteLine(kontaktiTemp.Count);
            for(int i = Kontakti.Count-1; i >=0; i--)
            {
                var kontakt = Kontakti[i];
                if (!kontaktiTemp.Contains(kontakt))
                {
                    Trace.WriteLine("Izbacio");
                    Kontakti.Remove(kontakt);
                }
            }
            foreach(var kontakt in kontaktiTemp)
            {
                if (!Kontakti.Contains(kontakt))
                {
                    Kontakti.Add(kontakt);
                }
            }
        }

        /* Dodavanje novog kontakta i postavljanje za izabrani */
        private void DodajKontakt()
        {
            
            var noviKontakt = new Kontakt
            {
                Id = Guid.NewGuid(),
                Ime = " ",
                Prezime = " ",
                BrojTelefona = " ",
                EPosta = " ",
                Adresa = " ",
                JeOmiljeni = false,
                ImgPutanja = null
            };

            Kontakti.Add(noviKontakt);
            IzabraniKontakt = noviKontakt;
        }


        /* Usnimavanje podataka novog ili izmenjenog kontakta */

        public void SacuvajKontakt(Kontakt kontakt)
        {
            //feature request
            Kontakti.Remove(kontakt);
            Kontakti.Add(kontakt);
            var sKontakti = JsonSerializer.Serialize(Kontakti);
            File.WriteAllText(_imeDatoteke, sKontakti);
            OnPropertyChanged(nameof(IzabraniKontakt));
            IzabraniKontakt = kontakt;
        }

        // Postavljanje View-a koji prikazije detalje o izabranom kontaktu
        //poziva ga IzabraniKontakt

        public void PrikaziDetalje()
        {
            //TrenutniPrikaz = new PrikazKontaktaViewModel(IzabraniKontakt, this);
            TrenutniPrikaz = new PrikazKontaktaView(IzabraniKontakt, this);
        }


        // Postavljanje View-a za izmenu izabrang kontakta/ili dodavanje novog 
        // Definisana u interfejsu i poziva se iz PrikazKontaktaView/Viewmodel 

        public void IzmeniKontakt()
        {
            //TrenutniPrikaz = new IzmenaKontaktaViewModel(IzabraniKontakt, this);
            TrenutniPrikaz = new IzmenaKontaktaView(IzabraniKontakt, this);
        }

        // Definisana u interfejsu i poziva se iz PrikazKontaktaView/Viewmodel
        //Brisanje izabranog kontakta
        public void ObrisiIzabraniKontakt()
        {
            Kontakti.Remove(IzabraniKontakt);
            OnPropertyChanged(nameof(Kontakti));
            IzabraniKontakt = Kontakti.FirstOrDefault();

        }
        // Definisana u interfejsu i poziva se iz IzmenaKontaktaView/Viewmodel
        // Sluzi za ubacivanje slike

        public string OtvoriDatoteku(string filter)
        {
            var dijalog = new OpenFileDialog();

            if (dijalog.ShowDialog() == true)
            {
                return dijalog.FileName;
            }

            return null;
        }

        public Kontakt VratiIzabraniKontakt()
        {
            return IzabraniKontakt;
        }
    }
}
