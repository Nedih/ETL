using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Models
{
    public class Record
    {
        [Key, Column(Order = 0)]
        public DateTime TpepPickupDatetime { get; set; }

        [Key, Column(Order = 1)]
        public DateTime TpepDropoffDatetime { get; set; }

        [Key, Column(Order = 2)]
        public int PassengerCount { get; set; }

        public double TripDistance { get; set; }
        public string StoreAndFwdFlag { get; set; }
        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }
        public decimal FareAmount { get; set; }
        public decimal TipAmount { get; set; }
    }
}
