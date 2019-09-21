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
using System.Data;

using System.IO;
using Microsoft.Win32;

namespace DateTodayProject
{
    /// <summary>
    /// Interaction logic for admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        public userInformation editing { get; set; }
        public admin()
        {
            InitializeComponent();
            string sqlcmd1 = "select * from userInformation";
            string sqlcmd2 = "select * from loginInformation";

            //填满两个表！！！！！！！(￢︿̫̿￢☆)
            fillDataA(sqlcmd1);
            fillDataB(sqlcmd2);
        }

        private void fillDataA(string cmd)
        {
            //DataTable table = new DataTable();
            string sql = "server=localhost;database=userData;Integrated Security=True";
            SqlConnection sqlcon = new SqlConnection(sql);
            string sql1 = cmd;
            SqlDataAdapter sqlada = new SqlDataAdapter(sql1, sqlcon);
            DataSet ds = new DataSet();
            ds.Clear();
            DataTable table1 = new DataTable();
            sqlada.Fill(ds, "userInformation");
            userDataGrid.DataContext = ds;
            sqlcon.Close();
        }

        public void fillDataB(string cmd)
        {
            //DataTable table = new DataTable();
            string sql = "server=localhost;database=userData;Integrated Security=True";
            SqlConnection sqlcon = new SqlConnection(sql);
            string sql1 = cmd;
            SqlDataAdapter sqlada = new SqlDataAdapter(sql1, sqlcon);
            DataSet ds = new DataSet();
            ds.Clear();
            DataTable table1 = new DataTable();
            sqlada.Fill(ds, "loginInformation");
            logInDataGrid.DataContext = ds;
            // sqlcon.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void UserDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //     editing = e.Row.Item as userInformation;
            //      updateInfo(editing,"update userInformation where ID=@index");
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder();

        }


