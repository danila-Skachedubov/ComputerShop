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
    /// Логика взаимодействия для Bought.xaml
    /// </summary>
    public partial class Bought : Window
    {
        public Bought(int Id_user)
        {
            InitializeComponent();
            SqlConnection sqlCon = new SqlConnection(Settings1.Default.connectionString);
            sqlCon.Open();
            string queryToOrder_product = "select op.TempID, p.name_product, p.price, p.country, p.manufacturer, o.status from order_product as op\r\nleft join product as p\r\non p.id_product = op.id_product\r\nleft join [order] as o\r\non o.id_order = op.id_order\r\nwhere o.id_user = @id and o.status NOT LIKE 'открыт'";
            SqlCommand createCommand = new SqlCommand(queryToOrder_product, sqlCon);
            createCommand.Parameters.AddWithValue("@id", Id_user);
            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            System.Data.DataTable dt = new System.Data.DataTable("product");
            dataAdp.Fill(dt);

            ProductGrid.ItemsSource = dt.DefaultView;
        }

      
    }
}
