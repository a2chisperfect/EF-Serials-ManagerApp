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

namespace EF.Pages
{
    /// <summary>
    /// Interaction logic for Channels.xaml
    /// </summary>
    public partial class ChannelsPage : Page
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
        public ChannelsPage()
        {
            InitializeComponent();
            btnSave.Click +=btnSave_Click;
        }
        void SetBindings()
        {
           dGChannels.ItemsSource = db.GetChannels();
        }
        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var g in db.GetChannels())
                {
                    if (String.IsNullOrEmpty(g.Name))
                    {
                        throw new Exception("Channel name can't be empty");
                    }
                }
                var duplicate = db.GetChannels().GroupBy(x => x.Name).Where(x => x.Count() != 1).Select(x => x.Key).ToList();
                if (duplicate.Count > 0)
                {
                    throw new Exception("Can't insert duplicate value");
                }
                db.SaveChanges();
                MessageBox.Show("Saved", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }

    public class ChannelValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            TVChannel course = (value as BindingGroup).Items[0] as TVChannel;
            if (String.IsNullOrEmpty(course.Name))
            {
                return new ValidationResult(false, "Channel name can't be empty");
            }
            //using (DbConnect db = new DbConnect())
            //{
            //    var d = db.GetChannels().FirstOrDefault(c => c.Name == course.Name && c.Id != course.Id);
            //    if (d != null)
            //    {
            //        return new ValidationResult(false, "Can't insert duplicate value");
            //    }
            //}

            return ValidationResult.ValidResult;

        }
    }
}
