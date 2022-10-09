using System;
using System.Collections.Generic;
using System.Data;
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
using System.Security.Cryptography;

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
                        MainWindow dashBoard = new MainWindow();
                        dashBoard.Show();
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

        private string hashPassword(string pass)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));

            return Convert.ToBase64String(hash);
        }
    }
}
