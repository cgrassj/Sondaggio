using System.Data.Entity;
using Questionario.Db.Models;

namespace Questionario.Db
{
	public class QuestionarioContext : DbContext
	{
		// Il contesto è stato configurato per utilizzare una stringa di connessione 'QuestionarioContext' dal file di configurazione 
		// dell'applicazione (App.config o Web.config). Per impostazione predefinita, la stringa di connessione è destinata al 
		// database 'ModelliEntityFramework.Sondaggi' nell'istanza di LocalDb. 
		// 
		// Per destinarla a un database o un provider di database differente, modificare la stringa di connessione 'QuestionarioContext' 
		// nel file di configurazione dell'applicazione.
		public QuestionarioContext()
				: base("name=QuestionarioContext")
		{
		}

		// Aggiungere DbSet per ogni tipo di entità che si desidera includere nel modello. Per ulteriori informazioni 
		// sulla configurazione e sull'utilizzo di un modello Code, vedere http://go.microsoft.com/fwlink/?LinkId=390109.

		// public virtual DbSet<MyEntity> MyEntities { get; set; }
		 public virtual DbSet<Sondaggio> Sondaggi { get; set; }
		 public virtual DbSet<Risposta> Risposte { get; set; }
		 public virtual DbSet<Domanda> Domande { get; set; }
		 public virtual DbSet<Utente> Utenti { get; set; }
	}
}