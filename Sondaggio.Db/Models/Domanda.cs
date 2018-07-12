using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

		[NotMapped]
		public string MediaStelle
		{
			get
			{
				double media = 0;
				if(Risposte?.Count > 0)
					try { media = Risposte.Average(a => a.StelleRisposta); }
					catch (OverflowException overflowException) { }
				return Math.Round(media, 1).ToString();
			}
		}

		private int conta(int stelle)
		{
			int res = 0;
			if (Risposte?.Count > 0)
				try { res = Risposte.Count(a => a.StelleRisposta == stelle); }
				catch (OverflowException overflowException) { }
			return res;
		}

		[NotMapped]
		public string unaStella => conta(1).ToString();
		[NotMapped]
		public string dueStelle => conta(2).ToString();
		[NotMapped]
		public string treStelle => conta(3).ToString();
		[NotMapped]
		public string quattroStelle => conta(4).ToString();
		[NotMapped]
		public string cinqueStelle => conta(5).ToString();
		[NotMapped]
		public int totaleRisposte => Math.Max(Risposte?.Count() ?? 0, 1);
		[NotMapped]
		public string unaStellaPercentuale => (100 * conta(1) / totaleRisposte).ToString();
		[NotMapped]
		public string dueStellePercentuale => (100 * conta(2) / totaleRisposte).ToString();
		[NotMapped]
		public string treStellePercentuale => (100 * conta(3) / totaleRisposte).ToString();
		[NotMapped]
		public string quattroStellePercentuale => (100 * conta(4) / totaleRisposte).ToString();
		[NotMapped]
		public string cinqueStellePercentuale => (100 * conta(5) / totaleRisposte).ToString();
	}
}