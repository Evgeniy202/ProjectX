using MySql.Data.MySqlClient;
using Tutorial.SqlConn;
using System.Windows;

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

            string dbPassword = null;

            if ((le != "") & (password != ""))
            {
                string query = $"SELECT password FROM baseDataUsers WHERE login = '{le}' OR email = '{le}'";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                try { dbPassword = cmd.ExecuteScalar().ToString(); }
                catch { dbPassword = null; }

                if (password == dbPassword)
                {
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
