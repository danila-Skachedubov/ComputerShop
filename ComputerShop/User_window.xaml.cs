using System.Data.SqlClient;
using System;
using System.Windows;
using System.Data;

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для User_window.xaml
    /// </summary>
    public partial class User_window : Window
    {
        public User_window()
        {
            InitializeComponent();
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            String query = "SELECT name_category FROM category";
            SqlCommand createCommand = new SqlCommand(query, sqlCon);

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("roles_user");
            dataAdp.Fill(dt);
            //UsersGrid.ItemsSource = dt.DefaultView;
            Category.ItemsSource = dt.DefaultView;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            String query = "SELECT name_product, price, country, manufacturer FROM product";
            SqlCommand createCommand = new SqlCommand(query, sqlCon);

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("product");
            dataAdp.Fill(dt);
            ProductGrid.ItemsSource = dt.DefaultView;
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            string CurrentCategory = Category.Text;
            int id_category = 0;
            

            switch (CurrentCategory)
            {
                case ("Видеокарта"):
                    id_category = 1;
                    break;
                case ("Процессор"):
                    id_category = 2;
                    break;
                case ("Корпус"):
                    id_category = 3;
                    break;
                case ("Оперативная память"):
                    id_category = 4;
                    break;
                case ("Жесткий диск"):
                    id_category = 5;
                    break;
                case ("Клавиатура"):
                    id_category = 6;
                    break;
                case ("Мышь"):
                    id_category = 7;
                    break;
                case ("Монитор"):
                    id_category = 8;
                    break;
                case ("Блок питания"):
                    id_category = 9;
                    break;
                case ("Материнская плата"):
                    id_category = 10;
                    break;
                default:
                    MessageBox.Show("Выберите категорию!");
                    break;
            }
            SqlParameter nameParam = new SqlParameter("@id", id_category);
            string query_id = "SELECT name_product, price, country, manufacturer FROM product WHERE id_product = @id";
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            SqlCommand command = new SqlCommand(query_id, sqlCon);
            command.Parameters.Add(nameParam);
            SqlDataAdapter dataAdp = new SqlDataAdapter(command);
            System.Data.DataTable dt = new System.Data.DataTable("product");
            dataAdp.Fill(dt);
            ProductGrid.ItemsSource = dt.DefaultView;

        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)ProductGrid.SelectedItem;
            string id2 = (string)drv["Name product"];

            DeleteRow(id2);
        }
        private void DeleteRow(string id)
        {

            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            if (sqlCon.State == ConnectionState.Closed)
                try
                {
                    string query = "INSERT INTO [order] (login, password, roles, name, surname, email) VALUES (@login, @password, @roles, @name, @surname, @email";
                    SqlCommand MySqlCommand = new SqlCommand(query, sqlCon);
                    //параметризованный запрос

                    sqlCon.Open();
                    //создаём команду

                    //создаем параметр и добавляем его в коллекцию
                    MySqlCommand.Parameters.AddWithValue("@id", id);
                    //выполняем sql запрос
                    MySqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

        }
    }
}
