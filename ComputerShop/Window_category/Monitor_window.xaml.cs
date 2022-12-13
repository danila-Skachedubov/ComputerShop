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
    /// Логика взаимодействия для Monitor_window.xaml
    /// </summary>
    public partial class Monitor_window : Window
    {
        public Monitor_window()
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

                String query = "INSERT INTO [product] (id_category, name_product, price, country, manufacturer) VALUES ( 8, @name, @price, @country, @manufacturer); SELECT SCOPE_IDENTITY()";
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
            if( Frequency.Text == "" || Convert.ToInt32(Size.Text) <= 0 || TypeMemory.Text == "" || TechnicalProcess.Text == "" || TDP.Text == "" || Convert.ToInt32(Massa.Text) <= 0 || Dlina.Text == "" || Shirina.Text == "")
            {
                MessageBox.Show("Введите корректное значение!");
            }
            else
            {
                SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                sqlCon.Open();

                String query = "INSERT INTO [characteristic_product] (id_characteristic, id_product,value) VALUES (49, @id, @frequency), (50, @id, @size), (51, @id, @type), (52, @id, @teh), (53, @id, @tdp), (54, @id, @massa), (55, @id, @dlinna), (9, @id, @shirina)";
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
