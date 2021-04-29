using System;

namespace APIReservationHotel.models
{
    [Serializable]
    public class ResponseHotel
    {
      
       public string nomHotel { get; set; }

        public string GetNomHotel()
        {
            return nomHotel;
        }

        public void SetNomHotel(string value)
        {
            nomHotel = value;
        }

        public int nombreChambre { get; set; }

        public int GetNombreChambre()
        {
            return nombreChambre;
        }

        public void SetNombreChambre(int value)
        {
            nombreChambre = value;
        }

        public String Ville { get; set; }
        public String Pays { get; set; }
        public int IdOffre { get; set; }

        public Chambre Chambres {get; set;}

        public Chambre GetChambres()
        {
            return Chambres;
        }

        public void SetChambres(Chambre chambres)
        {
            this.Chambres = chambres;
        }

        public ResponseHotel()
        {

        }

        public ResponseHotel(int idOffre, string nomHotel, int nombreChambre, String ville, Chambre chambres)
        {
            this.SetNomHotel(nomHotel);
            this.SetNombreChambre(nombreChambre);
            this.Ville = ville;
            this.Chambres = chambres;
            IdOffre = idOffre;
        }

    }
}