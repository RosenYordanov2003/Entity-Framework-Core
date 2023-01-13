using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace PrintAll_Minion_Names
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection sqlConnection =
                      new SqlConnection("Server=DESKTOP-955U7HH\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;"))
            {
                List<string> mionNames = new List<string>();
                sqlConnection.Open();
                string query = @"SELECT [Name] FROM Minions";
                SqlDataReader reader = FillMinionNamesList(sqlConnection, mionNames, query);
                reader.Close();
                string result = SortMinions(mionNames);
                Console.WriteLine(result);
            }
        }

        static SqlDataReader FillMinionNamesList(SqlConnection sqlConnection, List<string> mionNames, string query)
        {
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string mionionName = (string)reader["Name"];
                mionNames.Add(mionionName);

            }
            return reader;
        }

        static string SortMinions(List<string> mionNames)
        {
            int oddIndex = 0;
            int evenIndex = 0;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < mionNames.Count; i++)
            {
                if (i % 2 == 0)
                {
                    stringBuilder.AppendLine(mionNames[0 + evenIndex++]);
                }
                else
                {
                    stringBuilder.AppendLine(mionNames[mionNames.Count - 1 - oddIndex++]);
                }
            }
            return stringBuilder.ToString().Trim();
        }
    }
}
