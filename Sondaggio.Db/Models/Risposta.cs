using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questionario.Db.Models
{
	[Table("Risposte")]
	public class Risposta
	{
		[Key]
		public int IdRisposta { get; set; }
		public int IdDomanda { get; set; }
		public string IdUtente { get; set; }
		public string TestoRisposta { get; set; }
		public int StelleRisposta { get; set; }
		public DateTime? dtAgg { get; set; }
		[ForeignKey("IdDomanda")]
		public virtual Domanda Domanda { get; set; }
		[ForeignKey("IdUtente")]
		public virtual Utente Utente { get; set; }
		[NotMapped]
		public string StelleImmagine
		{
			get
			{
				var v = (StelleRisposta * 10).ToString("##") + ".png";
				if (v == ".png")
					v = "0.png";
				return v;
			}
		}
	}
}