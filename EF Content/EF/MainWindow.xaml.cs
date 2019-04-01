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
using System.Data.Entity;
using EF.Pages;
using Microsoft.Win32;
using System.Collections.ObjectModel;


namespace EF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int? Id = null;
        MainPage mainPage = new MainPage();
        Genres genresPage = new Genres();
        ChannelsPage channelsPage = new ChannelsPage();
        SerialInfoPage infoPage = new SerialInfoPage();
        LoginPage logPage = new LoginPage();
       
        public MainWindow()
        {
            InitializeComponent();
           

            #region eventHandlers
            MainFrame.Navigating += MainFrame_Navigating;
            mainPage.btnAddHandler += btnAddSerialHandler;
            mainPage.lbSerialsHandler += mainPage_lbSerialsHandler;
            logPage.LoggedHandler += logPage_LoggedHandler;
            #endregion

            MainFrame.Navigate(logPage);


        }
        void logPage_LoggedHandler(object sender, EventArgs e)
        {
            MainFrame.Navigate(mainPage);
        }

        void mainPage_lbSerialsHandler(object sender, SerialEventArgs e)
        {
            Id = e.idSerial;
            infoPage.AddNew = false;
            MainFrame.Navigate(infoPage);
        }
        void btnAddSerialHandler(object sender, EventArgs e)
        {
            infoPage.AddNew = true;
            MainFrame.Navigate(infoPage);
        }

        void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Content is LoginPage)
            {
                Menu.Visibility = System.Windows.Visibility.Collapsed;
                (e.Content as LoginPage).Logged = false;
                (e.Content as LoginPage).ResetContent();

            }
            else
            {
                Menu.Visibility = System.Windows.Visibility.Visible;
            }
            if (e.Content is MainPage)
            {
                (e.Content as MainPage).Db = new DbConnect();
                infoPage.AddNew = false;
            }
            if (e.Content is Genres)
            {

                (e.Content as Genres).Db = new DbConnect();
                infoPage.AddNew = false;

            }
            if (e.Content is ChannelsPage)
            {

                (e.Content as ChannelsPage).Db = new DbConnect();
                infoPage.AddNew = false;
            }

            if (e.Content is SerialInfoPage)
            {
                try
                {
                    var tmp = new DbConnect();
                    if(Id !=null)
                    {
                        tmp.GetSerial(Id);
                        Id = null;
                    }
                    else
                    {
                        tmp.SelectedSerial = new Serial();
                    }
                    (e.Content as SerialInfoPage).Db = tmp;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }        
        }
        


        private void ButtonMainPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(mainPage);
        }

        private void ButtonGenres_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(genresPage);
        }

        private void ButtonChannels_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(channelsPage);
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(logPage);
        }
    }
}
