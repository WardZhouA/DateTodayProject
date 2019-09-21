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
//using System.Drawing;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;

namespace DateTodayProject.pages
{
    /// <summary>
    /// 用户个人资料页面
    /// </summary>
    public partial class mePage : Page
    {
        public mePage()
        {

            InitializeComponent();
            // try
            {
                string sqlCon = "server=localhost;database=userData;Integrated Security=True";
                SqlConnection con = new SqlConnection(sqlCon);
                SqlDataReader myreader;
                SqlCommand cmd1 = new SqlCommand("select * from userInformation where ID=@index", con);
                cmd1.Parameters.AddWithValue("@index", Application.Current.Properties["index"].ToString());
                string debug = Application.Current.Properties["index"].ToString();
                con.Open();
                myreader = cmd1.ExecuteReader();
                myreader.Read();
               
                    uesrPhotoImage.Source = BitmapImageConvert((byte[])myreader["photo"]);
                
                



                nameLable.Content = myreader["name"].ToString();
                ageLable.Content = myreader["age"].ToString() + "岁";
                genderLable.Content = myreader["gender"].ToString();
                livingPlaceLable.Content = "现居地 " + myreader["livingPlace"].ToString();
                birthPlaceLable.Content = "籍贯 " + myreader["nativePlace"].ToString();
                relationStaLable.Content = myreader["relationStatus"].ToString();
                occpLable.Content = myreader["occupation"].ToString();
                birthLable.Content = "出生日期 " + Convert.ToDateTime(myreader["birth"]).ToString("D");
                heightLable.Content = "身高 "+myreader["height"].ToString() + "cm";
                weightLable.Content = "体重 " + myreader["weight"].ToString() + "kg";
                phoneLable.Content = "联系方式 " + myreader["phone"].ToString();
                salaryLable.Content = "年薪 " + myreader["salary"].ToString();
                selfIntroLable.Content = myreader["selfIntro"].ToString();
            }
            //  catch
            {
                ;
            }

        }

        //定义byte数组转BitmapImage方法
        public static BitmapImage BitmapImageConvert(byte[] value)
        {
            if (value != null && value is byte[])
            {
                byte[] bytes = value as byte[];

                MemoryStream stream = new MemoryStream(bytes);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }

            return null;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
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
                pageFrame.Source = new Uri("pages/userProfit.xaml", UriKind.Relative);
            }
        }

        private void EditButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            changePassword window = new changePassword();
            window.Show();
        }
    }
}
