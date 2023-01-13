using System;
using System.Data;
using System.Data.SqlClient;

namespace Increase_Age_Stored_Procedure
{
    public class Program
    {
        static void Main(string[] args)
        {
            int minionId = int.Parse(Console.ReadLine());
            using (SqlConnection sqlConnection =
                  new SqlConnection("Server=DESKTOP-955U7HH\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;"))
            {
                string procedureName = "usp_GetOlder";
                sqlConnection.Open();
                CallStoreProcedure(minionId, sqlConnection, procedureName);

                PrintMinion(minionId, sqlConnection);
            }
        }

        private static void CallStoreProcedure(int minionId, SqlConnection sqlConnection, string procedureName)
        {
            SqlCommand cmd = new SqlCommand(procedureName, sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@minionId",
                SqlDbType = SqlDbType.Int,
                Value = minionId,
                Direction = ParameterDirection.Input,
            };
            cmd.Parameters.Add(parameter);

            cmd.ExecuteNonQuery();
        }

        private static void PrintMinion(int minionId, SqlConnection sqlConnection)
        {
            string query = @"SELECT [Name]
                                   ,[Age]
                            FROM Minions
                            WHERE Id = @minionId";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@minionId", minionId);
           using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string minionName = (string)reader["Name"];
                int age = (int)reader["Age"];
                Console.WriteLine($"{minionName} {age}");
            }
        }
    }
}
