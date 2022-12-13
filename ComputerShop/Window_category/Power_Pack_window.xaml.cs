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
    /// Логика взаимодействия для Power_Pack_window.xaml
    /// </summary>
    public partial class Power_Pack_window : Window
    {
        public Power_Pack_window()
        {
            InitializeComponent();
        }
        int IdProd = 0;
        private int Check(int id_prod)
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();

            String query = "SELECT id_characteristic from characteristic_product where id_product = @id";

            SqlCommand com = new SqlCommand(query, sqlCon);
            com.Parameters.AddWithValue("@id", id_prod);
            int id_prod2 = Convert.ToInt32(com.ExecuteScalar());
            if (id_prod2 == 0)
            {

                String query2 = "delete from product where id_product = @id";

                SqlCommand comm = new SqlCommand(query2, sqlCon);
                comm.Parameters.AddWithValue("@id", id_prod);
                MessageBox.Show("Товар не был добавлен, попробуйте снова!");
                return 0;
            }
            else
            {
                return 1;
            }
        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Check(IdProd);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Name.Text == "" || Convert.ToInt32(Price.Text) <= 0 || Country.Text == "" || Manufactured.Text == "")
            {
                MessageBox.Show("Введите корректное значение!");
            }
            else
            {
                SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                sqlCon.Open();

                String query = "INSERT INTO [product] (id_category, name_product, price, country, manufacturer) VALUES ( 9, @name, @price, @country, @manufacturer); SELECT SCOPE_IDENTITY()";
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
                    MessageBox.Show("Товар успешно добавлен!");

                    this.Close();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Введите числовое значение");
            }

        }
        private void AddСharacteristic(int id)
        {
            if (Frequency.Text == "" || Size.Text == "" || Convert.ToInt32(TypeMemory.Text) <= 0 || TechnicalProcess.Text == "" || Convert.ToInt32(TDP.Text) <= 0)
            {
                MessageBox.Show("Введите корректное значение!");
            }
            else
            {
                SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                sqlCon.Open();

                String query = "INSERT INTO [characteristic_product] (id_characteristic, id_product,value) VALUES (13, @id, @frequency), (20, @id, @size), (21, @id, @type), (22, @id, @teh), (23, @id, @tdp)";
                // String query_replay = "SELECT COUNT(*)  FROM  users WHERE  (login LIKE @login) OR (email LIKE @email)";

                SqlCommand com = new SqlCommand(query, sqlCon);
                com.Parameters.AddWithValue("@frequency", Frequency.Text);
                com.Parameters.AddWithValue("@size", Size.Text);
                com.Parameters.AddWithValue("@type", TypeMemory.Text);
                com.Parameters.AddWithValue("@teh", TechnicalProcess.Text);
                com.Parameters.AddWithValue("@tdp", TDP.Text);

                com.Parameters.AddWithValue("@id", id);
                com.ExecuteNonQuery();
            }
        }
    }
}
