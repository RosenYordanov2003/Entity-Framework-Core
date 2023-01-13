using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Increase_Minion_Age
{
    public class Program
    {
        static void Main(string[] args)
        {
            int[] minionsId = Console.ReadLine().Split(" ",StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            using (SqlConnection sqlConnection =
                  new SqlConnection("Server=DESKTOP-955U7HH\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;"))
            {
                sqlConnection.Open();
                string result = ExecuteMethod(minionsId, sqlConnection);
                Console.WriteLine(result);
            }
        }

        private static string ExecuteMethod(int[] minionsId, SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();
            SqlTransaction transaction = sqlConnection.BeginTransaction();
            try
            {
                UpdateMinionsAge(minionsId, sqlConnection, transaction);

                UpdateMinionsName(minionsId, sqlConnection, transaction);

                GetAllMinions(sqlConnection, transaction, sb);

                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                Console.WriteLine(exception.Message);
            }
            return sb.ToString().Trim();
        }

        private static void UpdateMinionsAge(int[] minionsId, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            for (int i = 0; i < minionsId.Length; i++)
            {
                int currentId = minionsId[i];
                string query = @"UPDATE Minions
                            SET Age = Age+1
                            WHERE Id = @minionId";
                SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction);
                cmd.Parameters.AddWithValue("@minionId", currentId);
                cmd.ExecuteNonQuery();
            }
        }

        private static void UpdateMinionsName(int[] minionsId, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            for (int i = 0; i < minionsId.Length; i++)
            {
                int currentId = minionsId[i];
                string query = @"UPDATE Minions
                           SET [Name] = CONCAT(UPPER(SUBSTRING([Name],1,1)),(SUBSTRING([Name],2,LEN([Name])-1)))
                           WHERE Id = @minionId";
                SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction);
                cmd.Parameters.AddWithValue("@minionId", currentId);
                cmd.ExecuteNonQuery();
            }
        }

        private static void GetAllMinions(SqlConnection sqlConnection, SqlTransaction transaction, StringBuilder sb)
        {
            string query = @"SELECT [Name]
                           ,Age
                           FROM Minions";
            SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string minionName = (string)reader["Name"];
                int age = (int)reader["Age"];
                sb.AppendLine($"{minionName} {age}");
            }
        }
    }
}
