using System.Diagnostics.Metrics;

namespace radio1.Models.Entities
{


	/// <summary>
	/// le tchnicien
	/// </summary>
	public class Technicien
	{
		public int Id { get; set; }
		public string Prenom { get; set; }
		public string Nom { get; set; }
		public string Sexe { get; set; }
		public DateTime DateCreation { get; set; }

		public Technicien() { }
	}
}
