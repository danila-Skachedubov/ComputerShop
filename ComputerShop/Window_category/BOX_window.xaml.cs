using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
    /// Логика взаимодействия для BOX_window.xaml
    /// </summary>
    public partial class BOX_window : Window
    {
        public BOX_window()
        {
            InitializeComponent();
        }
        int IdProd = 0;
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
                    MessageBox.Show("Товар успешно добавлен!");

                    this.Close();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Введите числовое значение");
            }

        }
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
        private void AddСharacteristic(int id)
        {
            if ( TypeSize.Text == "" || MatPlat.Text == "" || Material.Text == "" || TypeSvet.Text == "" || FormPlat.Text == "" || FormBP.Text == "" || Ventil.Text == "" || Color.Text == "" )
            {
                MessageBox.Show("Введите корректное значение!");
            }
            else
            {
                SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                sqlCon.Open();

                String query = "INSERT INTO [characteristic_product] (id_characteristic, id_product,value) VALUES (24, @id, @frequency), (25, @id, @size), (26, @id, @type), (27, @id, @teh), (28, @id, @tdp), (29, @id, @massa), (30, @id, @dlinna), (9, @id, @shirina)";
                // String query_replay = "SELECT COUNT(*)  FROM  users WHERE  (login LIKE @login) OR (email LIKE @email)";

                SqlCommand com = new SqlCommand(query, sqlCon);
                com.Parameters.AddWithValue("@frequency", TypeSize.Text);
                com.Parameters.AddWithValue("@size", MatPlat.Text);
                com.Parameters.AddWithValue("@type", Material.Text);
                com.Parameters.AddWithValue("@teh", TypeSvet.Text);
                com.Parameters.AddWithValue("@tdp", FormPlat.Text);
                com.Parameters.AddWithValue("@massa", FormBP.Text);
                com.Parameters.AddWithValue("@dlinna", Ventil.Text);
                com.Parameters.AddWithValue("@shirina", Color.Text);
                com.Parameters.AddWithValue("@id", id);
                com.ExecuteNonQuery();
            }
        }
    }
}
