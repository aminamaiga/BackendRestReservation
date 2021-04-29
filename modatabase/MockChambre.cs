using APIReservationHotel.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIReservationHotel.modatabase
{
    public class MockChambre
    {
        public static List<Chambre> ListChambres { get; set; }

        public static List<Chambre> GetListChambre()
        {
            ListChambres = new List<Chambre>();
            ListChambres.Add(new Chambre(1, 70, 1, true, 5, null, "https://localhost:44341/api/hotel/chambres?filename=637552470663551395.jpg"));
            ListChambres.Add(new Chambre(2, 100, 1, true, 4, null, "https://localhost:44341/api/hotel/chambres?filename=637552470663551395.jpg"));
            ListChambres.Add(new Chambre(3, 170, 2, true, 4, null, "https://localhost:44341/api/hotel/chambres?filename=637552470663551395.jpg"));
            ListChambres.Add(new Chambre(4, 270, 1, true, 3, null, "https://localhost:44341/api/hotel/chambres?filename=637552470663551395.jpg"));
            ListChambres.Add(new Chambre(5, 400, 1, false, 2, null, "https://localhost:44341/api/hotel/chambres?filename=637552470663551395.jpg"));
            ListChambres.Add(new Chambre(6, 500, 1, true, 1, null, "https://localhost:44341/api/hotel/chambres?filename=637552470663551395.jpg"));
            ListChambres.Add(new Chambre(7, 1000, 2, true, 1, null, "https://localhost:44341/api/hotel/chambres?filename=637552470663551395.jpg"));
            return ListChambres;
        }

    }
}