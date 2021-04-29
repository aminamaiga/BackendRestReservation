using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace APIReservationHotel.models
{
    [Serializable]
    public class Chambre
    {
        public int IdChambre { get; set; }
        public int Prix { get; set; }
        public int NombreDeLiet { get; set; }
        public bool EstLibre { get; set; }
        public int? IdHotel { get; set; }
        public DateTime? Disponible { get; set; }
        public IFormFile? Photo { get; set; }
        public String PhotoPath { get; set; }
        public Chambre() { }
        public Chambre(int idChambre, int prix, int nombreDeLiet, bool estLibre, int? idHotel)
        {
            IdChambre = idChambre;
            Prix = prix;
            NombreDeLiet = nombreDeLiet;
            EstLibre = estLibre;
            IdHotel = idHotel;
      
        }

        public Chambre(int idChambre, int prix, int nombreDeLiet, bool estLibre, int? idHotel, DateTime? disponible, String photoPath, IFormFile? photo = null)
        {
            IdChambre = idChambre;
            Prix = prix;
            NombreDeLiet = nombreDeLiet;
            EstLibre = estLibre;
            IdHotel = idHotel;
            Disponible = disponible;
            Photo = photo;
            PhotoPath = photoPath;
        }
    }
}