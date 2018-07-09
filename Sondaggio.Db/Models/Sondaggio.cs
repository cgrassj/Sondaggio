using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
		public bool SondaggioAttivo { get; set; }
		public DateTime? DataScadenzaSondaggio { get; set; }
		public DateTime? dtAgg { get; set; }
		public virtual ICollection<Domanda> Domande { get; set; }
	}
}