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

namespace DateTodayProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        //跨窗口传递用户信息 嘤嘤嘤



        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {

            string userName = null;
            string password = null;
            string index = null;
            string sqlCon = "server=localhost;database=userData;Integrated Security=True";
            SqlConnection con = new SqlConnection(sqlCon);
            SqlDataReader myreader;
            SqlCommand cmd1 = new SqlCommand("select * from logInInformation", con);
            con.Open();
            myreader = cmd1.ExecuteReader();

            //判断是否登陆成功bool
            bool i = false;
            int admin;
            while (myreader.Read())
            {
                // if (myreader.Read())

                {
                    userName = myreader["name"].ToString();
                    password = myreader["password"].ToString();
                    password = password.Trim();
                    userName = userName.Trim();
                    index = myreader["link"].ToString();
                    Application.Current.Properties["index"] = index.Trim();
                    admin = Convert.ToInt32(myreader["admin"]);
                }


                //验证密码
                if (userName == textboxUserName.Text && password == textboxPassword.Password)
                {
                    i = true;
                    //管理员验证
                    if (admin == 1)
                    {
                        
                        //messagebox选择进入用户界面或管理界面
                        if (MessageBox.Show("您是管理员用户，是否进入管理系统？", "系统消息", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            admin window = new admin();
                            window.Show();
                            this.Close();
                            break;
                        }
                        else
                        {
                            goto a;
                        }
                    }
                    MessageBoxResult result = MessageBox.Show("欢迎" + userName + "!", "登陆成功");


                //请不要吐槽我的标签跳转2333333
                a:
                    {
                        //实例主界面并打开
                        userWindow windows = new userWindow();
                        windows.Show();
                        this.Close();
                        break;
                    }
                }

            }
            myreader.Close();

            //验证登陆是否失败
            //感觉逻辑可以优化，但我脑子不够用了QAQ
            if (!i)
            {
                MessageBoxResult result = MessageBox.Show("用户名或密码错误！", "登陆失败");
                this.Show();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            register window = new register();
            //Application.Run(window);
            window.Show();
            this.Close();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
