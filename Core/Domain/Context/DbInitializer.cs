using Core.Contract.Enum;
using Core.Domain.Model;

namespace Core.Domain.Context;

public class DbInitializer
{
    public static void Initialize(CoreContext context)
    {
        context.Database.EnsureCreated();

        //Seed Currency
        if (!context.Currency.Any())
        {
            var currency = new Currency
            {
                Name = "Swedish Krona",
                IsoCode = "SEK"
            };
            context.Currency.Add(currency);
            context.SaveChanges();
        }

        // Seed City
        if (!context.City.Any())
        {
            var city = new City
            {
                Name = "Gothenburg", DaysBeforeHoliday = 2, MaximumCongestionTaxPerDay = 60,
                ChargeInterspacedInMinute = 60, CurrencyId = 1
            };

            context.City.Add(city);
            context.SaveChanges();
        }

        // Seed Calendars for each City
        if (!context.Calendar.Any())
        {
            foreach (var city in context.City)
            {
                var calendar = new Calendar
                {
                    cityId = city.Id,
                    Name = $"{city.Name} Calendar",
                    Description = $"{city.Name} Calendar Description",
                    WorkingDaysInWeek = WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday | WeekDays.Thursday |
                                        WeekDays.Friday,
                    HolidaysMonth = Months.July,
                    StartDate = new DateTime(2013, 01, 01),
                    EndDate = new DateTime(2013, 12, 28)
                };

                context.Calendar.Add(calendar);
            }

            context.SaveChanges();
        }

        // Seed Holidays
        if (!context.Holiday.Any())
        {
            var holidays = new List<Holiday>();
            holidays.Add(new Holiday
                { CalendarId = context.Calendar.FirstOrDefault().Id, HolidayDate = new DateTime(2013, 01, 01) });
            holidays.Add(new Holiday
                { CalendarId = context.Calendar.FirstOrDefault().Id, HolidayDate = new DateTime(2013, 01, 05) });
            holidays.Add(new Holiday
                { CalendarId = context.Calendar.FirstOrDefault().Id, HolidayDate = new DateTime(2013, 06, 01) });
            holidays.Add(new Holiday
                { CalendarId = context.Calendar.FirstOrDefault().Id, HolidayDate = new DateTime(2013, 06, 06) });
            holidays.Add(new Holiday
                { CalendarId = context.Calendar.FirstOrDefault().Id, HolidayDate = new DateTime(2013, 06, 20) });
            holidays.Add(new Holiday
                { CalendarId = context.Calendar.FirstOrDefault().Id, HolidayDate = new DateTime(2013, 06, 26) });
            holidays.Add(new Holiday
                { CalendarId = context.Calendar.FirstOrDefault().Id, HolidayDate = new DateTime(2013, 12, 25) });
            holidays.Add(new Holiday
                { CalendarId = context.Calendar.FirstOrDefault().Id, HolidayDate = new DateTime(2013, 12, 26) });
            context.Holiday.AddRange(holidays);
            context.SaveChanges();
        }

        // Seed Vehicles
        if (!context.Vehicle.Any())
        {
            var vehicles = new List<Vehicle>();
            vehicles.Add(new Vehicle { Name = "Emergency" });
            vehicles.Add(new Vehicle { Name = "Buss" });
            vehicles.Add(new Vehicle { Name = "Diplomat" });
            vehicles.Add(new Vehicle { Name = "Motorcycle" });
            vehicles.Add(new Vehicle { Name = "Military" });
            vehicles.Add(new Vehicle { Name = "Foreign" });
            context.Vehicle.AddRange(vehicles);
            context.SaveChanges();
        }

        //Seed Tolls
        if (!context.Toll.Any())
        {
            var tolls = new List<Toll>();
            var cityId = context.City.FirstOrDefault().Id;
            foreach (var vehicle in context.Vehicle)
                switch (vehicle.Name)
                {
                    case "Emergency":
                    {
                        tolls.Add(new Toll { CityId = cityId, VehicleId = vehicle.Id, IsFree = true });
                        break;
                    }
                    case "Foreign":
                    {
                        tolls.Add(new Toll { CityId = cityId, VehicleId = vehicle.Id, IsFree = true });
                        break;
                    }
                    default:
                    {
                        tolls.Add(new Toll { CityId = cityId, VehicleId = vehicle.Id, IsFree = false });
                        break;
                    }
                }

            context.Toll.AddRange(tolls);
            context.SaveChanges();
        }

        //Seed Fees
        if (!context.Fee.Any())
        {
            var fees = new List<Fee>();
            var cityId = context.City.FirstOrDefault().Id;
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(6, 0, 0), ToTime = new TimeSpan(6, 29, 0), Charge = 8 });
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(6, 30, 0), ToTime = new TimeSpan(6, 59, 0), Charge = 13 });
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(7, 0, 0), ToTime = new TimeSpan(7, 59, 0), Charge = 18 });
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(8, 0, 0), ToTime = new TimeSpan(8, 29, 0), Charge = 13 });
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(8, 30, 0), ToTime = new TimeSpan(14, 59, 0), Charge = 8 });
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(15, 0, 0), ToTime = new TimeSpan(15, 29, 0), Charge = 13 });
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(15, 30, 0), ToTime = new TimeSpan(16, 59, 0), Charge = 18 });
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(17, 0, 0), ToTime = new TimeSpan(17, 59, 0), Charge = 13 });
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(18, 0, 0), ToTime = new TimeSpan(18, 29, 0), Charge = 8 });
            fees.Add(new Fee
                { CityId = cityId, FromTime = new TimeSpan(18, 30, 0), ToTime = new TimeSpan(5, 59, 0), Charge = 0 });
            context.Fee.AddRange(fees);
            context.SaveChanges();
        }
    }
}