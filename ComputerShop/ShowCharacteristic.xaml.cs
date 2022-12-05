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
    /// Логика взаимодействия для ShowCharacteristic.xaml
    /// </summary>
    public partial class ShowCharacteristic : Window
    {
        public ShowCharacteristic(string name)
        {
            InitializeComponent();
            string queryIdOrder = "SELECT id_product FROM product WHERE name_product = @name";
            SqlConnection con = new SqlConnection(Settings1.Default.connectionString);
            con.Open();
            SqlCommand com2 = new SqlCommand(queryIdOrder, con);
            com2.Parameters.AddWithValue("@name", name);
            int id = Convert.ToInt32(com2.ExecuteScalar());
            ShowCharact(id);
        }
       public void ShowCharact(int id_product)
        {
            string queryIdOrder = "select characteristic.name_characteristic, characteristic_product.value from product\r\nJoin characteristic_product ON product.id_product=characteristic_product.id_product\r\njoin characteristic ON characteristic_product.id_characteristic = characteristic.id_characteristic\r\nwhere characteristic_product.id_product = @name";
            SqlConnection con = new SqlConnection(Settings1.Default.connectionString);
            con.Open();
            SqlCommand com2 = new SqlCommand(queryIdOrder, con);
            com2.Parameters.AddWithValue("@name", id_product);
            
            SqlDataAdapter dataAdp = new SqlDataAdapter(com2);
            System.Data.DataTable dt = new System.Data.DataTable("product");
            dataAdp.Fill(dt);
            Characteristic.ItemsSource = dt.DefaultView;
            
        }

    }
}
