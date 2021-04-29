using APIReservationHotel.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIReservationHotel.modatabase
{
    public class MockOffre
    {

        public static List<OffreAgenceHotel> ListOffres { get; set; }
        public static List<OffreAgenceHotel> GetOffresLists()
        {
            ListOffres = new List<OffreAgenceHotel>();
            ListOffres.Add(new OffreAgenceHotel(1, 1, 1, 80, 1, new DateTime(2021, 06, 01)));
            ListOffres.Add(new OffreAgenceHotel(2, 1, 1, 100, 1, new DateTime(2021, 08, 01)));
            ListOffres.Add(new OffreAgenceHotel(3, 2, 2, 150, 1, new DateTime(2021, 07, 01)));
            ListOffres.Add(new OffreAgenceHotel(4, 2, 2, 200, 2, new DateTime(2021, 09, 01)));
            ListOffres.Add(new OffreAgenceHotel(9, 1, 1, 200, 2, new DateTime(2020, 09, 01)));
            ListOffres.Add(new OffreAgenceHotel(5, 3, 3, 500, 2, new DateTime(2021, 10, 01)));
            ListOffres.Add(new OffreAgenceHotel(6, 3, 3, 1000, 2, new DateTime(2021, 11, 01)));
            ListOffres.Add(new OffreAgenceHotel(7, 4, 4, 1000, 3, new DateTime(2021, 12, 01)));
            ListOffres.Add(new OffreAgenceHotel(8, 4, 4, 1000, 3, new DateTime(2021, 12, 01)));
            return ListOffres;
        }
    }
}
