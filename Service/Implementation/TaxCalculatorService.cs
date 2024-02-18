using Core.Contract;
using Core.Domain;
using Core.Domain.Model;
using Core.ShareExtension;
using Microsoft.EntityFrameworkCore;
using Service.Abstraction;

namespace Service.Implementation;

public class TaxCalculatorService : ITaxCalculatorService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaxCalculatorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Calculate tax with city name and vehicle name and selected dates
    /// </summary>
    /// <param name="cityName"></param>
    /// <param name="vehicleName"></param>
    /// <param name="dates"></param>
    /// <returns></returns>
    public async ValueTask<ResultDto> CalculateTax(string cityName, string vehicleName, List<DateTime> dates)
    {
        //Checking date in 2013 range
        if (dates.Any(d => d.Year != 2013))
            return new ResultDto { IsOk = false, Result = "The date should be in year 2013" };

        //Checking the existence of the city
        var city = await _unitOfWork.GenericRepository<City>().Get(c => c.Name.ToLower() == cityName.ToLower())
            .Include(c => c.Calendar)
            .ThenInclude(c => c.HoliDays)
            .Include(c => c.Currency)
            .SingleOrDefaultAsync();
        if (city.IsNull()) return new ResultDto { IsOk = false, Result = "The city does not exist." };

        //Checking the existence of the vehicle
        var vehicle = await _unitOfWork.GenericRepository<Vehicle>().Get(v => v.Name == vehicleName)
            .Include(v => v.Toll)
            .SingleOrDefaultAsync();
        if (vehicle.IsNull()) return new ResultDto { IsOk = false, Result = "The vehicle does not exist." };

        //Checking fee free vehicle 
        if (vehicle.Toll.FirstOrDefault(t => t.CityId == city.Id).IsFree)
            return new ResultDto { IsOk = true, Result = "This vehicle is free." };

        //Checking free toll date
        var isNotFreeDays = new List<DateTime>();
        foreach (var date in dates)
            if (!IsFreeTollDates(date, city))
                isNotFreeDays.Add(date);
        if (isNotFreeDays.Count == 0)
            return new ResultDto { IsOk = true, Result = "These days are free." };
        var finalResult = await GetTaxPerDay(isNotFreeDays, city);
        return new ResultDto
            { IsOk = true, Result = "The Calculated tax is: " + finalResult + " " + city.Currency.IsoCode };
    }

    private bool IsFreeTollDates(DateTime date, City city)
    {
        var isInHolidays = city.Calendar.HoliDays.Any(h => h.HolidayDate.Date == date.Date);
        var isInBeforeHolidays =
            city.Calendar.HoliDays.All(h => h.HolidayDate.AddDays(-city.DaysBeforeHoliday).Date >= date.Date);
        var isInHolidayMonths = Calendar.IsMonthInHolidaysMonth(date.ToDateOnly(), city.Calendar.HolidaysMonth);
        var isNotInWorkingDay = !Calendar.IsDateInWorkingDays(date.ToDateOnly(), city.Calendar.WorkingDaysInWeek);
        return isInHolidays || isInBeforeHolidays || isInHolidayMonths || isNotInWorkingDay;
    }

    private async ValueTask<decimal> GetTaxPerDay(List<DateTime> dates, City city)
    {
        var startDistance = dates[0];
        decimal totalFee = 0;

        foreach (var date in dates)
        {
            var fee = _unitOfWork.GenericRepository<Fee>();
            var nextFee = await fee.Get(t => t.ToTime >= date.ToTimeSpan() && t.FromTime <= date.ToTimeSpan())
                .FirstOrDefaultAsync();
            var tempFee = await fee.Get(t => t.ToTime >= date.ToTimeSpan() && t.FromTime <= date.ToTimeSpan())
                .FirstOrDefaultAsync();

            var nextFeeCharge = nextFee.IsNull() ? 0 : nextFee.Charge;
            var tempFeeCharge = tempFee.IsNull() ? 0 : nextFee.Charge;

            var difference = date.Subtract(startDistance);
            var minutes = (int)difference.TotalMinutes;

            if (minutes <= city.ChargeInterspacedInMinute)
            {
                if (totalFee > 0) totalFee -= tempFeeCharge;
                if (nextFeeCharge >= tempFeeCharge) tempFee = nextFee;
                totalFee += tempFeeCharge;
            }
            else
            {
                totalFee += nextFeeCharge;
                startDistance = date;
            }
        }

        if (totalFee > city.MaximumCongestionTaxPerDay) totalFee = city.MaximumCongestionTaxPerDay;

        return totalFee;
    }
}