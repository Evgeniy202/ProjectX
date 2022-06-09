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
using Tutorial.SqlConn;
using MySql.Data.MySqlClient;
using System.IO;

namespace ProjectX.Pages
{
    public partial class userInf : Page
    {
        public userInf()
        {
            InitializeComponent();
        }

        private void First_nameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (first_nameBox.Text == "First name...")
            {
                first_nameBox.Text = "";
            }
        }

        private void Last_nameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (last_nameBox.Text == "Last name (not necessarily)...")
            {
                last_nameBox.Text = "";
            }
        }

        private void First_nameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (first_nameBox.Text == "")
            {
                first_nameBox.Text = "First name...";
            }
        }

        private void Last_nameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (last_nameBox.Text == "")
            {
                last_nameBox.Text = "Last name (not necessarily)...";
            }
        }

        private void ConfBtn_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();

            string first_name = first_nameBox.Text;
            string last_name = last_nameBox.Text;

            if ((first_name != "") & (first_name != "First name..."))
            {
                StreamReader sr = new StreamReader("conf/data.txt");
                string line = sr.ReadLine();
                string[] data = line.Split('?');
                string id = data[0];

                if ((last_name != "") & (last_name != "Last name (not necessarily)..."))
                {               
                    string query = $"INSERT INTO userInformatin (id, first_name, last_name) " +
                        $"VALUES ('{id}', '{first_name}', '{last_name}')";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    string path = "conf/generalData.txt";
                    string datas = $"{id}?{first_name}?{last_name}";

                    FileStream fs = new FileStream(path, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine(datas);
                    sw.Close();
                }
                else
                {            
                    string query = $"INSERT INTO userInformatin (id, first_name) " +
                        $"VALUES ('{id}', '{first_name}')";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    string path = "conf/generalData.txt";
                    string datas = $"{id}?{first_name}";

                    FileStream fs = new FileStream(path, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine(datas);
                    sw.Close();
                }

                conn.Close();

                NavigationService.Navigate(new mainPage());
            }
            else
            {
                MessageBox.Show(
                    "Field 'First name' is empty!",
                    "Fields error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }   
    }
}
