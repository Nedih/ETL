using CsvHelper.Configuration;
using ETL.Util;

namespace ETL.Models
{
    internal class RecordMap : ClassMap<Record>
    {
        public RecordMap()
        {
            Map(m => m.TpepPickupDatetime)
                .Name("tpep_pickup_datetime")
                .TypeConverterOption.Format("MM/dd/yyyy hh:mm:ss tt")
                .TypeConverter<DateTimeUtcConverter>();

            Map(m => m.TpepDropoffDatetime)
                .Name("tpep_dropoff_datetime")
                .TypeConverterOption.Format("MM/dd/yyyy hh:mm:ss tt")
                .TypeConverter<DateTimeUtcConverter>();

            Map(m => m.PassengerCount).Name("passenger_count");
            Map(m => m.TripDistance).Name("trip_distance");

            Map(m => m.StoreAndFwdFlag).Name("store_and_fwd_flag")
                .Name("store_and_fwd_flag")
                .Convert(row => {
                    var value = row.Row.GetField("store_and_fwd_flag");
                    return value.Equals("Y", StringComparison.OrdinalIgnoreCase) ? "Yes"
                         : value.Equals("N", StringComparison.OrdinalIgnoreCase) ? "No"
                         : value;
                });

            Map(m => m.PULocationID).Name("PULocationID");
            Map(m => m.DOLocationID).Name("DOLocationID");
            Map(m => m.FareAmount).Name("fare_amount");
            Map(m => m.TipAmount).Name("tip_amount");
        }
    }
}
