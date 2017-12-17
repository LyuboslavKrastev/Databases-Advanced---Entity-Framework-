
namespace P01_HospitalDatabase
{
    using System;
    using P01_HospitalDatabase.Data;
    using P01_HospitalDatabase.Data.Models;
    using P01_HospitalDatabase.Initializer;
    class StartUp
    {
        static void Main(string[] args)
        {

            using (var db = new HospitalContext())
            {

                DatabaseInitializer.InitialSeed(db);
            }
        }
    }
}
