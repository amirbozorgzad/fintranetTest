using Core.Contract;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;

namespace API.Controller;

[Route("v1/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "TaxAreaV1")]
public class TaxController : ControllerBase
{
    private readonly ITaxCalculatorService _taxCalculateService;

    public TaxController(ITaxCalculatorService taxCalculateService)
    {
        _taxCalculateService = taxCalculateService;
    }

    [HttpPost]
    public async Task<ActionResult<ResultDto>> CalculateTax(TaxInputDto taxDto)
    {
        return Ok(await _taxCalculateService.CalculateTax(taxDto.CityName, taxDto.vehicleName, taxDto.dates));
    }
}