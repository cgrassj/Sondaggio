using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionario.Db.Models;

namespace Questionario.Db.BusinessObjects
{
	class ValutazioneDomanda
	{
		private readonly Domanda _domanda;

		public ValutazioneDomanda(Domanda domanda)
		{
			_domanda = domanda;
		}

		public Domanda Domanda()
		{
			return _domanda;
		}

		public string MediaStelle
		{
			get
			{
				double media = 0;
				if (_domanda.Risposte?.Count(b => b.StelleRisposta > 0) > 0)
					try { media = _domanda.Risposte.Where(b => b.StelleRisposta > 0).Average(a => a.StelleRisposta); }
					catch (OverflowException overflowException) { }
				return Math.Round(media, 1).ToString();
			}
		}

		public string MediaStelleImmagine
		{
			get
			{
				double media = 0;
				if (_domanda.Risposte?.Count(b => b.StelleRisposta > 0) > 0)
					try
					{
						media = _domanda.Risposte.Where(b => b.StelleRisposta > 0).Average(a => a.StelleRisposta);
					}
					catch (OverflowException overflowException)
					{
					}

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
}
