namespace AddMinion
{
    using System;
    using System.Data.SqlClient;
    using System.Text;

    public class Program
    {
        static void Main(string[] args)
        {

            string[] minionInformation = Console.ReadLine().Split(": ")[1].Split(" ");
            string[] vilianInformation = Console.ReadLine().Split(": ");
            using (SqlConnection sqlConnection =
                  new SqlConnection("Server=DESKTOP-955U7HH\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;"))
            {
                sqlConnection.Open();
                string result = ExecuteTransactionMethod(minionInformation, vilianInformation, sqlConnection);
                Console.WriteLine(result);
            }
        }

        private static string ExecuteTransactionMethod(string[] minionInformation, string[] vilianInformation, SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();
            SqlTransaction transaction = sqlConnection.BeginTransaction();
            try
            {
                int townId = GetTownId(minionInformation, sqlConnection, transaction, sb);
                int vilianId = GetVilianId(vilianInformation, sqlConnection, transaction, sb);
                InsertMinion(minionInformation, sqlConnection, transaction, townId);
                string insertMinionToVilianQuery = @"INSERT INTO MinionsVillains(MinionId,VillainId)VALUES
                                                    (@minionId, @vilianId)";
                int minionId = GetMinionId(minionInformation, sqlConnection, transaction);
                InsertMinionToVilianMethod(sqlConnection, transaction, vilianId, insertMinionToVilianQuery, minionId, sb, minionInformation[0], vilianInformation[1]);
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                Console.WriteLine(exception.Message);
            }
            return sb.ToString().Trim();
        }


        private static int GetTownId(string[] minionInformation, SqlConnection sqlConnection, SqlTransaction transaction, StringBuilder sb)
        {
            string townQuery = @"SELECT T.Id
                                FROM Towns AS T
                                WHERE T.[Name] = @townName";
            string townName = minionInformation[2];
            SqlCommand townCmd = new SqlCommand(townQuery, sqlConnection, transaction);
            townCmd.Parameters.AddWithValue("@townName", townName);
            object townId = townCmd.ExecuteScalar();
            if (townId == null)
            {
                string insertQuery = @"INSERT INTO Towns ([Name])VALUES
                                      (@townName)";
                SqlCommand insertTownCommand = new SqlCommand(insertQuery, sqlConnection, transaction);
                insertTownCommand.Parameters.AddWithValue("@townName", minionInformation[2]);
                insertTownCommand.ExecuteNonQuery();
                sb.AppendLine($"Town {minionInformation[2]} was added to the database.");
            }
            return (int)townCmd.ExecuteScalar();
        }

        private static int GetVilianId(string[] vilianInformation, SqlConnection sqlConnection, SqlTransaction transaction, StringBuilder sb)
        {
            string vilianName = vilianInformation[1];

            string vilianQuery = @"SELECT id
                                  FROM Villains
                                  WHERE [Name] = @vilianName";
            SqlCommand vilianCmd = new SqlCommand(vilianQuery, sqlConnection, transaction);
            vilianCmd.Parameters.AddWithValue("@vilianName", vilianName);
            object vilianId = vilianCmd.ExecuteScalar();
            if (vilianId == null)
            {
                string getEvilnesFactorIdQuery = @"SELECT Id 
                                          FROM EvilnessFactors
                                          WHERE [Name] ='Evil'";
                SqlCommand evilnessFactorCmd = new SqlCommand(getEvilnesFactorIdQuery, sqlConnection, transaction);
                int evilnessFactorId = (int)evilnessFactorCmd.ExecuteScalar();
                string insertVilianQuery = @"INSERT INTO Villains([Name], EvilnessFactorId)VALUES
                                           (@vilianName, @evilnessFactorId)";
                SqlCommand insertVilainCmd = new SqlCommand(insertVilianQuery, sqlConnection, transaction);
                insertVilainCmd.Parameters.AddWithValue("@vilianName", vilianInformation[1]);
                insertVilainCmd.Parameters.AddWithValue("@evilnessFactorId", evilnessFactorId);
                insertVilainCmd.ExecuteNonQuery();
                sb.AppendLine($"Villain {vilianName} was added to the database.");
            }
            return (int)vilianCmd.ExecuteScalar();
        }

        private static void InsertMinion(string[] minionInformation, SqlConnection sqlConnection, SqlTransaction transaction, int townId)
        {
            string insertMinionQuery = @"INSERT INTO Minions([Name],[Age],[TownId])VALUES
                                            (@minionName,@age, @townId)";

            SqlCommand insertMinionCmd = new SqlCommand(insertMinionQuery, sqlConnection, transaction);
            int minionAge = int.Parse(minionInformation[1]);
            insertMinionCmd.Parameters.AddWithValue("@minionName", minionInformation[1]);
            insertMinionCmd.Parameters.AddWithValue("@age", minionAge);
            insertMinionCmd.Parameters.AddWithValue("@townId", townId);
            insertMinionCmd.ExecuteNonQuery();
        }

        private static int GetMinionId(string[] minionInformation, SqlConnection sqlConnection, SqlTransaction transaction)
        {
            string getMinionIdQuery = @"SELECT Id
                                   FROM Minions
                                   WHERE [Name] = @name";
            SqlCommand getMinionIdCmd = new SqlCommand(getMinionIdQuery, sqlConnection, transaction);
            getMinionIdCmd.Parameters.AddWithValue("@name", minionInformation[1]);
            return (int)getMinionIdCmd.ExecuteScalar();
        }

        private static void InsertMinionToVilianMethod(SqlConnection sqlConnection, SqlTransaction transaction, int vilianId, string insertMinionToVilianQuery, int minionId, StringBuilder sb, string minionName, string vilianName)
        {
            SqlCommand insertMinionToVilianCmd = new SqlCommand(insertMinionToVilianQuery, sqlConnection, transaction);
            insertMinionToVilianCmd.Parameters.AddWithValue("@minionId", minionId);
            insertMinionToVilianCmd.Parameters.AddWithValue("@vilianId", vilianId);
            insertMinionToVilianCmd.ExecuteNonQuery();

            sb.AppendLine($"Successfully added {minionName} to be minion of {vilianName}.");
        }
    }
}
