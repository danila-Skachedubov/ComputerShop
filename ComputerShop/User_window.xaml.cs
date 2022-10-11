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


namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для User_window.xaml
    /// </summary>
    public partial class User_window : Window
    {
        private SqlDataAdapter _adapter = null;
        private DataTable _table = null;
        public User_window()
        {
            InitializeComponent();
        }


        private void btnShow_Click(object sender, RoutedEventArgs e)
        {

            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                    String query = "SELECT * FROM users";

                    SqlCommand createCommand = new SqlCommand(query, sqlCon);
                    createCommand.ExecuteNonQuery();

                    SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("Students"); // В скобках указываем название таблицы
                    dataAdp.Fill(dt);
                    StudentsGrid.ItemsSource = dt.DefaultView; // Сам вывод 
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
    }
    }

