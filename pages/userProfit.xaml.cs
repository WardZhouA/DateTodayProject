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
using System.IO;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Data;

namespace DateTodayProject.pages
{
    /// <summary>
    /// 修改资料页面
    /// </summary>
    public partial class userProfit : Page
    {

        FileStream fs;
        BitmapImage tempImage;
        public userProfit()
        {
            InitializeComponent();
            try
            {
                string sqlCon = "server=localhost;database=userData;Integrated Security=True";
                SqlConnection con = new SqlConnection(sqlCon);
                SqlDataReader myreader;
                SqlCommand cmd1 = new SqlCommand("select * from userInformation where ID=@index", con);
                cmd1.Parameters.AddWithValue("@index", Application.Current.Properties["index"]);
                con.Open();
                myreader = cmd1.ExecuteReader();
                myreader.Read();
                Image1.Source = mePage.BitmapImageConvert((byte[])myreader["photo"]);

                // 比较懒直接copy上一页代码改字Lable实际为Textbox
                //在TextBox显示原有信息
                nameLable.Text = myreader["name"].ToString();
                ageLable.Text = myreader["age"].ToString();
                genderLable.Text = myreader["gender"].ToString();
                livingPlaceLable.Text = myreader["livingPlace"].ToString();
                birthPlaceLable.Text = myreader["nativePlace"].ToString();
                relationStaLable.Text = myreader["relationStatus"].ToString();
                occpLable.Text = myreader["occupation"].ToString();

                //用datetime
                datepick.SelectedDate = Convert.ToDateTime(myreader["birth"]);
                heightLable.Text = myreader["height"].ToString();
                weightLable.Text = myreader["weight"].ToString();
                phoneLable.Text = myreader["phone"].ToString();
                salaryLable.Text = myreader["salary"].ToString();
                selfIntroLable.Text = myreader["selfIntro"].ToString();
            }
            catch
            {; }
        }

        //打开图片 并保存
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog { Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif|所有文件|*.*" };
            if ((bool)openfiledialog.ShowDialog())
            {
                tempImage = new BitmapImage(new Uri(openfiledialog.FileName));
                Image1.Source = tempImage;
                fs = new FileStream(openfiledialog.FileName.ToString(), FileMode.Open, FileAccess.Read);
            }
            saveImage(fs);
        }

        //修改资料
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try { saveInfor(); }
            catch {; }


            //完成后跳转上一页
            Frame pageFrame = null;
            DependencyObject currParent = VisualTreeHelper.GetParent(this);
            while (currParent != null && pageFrame == null)
            {
                pageFrame = currParent as Frame;
                currParent = VisualTreeHelper.GetParent(currParent);
            }
            // Change the page of the frame.
            if (pageFrame != null)
            {
                pageFrame.Source = new Uri("pages/mePage.xaml", UriKind.Relative);
            }
        }
        //保存图片方法
        public void saveImage(FileStream ImagFs)
        {
            byte[] Data = new byte[fs.Length];
            fs.Read(Data, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            try
            {
                SqlConnection con = new SqlConnection("server=localhost;database=userData;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE userInformation SET photo=@Image where ID=@index;", con);
                cmd.Parameters.Add("@Image", SqlDbType.Image);
                //不会写不会写 
                cmd.Parameters.AddWithValue("@index", Application.Current.Properties["index"]);
                cmd.Parameters["@Image"].Value = Data;
                cmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("图片上传成功");

            }
            catch
            {
                MessageBox.Show("您选择的图片不能被读取或文件类型不对！", "错误");

            }

        }



        //报错
        //保存个人信息方法（管理页面想在用一次嘛~~）
        public void saveInfor()
        {
            try
            {
                SqlConnection con = new SqlConnection("server=localhost;database=userData;Integrated Security=True");
                con.Open();
                // 
                SqlCommand cmd2 = new SqlCommand("UPDATE userInformation SET birth = @br,name=@name,gender=@gender,age=@age,nativePlace=@np,livingPlace=@lp,relationStatus=@rs,occupation=@ocp,height=@ht,weight=@wt,selfIntro=@si,phone=@ph,salary=@sa where ID=@index;", con);
                //cmd2.Parameters.Add("@Image", SqlDbType.Image);
                cmd2.Parameters.AddWithValue("@index", Application.Current.Properties["index"]);
                cmd2.Parameters.AddWithValue("@name", nameLable.Text.ToString());
                cmd2.Parameters.AddWithValue("@gender", genderLable.Text.ToString());
                cmd2.Parameters.AddWithValue("@age", Convert.ToInt16(ageLable.Text));
                cmd2.Parameters.AddWithValue("@np", birthPlaceLable.Text.ToString());
                cmd2.Parameters.AddWithValue("@lp", livingPlaceLable.Text.ToString());
                cmd2.Parameters.AddWithValue("@rs", relationStaLable.Text.ToString());
                cmd2.Parameters.AddWithValue("@ocp", occpLable.Text.ToString());
                cmd2.Parameters.AddWithValue("@br", datepick.SelectedDate);
                cmd2.Parameters.AddWithValue("@ht", Convert.ToDecimal(heightLable.Text));
                cmd2.Parameters.AddWithValue("@wt", Convert.ToDecimal(weightLable.Text));
                cmd2.Parameters.AddWithValue("@sa", Convert.ToInt32(salaryLable.Text));
                cmd2.Parameters.AddWithValue("@ph", phoneLable.Text.ToString());
                cmd2.Parameters.AddWithValue("@si", selfIntroLable.Text.ToString());
                cmd2.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("资料更新成功");

            }
            catch
            {
                MessageBox.Show("别空着呀！快填满(。・∀・)ノ", "错误");
            }
        }


        //返回上一页哟
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame pageFrame = null;
            DependencyObject currParent = VisualTreeHelper.GetParent(this);
            while (currParent != null && pageFrame == null)
            {
                pageFrame = currParent as Frame;
                currParent = VisualTreeHelper.GetParent(currParent);
            }
            // Change the page of the frame.
            if (pageFrame != null)
            {
                pageFrame.Source = new Uri("pages/mePage.xaml", UriKind.Relative);
            }
        }
    }
}
