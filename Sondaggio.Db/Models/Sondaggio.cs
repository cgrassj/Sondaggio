using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Questionario.Db.Models
{
	[Table("Sondaggi")]
	public class Sondaggio
	{
		[Key]
		public int IdSondaggio { get; set; }
		public string TitoloSondaggio { get; set; }
		public string SottoTitoloSondaggio { get; set; }
		public string DescrizioneSondaggio { get; set; }
		public string NoteSondaggio { get; set; }
		public string UrlSondaggio { get; set; }
		[NotMapped]
		public string ListaServizi { get; set; }
		public string TestoEmail { get; set; }
		public bool SondaggioAttivo { get; set; }
		public DateTime? DataScadenzaSondaggio { get; set; }
		public DateTime? dtAgg { get; set; }
		public virtual ICollection<Domanda> Domande { get; set; }
		[NotMapped]
		public string MediaStelle
		{
			get
			{
				int numeroDomande = Domande?.Sum(domanda => domanda.Risposte?.Count ?? 0) ?? 0;
				int numeroStelle = Domande?.Sum(domanda => domanda.Risposte?.Sum(a => a.StelleRisposta) ?? 0) ?? 0;
				double media = 0;

				if (numeroDomande > 0)
					try { media = numeroStelle / numeroDomande; }
					catch (OverflowException overflowException) { }
				
				return Math.Round(media, 1).ToString();
			}
		}
		[NotMapped]
		public string MediaStelleImmagine
		{
			get
			{
				int numeroDomande = Domande?.Sum(domanda => domanda.Risposte?.Count ?? 0) ?? 0;
				int numeroStelle = Domande?.Sum(domanda => domanda.Risposte?.Sum(a => a.StelleRisposta) ?? 0) ?? 0;

				double media = 0;
				if (numeroDomande > 0)
					try { media = numeroStelle/ numeroDomande; }
					catch (OverflowException overflowException) { }

				double ret = 0;
				var d = (Math.Round(media, 1));
				double t = d - Math.Floor(d);
				if (t >= 0.3d && t <= 0.7d)
					ret = Math.Floor(d) + 0.5d;
				else if (t > 0.7d)
					ret = Math.Ceiling(d);
				else if (t < 0.3)
					ret = Math.Floor(d);

				var v = (ret * 10).ToString("##") + ".png";
				if (v == ".png")
					v = "0.png";
				return v;
			}
		}
	}
}