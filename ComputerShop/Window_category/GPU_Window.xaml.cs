using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ComputerShop.Window_category
{
    /// <summary>
    /// Логика взаимодействия для GPU_Window.xaml
    /// </summary>
    public partial class GPU_Window : Window
    {
        public GPU_Window()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || Convert.ToInt32(Price.Text) <= 0 || Country.Text == "" || Manufactured.Text == "")
            {
                MessageBox.Show("Введите корректное значение!");
            }
            else
            {
                SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                sqlCon.Open();

                String query = "INSERT INTO [product] (id_category, name_product, price, country, manufacturer) VALUES ( 3, @name, @price, @country, @manufacturer); SELECT SCOPE_IDENTITY()";
                // String query_replay = "SELECT COUNT(*)  FROM  users WHERE  (login LIKE @login) OR (email LIKE @email)";

                SqlCommand com = new SqlCommand(query, sqlCon);
                com.Parameters.AddWithValue("@name", Name.Text);
                com.Parameters.AddWithValue("@price", Price.Text);
                com.Parameters.AddWithValue("@country", Country.Text);
                com.Parameters.AddWithValue("@manufacturer", Manufactured.Text);
                com.ExecuteNonQuery();
                
                    String query2 = "select id_product from product where name_product = @name";
                
                SqlCommand comm = new SqlCommand(query2, sqlCon);
                comm.Parameters.AddWithValue("@name", Name.Text);
                int id_prod = Convert.ToInt32(comm.ExecuteScalar());

                AddСharacteristic(id_prod);
            }

        }
        private void AddСharacteristic(int id)
        {
            if (Name.Text == "" || Convert.ToInt32(Price.Text) <= 0 || Country.Text == "" || Manufactured.Text == "")
            {
                MessageBox.Show("Введите корректное значение!");
            }
            else
            {
                SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                sqlCon.Open();

                String query = "INSERT INTO [characteristic_product] (id_characteristic, id_product,value) VALUES (1, @id, @frequency), (2, @id, @size), (3, @id, @type), (4, @id, @teh), (5, @id, @tdp), (21, @id, @massa), (14, @id, @dlinna), (15, @id, @shirina)";
                // String query_replay = "SELECT COUNT(*)  FROM  users WHERE  (login LIKE @login) OR (email LIKE @email)";

                SqlCommand com = new SqlCommand(query, sqlCon);
                com.Parameters.AddWithValue("@frequency", Frequency.Text);
                com.Parameters.AddWithValue("@size", Size.Text);
                com.Parameters.AddWithValue("@type", TypeMemory.Text);
                com.Parameters.AddWithValue("@teh", TechnicalProcess.Text);
                com.Parameters.AddWithValue("@tdp", TDP.Text);
                com.Parameters.AddWithValue("@massa", Massa.Text);
                com.Parameters.AddWithValue("@dlinna", Dlina.Text);
                com.Parameters.AddWithValue("@shirina", Shirina.Text);
                com.Parameters.AddWithValue("@id", id);
                com.ExecuteNonQuery();
            }
        }
    }
}
