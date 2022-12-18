using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using ComputerShop.Window_category;

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для Manager_window.xaml
    /// </summary>
    public partial class Manager_window : Window
    {
        public Manager_window()
        {
            InitializeComponent();
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            String query = "SELECT name_category FROM category";
            SqlCommand createCommand = new SqlCommand(query, sqlCon);

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("category");
            dataAdp.Fill(dt);
            CategoryProd.ItemsSource = dt.DefaultView;
            ShowProduct();
            ShowOrder();
            AddStatus();
        }

        private void btn_AddProd(object sender, RoutedEventArgs e)
        {
            string CurrentCategory = CategoryProd.Text;
            switch(CurrentCategory)
            {
                case "Процессор":
                    CPU_window cPU_Window = new CPU_window();
                    cPU_Window.Show();
                    ShowProduct();
                    break;                  
                case "Видеокарта":
                    GPU_Window gPU_Window = new GPU_Window();
                    gPU_Window.Show();
                    ShowProduct();
                    break;
                case "Корпус":
                    BOX_window boxPU_Window = new BOX_window(); 
                    boxPU_Window.Show();
                    ShowProduct();
                    break;
                case "Оперативная память":
                    Operative_memory_window operative_Memory_Window = new Operative_memory_window();
                    operative_Memory_Window.Show();
                    ShowProduct();
                    break;
                case "Жесткий диск":
                    Hard_drive_window hard_Drive_Window = new Hard_drive_window();      
                    hard_Drive_Window.Show();
                    ShowProduct();
                    break;
                case "Клавиатура":
                    Keyboard_window keyboard_Window = new Keyboard_window();    
                    keyboard_Window.Show();
                    ShowProduct();
                    break;
                case "Мышь":
                    Mouse_window mouse_Window = new Mouse_window();
                    mouse_Window.Show();
                    ShowProduct();
                    break;
                case "Монитор":
                    Monitor_window monitor_Window = new Monitor_window();   
                    monitor_Window.Show();
                    ShowProduct();
                    break;
                case "Блок питания":
                    Power_Pack_window power_Pack_Window = new Power_Pack_window();
                    power_Pack_Window.Show();
                    ShowProduct();
                    break;
                case "Материнская плата":
                    Motherboard_window motherboard_Window = new Motherboard_window();
                    motherboard_Window.Show();
                    ShowProduct();
                    break;
                default:
                    MessageBox.Show("Выберите категорию товара!");
                    break;

            }
        }
        public void ShowProduct()
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

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить этот товар?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DataRowView drv = (DataRowView)ProductGrid.SelectedItem;
                
                if (drv == null)
                {
                    MessageBox.Show("Выберите nовар!");
                }
                else
                {
                    string id2 = (string)drv["name_product"];
                    DeleteRow(id2);
                }
            }
        }
        private async void DeleteRow(string id)
        {

            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            if (sqlCon.State == ConnectionState.Closed)
                try
                {
                    string query = "DELETE FROM product WHERE (name_product = @id)";
                    SqlCommand MySqlCommand = new SqlCommand(query, sqlCon);

                    sqlCon.Open();

                    MySqlCommand.Parameters.AddWithValue("@id", id);

                    MySqlCommand.ExecuteNonQuery();
                    string path = @"C:\Users\Uset\Documents\\GitHub\ComputerShop\ComputerShop\Logger.txt";
                    DateTime dateTime = new DateTime();
                    using (StreamWriter writer = new StreamWriter(path, true))

                    {
                        await writer.WriteLineAsync("Продукт id " +id + "был удален " + dateTime);

                    }
                    ShowProduct();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

        }
        private void ShowOrder()
        {
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            String query = "select id_user, [order].id_order, product.name_product, users.email ,status from [order] join order_product ON [order].id_order = order_product.id_order join product on order_product.id_product = product.id_product join users on [order].id_user = users.id";
            SqlCommand createCommand = new SqlCommand(query, sqlCon);

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("order");
            dataAdp.Fill(dt);
            Order.ItemsSource = dt.DefaultView;
        }
        private void AddStatus() 
        {
            Status.Items.Add("Отправлен на сборку");
            Status.Items.Add("На складе");
            Status.Items.Add("В пути на пункт выдачи");
            Status.Items.Add("Готов к выдаче");
        }
        private void btnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите изменить статус заказа?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DataRowView drv = (DataRowView)Order.SelectedItem;

                if (drv == null)
                {
                    MessageBox.Show("Выберите заказ!");
                }
                else
                {
                    int id2 = (int)drv["id_order"];
                    ChangeStatus(id2);
                }
            }
        }

        private async void ChangeStatus(int id)
        {
            string status = Status.Text;
            if (status == "")
            {
                MessageBox.Show("Выберите статус!");
            }

            else 
            { 
                    SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
                if (sqlCon.State == ConnectionState.Closed)
                    try
                    {
                        string query = "update [order] set status = @status where id_order = @id";
                        SqlCommand MySqlCommand = new SqlCommand(query, sqlCon);

                        sqlCon.Open();

                        MySqlCommand.Parameters.AddWithValue("@id", id);
                        MySqlCommand.Parameters.AddWithValue("@status", status);

                        MySqlCommand.ExecuteNonQuery();
                        string path = @"C:\Users\Uset\Documents\\GitHub\ComputerShop\ComputerShop\Logger.txt";
                        DateTime dateTime = new DateTime();
                        using (StreamWriter writer = new StreamWriter(path, true))

                        {
                            await writer.WriteLineAsync("Заказ № " + id + "изменил статус на " + status + dateTime);

                        }
                        ShowOrder();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
            }
        }

        private void WinLoad(object sender, RoutedEventArgs e)
        {
            ShowProduct();
        }
    }
}
