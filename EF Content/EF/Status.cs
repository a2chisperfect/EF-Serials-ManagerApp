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

    public partial class Status
    {
        public Status()
        {
            this.Serials = new ObservableCollection<Serial>();
        }

        public int Id { get; set; }
        public string Status1 { get; set; }

        public virtual ICollection<Serial> Serials { get; set; }
    }
}
