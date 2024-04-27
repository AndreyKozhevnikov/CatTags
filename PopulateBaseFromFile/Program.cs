using System;
using System.Data.SqlClient;

namespace MyApp {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            var cnn = new SqlConnection("Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=DXCatBase");
            cnn.Open();
            var comm = new SqlCommand("select name from Category", cnn);

         

            using(SqlDataReader reader = comm.ExecuteReader()) {

                    while(reader.Read()) {

                  
                        Console.WriteLine(reader[0].ToString());
                
                }
            }
            cnn.Close();

            Console.ReadLine();
        }
    }
}