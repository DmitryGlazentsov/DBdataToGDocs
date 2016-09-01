using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;


namespace TestConnectionDB
{
    class Program
    {
        static void Main(string[] args)
        {
            //\SQLEXPRESS
            
            string connectionString = @"Data Source=Z14-0368-KFTEST;
                                        Initial Catalog=AlphaEds;";
        #region Login/password
            Console.WriteLine("User:");
            string login = Console.ReadLine();
            login = System.Text.RegularExpressions.Regex.Replace(login, " +", "");

            Console.WriteLine("Pwd:");
            string pwd = Console.ReadLine();
            pwd = System.Text.RegularExpressions.Regex.Replace(pwd, " +", "");
            #endregion
            
            connectionString += "User ID=" + login + ";Password=" + pwd;
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Что-то пошло не так: {0}", se.Message);
                Console.ReadLine();
                return;
            }

            //все пакеты с ШК документами
            using (SqlCommand get_All_SK_documents_in_one_day = new SqlCommand(@"select * from ValStatBatches
                                                               where DocumentsAfter like '%ШК%' 
                                                               and BatchName like '2016-08-30%'",
                                                               connection)){
                
                SqlDataReader data = get_All_SK_documents_in_one_day.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                for (int col = 0; col < data.FieldCount; col++)
                {
                    Console.Write("{0}\t", data.GetName(col).ToString().Trim());
                }

                while (data.Read())
                { 
                    Console.WriteLine("\n{0}\t{1}\t{2}", data.GetValue(0).ToString().Trim(),
                                                         data.GetValue(1).ToString().Trim(),
                                                         data.GetValue(2).ToString().Trim());
                }
                
                              
                connection.Close();
                connection.Dispose();
                Console.WriteLine("\nОтключился");
                Console.ReadLine();
                
            }
            
        }
    }
}
