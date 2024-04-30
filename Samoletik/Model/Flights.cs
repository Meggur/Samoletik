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
    
    public partial class Flights
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Flights()
        {
            this.Booking = new HashSet<Booking>();
            this.FlightSchedule = new HashSet<FlightSchedule>();
        }
    
        public int IDFlights { get; set; }
        public int IDFlightStatus { get; set; }
        public int IDDepartureAirport { get; set; }
        public int IDArrivalAirport { get; set; }
        public System.DateTime DateOfDeparture { get; set; }
        public int FlightNumber { get; set; }
        public System.TimeSpan TimeOfDeparture { get; set; }
    
        public virtual Airports Airports { get; set; }
        public virtual Airports Airports1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Booking { get; set; }
        public virtual FlightStatus FlightStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FlightSchedule> FlightSchedule { get; set; }
    }
}