        //更新个人信息
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("server=localhost;database=userData;Integrated Security=True");
                con.Open();
                // 
                SqlCommand cmd2 = new SqlCommand("UPDATE userInformation SET birth = @br,name=@name,gender=@gender,age=@age,nativePlace=@np,livingPlace=@lp,relationStatus=@rs,occupation=@ocp,height=@ht,weight=@wt,selfIntro=@si,phone=@ph,salary=@sa where ID=@index;", con);
                //cmd2.Parameters.Add("@Image", SqlDbType.Image);
                cmd2.Parameters.AddWithValue("@index", tempIndex);
                cmd2.Parameters.AddWithValue("@name", nameTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@gender", genderTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@age", Convert.ToInt16(ageTextbox.Text));
                cmd2.Parameters.AddWithValue("@np", birthPlaceTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@lp", livingPlaceTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@rs", relationStaTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@ocp", occpTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@br", datepick.SelectedDate);
                cmd2.Parameters.AddWithValue("@ht", Convert.ToDecimal(heightTextbox.Text));
                cmd2.Parameters.AddWithValue("@wt", Convert.ToDecimal(weightTextbox.Text));
                cmd2.Parameters.AddWithValue("@sa", Convert.ToInt32(salaryTextbox.Text));
                cmd2.Parameters.AddWithValue("@ph", phoneTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@si", selfIntroTextbox.Text.ToString());
                cmd2.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("资料更新成功");
                string sqlcmd1 = "select * from userInformation";
                fillDataA(sqlcmd1);

            }
            catch
            {
                MessageBox.Show("出错啦嘤嘤嘤！", "错误");
            }

        }
        //FS必须定义为类里的变量
        FileStream fs;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage tempImage;
            OpenFileDialog openfiledialog = new OpenFileDialog { Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif|所有文件|*.*" };
            if ((bool)openfiledialog.ShowDialog())
            {
                tempImage = new BitmapImage(new Uri(openfiledialog.FileName));
                userPhotoImage.Source = tempImage;
                fs = new FileStream(openfiledialog.FileName.ToString(), FileMode.Open, FileAccess.Read);
            }
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
                cmd.Parameters.AddWithValue("@index", tempIndex);
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


        //显示所选类的详细信息
        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlCon = "server=localhost;database=userData;Integrated Security=True";
                SqlConnection con = new SqlConnection(sqlCon);
                SqlDataReader myreader;
                SqlCommand cmd1 = new SqlCommand("select * from userInformation where ID=@index", con);
                cmd1.Parameters.AddWithValue("@index", tempIndex);
                con.Open();
                myreader = cmd1.ExecuteReader();
                myreader.Read();
                userPhotoImage.Source = pages.mePage.BitmapImageConvert((byte[])myreader["photo"]);


                //在TextBox显示原有信息
                nameTextbox.Text = myreader["name"].ToString().Trim();
                ageTextbox.Text = myreader["age"].ToString().Trim();
                genderTextbox.Text = myreader["gender"].ToString().Trim();
                livingPlaceTextbox.Text = myreader["livingPlace"].ToString().Trim();
                birthPlaceTextbox.Text = myreader["nativePlace"].ToString().Trim();
                relationStaTextbox.Text = myreader["relationStatus"].ToString().Trim();
                occpTextbox.Text = myreader["occupation"].ToString().Trim();

                //用datetime
                datepick.SelectedDate = Convert.ToDateTime(myreader["birth"]);
                heightTextbox.Text = myreader["height"].ToString().Trim();
                weightTextbox.Text = myreader["weight"].ToString().Trim();
                phoneTextbox.Text = myreader["phone"].ToString().Trim();
                salaryTextbox.Text = myreader["salary"].ToString().Trim();
                selfIntroTextbox.Text = myreader["selfIntro"].ToString().Trim();


            }
            catch
            {
                MessageBox.Show("您还没有选择任何列哦");
            }
        }



        /// <summary>
        /// 用户登录信息维护！
        /// </summary>

        //显示信息
        private void ShowButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlCon = "server=localhost;database=userData;Integrated Security=True";
                SqlConnection con = new SqlConnection(sqlCon);
                SqlDataReader myreader;
                SqlCommand cmd1 = new SqlCommand("select * from loginInformation where ID=@index", con);
                cmd1.Parameters.AddWithValue("@index", tempIndex2);
                con.Open();
                myreader = cmd1.ExecuteReader();
                myreader.Read();
                usernameTextbox.Text = myreader["name"].ToString().Trim();
                passwordTextbox.Text = myreader["password"].ToString().Trim();
                adminTextbox.Text = myreader["admin"].ToString().Trim();
                Link.Text = myreader["link"].ToString().Trim();
                // ID.Text = myreader["ID"].ToString().Trim();
            }
            catch
            {
                MessageBox.Show("您还没有选择任何列哦");
            }
        }
        //更新
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection("server=localhost;database=userData;Integrated Security=True");
                con.Open();
                // 
                SqlCommand cmd2 = new SqlCommand("UPDATE loginInformation SET name=@name,password=@ps,admin=@ad,link=@link where ID=@id1", con);
                //cmd2.Parameters.Add("@Image", SqlDbType.Image);

                cmd2.Parameters.AddWithValue("@id1", tempIndex2);
                cmd2.Parameters.AddWithValue("@link", Link.Text.Trim());
                cmd2.Parameters.AddWithValue("@name", usernameTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@ps", passwordTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@ad", Convert.ToInt32(adminTextbox.Text));
                cmd2.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("资料更新成功");

                string sqlcmd2 = "select * from loginInformation";


                fillDataB(sqlcmd2);

            }
            catch
            {
                MessageBox.Show("出错啦嘤嘤嘤！", "错误");
            }
        }







        // 增加用户
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // try
            {
                SqlConnection con = new SqlConnection("server=localhost;database=userData;Integrated Security=True");
                con.Open();
                // 
                SqlCommand cmd2 = new SqlCommand("INSERT INTO userInformation(birth,name,gender,age,nativePlace,livingPlace,relationStatus,occupation,height,weight,selfIntro,phone,salary) VALUES(@br,@name,@gender,@age,@np,@lp,@rs,@ocp,@ht,@wt,@si,@ph,@sa)", con);
                //cmd2.Parameters.Add("@Image", SqlDbType.Image);
                cmd2.Parameters.AddWithValue("@index", tempIndex);
                cmd2.Parameters.AddWithValue("@name", nameTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@gender", genderTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@age", Convert.ToInt16(ageTextbox.Text));
                cmd2.Parameters.AddWithValue("@np", birthPlaceTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@lp", livingPlaceTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@rs", relationStaTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@ocp", occpTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@br", datepick.SelectedDate);
                cmd2.Parameters.AddWithValue("@ht", Convert.ToDecimal(heightTextbox.Text));
                cmd2.Parameters.AddWithValue("@wt", Convert.ToDecimal(weightTextbox.Text));
                cmd2.Parameters.AddWithValue("@sa", Convert.ToInt32(salaryTextbox.Text));
                cmd2.Parameters.AddWithValue("@ph", phoneTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@si", selfIntroTextbox.Text.ToString());
                cmd2.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("添加用户成功！");

            }
            //catch
            {
                MessageBox.Show("请输入完整信息哟", "错误");
            }

        }


        //增加账号登录信息
        private void AddButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection("server=localhost;database=userData;Integrated Security=True");
                con.Open();
                SqlCommand cmd2 = new SqlCommand("INSERT INTO loginInformation(name,password,admin) VALUES(@name,@ps,@ad)", con);
                cmd2.Parameters.AddWithValue("@name", usernameTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@ps", passwordTextbox.Text.ToString());
                cmd2.Parameters.AddWithValue("@ad", Convert.ToInt32(adminTextbox.Text));
                cmd2.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("用户创建成功");
                //刷新
                string sqlcmd2 = "select * from loginInformation";
                fillDataB(sqlcmd2);

            }
            catch
            {
                MessageBox.Show("出错啦嘤嘤嘤！", "错误");
            }
        }

        //



        //删除用户信息表
        private void AddButton_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection("server=localhost;database=userData;Integrated Security=True");
                con.Open();
                SqlCommand cmd2 = new SqlCommand("DELETE FROM userInformation WHERE ID=@index", con);
                cmd2.Parameters.AddWithValue("@index", tempIndex);
                cmd2.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("用户删除成功");
                //刷新
                string sqlcmd1 = "select * from userInformation";
                fillDataA(sqlcmd1);

            }
            catch
            {
                MessageBox.Show("出错啦嘤嘤嘤！", "错误");
            }
        }

        private void AddButton_Copy2_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection("server=localhost;database=userData;Integrated Security=True");
                con.Open();
                SqlCommand cmd2 = new SqlCommand("DELETE FROM loginInformation WHERE ID=@index", con);
                cmd2.Parameters.AddWithValue("@index", tempIndex2);
                cmd2.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("用户删除成功");
                //刷新
                string sqlcmd2 = "select * from loginInformation";
                fillDataB(sqlcmd2);

            }
            catch
            {
                MessageBox.Show("出错啦嘤嘤嘤！", "错误");
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }



        //因selectedIndex属性会因为按钮事件重置
        //设置全局属性，用CurrentCellChanged事件触发
        int tempIndex { get; set; }


        private void UserDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //tempIndex = userDataGrid.SelectedIndex + 1;
            try
            {
                var a = this.userDataGrid.SelectedItem;
                var b = a as DataRowView;
                tempIndex = Convert.ToInt32(b.Row[0]);
            }
            catch {; }
        }

        int tempIndex2 { get; set; }
        private void LogInDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var a = this.logInDataGrid.SelectedItem;
                var b = a as DataRowView;
                tempIndex2 = Convert.ToInt32(b.Row[3]);
            }
            catch {; }
        }
    }
}

