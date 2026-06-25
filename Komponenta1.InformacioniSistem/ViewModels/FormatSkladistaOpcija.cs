namespace Komponenta1.InformacioniSistem.ViewModels
{
    public class FormatSkladistaOpcija
    {
        public string Naziv { get; set; }

        public string Ekstenzija { get; set; }

        public FormatSkladistaOpcija()
        {
            Naziv = string.Empty;
            Ekstenzija = string.Empty;
        }

        public FormatSkladistaOpcija(string naziv, string ekstenzija)
        {
            Naziv = naziv;
            Ekstenzija = ekstenzija;
        }

        public override string ToString()
        {
            return Naziv;
        }
    }
}
