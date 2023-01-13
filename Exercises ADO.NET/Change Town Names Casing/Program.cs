using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Change_Town_Names_Casing
{
    public class Program
    {
        static void Main(string[] args)
        {
            string country = Console.ReadLine();
            using (SqlConnection sqlConnection =
                     new SqlConnection("Server=DESKTOP-955U7HH\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;"))
            {
                sqlConnection.Open();
                string result = ExecuteMethod(country, sqlConnection);
                Console.WriteLine(result);
            }
        }

        private static string ExecuteMethod(string country, SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();
            SqlTransaction transaction = sqlConnection.BeginTransaction();
            try
            {
                int countryCode = GetCountryCode(country, sqlConnection, transaction);
                int countAfectedTowns = UpdateTownsToUpper(countryCode, sqlConnection, transaction);
                string result = countAfectedTowns > 0 ? $"{countAfectedTowns} town names were affected." : "No town names were affected.";
                sb.AppendLine(result);
                if (countAfectedTowns > 0)
                {
                    GetTowns(countryCode, sqlConnection, transaction, sb);
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                Console.WriteLine(exception.Message);
            }
            return sb.ToString().Trim();
        }


        private static int GetCountryCode(string country, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            string getCountryCodeQuery = @"SELECT Id
                                       FROM Countries
                                       WHERE [Name] = @CountryName";
            SqlCommand getCountryCodeCmd = new SqlCommand(getCountryCodeQuery, sqlConnection, transaction);
            getCountryCodeCmd.Parameters.AddWithValue("@CountryName", country);
            return (int)getCountryCodeCmd.ExecuteScalar();
        }

        private static int UpdateTownsToUpper(int countryCode, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            string updateTownsQuery = @"UPDATE Towns
                           SET [Name] = UPPER([Name])
                           WHERE CountryCode = @CountryCode";
            SqlCommand updateTownsCmd = new SqlCommand(updateTownsQuery, sqlConnection, transaction);
            updateTownsCmd.Parameters.AddWithValue("@countryCode", countryCode);

            return updateTownsCmd.ExecuteNonQuery();
        }

        private static void GetTowns(int countryCode, SqlConnection sqlConnection, SqlTransaction transaction, StringBuilder sb)
        {
            List<string> towns = new List<string>();
            string getTownsQuery = @"SELECT [Name]
                                FROM Towns
                                WHERE CountryCode = @CountryCode";
            SqlCommand getTownsCmd = new SqlCommand(getTownsQuery, sqlConnection, transaction);
            getTownsCmd.Parameters.AddWithValue("@CountryCode", countryCode);
          using  SqlDataReader sqlDataReader = getTownsCmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string townName = (string)sqlDataReader["Name"];
                towns.Add(townName);
            }
            string result = string.Join(", ", towns);
            sb.AppendLine($"[{result}]");
        }
    }
}
