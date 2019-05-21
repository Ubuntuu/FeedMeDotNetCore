using FeedMe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Controllers
{
    public class Sql_connection
    {
        string ConnectionString = "Server=DESKTOP-A44OUAA;Database=FeedMe;User Id=sa;Password=test1234;";
        
        SqlConnection con;

        public void OpenConection()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }


        public void CloseConnection()
        {
            con.Close();
        }


        public void ExecuteQueries(string Query_)
        {
            SqlCommand cmd = new SqlCommand(Query_, con);
            cmd.ExecuteNonQuery();
        }


        public SqlDataReader DataReader(string Query_)
        {
            SqlCommand cmd = new SqlCommand(Query_, con);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }


        public DataTable ReturnDataInDatatable(string Query_)
        {
            SqlDataAdapter dr = new SqlDataAdapter(Query_, ConnectionString);
            DataTable dt = new DataTable();
            dr.Fill(dt);          
            return dt;
        }



        public List<T> ConvertDataTableToGenericList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                   .Select(c => c.ColumnName)
                   .ToList();

            var properties = typeof(T).GetProperties();
            DataRow[] rows = dt.Select();
            return rows.Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                        pro.SetValue(objT, row[pro.Name]);
                }

                return objT;
            }).ToList();
        }

        public IList<Role> GetAllRoleAsList(string Query_)
        {
            var wsl = new List<Role>();

            SqlCommand cmd = new SqlCommand(Query_, con);
            con.Open(); // no need of try/catch or using, as if this fails, no resources are leaked
            using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) //close con on reader.Close
            {
                while (reader.Read())
                {
                   // wsl.Add(CreateFromReader(reader));
                }
            }

            return wsl;
        }
    }
}
