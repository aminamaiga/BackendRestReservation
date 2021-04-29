using APIReservationHotel.modatabase;
using APIReservationHotel.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RestSharp;
using Newtonsoft.Json;

namespace APIReservationHotel.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;

        public List<Chambre> ListeChambre;
        public List<Categorie> ListeCategorie;
        public List<Hotel> ListeHotel = new List<Hotel>();
        public List<Hotel> ReturnListeHotel = new List<Hotel>();
        public List<ResponseHotel> ResponseH = new List<ResponseHotel>();
        public List<Reservation> ListeReservations = new List<Reservation>();
        List<Client> ListeClients = new List<Client>();
        List<Agence> ListeAgence = new List<Agence>();
        List<OffreAgenceHotel> ListeOffreAgenceHotel = new List<OffreAgenceHotel>();

        //Hosted web API REST Service base url  

        public HotelController(ILogger<HotelController> logger )
        {
            _logger = logger;
            ListeClients = ClientMock.GetClientsLists();
             ListeHotel = HotelMock.GetHotels();
            ListeChambre = MockChambre.GetListChambre();
            ListeCategorie = MockeCategorie.GetListCategories();
            ListeReservations = MockReservation.GetListeReservations();
            ListeAgence = MockAgence.GetAgencesLists();
            ListeOffreAgenceHotel = MockOffre.GetOffresLists();

        }

        [Microsoft.AspNetCore.Mvc.HttpGet("weatherforecast")]
        public String Get()
        {
            return "bonjour";
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("hotels")]
        public Hotel[] GetHotels()
        {
            return HotelMock.GetHotels().ToArray();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("reservations")]
        public List<Reservation> GetReservations()
        {
            return MockReservation.GetListeReservations();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("clients")]
        public List<Client> GetClients()
        {
            return ClientMock.GetClientsLists();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("categories")]
        public List<Categorie> GetCategories()
        {
            return MockeCategorie.GetListCategories();
        }

        // rechercher des hotel par les clients; question 1
        [Microsoft.AspNetCore.Mvc.HttpGet("search")]
        public Response<Hotel> SearchHotel([FromUri] String ville, [FromUri] String nomHotel,
            [FromUri] int categorie,
            [FromUri] DateTime dateArrivee, [FromUri] DateTime dateDepart,
            [FromUri] int prixMin, [FromUri] int prixMax, [FromUri] int nombrePersonne)
        {
            Response<Hotel> response = new Response<Hotel>();

            IEnumerable<Hotel> allHotels = from Hotel hotel in ListeHotel
                                           select hotel;

            if (!String.IsNullOrEmpty(ville))
            {
                allHotels = allHotels.Where(p => p.Ville.ToLower().Equals(ville.ToLower()));
            }
            if (nomHotel != null)
            {
                allHotels = allHotels.Where(p => p.nomHotel.ToLower().Equals(nomHotel.ToLower()));
            }

            if (categorie >= 1)
            {
                allHotels = allHotels.Where(p => p.idCategorie == categorie);
            }
            if (nombrePersonne >= 1)
            {
                allHotels = allHotels.Where(p => p.nombreLit <= nombrePersonne);
            }

            foreach (Hotel q in allHotels)
            {
                ReturnListeHotel.Add(new Hotel(q.IdHotel, q.nomHotel, q.nombreChambre, q.nombreLit, q.lieuDit, q.idCategorie,
                 q.Rue, q.Pays, q.Numero, q.Latitude, q.Longitude, q.Ville));
            }

            if (prixMin >= 1 && prixMax >= 2)
            {
                ReturnListeHotel = new List<Hotel>();
                var hc = from Hotel hotel in allHotels
                         join chambre in ListeChambre
                            on hotel.IdHotel
                            equals chambre.IdHotel
                         where chambre.Prix >= prixMin && chambre.Prix <= prixMax
                         select
                             new
                             {
                                 hotel,
                                 chambre
                             };

                foreach (var q in hc)
                {
                    Hotel h = new Hotel(q.hotel.IdHotel, q.hotel.nomHotel, q.hotel.nombreChambre, q.hotel.nombreLit, q.hotel.lieuDit, q.hotel.idCategorie,
                  q.hotel.Rue, q.hotel.Pays, q.hotel.Numero, q.hotel.Latitude, q.hotel.Longitude, q.hotel.Ville);

                    if (q.chambre != null)
                    {
                        h.SetChambres(q.chambre);
                    }
                    ReturnListeHotel.Add(h);
                }
            }
            if (dateDepart >= DateTime.Now && dateArrivee >= DateTime.Now)
            {
                ReturnListeHotel = new List<Hotel>();
                var hc = from Hotel hotel in allHotels
                         join chambre in ListeChambre
                            on hotel.IdHotel
                            equals chambre.IdHotel
                         join reservation in ListeReservations
                         on chambre.IdChambre
                         equals reservation.IdChambre
                         select
                             new
                             {
                                 hotel,
                                 chambre
                             };

                foreach (var q in hc)
                {
                    Hotel h = new Hotel(q.hotel.IdHotel, q.hotel.nomHotel, q.hotel.nombreChambre, q.hotel.nombreLit, q.hotel.lieuDit, q.hotel.idCategorie,
                  q.hotel.Rue, q.hotel.Pays, q.hotel.Numero, q.hotel.Latitude, q.hotel.Longitude, q.hotel.Ville);

                    if (q.chambre != null)
                    {
                        h.SetChambres(q.chambre);
                    }
                    ReturnListeHotel.Add(h);
                }
            }

            response.Responses = ReturnListeHotel.ToArray();
            response.Message = "Reponse true. Resultat trouvé " + ReturnListeHotel.Count;
            return response;
        }

        //reserver dans un hotel
        [HttpPost("reserver")]
        public ReserverRequest DoReservat([FromForm] ReserverRequest reserverRequest)
        {
            int idReservation = ListeReservations.Count;
            int idClient = ListeClients.Count;
            int prix = reserverRequest.PrixReservation * reserverRequest.NombrePersonne;
            Reservation reservation = new Reservation(idReservation, idClient,
                prix, reserverRequest.IdChambre, reserverRequest.DateDebut, reserverRequest.DateFin,
                reserverRequest.Isfree
                );
            Client client = new Client(idClient, reserverRequest.NomClient, reserverRequest.PrenomClient, reserverRequest.InfosPayement);

            ListeReservations.Add(reservation);
            ListeClients.Add(client);

            reserverRequest.PrixReservation = prix;
            return reserverRequest;
        }

        //ajouter une chambre avec image
        [HttpPost("chambres")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data", "application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFile([FromForm] Chambre chambre)
        {
                await addChambre(chambre);
            return Ok(chambre.Photo.FileName);
        }

        //recuperer une image 
        [Microsoft.AspNetCore.Mvc.HttpGet("chambres")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data", "application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> getFile([FromUri] String filename)
        {
            Byte[] file;
            FileContentResult f = null;
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files", filename);
                file = System.IO.File.ReadAllBytes(path);
                 f = File(file, "image/png");
            }
            catch (Exception e)
            {
            }
            return f;
        }

        private async Task<bool> addChambre( Chambre chambre)
        {
            bool isSaveSuccess = false;
            string fileName;
            try
            {
                var extension = "." + chambre.Photo.FileName.Split('.')[chambre.Photo.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks + extension; 

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await chambre.Photo.CopyToAsync(stream);
                }

                isSaveSuccess = true;
            }
            catch (Exception e)
            {
            }
            return isSaveSuccess;
        }

        // Service	web	1 :	Consulter	les	disponibilités	par	les	agences	partenaires	
        [Microsoft.AspNetCore.Mvc.HttpGet("search/offres")]
        public Response<ResponseHotel> SearchOffre([FromUri] int idHotel, [FromUri] String  identifiant, [FromUri] String  motpass, [FromUri] DateTime dateDebut,
            [FromUri] DateTime dateFin, [FromUri] int nombrePersonne)
        {
            Response<ResponseHotel> response = new Response<ResponseHotel>();

            Agence agence = ListeAgence.Find(e => e.Identifiant == identifiant && e.Password == motpass);

            var allHotels = from Hotel hotel in ListeHotel
                                            join offre in ListeOffreAgenceHotel on hotel.IdHotel equals offre.IdHotel
                                            join chambre in ListeChambre on offre.IdChambre equals chambre.IdChambre
                                           where hotel.IdHotel == idHotel 
                                           && agence.IdAgence == offre.IdAgence
                                           && hotel.nombreChambre >= nombrePersonne
                                           && offre.Diponibilite >= dateDebut
                                           select new { chambre, offre, hotel};

            foreach (var q in allHotels)
            {
                Chambre c = q.chambre;
                c.Disponible = q.offre.Diponibilite;
                c.Prix = q.offre.IdPrixChambre; //changer le prix de la chambre en fonction du offre

                ResponseHotel h = new ResponseHotel(q.offre.IdOffre, q.hotel.nomHotel, q.hotel.nombreChambre, q.hotel.Ville, c);
                ResponseH.Add(h);
            }
            response.Message = "resultat " + ResponseH.Count;
            response.Responses = ResponseH.ToArray();
                return response;

        }

      //  Service web	2: Effectuer une réservation
      [Microsoft.AspNetCore.Mvc.HttpPost("reserver/offres")]
        public String ReserverOffre([FromForm] RequestReseverOffre request)
        {
            OffreAgenceHotel offr = null;
            Reservation reservation = new Reservation();
            Agence agence = ListeAgence.Find(e => e.Identifiant == request.Identifiant && e.Password == request.MotDepasse && e.IdAgence == request.IdAgence);
                      
            IEnumerable<OffreAgenceHotel> offres = from OffreAgenceHotel of in ListeOffreAgenceHotel
                        where of.IdOffre == request.IdOffre && of.IdAgence == agence.IdAgence select of;
            if(offres.Count() > 0)
            {
                offr = offres.ElementAt(0);
                ListeOffreAgenceHotel.RemoveAll( e => e.IdOffre == request.IdOffre);
                reservation = new Reservation(new Random(8).Next(10, 20), agence.IdAgence, offr.IdPrixChambre, offr.IdChambre, offr.Diponibilite, offr.Diponibilite, false);            
            }
                         
            return offr == null ? "Cette offre n'existe plus" : "Réservation comfirmé ref: ref" + reservation.IdReservation;

        }

        //comparateurs service Web par agence

        [Microsoft.AspNetCore.Mvc.HttpGet("agences")]
        public Response<Hotel> SearchByAgence([FromUri] int idAgence)
        {
            Response<Hotel> response = new Response<Hotel>();

            Agence agence = ListeAgence.Find(e => e.IdAgence == idAgence);
            if (agence == null)
            {
                response.Message = "Cette Agence n'existe pas";
                return response;
            }

            var allHotels = from Hotel hotel in ListeHotel
                            join offre in ListeOffreAgenceHotel on hotel.IdHotel equals offre.IdHotel
                            join chambre in ListeChambre on offre.IdChambre equals chambre.IdChambre
                            join agenc in ListeAgence on offre.IdAgence equals agenc.IdAgence
                            where offre.IdAgence == idAgence
                            && agence.IdAgence == offre.IdAgence select new { agenc, offre, chambre, hotel };

            foreach (var q in allHotels)
            {
                Chambre c = q.chambre;
                c.Disponible = q.offre.Diponibilite;
                c.Prix = q.offre.IdPrixChambre; //changer le prix de la chambre en fonction du offre

                Hotel h = new Hotel(q.hotel.IdHotel, q.hotel.nomHotel, q.hotel.nombreChambre, q.hotel.nombreLit, q.hotel.lieuDit, q.hotel.idCategorie,
                                q.hotel.Rue, q.hotel.Pays, q.hotel.Numero, q.hotel.Latitude, q.hotel.Longitude, q.hotel.Ville);
                h.Agence = q.agenc.Name;
                h.Chambres = c;
                ReturnListeHotel.Add(h);
            }
            response.Message = "resultat " + ReturnListeHotel.Count;
            response.Responses = ReturnListeHotel.ToArray();
            return response;

        }

        // comparateurs service web pour usager
        [Microsoft.AspNetCore.Mvc.HttpGet("search-by-usager")]
        public async Task<Response<Hotel>> SearchByUsagerHotel([FromUri] String ville, [FromUri] String nomHotel,
            [FromUri] int categorie, [FromUri] DateTime dateArrivee, [FromUri] DateTime dateDepart,
             [FromUri] int nombrePersonne)
        {
            //URL d'appel d'Api Externe fournie par les agence
            string Baseurl = "https://localhost:44341/api/hotel/";

            Response<Hotel> response = new Response<Hotel>();
            List<Hotel> lHotel = new List<Hotel>();
            Response<Hotel> content = new Response<Hotel>();

            //appel web services par agences
            foreach (Agence ag in ListeAgence)
            {
                var client = new RestClient(Baseurl + "agences?idAgence=" + ag.IdAgence);
                var request = new RestRequest(Method.GET);
                IRestResponse resp = await client.ExecuteAsync(request);
                if (resp.IsSuccessful)
                {
                    content = JsonConvert.DeserializeObject<Response<Hotel>>(resp.Content);
                    if (content.Responses.Length > 0)
                    {
                        foreach (Hotel h in content.Responses)
                        {
                            lHotel.Add(h);
                        }
                    }
                }
            }

            IEnumerable<Hotel> allHotels = from Hotel hotel in lHotel
                                           select hotel;
            //effectuer un filtre sur l'ensemble des offres
            if (!String.IsNullOrEmpty(ville))
            {
                allHotels = allHotels.Where(p => p.Ville.ToLower().Equals(ville.ToLower()));
            }
            if (!String.IsNullOrEmpty(nomHotel))
            {
                allHotels = allHotels.Where(p => p.nomHotel.ToLower().Equals(nomHotel.ToLower()));
            }

            if (categorie >= 1)
            {
                allHotels = allHotels.Where(p => p.idCategorie == categorie);
            }
            if (nombrePersonne >= 1)
            {
                allHotels = allHotels.Where(p => p.nombreLit >= nombrePersonne);
            }
            if (dateDepart != new DateTime())
            {
                allHotels = allHotels.Where(p => p.Chambres.Disponible <= dateDepart);
            }

            response.Responses = allHotels.ToArray();
            return response;
        }

    }
}