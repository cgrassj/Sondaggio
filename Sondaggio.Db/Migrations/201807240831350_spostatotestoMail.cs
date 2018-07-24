namespace Questionario.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spostatotestoMail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sondaggi", "TestoEmail", c => c.String());
            DropColumn("dbo.Domande", "TestoEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Domande", "TestoEmail", c => c.String());
            DropColumn("dbo.Sondaggi", "TestoEmail");
        }
    }
}
