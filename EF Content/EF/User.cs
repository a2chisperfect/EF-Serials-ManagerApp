//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EF
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public partial class User
    {
        public User()
        {
            this.Marks = new ObservableCollection<Mark>();
            this.Series = new ObservableCollection<Series>();
            this.Serials = new ObservableCollection<Serial>();
            this.Serials1 = new ObservableCollection<Serial>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }
        public virtual ICollection<Series> Series { get; set; }
        public virtual ICollection<Serial> Serials { get; set; }
        public virtual ICollection<Serial> Serials1 { get; set; }
    }
}