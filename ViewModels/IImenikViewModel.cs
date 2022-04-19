using Imenik.Model;

namespace Imenik.ViewModels
{
    public interface IImenikViewModel
    {
        void UcitajKontakte();
        void SacuvajKontakt(Kontakt kontakt);
        string OtvoriDatoteku(string filter);
        void IzmeniKontakt();
        void ObrisiIzabraniKontakt();

        Kontakt VratiIzabraniKontakt();
    }
}
