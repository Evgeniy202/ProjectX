using MySql.Data.MySqlClient;
using Tutorial.SqlConn;
using System;
using System.Collections.Generic;
using System.IO;
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
                    string[] data = line.Split('?');

                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();

                    string query = $"SELECT login FROM baseDataUsers WHERE " +
                        $"login = '{data[0]}' OR email = '{data[0]}' AND password = '{data[1]}'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (cmd.ExecuteScalar().ToString() != "")
                    {
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
