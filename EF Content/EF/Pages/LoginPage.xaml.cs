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
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.EntityClient;

namespace EF
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private bool logged;
        public EventHandler LoggedHandler;
        public bool Logged
        {
            get { return logged; }
            set { logged = value;
                  if(LoggedHandler !=null && Logged == true)
                  {
                      LoggedHandler(this, new EventArgs());
                  }
            }
        }
        
        public LoginPage()
        {
            InitializeComponent();
        }
        public void ResetContent()
        {
            Password.Clear();
            Login.Clear();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectionStringBuilder.CreateConnectionString(Login.Text, Password.Password);
                DbConnect db = new DbConnect();
                if (db.CheckUserRole(Login.Text.Trim()) == false)
                {
                    throw(new Exception());
                }
                Logged = true;
            }
            catch
            {
                Logged = false;
                MessageBox.Show(String.Format("Login failed for user '{0}'.",Login.Text.Trim()),"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
