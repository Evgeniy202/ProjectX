using MySql.Data.MySqlClient;
using Tutorial.SqlConn;
using System;
using System.IO;
using System.Windows;

namespace ProjectX
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RegBtn_Copy_Click(object sender, RoutedEventArgs e)
        {
            RegWindow a = new RegWindow();
            a.Show();
            Close();
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            logInWindow w = new logInWindow();
            w.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader("conf/config.txt");
                string line = sr.ReadLine();

                if (line != "")
                {
                    MySqlDataReader datas;
                    string dbId = null;
                    string login = null;

                    string[] data = line.Split('?');

                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();

                    string query = $"SELECT id, login FROM baseDataUsers WHERE " +
                        $"login = '{data[0]}' OR email = '{data[0]}' AND password = '{data[1]}'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    try
                    {
                        datas = cmd.ExecuteReader();

                        while (datas.Read())
                        {
                            dbId = datas[0].ToString();
                            login = datas[1].ToString();
                        }
                    }
                    catch { datas = null; }

                    if (login != null)
                    {
                        string path = "conf/data.txt";
                        string str = $"{data[0]}?{data[1]}";

                        FileStream fs = new FileStream(path, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);

                        sw.WriteLine(str);
                        sw.Close();

                        programWindow w = new programWindow();
                        w.Show();
                        Close();
                        path = "conf/data.txt";
                        str = $"{dbId}?{login}?{data[1]}";

                        fs = new FileStream(path, FileMode.Create);
                        sw = new StreamWriter(fs);

                        sw.WriteLine(data);
                        sw.Close();
                    }

                    conn.Close();
                }

                sr.Close();
            }
            catch
            {    
            }
        }
    }
}
