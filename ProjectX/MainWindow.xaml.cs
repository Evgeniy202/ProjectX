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

                    datas.Close();

                    if (login != null)
                    {                                  
                        string path = "conf/data.txt";
                        string str = $"{dbId}?{login}?{data[1]}";

                        FileStream fs = new FileStream(path, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);

                        sw.WriteLine(str);
                        sw.Close();

                        string first_name = "";
                        string last_name = "";
                        query = $"SELECT id, first_name, last_name FROM userInformatin WHERE id = '{dbId}'";

                        try
                        {
                            cmd = new MySqlCommand(query, conn);
                            datas = cmd.ExecuteReader();

                            while (datas.Read())
                            {
                                dbId = datas[0].ToString();
                                first_name = datas[1].ToString();
                                last_name = datas[2].ToString();
                            }
                        }
                        catch(Exception er) { Console.WriteLine(er);  }

                        if ((first_name != "") & (last_name != ""))
                        {
                            path = "conf/generalData.txt";
                            str = $"{dbId}?{first_name}?{last_name}";

                            fs = new FileStream(path, FileMode.Create);
                            sw = new StreamWriter(fs);

                            sw.WriteLine(str);
                            sw.Close();
                        }
                        else if ((first_name != "") & (last_name == ""))
                        {
                            path = "conf/generalData.txt";
                            str = $"{dbId}?{first_name}";

                            fs = new FileStream(path, FileMode.Create);
                            sw = new StreamWriter(fs);

                            sw.WriteLine(str);
                            sw.Close();
                        }

                        programWindow w = new programWindow();
                        w.Show();
                        Close();
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
