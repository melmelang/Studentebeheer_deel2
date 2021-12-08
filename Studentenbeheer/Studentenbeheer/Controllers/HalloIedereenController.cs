using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Studentenbeheer.Controllers
{
    public class HalloIedereenController : Controller
    {
        public string Index()
        {
            return "Dit is de standaard pagina om iedereen welkom te heten";
        }

        public string Welkom(string voornaam, string achternaam)
        {
            //return "Welkom " + voornaam + " " + achternaam;
            return HtmlEncoder.Default.Encode($"Hallo {voornaam} {achternaam}");
        }
    }
}
