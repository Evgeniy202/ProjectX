using MySql.Data.MySqlClient;
using Tutorial.SqlConn;
using System.Windows;
using System.IO;
using System;

namespace ProjectX
{
    public partial class logInWindow : Window
    {
        MySqlConnection conn;

        public logInWindow()
        {
            InitializeComponent();

            conn = DBUtils.GetDBConnection();
            conn.Open();
        }

        private void NewAccBtn_Click(object sender, RoutedEventArgs e)
        {
            RegWindow w = new RegWindow();
            w.Show();
            Close();
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            string le = loginBox.Text;
            string password = passwordBox.Password;

            string dbId = null;
            string dbPassword = null;
            MySqlDataReader datas;

            if ((le != "") & (password != ""))
            {
                string query = $"SELECT id, password FROM baseDataUsers WHERE login = '{le}' OR email = '{le}'";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                try
                {
                    datas = cmd.ExecuteReader();

                    while (datas.Read())
                    {
                        dbId = datas[0].ToString();
                        dbPassword = datas[1].ToString();
                    }
                }
                catch { datas = null; }

                if (password == dbPassword)
                {
                    if (saveCB.IsChecked == true)
                    {
                        string path = "conf/config.txt";
                        string data = $"{le}?{password}";

                        FileStream fs = new FileStream(path, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);

                        sw.WriteLine(data);
                        sw.Close();

                        path = "conf/data.txt";
                        data = $"{dbId}?{le}?{password}";

                        fs = new FileStream(path, FileMode.Create);
                        sw = new StreamWriter(fs);

                        sw.WriteLine(data);
                        sw.Close();
                    }
                    else
                    {
                        string path = "conf/config.txt";

                        FileStream fs = new FileStream(path, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);

                        sw.WriteLine("");
                        sw.Close();
                    }

                    programWindow w = new programWindow();
                    w.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "Invalid login or password", 
                        "Validation error",
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error
                    );
                    passwordBox.Password = "";
                }
            }
            else
            {
                MessageBox.Show("Fill in all fields", "Fields error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            conn.Close();
        }

        private void FogPasscBtn_Click(object sender, RoutedEventArgs e)
        {
            passRecWindow w = new passRecWindow();
            w.Show();
            Close();
        }
    }
}
