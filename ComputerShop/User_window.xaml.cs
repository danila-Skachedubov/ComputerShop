﻿using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для User_window.xaml
    /// </summary>
    public partial class User_window : System.Windows.Window
    {
        private SqlDataAdapter _adapter = null;
       
        private SqlCommandBuilder builder;
        private DataSet DealTable;
        public User_window()
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
                     sqlCon.Open();
                     String query = "SELECT * FROM users";

                    int selectedColumn = UsersGrid.CurrentCell.Column.DisplayIndex;
                    var selectedCell = UsersGrid.SelectedCells[selectedColumn];
                    var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                    if (cellContent is TextBlock)
                    {
                        MessageBox.Show((cellContent as TextBlock).Text);
                    }

                    SqlCommand createCommand = new SqlCommand(query, sqlCon);
                     createCommand.ExecuteNonQuery();

                     SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                    System.Data.DataTable dt = new System.Data.DataTable("Students"); // В скобках указываем название таблицы
                     dataAdp.Fill(dt);
                     UsersGrid.ItemsSource = dt.DefaultView; // Сам вывод 
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
            String query = "SELECT * FROM users";
            SqlCommand createCommand = new SqlCommand(query, sqlCon);

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("Users");
            dataAdp.Fill(dt);
            UsersGrid.ItemsSource = dt.DefaultView;
        }

        private void btnShow_Copy1_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Add_user LoginScreen = new Add_user();
            LoginScreen.Show();
            
        }
        private void DeleteRow(int id)
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
               

                DataRowView drv = (DataRowView)UsersGrid.SelectedItem;
                int id2 = (int)drv["id"];

                DeleteRow(id2);
            }
        }
    }
}

