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

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для Add_user.xaml
    /// </summary>
    public partial class Add_user : Window
    {
        public Add_user()
        {
            InitializeComponent();
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            String query = "SELECT value FROM roles_user";
            SqlCommand createCommand = new SqlCommand(query, sqlCon);

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("roles_user");
            dataAdp.Fill(dt);
            //UsersGrid.ItemsSource = dt.DefaultView;
            lIST_ROLE.ItemsSource = dt.DefaultView;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.Login = Login_text.Text;
            user.Password = Password_text.Text;          
            user.Name = Name_text.Text;
            user.Surname = Surname_text.Text;
            user.Email = Email_text.Text;
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            int roles = 0;
            string CurrentRole = lIST_ROLE.Text;
            String query = "INSERT INTO [users] (login, password, roles, name, surname, email) VALUES (@login, @password, @roles, @name, @surname, @email)";
            SqlCommand createCommand = new SqlCommand(query, sqlCon);
            switch (CurrentRole)
            {
                case "Администратор":
                    roles = 1;
                    break;
                case "Менеджер":
                    roles = 2;
                    break;
                case "Пользователь":
                    roles = 3;
                    break;
                default:
                    MessageBox.Show("Выберите роль!");
                    break;
            }
            createCommand.Parameters.AddWithValue("login", Login_text.Text);
            createCommand.Parameters.AddWithValue("password", Password_text.Text);
            createCommand.Parameters.AddWithValue("roles", roles);
            createCommand.Parameters.AddWithValue("name", Name_text.Text);
            createCommand.Parameters.AddWithValue("surname", Surname_text.Text);
            createCommand.Parameters.AddWithValue("email", Email_text.Text);
            

            MessageBox.Show(createCommand.ExecuteNonQuery().ToString());
        }
    }
}
