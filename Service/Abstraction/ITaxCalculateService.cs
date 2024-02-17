using Core.Contract;

namespace Service.Abstraction;

public interface ITaxCalculatorService
{
    ValueTask<ResultDto> CalculateTax(string cityName, string vehicleName, List<DateTime> dates);
}