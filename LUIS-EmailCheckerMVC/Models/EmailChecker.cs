using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUIS_EmailCheckerMVC.Models
{
    public class EmailChecker
    {
        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
        public IList<Intent> intents { get; set; }
        public IList<Entity> entities { get; set; }
    }

    public class TopScoringIntent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public double score { get; set; }
    }

    public class Query
    {
        public string DescrProblema { get; set; }
        public string Emme { get; set; }
        public string FirmaCalce { get; set; }
        public string NoFix { get; set; }
        public string NomeHotel { get; set; }
        public string OggettoMail{ get; set; }
        public string Società { get; set; }
        public string Software { get; set; }
    }
}