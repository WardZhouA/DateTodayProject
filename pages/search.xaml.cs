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
using System.Data;


namespace DateTodayProject.pages
{
    /// <summary>
    /// Interaction logic for search.xaml
    /// </summary>
    public partial class search : Page
    {
        public search()
        {
            InitializeComponent();
        }

        public void GoSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchContext = searchTextbox.Text;
            string sqlCon = "server=localhost;database=userData;Integrated Security=True";
            SqlConnection con = new SqlConnection(sqlCon);
            con.Open();
            SqlDataReader myreader;
            SqlCommand cmd1 = new SqlCommand(sqlcommandStrComand(searchContext), con);
            myreader = cmd1.ExecuteReader();

            //循环读取信息 实例化并添加List
            List<searchItem> collection = new List<searchItem>();
            while (myreader.Read())
            {
                searchItem temp = new searchItem();
                temp.name = myreader["name"].ToString().Trim();
                if (myreader["photo"] != System.DBNull.Value)
                {
                    temp.photo = mePage.BitmapImageConvert((byte[])myreader["photo"]);
                }
                temp.livingPlace = myreader["livingPlace"].ToString().Trim();
                temp.age = myreader["age"].ToString().Trim();
                temp.phone = myreader["phone"].ToString().Trim();
                collection.Add(temp);
            }
            searchResultListbox.ItemsSource = collection;
            con.Close();
        }

        //拼接命令字符串
        private string sqlcommandStrComand(string context)
        {
            string result = "select * from userInformation ";
            string context1 = "'%" + context + "%'";
            result += ("where name like" + context1);
            result += ("or livingPlace like" + context1);
            result += ("or occupation like" + context1);
            result += ("or selfIntro like" + context1);
            result += ("or nativePlace like" + context1);
            result += ("or relationStatus like" + context1);
            result += ("or relationStatus like" + context1);
            result += ("or gender like" + context1);
            return result;
        }

        //查询
        public SqlDataReader GetData(string sqlStr)
        {
            string sqlCon = "server=localhost;database=userData;Integrated Security=True";
            SqlConnection con = new SqlConnection(sqlCon);
            SqlDataReader myreader;
            SqlCommand cmd1 = new SqlCommand("select * from userInformation where ID=@index", con);
            cmd1.Parameters.AddWithValue("@index", Application.Current.Properties["index"]);
            con.Open();
            myreader = cmd1.ExecuteReader();
            myreader.Read();
            //uesrPhotoImage.Source = mePage.BitmapImageConvert((byte[])myreader["photo"]);
            con.Close();
            return myreader;
        }
    }
    public class searchItem
    {
        public BitmapImage photo { get; set; }
        public string age { get; set; }
        public string livingPlace { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
    }
}
