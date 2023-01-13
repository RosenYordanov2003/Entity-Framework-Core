using System;
using System.Data.SqlClient;
using System.Text;

namespace Remove_Villain
{
    public class Program
    {
        static void Main(string[] args)
        {
            int vilianId = int.Parse(Console.ReadLine());
            using (SqlConnection sqlConnection =
                   new SqlConnection("Server=DESKTOP-955U7HH\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;"))
            {
                sqlConnection.Open();
                string result = ExecuteMethodOperations(vilianId, sqlConnection);
                Console.WriteLine(result);
            }
        }

        private static string ExecuteMethodOperations(int vilianId, SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();
            SqlTransaction transaction = sqlConnection.BeginTransaction();
            try
            {
                string query = @"SELECT Id
                                 FROM Villains
                                 WHERE Id =@vilianId";
                SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction);
                cmd.Parameters.AddWithValue("@vilianId", vilianId);
                object id = cmd.ExecuteScalar();

                CheckIdIsExist(id);

                BreakEvilnessFactorConnection(vilianId, sqlConnection, transaction);

                int countReleasedMinions = RelaseMinions(vilianId, sqlConnection, transaction);

                string vilianNameToDelete = GetVilianNameToDelete(vilianId, sqlConnection, transaction);

                DeleteMinionsFromVilian(vilianId, sqlConnection, transaction);

                DeleteVilian(vilianId, sqlConnection, transaction);

                sb.AppendLine($"{vilianNameToDelete} was deleted.");
                sb.AppendLine($"{countReleasedMinions} minions were released.");

                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                Console.WriteLine(exception.Message);
            }
            return sb.ToString();
        }


        private static void CheckIdIsExist(object id)
        {
            if (id == null)
            {
                Console.WriteLine("No such villain was found.");
                Environment.Exit(0);
            }
        }

        private static void BreakEvilnessFactorConnection(int vilianId, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            string query = @"UPDATE Villains
                            SET EvilnessFactorId = NULL
                            WHERE Id = @vilianId";
            SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction);
            cmd.Parameters.AddWithValue("@vilianId", vilianId);
            cmd.ExecuteNonQuery();
        }

        private static int RelaseMinions(int vilianId, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            string query = @"SELECT COUNT(*)
                            FROM MinionsVillains
                            WHERE VillainId = @vilianId";
            SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction);
            cmd.Parameters.AddWithValue("@vilianId", vilianId);
            return (int)cmd.ExecuteScalar();
        }

        private static string GetVilianNameToDelete(int vilianId, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            string query = @"SELECT [Name]
                          FROM Villains
                          WHERE Id = @vilianId";
            SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction);
            cmd.Parameters.AddWithValue("@vilianId", vilianId);
            return (string)cmd.ExecuteScalar();
        }

        private static void DeleteMinionsFromVilian(int vilianId, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            string query = @"DELETE FROM MinionsVillains
                            WHERE VillainId = @vilianId";

            SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction);
            cmd.Parameters.AddWithValue("@vilianId", vilianId);
            cmd.ExecuteNonQuery();
        }

        private static void DeleteVilian(int vilianId, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            string query = @"DELETE FROM Villains
                             WHERE Id = @vilianId";
            SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction);
            cmd.Parameters.AddWithValue("@vilianId", vilianId);
            cmd.ExecuteNonQuery();
        }
    }
}
