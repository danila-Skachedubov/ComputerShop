//  2006-2008 (c) Viva64.com Team
//  2008-2020 (c) OOO "Program Verification Systems"
//  2020-2022 (c) PVS-Studio LLC

using System.Data.SqlClient;
using System;
using System.Windows;
using System.Data;
using System.ComponentModel;


namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для User_window.xaml
    /// </summary>
    public partial class User_window : Window 
    {
        private int idOrder = 0;

        public int IdOrder { get => idOrder; set => idOrder = value; }

        public User_window(string Name, int id)
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            string name = Name;
            Name_user.Text += name;
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

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            DeleteEmptyOrder();
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
            string query_id = "SELECT name_product, price, country, manufacturer FROM product WHERE id_category = @id";
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
            if (drv == null)
            {
                MessageBox.Show("Выберите товар");
            }
            else
            {
                string name = (string)drv["name_product"];
                int g = AddToOrder(name);
                string queryIdOrder = "SELECT * FROM [order] WHERE id_user = @id_user and status = 'открыт' ";
                SqlConnection con = new SqlConnection(Settings1.Default.connectionString);
                con.Open();
                SqlCommand com2 = new SqlCommand(queryIdOrder, con);
                com2.Parameters.AddWithValue("@id_user", GetId(Name_user.Text));
                int id = Convert.ToInt32(com2.ExecuteScalar());
                order_product(id, g);
            }
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

        private static int GetId(string name)
        {
            int id = 0;
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
;
            if (sqlCon.State == ConnectionState.Closed)
                try
                {
                    string query = "SELECT id FROM users WHERE login = @login";
                    SqlCommand MySqlCommand = new SqlCommand(query, sqlCon);
                    sqlCon.Open();
                    MySqlCommand.Parameters.AddWithValue("@login", name);
                    id = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return 0;
                }
            return id;
        }
        private  void CreateOrder()
        {
           
            int cost = 0;
            try
            {
                int idOrder = 0;

                SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                sqlCon.Open();
                string queryToCreateOrder = "INSERT INTO [order] (id_user, date, cost_order, status) VALUES (@id, @date, @cost, 'открыт'); SELECT SCOPE_IDENTITY() AS NewID";
                SqlCommand com = new SqlCommand(queryToCreateOrder, sqlCon);
                com.Parameters.AddWithValue("@id", GetId(Name_user.Text));
                com.Parameters.AddWithValue("@date", DateTime.Now);
                com.Parameters.AddWithValue("@cost", cost);
                IdOrder = (int)(decimal)com.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            
            }
        }



       /* private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            DeleteEmptyOrder();
        }*/

       

        private void DeleteEmptyOrder()
        {
            String id = Name_user.Text;
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            string queryToorderDelete = "select * from order_product where id_order = @lastId";
            //string queryToorderDelete = "delete  [order] from (select id from users where login = @id) as selected";
            sqlCon.Open();
            SqlCommand com = new SqlCommand(queryToorderDelete, sqlCon);
            com.Parameters.AddWithValue("@lastId", IdOrder);
            int TempId = Convert.ToInt32(com.ExecuteScalar());
            if (TempId ==0)
            {
                SqlConnection sqlCon2 = new SqlConnection(Settings1.Default.connectionString);
                string queryToorderDeleteEmpty = "delete from [order] where id_order = @lastId";
                //string queryToorderDelete = "delete  [order] from (select id from users where login = @id) as selected";
                sqlCon2.Open();
                SqlCommand comm = new SqlCommand(queryToorderDeleteEmpty, sqlCon);
                comm.Parameters.AddWithValue("@lastId", IdOrder);
                comm.ExecuteNonQuery();
            }
        }

        private void GoToOrder(object sender, RoutedEventArgs e)
        {
            string queryIdOrder = "SELECT id FROM  users WHERE id = @id_user";


            SqlConnection con = new SqlConnection(Settings1.Default.connectionString);
            con.Open();
            SqlCommand com2 = new SqlCommand(queryIdOrder, con);

            com2.Parameters.AddWithValue("@id_user", GetId(Name_user.Text));
            int id = Convert.ToInt32(com2.ExecuteScalar());
            //User_window userWindow = new User_window();
            //userWindow.Close();
            OrderWindow newOrderWindow = new OrderWindow(id);
            newOrderWindow.Show();
           // this.Close();
        }

        private void btn_ShowCharack(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)ProductGrid.SelectedItem;
            if (drv == null)
            {
                MessageBox.Show("Выберите товар");
            }
            else
            {
                string name = (string)drv["name_product"];
                ShowCharacteristic newWindow = new ShowCharacteristic(name);
                newWindow.Show();
            }
           // DataRowView drv = (DataRowView)ProductGrid.SelectedItem;
       
        }

        private void Category_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
    
}
