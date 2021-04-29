using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIReservationHotel.models
{
    public class Agence
    {
        public int IdAgence { get; set; }
        public String Identifiant { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }

        public Agence()
        {

        }

        public Agence(int idAgence, string identifiant, string password, string name)
        {
            IdAgence = idAgence;
            Identifiant = identifiant;
            Password = password;
            Name = name;
        }
    }
}
