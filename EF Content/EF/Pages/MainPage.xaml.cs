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

namespace EF.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    /// 
    public partial class MainPage : Page
    {
        private DbConnect db;
        public event EventHandler<EventArgs> btnAddHandler;
        public event EventHandler<SerialEventArgs> lbSerialsHandler;

        public DbConnect Db
        {
            get { return db; }
            set { db = value;
            SetBindings();
            }
        }
        public MainPage()
        {
            InitializeComponent();
            AddDoubleClickEventStyle(dGSerials, new MouseButtonEventHandler(dGSerials_MouseDoubleClick));
        }
        public void SetBindings()
        {
            CommandBinding binding = new CommandBinding(Comands.DeleteSerialCommand);
            binding.Executed += Delete_Serial_Executed;
            binding.CanExecute += DeleteSerial_CanExecute;
            this.CommandBindings.Add(binding);
            this.btnDelete.Command = Comands.DeleteSerialCommand;

            dGSerials.ItemsSource = db.GetSerials();
            btnAdd.Click += btnAdd_Click;
        }

        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (btnAddHandler !=null)
            {
                btnAddHandler(this, new EventArgs());
            }
        }

        void Delete_Serial_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(String.Format("Are you sure you want to delete {0}?", (dGSerials.SelectedItem as Serial).Name), "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.DeleteSerial(dGSerials.SelectedItem as Serial);
            }
        }
        void DeleteSerial_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (dGSerials.SelectedItem == null)
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }
        public void dGSerials_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListBoxItem)
            {
                if(lbSerialsHandler!=null)
                lbSerialsHandler(this, new SerialEventArgs(((sender as ListBoxItem).Content as Serial).Id));
            }
        }
        private void AddDoubleClickEventStyle(ListBox listBox, MouseButtonEventHandler mouseButtonEventHandler)
        {
            if (listBox.ItemContainerStyle == null)
                listBox.ItemContainerStyle = new Style(typeof(ListBoxItem));
            listBox.ItemContainerStyle.Setters.Add(new EventSetter()
            {
                Event = System.Windows.Controls.Control.MouseDoubleClickEvent,
                Handler = mouseButtonEventHandler
            });
        }
    }
    public class SerialEventArgs : EventArgs
    {
        public int idSerial { get; set; }
        public SerialEventArgs(int id)
        {
            idSerial = id;
        }
    }
}
