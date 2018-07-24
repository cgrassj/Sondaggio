namespace Questionario.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testoMail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Domande", "TestoEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Domande", "TestoEmail");
        }
    }
}
