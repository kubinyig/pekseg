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

namespace pekseg
{
    public partial class Form1 : Form
    {
        databaseHandler db = new databaseHandler();
        public Form1()
        {
            InitializeComponent();
            start();
        }
        void start()
        {
            updateitemslist();
            button1.Click += (s, e) =>
            {
                if(namebox.Text.Length >= 3 && stockbox.Text.Length > 0 && pricebox.Text.Length > 0)
                {
                    item oneitem = new item();
                    oneitem.id = listBox1.Items.Count;
                    oneitem.name = namebox.Text;
                    oneitem.price = Convert.ToInt32(pricebox.Text);
                    oneitem.stock = Convert.ToInt32(stockbox.Text);
                    db.addone(oneitem);
                    updateitemslist();
                }
            };
            button2.Click += (s, e) =>
            {
                db.deleteone(namebox.Text);
                updateitemslist();
            };
        }
        void updateitemslist()
        {
            listBox1.Items.Clear();
            List<item> items = db.readall();
            foreach (item item in items)
            {
                listBox1.Items.Add(item.name + ": " + item.price + "ft");
            }
        }

    }
    public class databaseHandler
    {
        string serverAddress;
        string username;
        string password;
        string databaseName;
        string connectionString;
        MySqlConnection connection;
        public databaseHandler()
        {
            //szerver címe
            serverAddress = "localhost";
            username = "root";
            password = "";
            databaseName = "pekseg";
            connectionString = $"Server={serverAddress};Database={databaseName};User={username};Password={password}";
            connection = new MySqlConnection(connectionString);
        }
        public List<item> readall()
        {
            List<item> users = new List<item>();
            try
            {
                connection.Open();
                string query = "select * from keszlet";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    item oneuser = new item();
                    oneuser.id = read.GetInt32(read.GetOrdinal("id"));
                    oneuser.name = read.GetString(read.GetOrdinal("name"));
                    oneuser.stock = read.GetInt32(read.GetOrdinal("stock"));
                    oneuser.price = read.GetInt32(read.GetOrdinal("price"));
                    users.Add(oneuser);
                }
                read.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("A hiba: " + e.Message);
            }
            return users;
        }
        public void addone(item oneitem)
        {
                try
                {
                    connection.Open();
                    string query = $"insert into keszlet (name, stock, price) values ('{oneitem.name}',{oneitem.stock},{oneitem.price})";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: " + e.Message);
                }
            } 
        public void deleteone(string name)
        {
            try
            {
                connection.Open();
                                string query = $"DELETE FROM keszlet WHERE name =  '{name}'";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
            
        }
    }
    public class item{
        public int id { get; set; }
        public string name { get; set; }
        public int stock { get; set; }
        public int price { get; set; }
    }
}
