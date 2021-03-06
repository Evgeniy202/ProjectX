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
    public partial class programWindow : Window
    {
        public programWindow()
        {
            InitializeComponent();

            string line = "";
            try
            {
                StreamReader sr = new StreamReader("conf/generalData.txt");
                line = sr.ReadLine();
            }
            catch { line = ""; }
            if (line != "")
            {
                MainFrame.Content = new Pages.mainPage();
            }
            else
            {
                MainFrame.Content = new Pages.userInf();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            string path = "conf/data.txt";
            string str = "";

            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(str);
            sw.Close();

            path = "conf/generalData.txt";
            str = "";

            fs = new FileStream(path, FileMode.Create);
            sw = new StreamWriter(fs);

            sw.WriteLine(str);
            sw.Close();
        }
    }
}
