using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace DateTodayProject.pages
{
    /// <summary>
    /// Interaction logic for mainPage.xaml
    /// </summary>
    public partial class mainPage : Page
    {
        public mainPage()
        {
            InitializeComponent();
            //从数据库读取图片
            //新建list
            List<images> bindingList = new List<images>();
            string sqlCon = "server=localhost;database=userData;Integrated Security=True";
            string sqlquery = "select * from userInformation where ID not in (@index)";

            SqlConnection con = new SqlConnection(sqlCon);
            con.Open();
            SqlDataReader myreader;
            SqlCommand cmd1 = new SqlCommand(sqlquery, con);
            cmd1.Parameters.AddWithValue("@index", Application.Current.Properties["index"].ToString());
            
            myreader = cmd1.ExecuteReader();
            //myreader.Read();
            //uesrPhotoImage.Source = mePage.BitmapImageConvert((byte[])myreader["photo"]);



            while (myreader.Read())
            {
                images binding = new images();
                if (myreader["photo"]!= System.DBNull.Value)
                {
                    binding.photo = mePage.BitmapImageConvert((byte[])myreader["photo"]);
                }
                
                binding.name = myreader["name"].ToString().Trim();
                binding.age = myreader["age"].ToString().Trim();
                bindingList.Add(binding);
            }
            
            photoListBox.ItemsSource = bindingList;
            con.Close();
        }
    }
    public class images
    {
        public BitmapImage photo { set; get; }
        public string name { set; get; }
        public string age { set; get; }
    }
}
