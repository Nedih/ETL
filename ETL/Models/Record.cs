using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Models
{
    public class Record
    {
        [Key]  
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime TpepPickupDatetime { get; set; }
        public DateTime TpepDropoffDatetime { get; set; }
        public int PassengerCount { get; set; }
        public double TripDistance { get; set; }
        public string StoreAndFwdFlag { get; set; }
        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }
        public decimal FareAmount { get; set; }
        public decimal TipAmount { get; set; }
    }
}
