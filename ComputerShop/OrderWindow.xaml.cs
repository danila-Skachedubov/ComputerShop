using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private int id_order;

        public int Id_order { get => id_order; set => id_order = value; }

        public OrderWindow(int id)
        {
            InitializeComponent();
            Id_order = id;
            
        }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            string queryToOrder_product = "select op.TempID, p.name_product, p.price, p.country, p.manufacturer from order_product as op\r\nleft join product as p\r\non p.id_product = op.id_product\r\nleft join [order] as o\r\non o.id_order = op.id_order\r\nwhere o.id_user = @id";
            SqlCommand createCommand = new SqlCommand(queryToOrder_product, sqlCon);
            createCommand.Parameters.AddWithValue("@id", Id_order);
            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("product");
            dataAdp.Fill(dt);
            //UsersGrid.ItemsSource = dt.DefaultView;
            ProductGrid.ItemsSource = dt.DefaultView;
        }

        private void btnClick_Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить пользователя", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {


                DataRowView drv = (DataRowView)ProductGrid.SelectedItem;
                int id2 = (int)drv["TempID"];

                DeleteRow(id2);
            }
        }

        private void DeleteRow(int tempid)
        {

            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            if (sqlCon.State == ConnectionState.Closed)
                try
                {
                    string query = "DELETE FROM order_product WHERE (TempID = @tempid)";
                    SqlCommand MySqlCommand = new SqlCommand(query, sqlCon);
                    //параметризованный запрос

                    sqlCon.Open();
                    //создаём команду

                    //создаем параметр и добавляем его в коллекцию
                    MySqlCommand.Parameters.AddWithValue("@tempid", tempid);
                    //выполняем sql запрос
                    MySqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            ShowOrder();

        }

        private void ShowOrder()
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            string queryToOrder_product = "select op.TempID, p.name_product, p.price, p.country, p.manufacturer from order_product as op\r\nleft join product as p\r\non p.id_product = op.id_product\r\nleft join [order] as o\r\non o.id_order = op.id_order\r\nwhere o.id_user = @id";
            SqlCommand createCommand = new SqlCommand(queryToOrder_product, sqlCon);
            createCommand.Parameters.AddWithValue("@id", Id_order);
            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("product");
            dataAdp.Fill(dt);
            //UsersGrid.ItemsSource = dt.DefaultView;
            ProductGrid.ItemsSource = dt.DefaultView;
        }

    }
}
