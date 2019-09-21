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

namespace DateTodayProject
{
    /// <summary>
    /// Interaction logic for changePassword.xaml
    /// </summary>
    public partial class changePassword : Window
    {
        public changePassword()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //修改密码
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection("server=localhost;database=userData;Integrated Security=True");
            con.Open();
                        SqlCommand cmd1 = new SqlCommand("SELECT * FROM loginInformation where link=@id", con);
            cmd1.Parameters.AddWithValue("@id", Application.Current.Properties["index"]);
            SqlDataReader myreader;
            myreader = cmd1.ExecuteReader();
            myreader.Read();
            if (myreader["password"].ToString().Trim()==oldPasswordTextBox.Text.Trim())
            {
                myreader.Close();
                SqlCommand cmd2 = new SqlCommand("UPDATE loginInformation SET password=@ps where link=@id1", con);
                //cmd2.Parameters.Add("@Image", SqlDbType.Image);

                cmd2.Parameters.AddWithValue("@id1", Application.Current.Properties["index"]);


                cmd2.Parameters.AddWithValue("@ps", newPasswordTextBox.Text.ToString().Trim());

                cmd2.ExecuteNonQuery();
                
                con.Close();
                MessageBox.Show("修改成功");
                this.Close();
            }
            else
            {
                MessageBox.Show("密码错误");
            }
        }
    }
}
