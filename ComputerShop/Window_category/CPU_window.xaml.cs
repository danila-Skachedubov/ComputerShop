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
    /// Логика взаимодействия для CPU_window.xaml
    /// </summary>
    public partial class CPU_window : Window
    {
        public CPU_window()
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

                String query = "INSERT INTO [product] (id_category, name_product, price, country, manufacturer) VALUES ( 2, @name, @price, @country, @manufacturer); SELECT SCOPE_IDENTITY()";
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
            if (Convert.ToInt32(Frequency.Text) <= 0 || Convert.ToInt32(Frequency.Text) <= 0 || Convert.ToInt32(QuantityPotokov.Text) <= 0 || Soket.Text == "" || GPU.Text == "" || TypeMemory.Text == "" || Convert.ToInt32(TDP.Text) <= 0 || Convert.ToInt32(Temp.Text) <= 0)
            {
                MessageBox.Show("Введите корректное значение!");
            }
            else
            {
                SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                sqlCon.Open();

                String query = "INSERT INTO [characteristic_product] (id_characteristic, id_product,value) VALUES (6, @id, @frequency), (7, @id, @size), (8, @id, @type), (10, @id, @teh), (11, @id, @tdp), (17, @id, @massa), (5, @id, @dlinna), (37, @id, @shirina)";
                // String query_replay = "SELECT COUNT(*)  FROM  users WHERE  (login LIKE @login) OR (email LIKE @email)";

                SqlCommand com = new SqlCommand(query, sqlCon);
                com.Parameters.AddWithValue("@frequency", Quantity.Text);
                com.Parameters.AddWithValue("@size", Frequency.Text);
                com.Parameters.AddWithValue("@type", QuantityPotokov.Text);
                com.Parameters.AddWithValue("@teh", Soket.Text);
                com.Parameters.AddWithValue("@tdp", GPU.Text);
                com.Parameters.AddWithValue("@massa", TypeMemory.Text);
                com.Parameters.AddWithValue("@dlinna", TDP.Text);
                com.Parameters.AddWithValue("@shirina", Temp.Text);
                com.Parameters.AddWithValue("@id", id);
                com.ExecuteNonQuery();
            }
        }
    }
}
