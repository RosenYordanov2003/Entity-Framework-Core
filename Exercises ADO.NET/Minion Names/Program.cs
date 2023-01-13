using System;
using System.Data.SqlClient;
using System.Text;

namespace Minion_Names
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection sqlConnection =
                   new SqlConnection("Server=DESKTOP-955U7HH\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;"))
            {
                sqlConnection.Open();
                string viliandId = Console.ReadLine();
                string query = @"SELECT [Name]
                                FROM Villains
                                WHERE Id = @VilianId";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@VilianId", viliandId);
                string name = (string)cmd.ExecuteScalar();
                if (name == null)
                {
                    Console.WriteLine($"No villain with ID {viliandId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {name}");
                    string minionsQuery = @"SELECT M.[Name]
                                                  ,M.Age
                                         FROM MinionsVillains AS MV
                                        JOIN Minions AS M ON MV.MinionId = M.Id
                                        WHERE VillainId = @VilianId
                                      ORDER BY M.[Name] ASC";

                    SqlCommand getMinionsCmd = new SqlCommand(minionsQuery, sqlConnection);

                    getMinionsCmd.Parameters.AddWithValue("@VilianId", viliandId);

                   using SqlDataReader minionReader = getMinionsCmd.ExecuteReader();

                    string result = PrintMinions(minionReader);
                    string finalResult = result == null ? "(no minions)" : result;
                    Console.WriteLine(finalResult);
                }
            }
        }

        private static string PrintMinions(SqlDataReader minionReader)
        {
            StringBuilder minionsResult = new StringBuilder();
            int count = 0;
            while (minionReader.Read())
            {
                string minionName = (string)minionReader["Name"];
                int age = (int)minionReader["age"];
                minionsResult.AppendLine($"{++count}. {minionName} {age}");
            }
            return minionsResult.ToString().Trim();
        }
    }
}
