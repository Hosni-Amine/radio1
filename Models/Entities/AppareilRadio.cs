using System.Diagnostics.Metrics;

namespace radio1.Models.Entities
{
	public class AppareilRadio
	{
		public int? Id { get; set; }
		public string? NumSerie { get; set; }
		public int? Maintenance { get; set; }
		public List<TypeOperation>? Operations { get; set; }
		public DateTime? DateCreation { get; set; }
		public int? SalleId { get; set; }

		public AppareilRadio () { }

	}
}