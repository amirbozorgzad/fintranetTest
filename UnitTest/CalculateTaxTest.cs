using Core.Contract.Enum;
using Core.Domain;
using Core.Domain.Context;
using Core.Domain.Model;
using MockQueryable.Moq;
using Moq;
using Service.Abstraction;
using Service.Implementation;
using Xunit;

namespace UnitTest;

public class CalculateTaxTest : IDisposable
{
    private static Mock<IUnitOfWork> _mockUnitOfWork;
    private static Mock<CoreContext> _mockCoreContext;
    private static Mock<ITaxCalculatorService> _mockTaxCalculatorService;

    public CalculateTaxTest()
    {
        //Init
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTaxCalculatorService = new Mock<ITaxCalculatorService>();
        _mockCoreContext = new Mock<CoreContext>();
    }

    public void Dispose()
    {
    }

    public class CheckCalculationIsWork : IClassFixture<CheckCalculationIsWork>
    {
        [Fact]
        public async ValueTask ShouldGetCorrectTaxValue()
        {
            var currency = new List<Currency>
            {
                new()
                {
                    Id = 1,
                    Name = "Swedish Krona",
                    IsoCode = "SEK"
                }
            };
            var city = new List<City>
            {
                new()
                {
                    Id = 1,
                    Name = "Gothenburg", DaysBeforeHoliday = 2, MaximumCongestionTaxPerDay = 60,
                    ChargeInterspacedInMinute = 60, CurrencyId = 1
                }
            };
            var calendar = new List<Calendar>
            {
                new()
                {
                    cityId = 1,
                    Name = "My city Calendar",
                    Description = "Calendar Description",
                    WorkingDaysInWeek = WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday | WeekDays.Thursday |
                                        WeekDays.Friday,
                    HolidaysMonth = Months.July,
                    StartDate = new DateTime(2013, 01, 01),
                    EndDate = new DateTime(2013, 12, 28)
                }
            };
            var holidays = new List<Holiday>();
            holidays.Add(new Holiday
                { CalendarId = 1, HolidayDate = new DateTime(2013, 01, 01) });
            holidays.Add(new Holiday
                { CalendarId = 1, HolidayDate = new DateTime(2013, 01, 05) });
            holidays.Add(new Holiday
                { CalendarId = 1, HolidayDate = new DateTime(2013, 06, 01) });
            holidays.Add(new Holiday
                { CalendarId = 1, HolidayDate = new DateTime(2013, 06, 06) });
            holidays.Add(new Holiday
                { CalendarId = 1, HolidayDate = new DateTime(2013, 06, 20) });
            holidays.Add(new Holiday
                { CalendarId = 1, HolidayDate = new DateTime(2013, 06, 26) });
            holidays.Add(new Holiday
                { CalendarId = 1, HolidayDate = new DateTime(2013, 12, 25) });
            holidays.Add(new Holiday
                { CalendarId = 1, HolidayDate = new DateTime(2013, 12, 26) });

            var vehicles = new List<Vehicle>();
            vehicles.Add(new Vehicle { Id = 1, Name = "Emergency" });
            vehicles.Add(new Vehicle { Id = 2, Name = "Buss" });
            vehicles.Add(new Vehicle { Id = 3, Name = "Diplomat" });
            vehicles.Add(new Vehicle { Id = 4, Name = "Motorcycle" });
            vehicles.Add(new Vehicle { Id = 5, Name = "Military" });
            vehicles.Add(new Vehicle { Id = 6, Name = "Foreign" });

            var tolls = new List<Toll>();
            tolls.Add(new Toll { CityId = 1, VehicleId = 1, IsFree = true });
            tolls.Add(new Toll { CityId = 1, VehicleId = 2, IsFree = true });

            var fees = new List<Fee>();
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(6, 0, 0), ToTime = new TimeSpan(6, 29, 0), Charge = 8 });
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(6, 30, 0), ToTime = new TimeSpan(6, 59, 0), Charge = 13 });
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(7, 0, 0), ToTime = new TimeSpan(7, 59, 0), Charge = 18 });
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(8, 0, 0), ToTime = new TimeSpan(8, 29, 0), Charge = 13 });
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(8, 30, 0), ToTime = new TimeSpan(14, 59, 0), Charge = 8 });
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(15, 0, 0), ToTime = new TimeSpan(15, 29, 0), Charge = 13 });
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(15, 30, 0), ToTime = new TimeSpan(16, 59, 0), Charge = 18 });
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(17, 0, 0), ToTime = new TimeSpan(17, 59, 0), Charge = 13 });
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(18, 0, 0), ToTime = new TimeSpan(18, 29, 0), Charge = 8 });
            fees.Add(new Fee
                { CityId = 1, FromTime = new TimeSpan(18, 30, 0), ToTime = new TimeSpan(5, 59, 0), Charge = 0 });

            var initializer = new CalculateTaxTest();
            _mockCoreContext.Setup(p => p.Calendar).Returns(calendar.AsQueryable().BuildMockDbSet().Object);
            _mockCoreContext.Setup(p => p.City).Returns(city.AsQueryable().BuildMockDbSet().Object);
            _mockCoreContext.Setup(p => p.Currency).Returns(currency.AsQueryable().BuildMockDbSet().Object);
            _mockCoreContext.Setup(p => p.Toll).Returns(tolls.AsQueryable().BuildMockDbSet().Object);
            _mockCoreContext.Setup(p => p.Fee).Returns(fees.AsQueryable().BuildMockDbSet().Object);
            _mockCoreContext.Setup(p => p.Vehicle).Returns(vehicles.AsQueryable().BuildMockDbSet().Object);
            _mockCoreContext.Setup(p => p.Holiday).Returns(holidays.AsQueryable().BuildMockDbSet().Object);
            var service = new TaxCalculatorService(_mockUnitOfWork.Object);
            //Check return false for wrong date
            var result1 =
                await service.CalculateTax("Gothenburg", "Emergency", new List<DateTime> { new(2023, 01, 02) });

            //Check return true for correct data
            var result2 =
                await service.CalculateTax("string", "Emergency", new List<DateTime> { new(2013, 01, 02) });


            Assert.False(result1.IsOk);
            Assert.True(result2.IsOk);

            // there are some other ways to test this endpoint 
            // these tests are sample to show unit test part
        }
    }
}