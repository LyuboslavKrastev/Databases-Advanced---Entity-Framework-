using System;
using P03_FootballBetting.Data;
using P03_FootballBetting.Data.Models;
namespace P03_FootballBetting
{
    public class Startup
    {
        static void Main(string[] args)
        {
            var db = new FootballBettingContext();
            db.Database.EnsureCreated();
        }
    }
}
