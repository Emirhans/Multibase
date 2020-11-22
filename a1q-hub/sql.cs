using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace a1q_hub
{
    class sql
    {
        SqlCommand command;
        SqlConnection connect = new SqlConnection("Data Source=multibase.database.windows.net;Initial Catalog=MultibaseDB;Persist Security Info=False;User ID=mbadmin; Password=170403yE!; Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False");
       
        public void cn(int a)
        {
            if (a==1)
            connect.Open();

            if (a == 0)
            connect.Close();
        }

        public void cmd(string table, string where)
        {
            command = new SqlCommand("SELECT*FROM " + table.ToString() + " WHERE "+ where.ToString() + "", connect);
        }

        public bool login(string us, string pass)
        {
            cn(1);
            cmd("Users", "userName=@u and password=@p");
            command.Parameters.AddWithValue("@u", us);
            command.Parameters.AddWithValue("@p", pass);
            SqlDataReader read = command.ExecuteReader();

            if (read.Read())
            {
                cn(0);
                return true;
            }
            else
            {
                cn(0);
                return false;
            }
        }

        public string read(string data)
        {
            cn(1);
            SqlDataReader dr = command.ExecuteReader();
            string pull="";
            if (dr.Read())
            {
                pull = dr[data].ToString();
            }
            cn(0);
            return pull;           
        }
       

        public void info()
        {
            connect.Open();
          
        }
    }   
}
