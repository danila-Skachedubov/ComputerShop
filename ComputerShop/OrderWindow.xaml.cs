using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
    public partial class OrderWindow 
    {
        private int id_user;

        public int Id_user { get => id_user; set => id_user = value; }

        public OrderWindow(int id)
        {
            InitializeComponent();
            Id_user = id;
            
        }

        private int Get_IdOrder(int idUser)
        {
            string queryIdOrder = "SELECT id_order FROM [order] WHERE id_user = @id_user and status = 'открыт'";


            SqlConnection con = new SqlConnection(Settings1.Default.connectionString);
            con.Open();
            SqlCommand com2 = new SqlCommand(queryIdOrder, con);

            com2.Parameters.AddWithValue("@id_user", idUser);
            int idOrder = Convert.ToInt32(com2.ExecuteScalar());
            return idOrder;
        }
        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            string queryToOrder_product = "select op.TempID, p.name_product, p.price, p.country, p.manufacturer, o.status from order_product as op\r\nleft join product as p\r\non p.id_product = op.id_product\r\nleft join [order] as o\r\non o.id_order = op.id_order\r\nwhere o.id_user = @id and o.status = 'открыт'";
            //select op.TempID, p.name_product, p.price, p.country, p.manufacturer from order_product as on left join product as on p.id_product = op.id_product\r\nleft join [order] as o\r\non o.id_order = op.id_order\r\nwhere o.id_user = @id
            SqlCommand createCommand = new SqlCommand(queryToOrder_product, sqlCon);
            createCommand.Parameters.AddWithValue("@id", Id_user);
            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("product");
            dataAdp.Fill(dt);
            //UsersGrid.ItemsSource = dt.DefaultView;
            ProductGrid.ItemsSource = dt.DefaultView;



            int sum = 0;
            SqlConnection sqlConn = new SqlConnection(Settings1.Default.connectionString);
            sqlConn.Open();
            string queryGetSum = "select SUM(price) from order_product as op left join product as p on p.id_product = op.id_product left join [order] as o on o.id_order = op.id_order where o.id_user = @id";
            //select op.TempID, p.name_product, p.price, p.country, p.manufacturer from order_product as on left join product as on p.id_product = op.id_product\r\nleft join [order] as o\r\non o.id_order = op.id_order\r\nwhere o.id_user = @id
            SqlCommand createCommandd = new SqlCommand(queryGetSum, sqlConn);
            createCommandd.Parameters.AddWithValue("@id", Id_user);
            try
            {
                sum = Convert.ToInt32(createCommandd.ExecuteScalar());
                Summa.Text = sum.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Корзина пустая");
            }
          

        }

        public void GetStatus(int idOrder)
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            string queryToOrder_product = "";
            
            SqlCommand createCommand = new SqlCommand(queryToOrder_product, sqlCon);
            createCommand.Parameters.AddWithValue("@id", Id_user);
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
            string queryToOrder_product = "select op.TempID, p.name_product, p.price, p.country, p.manufacturer, o.status from order_product as op\r\nleft join product as p\r\non p.id_product = op.id_product\r\nleft join [order] as o\r\non o.id_order = op.id_order\r\nwhere o.id_user = @id";
            SqlCommand createCommand = new SqlCommand(queryToOrder_product, sqlCon);
            createCommand.Parameters.AddWithValue("@id", Id_user);
            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("product");
            dataAdp.Fill(dt);
            //UsersGrid.ItemsSource = dt.DefaultView;
            ProductGrid.ItemsSource = dt.DefaultView;
            
        }

        private void btnClic_Buy(object sender, RoutedEventArgs e)
        {

            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            string queryToOrder_product = "update [order] set status = 'closed' where id_order = @idOrder";
            SqlCommand createCommand = new SqlCommand(queryToOrder_product, sqlCon);
            createCommand.Parameters.AddWithValue("@idOrder", Get_IdOrder(Id_user));
            createCommand.ExecuteNonQuery();
        }

        private void btnClick_Bought(object sender, RoutedEventArgs e)
        {
            Bought newBought = new Bought(Id_user);
            newBought.Show();
        }

        public static string GetLogin(int Id)
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
           
            try
            {
                    sqlCon.Open();
                    String query = "SELECT login FROM users  WHERE id = @Id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

                    sqlCmd.Parameters.AddWithValue("@Id", Id);
                    string login =sqlCmd.ExecuteScalar().ToString();
                    return login;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "0";              
            }
            finally
            {
                sqlCon.Close();
                
            }
        }

        private void btn_Back(object sender, RoutedEventArgs e)
        {
            
            new User_window(GetLogin(Id_user), Id_user).Show();
            this.Close();
        }

        /* private void DeleteOrder()
         {
             SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
             if (sqlCon.State == ConnectionState.Closed)
                 try
                 {
                     string query = "delete from [order] where id_order = @idOrder";
                     SqlCommand MySqlCommand = new SqlCommand(query, sqlCon);

                     sqlCon.Open();
                     MySqlCommand.Parameters.AddWithValue("@idOrder", Get_IdOrder(Id_user));
                     MySqlCommand.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }
         }*/


        /*  private void SQLToCSV(string query, string Filename)
          {
              SqlConnection connection = new SqlConnection(Settings1.Default.connectionString);
              connection.Open();


              SqlCommand cmd = new SqlCommand(query, connection);
              cmd.Parameters.AddWithValue("@id", Id_user);
              SqlDataReader dr = cmd.ExecuteReader();
              string path = @"C:\Users\Uset\Documents\GitHub\ComputerShop\ComputerShop\ordersList";
              using (System.IO.StreamWriter fs = new System.IO.StreamWriter(Filename))
              {

                  for (int i = 0; i < dr.FieldCount; i++)
                  {
                      string name = dr.GetName(i);
                      if (name.Contains(","))
                          name = "\"" + name + "\"";

                      fs.Write(name + ",");
                  }
                  fs.WriteLine();


                  while (dr.Read())
                  {
                      for (int i = 0; i < dr.FieldCount; i++)
                      {
                          string value = dr[i].ToString();
                          if (value.Contains(","))
                              value = "\"" + value + "\"";

                          fs.Write(value + ",");
                      }
                      fs.WriteLine();
                  }

                  fs.Close();
              }
          }*/

    }
}
