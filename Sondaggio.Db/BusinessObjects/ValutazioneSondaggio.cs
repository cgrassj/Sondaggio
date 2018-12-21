using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionario.Db.Models;

namespace Questionario.Db.BusinessObjects
{
	public class ValutazioneSondaggio
	{
		private readonly Sondaggio _sondaggio;

		public ValutazioneSondaggio(Sondaggio sondaggio)
		{
			_sondaggio = sondaggio;
		}

		public Sondaggio sondaggio
		{
			get { return _sondaggio; }
		}
		public string NumeroFeedback => (_sondaggio.Domande?.Sum(domanda => domanda.Risposte?.Count(b => b.StelleRisposta > 0) ?? 0) ?? 0).ToString();

		public string MediaStelleImmagine
		{
			get
			{
				int numeroDomande = _sondaggio.Domande?.Sum(domanda => domanda.Risposte?.Count(b => b.StelleRisposta > 0) ?? 0) ?? 0;
				int numeroStelle = _sondaggio.Domande?.Sum(domanda => domanda.Risposte?.Where(b => b.StelleRisposta > 0).Sum(a => a.StelleRisposta) ?? 0) ?? 0;

				double media = 0;
				if (numeroDomande > 0)
					try { media = numeroStelle / numeroDomande; }
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
				if (v == "-10.png")
					v = "0.png";
				return v;
			}
		}
	}
}

