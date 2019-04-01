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
    /// Interaction logic for Genres.xaml
    /// </summary>
    public partial class Genres : Page
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
        public Genres()
        {
            InitializeComponent();
            btnSave.Click += btnSave_Click;
        }
        void SetBindings()
        {
          dGGenres.ItemsSource = db.GetGenres();
        }
        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var g in db.GetGenres())
                {
                    if(String.IsNullOrEmpty(g.Name))
                    {
                        throw new Exception("Genres name can't be empty");
                    }
                }
                var duplicate = db.GetGenres().GroupBy(x => x.Name).Where(x => x.Count() != 1).Select(x => x.Key).ToList();
                if (duplicate.Count >0)
                {
                    throw new Exception("Can't insert duplicate value");
                }
                db.SaveChanges();
                MessageBox.Show("Saved", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
    public class GenreValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            Genre course = (value as BindingGroup).Items[0] as Genre;
            if (String.IsNullOrEmpty(course.Name))
            {
                return new ValidationResult(false, "Genres name can't be empty");
            }

            //using (DbConnect db = new DbConnect())
            //{
            //    var d = db.GetGenres().FirstOrDefault(c => c.Name == course.Name && c.Id!=course.Id);
            //    if (d != null)
            //    {
            //        return new ValidationResult(false, "Can't insert duplicate value");
            //    }
            //}

            return ValidationResult.ValidResult;
            
        }
    }
}

