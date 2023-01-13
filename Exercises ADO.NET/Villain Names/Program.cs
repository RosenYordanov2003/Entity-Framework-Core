using System;
using System.Data.SqlClient;
using System.Text;

namespace ConsoleApp301
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection sqlConnection =
                   new SqlConnection("Server=DESKTOP-955U7HH\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;"))
            {
                sqlConnection.Open();
                string result = GetVillianNamesAndTheirMinionsCount(sqlConnection);
                Console.WriteLine(result);
            }
        }
        private static string GetVillianNamesAndTheirMinionsCount(SqlConnection sqlConnection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string query = @"SELECT V.[Name]
                              ,COUNT(M.[Name]) AS MinionsCount
                               FROM MinionsVillains AS MV
                              JOIN Villains AS V ON MV.VillainId = V.Id
                              JOIN Minions AS M ON MV.MinionId = M.Id
                              GROUP BY V.[Name]
                              HAVING COUNT(M.[Name])> 3
                              ORDER BY  COUNT(M.[Name]) DESC";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string firstName = (string)reader["Name"];
                int count = (int)reader["MinionsCount"];
                stringBuilder.AppendLine($"{firstName}  {count}");
            }
            reader.Close();
            return stringBuilder.ToString().TrimEnd();
        }
    }
}
