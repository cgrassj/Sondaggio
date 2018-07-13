namespace Questionario.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aggiuntoCampoMail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Utenti", "Mail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Utenti", "Mail");
        }
    }
}
