using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace EF
{
    public class DbConnect : IDisposable
    {
        private SerialsEntities db;
        public Serial SelectedSerial { get; set; }
        public DbConnect()
        {
            db =  new SerialsEntities();
        }

        public void Dispose()
        {
            db.Dispose();
        }
        public ObservableCollection<Genre> GetGenres()
        {
            db.Genres.Load();
            return db.Genres.Local;
        }
        public void AddGenre(string genreName)
        {
            db.Genres.Add(new Genre() { Name = genreName });
            db.SaveChanges();
        }
        public void DeleteGenre(int id)
        {
            Genre tmp = db.Genres.Find(id);
            db.Genres.Remove(tmp);
            db.SaveChanges();
        }
        public ObservableCollection<TVChannel> GetChannels()
        {
            db.TVChannels.Load();
            return db.TVChannels.Local;
        }
        public void AddChannel(string channelName)
        {
            db.TVChannels.Add(new TVChannel() { Name = channelName});
            db.SaveChanges();
        }
        public void DeleteChannel(int id)
        {
            TVChannel tmp = db.TVChannels.Find(id);
            if (tmp != null)
            {
                db.TVChannels.Remove(tmp);
            }
            db.SaveChanges();
        }

        public ObservableCollection<Serial> GetSerials()
        {
            db.Serials.Load();
            return db.Serials.Local;
        }

        public ObservableCollection<Series> GetSeries()
        {
            db.Series.Load();
            return db.Series.Local;
        }
        public void DeleteSeries(Series s)
        {
            db.Series.Remove(s);
        }
        public ObservableCollection<Status> GetStatus()
        {
            db.Status.Load();
            return db.Status.Local;
        }
        public void GetSerial(int? id)
        {

            var serial = db.Serials.Where(s => s.Id == id).First();
            
            SelectedSerial = serial;
        }
        public void AddSerial(Serial s)
        {
            db.Serials.Add(s);
        }
        public void DeleteSerial(Serial serial)
        {
            Serial del = db.Serials.Find(serial.Id);
            List<Series> series = new List<Series>();
            db.Series.Load();
            foreach (var s in db.Series.Local)
            {
                if (s.Id_Serial == del.Id)
                    series.Add(s);
            }
            db.Series.RemoveRange(series);
            db.Serials.Remove(del);
            db.SaveChanges();
            
        }
        public bool? CheckUserRole(string username)
        {
          var tmp = db.sp_check_content_role(username).FirstOrDefault();
          return tmp;
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
