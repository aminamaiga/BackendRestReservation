using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIReservationHotel.models
{
    public class RequestReseverOffre
    {
        public int IdOffre { get; set; }
        public String Identifiant { get; set; }
        public String MotDepasse { get; set; }
        public int IdAgence { get; set; }
        public String Infos { get; set; }

        public RequestReseverOffre() { }
    }
}
