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
using Tutorial.SqlConn;
using MySql.Data.MySqlClient;

namespace ProjectX
{
    public partial class passRecWindow : Window
    {
        string code = "";
        string email = "";

        MySqlConnection conn;

        public passRecWindow()
        {
            InitializeComponent();

            conn = DBUtils.GetDBConnection();
            conn.Open();
        }

        private void actionBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((actionBtn.Content).ToString() == "Send")
            {
                email = actionBox.Text;

                if (email != "")
                {
                    if (email.Contains("@"))
                    {
                        string ans = null;

                        string query = $"SELECT email FROM baseDataUsers WHERE email = '{email}'";
                        MySqlCommand cmd = new MySqlCommand(query, conn);

                        try { ans = cmd.ExecuteScalar().ToString(); }
                        catch { ans = null; }

                        if (ans != null)
                        {
                            Random rand = new Random();
                            code = "";

                            for (int i = 0; i < 6; i++)
                            {
                                code += (rand.Next(9)).ToString();           
                            }
                            Console.WriteLine(code);

                            //Feature sending code to email

                            actionBox.Text = "";
                            emL.Content = "Confirmation code";
                            actionBtn.Content = "Confirm";
                        }
                        else if (ans == null)
                        {
                            string msgText = $"There is no user with {email}! Click Yes if you want to register...";
                            string caption = "Email error";
                            var result = MessageBox.Show(
                                msgText, 
                                caption, 
                                MessageBoxButton.YesNo, 
                                MessageBoxImage.Error
                             );

                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    RegWindow w = new RegWindow();
                                    w.Show();
                                    Close();
                                    break;
                                case MessageBoxResult.No:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "Incorrect email!", 
                            "Field error", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error
                         );
                    }
                }
                else
                {
                    MessageBox.Show("Empty field!", "Field error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if ((actionBtn.Content).ToString() == "Confirm")
            {
                if (actionBox.Text != "")
                {
                    Console.WriteLine(actionBox.Text + "   " + code);
                    if (actionBox.Text == code)
                    {
                        actionBox.Visibility = Visibility.Hidden;
                        actionBtn.Visibility = Visibility.Hidden;
                        emL.Visibility = Visibility.Hidden;

                        passL.Visibility = Visibility.Visible;
                        repL.Visibility = Visibility.Visible;
                        passwordBox.Visibility = Visibility.Visible;
                        repPassBox.Visibility = Visibility.Visible;
                        confirmBtn.Visibility = Visibility.Visible;
                    }
                    else if (actionBox.Text != code)
                    {
                        MessageBox.Show("Wrong code!", "Code error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (actionBox.Text == "")
                {
                    MessageBox.Show("Empty field!", "Field error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordBox.Password;
            string repPassword = repPassBox.Password;

            if ((password != "") & (repPassword != ""))
            {
                if (password.Length > 5)
                {
                    if (password == repPassword)
                    {
                        string query = $"UPDATE baseDataUsers SET password = '{password}' WHERE email = '{email}'";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.ExecuteNonQuery();

                        var result = MessageBox.Show(
                            "Password changed!", 
                            "Successful", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information
                        );

                        if (result == MessageBoxResult.OK)
                        {
                            logInWindow w = new logInWindow();
                            w.Show();
                            Close();
                        }
                    }
                    else
                    {
                        string msgText = "Password does not match...";
                        string caption = "Password error";
                        MessageBox.Show(msgText, caption, MessageBoxButton.OK, MessageBoxImage.Error);

                        passwordBox.Password = "";
                        repPassBox.Password = "";
                    }
                }
                else
                {
                    MessageBox.Show("Short password!", "Length error", MessageBoxButton.OK, MessageBoxImage.Error);
                    passwordBox.Password = "";
                    repPassBox.Password = "";
                }
            }
            else if ((password == "") | (repPassword == ""))
            {
                MessageBox.Show("Empty field!", "Field error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            conn.Close();
        }
    }
}
