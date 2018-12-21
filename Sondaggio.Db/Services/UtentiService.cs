using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionario.Db.Models;

namespace Questionario.Db.Services
{
	public class UtentiService
	{
		public IQueryable<Utente> GetUtente(string key, IQueryable<Utente> utenti)
		{
			return utenti.Where(utente => utente.IdUtente == key);
		}
	}
}
