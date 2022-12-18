//  2006-2008 (c) Viva64.com Team
//  2008-2020 (c) OOO "Program Verification Systems"
//  2020-2022 (c) PVS-Studio LLC

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для User_window.xaml
    /// </summary>
    public partial class Admin_window : System.Windows.Window
    {
        //private SqlDataAdapter _adapter = null;

        //private SqlCommandBuilder builder;
        // private DataSet DealTable;
        public Admin_window()
        {

            InitializeComponent();
        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            UsersGrid.ItemsSource = null;
            sqlCon.Close();
        }
        private void btnShow_Click(object sender, RoutedEventArgs e)
        {

            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    UsersGrid.ItemsSource = null;

                    String query = "select id, login, password, roles_user.value,name, surname, email from users join roles_user ON users.roles = roles_user.id_roles";
                    //select id, login, password, name, surname, email from users join roles_user ON users.roles = roles_user.value
                    SqlCommand createCommand = new SqlCommand(query, sqlCon);

                    SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                    System.Data.DataTable dt = new System.Data.DataTable("Users");
                    dataAdp.Fill(dt);
                    UsersGrid.ItemsSource = dt.DefaultView;
                    sqlCon.Close();

                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen LoginScreen = new LoginScreen();
            LoginScreen.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            String query = "select id, login, password, roles_user.value,name, surname, email from users join roles_user ON users.roles = roles_user.id_roles";
            SqlCommand createCommand = new SqlCommand(query, sqlCon);

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("Users");
            dataAdp.Fill(dt);
            UsersGrid.ItemsSource = dt.DefaultView;
        }



        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Add_user LoginScreen = new Add_user();
            LoginScreen.Show();

        }
        private async void DeleteRow(int id)
        {

            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            if (sqlCon.State == ConnectionState.Closed)
                try
                {
                    string query = "DELETE FROM users WHERE (id = @id)";
                    SqlCommand MySqlCommand = new SqlCommand(query, sqlCon);
                    //параметризованный запрос

                    sqlCon.Open();
                    //создаём команду

                    //создаем параметр и добавляем его в коллекцию
                    MySqlCommand.Parameters.AddWithValue("@id", id);
                    //выполняем sql запрос
                    MySqlCommand.ExecuteNonQuery();
                    string path = @"C:\Users\Uset\Documents\\GitHub\ComputerShop\ComputerShop\Logger.txt";
                    DateTime dateTime = new DateTime();
                    using (StreamWriter writer = new StreamWriter(path, true))

                    {
                        await writer.WriteLineAsync("Пользователь id:" + id + "был удален " + dateTime);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить пользователя", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int id2 = 0;

                DataRowView drv = (DataRowView)UsersGrid.SelectedItem;
                if (drv == null)
                {
                    MessageBox.Show("Выберите пользователя");

                }
                else 
                {
                    id2 = (int)drv["id"];
                    DeleteRow(id2);
                }
                
                
                
            }
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}

