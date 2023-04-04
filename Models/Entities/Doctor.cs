using System.Diagnostics.Metrics;

namespace radio1.Models.Entities
{


    /// <summary>
    /// le medecin
    /// </summary>
    public class Doctor
    {


        public int Id { get; set; }
        public string? Prenom { get; set; }
        public string? Nom { get; set; }
        public string? Matricule { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? DateN { get; set; }
        public string? LieuN { get; set; }
        public string? SituationC { get; set; }
        public string? Sexe { get; set; }
        public string? Adresse { get; set; }
        public string? Ville { get; set; }
        public int? CodePostal { get; set; }
		public DateTime? DateCreation { get; set; }

		public Doctor() { }
    }
}
