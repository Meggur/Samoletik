//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Samoletik.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            this.Payments = new HashSet<Payments>();
        }
    
        public int IDBooking { get; set; }
        public int IDPassengers { get; set; }
        public int IDFlights { get; set; }
        public int IDBookingStatus { get; set; }
        public System.DateTime BookingDate { get; set; }
    
        public virtual BookingStatus BookingStatus { get; set; }
        public virtual Flights Flights { get; set; }
        public virtual Passengers Passengers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payments> Payments { get; set; }
    }
}
