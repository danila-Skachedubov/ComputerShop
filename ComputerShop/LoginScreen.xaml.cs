using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        //Data Source=DESKTOP-P5D357J\SQLEXPRESS;Initial Catalog=comp_magazin;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                    String query = "SELECT * FROM users  WHERE Login=@Username AND Password =@password";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@username", Username.Text);
                    sqlCmd.Parameters.AddWithValue("@Password", hashPassword((Password.Password)));

                    int count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    
                    if (count > 0)
                    {

                        user_role();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин ии пароль!");
                    }
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

        internal int user_id()
        {
            string Name = Username.Text;
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            string sql = "SELECT id FROM users WHERE Login = @un";
            sqlCon.Open();

            SqlParameter nameParam = new SqlParameter("@un", Name);

            SqlCommand command = new SqlCommand(sql, sqlCon);
            command.Parameters.Add(nameParam);

            int count = Convert.ToInt32(command.ExecuteScalar());
            return count;
        }

        private void user_role()
        {
            string Name = Username.Text;
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            string sql = "SELECT roles FROM users WHERE Login = @un";
            sqlCon.Open();

            SqlParameter nameParam = new SqlParameter("@un", Name);

            SqlCommand command = new SqlCommand(sql, sqlCon);
            command.Parameters.Add(nameParam);

            byte Form_Role = (byte)command.ExecuteScalar();

            switch (Form_Role)
            {
                default:
                    break;
                case 1:
                    Admin_window admin_window = new Admin_window();
                    admin_window.Show();
                    sqlCon.Close();
                    break;
                case 3:
                    User_window user_window = new User_window();
                    user_window.Show();
                    sqlCon.Close();
                    break;

            }
            sqlCon.Close();

        }
        internal static string hashPassword(string pass)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));
            return Convert.ToBase64String(hash);
        }


    }
}
