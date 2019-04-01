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
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace EF.Pages
{
    /// <summary>
    /// Interaction logic for SerialInfoPage.xaml
    /// </summary>
    public partial class SerialInfoPage : Page
    {
        private DbConnect db;
        public DbConnect Db
        {
            get { return db; }
            set
            {
                db = value;
                SetBindings();
            }
        }
        public bool AddNew { get; set; }
        public SerialInfoPage()
        {
            InitializeComponent();
            AddNew = false;
            btnSelect.Click += btnSelect_Click;
            btnSave.Click += btnSave_Click;
        }
        void SetBindings()
        {
           
            cbStatus.ItemsSource = db.GetStatus();
            cbChannel.ItemsSource = db.GetChannels();

            Binding img = new Binding();
            img.Source = db.SelectedSerial;
            img.Path = new PropertyPath("Path");
            img.Mode = BindingMode.TwoWay;
            img.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            Img.SetBinding(Image.SourceProperty, img);
            Img.DataContext = db.SelectedSerial;

            StackSerial.DataContext = db.SelectedSerial;
            Binding status = new Binding();
            status.Source = db.SelectedSerial;
            status.Path = new PropertyPath("Status");
            status.Mode = BindingMode.TwoWay;
            status.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            cbStatus.SetBinding(ComboBox.SelectedItemProperty, status);

            Binding channel = new Binding();
            channel.Source = db.SelectedSerial;
            channel.Path = new PropertyPath("TVChannel");
            channel.Mode = BindingMode.TwoWay;
            channel.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            cbChannel.SetBinding(ComboBox.SelectedItemProperty, channel);

            Binding genreslb = new Binding();
            genreslb.Source = db.SelectedSerial.Genres;
            //genreslb.Path = new PropertyPath("Name");
            genreslb.Mode = BindingMode.OneWay;
            genreslb.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            lbGenres.SetBinding(ListBox.ItemsSourceProperty, genreslb);

            Series.ItemsSource = db.SelectedSerial.Series;

            if (AddNew)
            {
                tbSerialName.BorderBrush = Brushes.Black;
                tbDescription.BorderBrush = Brushes.Black;
            }
            else
            {
                tbSerialName.BorderBrush = Brushes.Transparent;
                tbDescription.BorderBrush = Brushes.Transparent;
            }
            ObservableCollection<SelectableObject<Genre>> genres = new ObservableCollection<SelectableObject<Genre>>();
            //cbGenres.ItemsSource = db.GetGenres();
            foreach (var item in db.GetGenres())
            {
               
                if (db.SelectedSerial.Genres.Contains(item))
                {
                    genres.Add(new SelectableObject<Genre>(item,true));
                }
                else
                {
                    genres.Add(new SelectableObject<Genre>(item, false));
                }
                //MessageBox.Show((item as Genre).Name);
                
                cbGenres.ItemsSource = genres;
            }
            
        }
        void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "jpg|*jpg| png|*png";
            if (dialog.ShowDialog() == true)
            {
                Img.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }
        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(db.SelectedSerial.Name))
                    throw new Exception("Serials name can't be empty");

                foreach (var series in db.SelectedSerial.Series)
                {
                    if(String.IsNullOrEmpty(series.Name)) throw new Exception("Series name can't be empty");
                }

                var duplicateName = db.SelectedSerial.Series.GroupBy(x => x.Name).Where(x => x.Count() != 1).Select(x => x.Key).ToList();
                var duplicateNumber = db.SelectedSerial.Series.GroupBy(x => new { x.Season, x.Number }).Where(x => x.Count() != 1).Select(x => x.Key).ToList();
                if (duplicateName.Count > 0 || duplicateNumber.Count > 0)
                {
                    throw new Exception("Can't insert duplicate value");
                }

                if (AddNew)
                {
                    db.AddSerial(db.SelectedSerial);
                    AddNew = false;
                }
                var deleted = db.GetSeries().Where(s => s.Id_Serial == db.SelectedSerial.Id).Except(db.SelectedSerial.Series).ToList();
                foreach (var item in deleted)
                {
                    db.DeleteSeries(db.GetSeries().First(s => s.Id == item.Id));
                }

                db.SaveChanges();
                MessageBox.Show("Saved", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OnCbObjectCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            //StringBuilder sb = new StringBuilder();
            //foreach (SelectableObject<Genre> cbObject in cbGenres.Items)
            //    if (cbObject.IsSelected)
            //        sb.AppendFormat("{0}, ", cbObject.ObjectData.Name);
            //cbGenres.Text = sb.ToString().Trim().TrimEnd(',');

            var genre = db.GetGenres().First(g => g.Name == (sender as CheckBox).Content.ToString());
            if (db.SelectedSerial.Genres.Contains(genre))
            {
                db.SelectedSerial.Genres.Remove(genre);
            }
            else
            {
                db.SelectedSerial.Genres.Add(db.GetGenres().First(g => g.Name == genre.Name));
            }
        }

        private void OnCbObjectsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            comboBox.SelectedItem = null;
        }
    }
    public class SeriesValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            var course = (value as BindingGroup).Items[0] as Series;
            if (String.IsNullOrEmpty(course.Name))
            {
                return new ValidationResult(false, "Series mame can't be empty");
            }

            if(course.Number <=0)
            {
                return new ValidationResult(false, "Series number can't be zero or less");
            }
            if (course.Season <= 0)
            {
                return new ValidationResult(false, "Series season can't be zero or less");
            }
            return ValidationResult.ValidResult;

        }
    }

    public class SelectableObject<T>
    {
        public bool IsSelected { get; set; }
        public T ObjectData { get; set; }

        public SelectableObject(T objectData)
        {
            ObjectData = objectData;
        }

        public SelectableObject(T objectData, bool isSelected)
        {
            IsSelected = isSelected;
            ObjectData = objectData;
        }
    }
}
