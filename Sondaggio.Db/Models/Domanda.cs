using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questionario.Db.Models
{
	[Table("Domande")]
	public class Domanda
	{
		[Key]
		public int IdDomanda { get; set; }
		public string TitoloDomanda { get; set; }
		public int? Priorita { get; set; }
		public DateTime? dtAgg { get; set; }
		public int IdSondaggio { get; set; }
		[ForeignKey("IdSondaggio")]
		public virtual Sondaggio Sondaggio { get; set; }
		public virtual ICollection<Risposta> Risposte { get; set; }
	}
}