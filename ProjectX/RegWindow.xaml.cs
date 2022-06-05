﻿using System;
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

namespace ProjectX
{
    public partial class RegWindow : Window
    {
        string login = "";
        string password = "";
        string repPassword = "";
        string email = "";
        string code = "";

        MySqlConnection conn;

        public RegWindow()
        {
            InitializeComponent();

            conn = DBUtils.GetDBConnection();
            conn.Open();
        }

        private void regBtn_Click(object sender, RoutedEventArgs e)
        {
            login = loginBox.Text;
            password = passwordBox.Password;
            repPassword = repPassBox.Password;
            email = emailBox.Text;

            string ansLogin = null;
            string ansEmail = null;

            string query = $"SELECT login FROM baseDataUsers WHERE login = '{login}'";
            MySqlCommand cmd1 = new MySqlCommand(query, conn);

            try { ansLogin = cmd1.ExecuteScalar().ToString(); }
            catch { ansLogin = null; }

            query = $"SELECT login FROM baseDataUsers WHERE email = '{email}'";
            MySqlCommand cmd2 = new MySqlCommand(query, conn);

            try { ansEmail = cmd2.ExecuteScalar().ToString(); }
            catch { ansEmail = null; }

            if ((login.Length > 3) & (password.Length > 5) & (repPassword.Length > 5) & (email != null))
            {
                if ((ansLogin == null) & (ansEmail == null))
                {
                    if (password == repPassword)
                    {                      
                        if (email.Contains("@"))
                        {
                            string msgText = "Successful! Check your email and enter the code.";
                            string caption = "Successful";
                            var result = MessageBox.Show(
                                msgText, 
                                caption, MessageBoxButton.OK, 
                                MessageBoxImage.Asterisk
                            );

                            if (result == MessageBoxResult.OK)
                            {
                                var rand = new Random();

                                for (int i = 0; i < 6; i++)
                                {
                                    int num = rand.Next(9);
                                    code = code + Convert.ToString(num);
                                }

                                //Feature add send in Email...
                                Console.WriteLine(code);

                                lL.Visibility = Visibility.Hidden;
                                emL.Visibility = Visibility.Hidden;
                                pL.Visibility = Visibility.Hidden;
                                rPL.Visibility = Visibility.Hidden;
                                autBtn.Visibility = Visibility.Hidden;
                                regBtn.Visibility = Visibility.Hidden;
                                loginBox.Visibility = Visibility.Hidden;
                                passwordBox.Visibility = Visibility.Hidden;
                                repPassBox.Visibility = Visibility.Hidden;
                                emailBox.Visibility = Visibility.Hidden;

                                confCodeBox.Visibility = Visibility.Visible;
                                confCodeBtn.Visibility = Visibility.Visible;
                                confCodeLabel.Visibility = Visibility.Visible;                               
                            }
                        }
                        else
                        {
                            MessageBox.Show(
                                "Incorrect email!", 
                                "Email error", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error
                            );
                            emailBox.Text = "";
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
                else if (ansLogin != null)
                {
                    string msgText = "This login already exists. Please try another...";
                    string caption = "Login error";
                    MessageBox.Show(msgText, caption, MessageBoxButton.OK, MessageBoxImage.Error);

                    loginBox.Text = "";
                }
                else if (ansEmail != null)
                {
                    string msgText = "This email already exists, try another. " +
                        "If you want to restore the old account, click on Yes.";
                    string caption = "Email error";
                    var result = MessageBox.Show(msgText, 
                        caption, MessageBoxButton.YesNo, 
                        MessageBoxImage.Error
                    );

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            emailBox.Text = "";

                            //Add feature...

                            break;
                        case MessageBoxResult.No:
                            emailBox.Text = "";
                            break;
                    }

                    loginBox.Text = "";
                }
            }
            else
            {
                string mes = "Check that all fields are filled in correcttly...";
                string caption = "Fields error";
                MessageBox.Show(mes, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfCodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (confCodeBox.Text == code)
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();

                string query = $"INSERT INTO baseDataUsers (login, password, email) " +
                    $"VALUES ('{login}', '{password}', '{email}')";
                MySqlCommand cmd3 = new MySqlCommand(query, conn);
                cmd3.ExecuteNonQuery();

                string msgText = "Successful! Your account has been created!";
                string caption = "Succesful!";

                var result = MessageBox.Show(
                    msgText, caption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk
                );

                if (result == MessageBoxResult.OK)
                {
                    logInWindow a = new logInWindow();
                    a.Show();
                    Close();
                }
            }
            else
            {
                MessageBox.Show(
                    "Wrong code!",
                    "Code error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            conn.Close();
        }
    }
}
