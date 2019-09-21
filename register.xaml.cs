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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace DateTodayProject
{
    /// <summary>
    /// Interaction logic for register.xaml
    /// </summary>
    public partial class register : Window
    {
        public register()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            MainWindow window1 = new MainWindow();
            window1.Show();
            this.Close();
        }



        //注册账号
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //数据库操作
            string sqlCon = "server=localhost;database=userData;Integrated Security=True";
            SqlConnection con = new SqlConnection(sqlCon);
            con.Open();
            



            /*
            SqlCommand cmd2 = new SqlCommand("select count(1) from logInInformation", con);
            SqlDataReader myreader;
            myreader = cmd2.ExecuteReader();
            myreader.Read();
            int index = 1 + Convert.ToInt32(myreader[0]);
            Application.Current.Properties["index"] = index.ToString();
            myreader.Close(); */



            //插入新键值
            SqlCommand cmd2 = new SqlCommand("INSERT INTO userInformation(birth,name,gender,age,nativePlace,livingPlace,relationStatus,occupation,height,weight,selfIntro,phone,salary,photo) VALUES(@br,@name,@gender,@age,@np,@lp,@rs,@ocp,@ht,@wt,@si,@ph,@sa,@image)", con);
            FileStream fs = new FileStream(@"C:\Users\wardz\source\repos\DateTodayProject\image\null.png", FileMode.Open, FileAccess.Read);
            byte[] Data = new byte[fs.Length];
            fs.Read(Data, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            // con.Open();

            cmd2.Parameters.Add("@Image", SqlDbType.Image);
          //  cmd2.Parameters.AddWithValue("@index", Application.Current.Properties["index"]);
            cmd2.Parameters.AddWithValue("@name", "请更新个人信息");
            cmd2.Parameters.AddWithValue("@gender", "nu");
            cmd2.Parameters.AddWithValue("@age", 0);
            cmd2.Parameters.AddWithValue("@np", "null");
            cmd2.Parameters.AddWithValue("@lp", "null");
            cmd2.Parameters.AddWithValue("@rs", "null");
            cmd2.Parameters.AddWithValue("@ocp", "null");
            cmd2.Parameters.AddWithValue("@br", DateTime.Today);
            cmd2.Parameters.AddWithValue("@ht", 0);
            cmd2.Parameters.AddWithValue("@wt", 0);
            cmd2.Parameters.AddWithValue("@sa", 0);
            cmd2.Parameters.AddWithValue("@ph", "null");
            cmd2.Parameters.AddWithValue("@si", "null");
            cmd2.Parameters["@Image"].Value = Data;
            cmd2.ExecuteNonQuery();








            SqlCommand cmd1 = new SqlCommand("SELECT * FROM userInformation ORDER BY ID DESC", con);
            SqlDataReader myreader;
            myreader= cmd1.ExecuteReader();
            myreader.Read();
            var tempIndex = myreader["ID"];
            myreader.Close();
            //插入登录信息表
            SqlCommand cmd3 = new SqlCommand("INSERT INTO loginInformation(name,password,admin,link) VALUES(@name,@password,'0',@index) ", con);
            cmd3.Parameters.AddWithValue("@index", tempIndex);
            cmd3.Parameters.AddWithValue("@name", textboxUserName.Text.Trim());
            cmd3.Parameters.AddWithValue("@password", textboxPassword.Password.Trim());
            cmd3.ExecuteReader();
            con.Close();

            //跳转主窗口引导用户修改资料
            Application.Current.Properties["index"] = tempIndex;
            userWindow window = new userWindow();
            window.Show();
            this.Close();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
