//  2006-2008 (c) Viva64.com Team
//  2008-2020 (c) OOO "Program Verification Systems"
//  2020-2022 (c) PVS-Studio LLC

using System;
using System.Data.SqlClient;
using System.Windows;

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
            String query_replay = "SELECT COUNT(*)  FROM  users WHERE  (login LIKE @login) OR (email LIKE @email)";

            SqlCommand com = new SqlCommand(query_replay, sqlCon);
            com.Parameters.AddWithValue("login", Login_text.Text);
            com.Parameters.AddWithValue("email", Email_text.Text);
            int smth = (int)(com.ExecuteScalar());
            if (smth == 0)
            {

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
                createCommand.Parameters.AddWithValue("password", LoginScreen.hashPassword(Password_text.Text));
                createCommand.Parameters.AddWithValue("roles", roles);
                createCommand.Parameters.AddWithValue("name", Name_text.Text);
                createCommand.Parameters.AddWithValue("surname", Surname_text.Text);
                createCommand.Parameters.AddWithValue("email", Email_text.Text);


                MessageBox.Show(createCommand.ExecuteNonQuery().ToString());
            }
            else
                MessageBox.Show("Пользователь с таким логином или почтой уже существует");
        }


    }
}
