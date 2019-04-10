using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Web;

namespace Csharp_SQL
{
    public partial class Form1 : Form
    {
        string connString = "server=127.0.0.1;port=3306;user id=root;password='';database=test;charset=utf8;";
        MySqlConnection conn = new MySqlConnection();
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string sql = @"INSERT INTO `City` (`Id`, `City`) VALUES
                           ('0', '基隆市'),
                           ('1', '臺北市'),
                           ('2', '新北市'),
                           ('3', '桃園市'),
                           ('4', '新竹市'),
";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int index = cmd.ExecuteNonQuery();
            if (index > 0)
            {
                Console.WriteLine("success");
            }
            else
            {
                Console.WriteLine("error");
            }
            conn.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string sql = @"select * from City";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(dt);
            dt.CreateDataReader();

            for (int i= 0;i< dt.Rows.Count; i++)
                Console.WriteLine("{0},{1}", dt.Rows[i]["id"],dt.Rows[i]["city"]);
        }
    }
}