using APIReservationHotel.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIReservationHotel.modatabase
{
    public class MockAgence
    {
        public static List<Agence> ListAgences { get; set; }
        public static List<Agence> GetAgencesLists()
        {
            ListAgences = new List<Agence>();
            ListAgences.Add(new Agence(1, "agence1", "agence1", "Agence1" ));
            ListAgences.Add(new Agence(2, "agence2", "agence2", "Agence2" ));
            ListAgences.Add(new Agence(3, "agence3", "agence3", "Agence3" ));
            return ListAgences;
        }
    }

}
