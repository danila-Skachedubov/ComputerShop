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
            CreateOrder();
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
            string name = (string)drv["name_product"];

           
            int g = AddToOrder(name);
    
            string queryIdOrder = "SELECT * FROM [order]";
           
            SqlConnection con = new SqlConnection(Settings1.Default.connectionString);
           con.Open();            
            SqlCommand com2 = new SqlCommand(queryIdOrder, con);
            int id = Convert.ToInt32(com2.ExecuteScalar());
            order_product(id, g);
        }

        private void order_product(int id_order, int id_product)
        {
            string query = "INSERT [order_product] (id_order, id_product) VALUES (@id_order, @id_product)";
            SqlConnection con = new SqlConnection(Settings1.Default.connectionString);
            con.Open();
            SqlCommand com = new SqlCommand(query, con);

            com.Parameters.AddWithValue("id_order", id_order);
            com.Parameters.AddWithValue("id_product", id_product);
            com.ExecuteNonQuery();
        }
        private static int AddToOrder(string name)
        {

            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);

            int id = 0;
            if (sqlCon.State == ConnectionState.Closed)
                try
                {
                    string query = "SELECT id_product FROM product WHERE name_product = @name";
                    SqlCommand MySqlCommand = new SqlCommand(query, sqlCon);
                    sqlCon.Open();
                    MySqlCommand.Parameters.AddWithValue("@name", name);
                    MySqlCommand.ExecuteNonQuery();
                    id = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return 0;
                }
            return id;
        }

        private static void CreateOrder()
        {
            LoginScreen id_user = new LoginScreen();
            int id = id_user.user_id();
            int cost = 0;
            try
            {
                SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                sqlCon.Open();
                string queryToCreateOrder = "INSERT INTO [order] (id_user, date, cost_order) VALUES (@id, @date, @cost)";
                SqlCommand com = new SqlCommand(queryToCreateOrder, sqlCon);
                com.Parameters.AddWithValue("@id", id);
                com.Parameters.AddWithValue("@date", DateTime.Now);
                com.Parameters.AddWithValue("@cost", cost);
                
                id = Convert.ToInt32(com.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            
            }
        }



        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            DeleteFromOrder_Prodeuct();
            DeleteFromOrder();
        }

        private void DeleteFromOrder_Prodeuct()
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            string queryToorder_productDelete = "delete order_product";
            sqlCon.Open();
            SqlCommand com = new SqlCommand(queryToorder_productDelete, sqlCon);
            com.ExecuteNonQuery();
        }

        private void DeleteFromOrder()
        {
            //LoginScreen id_user = new LoginScreen();        //удалять только свою корзину
            //int id = id_user.user_id();
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            string queryToorderDelete = "delete [order] WHERE id_user = @current_id";
            sqlCon.Open();
            SqlCommand com = new SqlCommand(queryToorderDelete, sqlCon);
            //com.Parameters.AddWithValue("@current_id", id);
            com.ExecuteNonQuery();
        }

        private void GoToOrder(object sender, RoutedEventArgs e)
        {
            OrderWindow newOrderWindow = new OrderWindow();
            newOrderWindow.Show();
            
        }
    }
}
