using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIReservationHotel.models
{
    public class OffreAgenceHotel
    {
        public int IdOffre { get; set; }
        public int IdHotel { get; set; }
        public int IdChambre { get; set; }
        public int IdPrixChambre { get; set; }
        public int IdAgence { get; set; }
        public DateTime Diponibilite { get; set; }

        public OffreAgenceHotel() { }

        public OffreAgenceHotel(int idOffre, int idHotel, int idChambre, int idPrixChambre, int idAgence, DateTime diponibilite)
        {
            IdOffre = idOffre;
            IdHotel = idHotel;
            IdChambre = idChambre;
            IdPrixChambre = idPrixChambre;
            Diponibilite = diponibilite;
            IdAgence = idAgence;
        }
    }
}
