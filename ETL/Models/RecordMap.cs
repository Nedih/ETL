﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace ETL.Models
{
    internal class RecordMap : ClassMap<Record>
    {
        public RecordMap()
        {
            Map(m => m.TpepPickupDatetime).Name("tpep_pickup_datetime").TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");
            Map(m => m.TpepDropoffDatetime).Name("tpep_dropoff_datetime").TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");
            Map(m => m.PassengerCount).Name("passenger_count");
            Map(m => m.TripDistance).Name("trip_distance");
            Map(m => m.StoreAndFwdFlag).Name("store_and_fwd_flag");
            Map(m => m.PULocationID).Name("PULocationID");
            Map(m => m.DOLocationID).Name("DOLocationID");
            Map(m => m.FareAmount).Name("fare_amount");
            Map(m => m.TipAmount).Name("tip_amount");
        }
    }
}
