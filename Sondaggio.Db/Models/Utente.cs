using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questionario.Db.Models
{
	[Table("Utenti")]
	public class Utente
	{
		[Key]
		public string IdUtente { get; set; }
		public string Cognome { get; set; }
		public string Nome { get; set; }
		public string Mail { get; set; }
		[NotMapped]
		public string CognomeNome => $"{Cognome} {Nome}";
		public DateTime? dtAgg { get; set; }
	}
}