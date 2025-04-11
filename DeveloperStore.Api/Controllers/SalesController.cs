using Microsoft.AspNetCore.Mvc;
using DeveloperStore.Services.DTOs;
using AutoMapper;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Services.Interfaces;

[ApiController]
[Route("sales")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly IMapper _mapper;

    public SalesController(ISaleService saleService, IMapper mapper)
    {
        _saleService = saleService;
        _mapper = mapper;
    }

    // GET /sales
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSales()
    {
        var sales = await _saleService.GetAllSalesAsync();
        var salesDto = _mapper.Map<IEnumerable<SaleDto>>(sales);
        return Ok(salesDto);
    }

    // GET /sales/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSale(Guid id)
    {
        var sale = await _saleService.GetSaleByIdAsync(id);
        if (sale == null)
            return NotFound();

        var saleDto = _mapper.Map<SaleDto>(sale);
        return Ok(saleDto);
    }

    // POST /sales
    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale([FromBody] SaleDto saleDto)
    {
        var saleEntity = _mapper.Map<Sale>(saleDto);
        var createdSale = await _saleService.CreateSaleAsync(saleEntity);
        var createdSaleDto = _mapper.Map<SaleDto>(createdSale);

        return CreatedAtAction(nameof(GetSale), new { id = createdSaleDto.Id }, createdSaleDto);
    }

    // PUT /sales/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSale(Guid id, [FromBody] SaleDto saleDto)
    {
        if (id != saleDto.Id)
            return BadRequest("ID da URL não coincide com ID do objeto");

        var saleEntity = _mapper.Map<Sale>(saleDto);
        saleEntity.Id = id;

        await _saleService.UpdateSaleAsync(saleEntity);
        return NoContent();
    }

    // DELETE /sales/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(Guid id)
    {
        await _saleService.DeleteSaleAsync(id);
        return NoContent();
    }

    [HttpPatch("{saleId}/items/{itemId}/cancel")]
    public async Task<IActionResult> CancelItem(Guid saleId, Guid itemId)
    {
        try
        {
            await _saleService.CancelSaleItemAsync(saleId, itemId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
