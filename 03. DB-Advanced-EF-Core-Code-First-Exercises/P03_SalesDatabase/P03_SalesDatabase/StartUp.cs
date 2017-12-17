namespace SalesStartup
{
    using System;
    using P03_SalesDatabase.Data;
    using P03_SalesDatabase.Data.Models;
    using P03_SalesDatabase;
    class StartUp
    {
        static void Main(string[] args)
        {
            using (var db = new SalesContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}
