using Microsoft.AspNetCore.Mvc;
using DeveloperStore.Services;
using DeveloperStore.Services.DTOs;
using DeveloperStore.Repository.Models;
using AutoMapper;
using DeveloperStore.Domain.Entities;

[ApiController]
[Route("sales")]  // Rota base para este controller (expondo /sales)
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly IMapper _mapper;

    public SalesController(ISaleService saleService, IMapper mapper)
    {
        _saleService = saleService;
        _mapper = mapper;
    }

    // GET /sales - lista todas as vendas
    [HttpGet]
    public ActionResult<IEnumerable<SaleDto>> GetSales()
    {
        var sales = _saleService.GetAllSales();              // Recupera todas as vendas (entidades de dom�nio)
        var salesDto = _mapper.Map<IEnumerable<SaleDto>>(sales);  // Mapeia para DTOs de sa�da
        return Ok(salesDto);
    }

    // GET /sales/{id} - obt�m uma venda pelo ID
    [HttpGet("{id}")]
    public ActionResult<SaleDto> GetSale(int id)
    {
        var sale = _saleService.GetSaleById(id);      // Busca a venda pelo ID
        if (sale == null)
            return NotFound();

        var saleDto = _mapper.Map<SaleDto>(sale);     // Converte entidade para DTO
        return Ok(saleDto);
    }

    // POST /sales - cria uma nova venda
    [HttpPost]
    public ActionResult<SaleDto> CreateSale([FromBody] SaleDto saleDto)
    {
        // Mapeia o DTO de entrada para a entidade de dom�nio
        var saleEntity = _mapper.Map<Sale>(saleDto);

        // Chama o servi�o para adicionar a nova venda (o servi�o aplica regras de desconto conforme necess�rio)
        var createdSale = _saleService.AddSale(saleEntity);
        // Ap�s cria��o, mapeia de volta para DTO para retornar ao cliente
        var createdSaleDto = _mapper.Map<SaleDto>(createdSale);

        // Retorna 201 Created com o objeto criado e o header Location (URL do novo recurso)
        return CreatedAtAction(nameof(GetSale), new { id = createdSaleDto.Id }, createdSaleDto);
    }

    // PUT /sales/{id} - atualiza uma venda existente
    [HttpPut("{id}")]
    public IActionResult UpdateSale(int id, [FromBody] SaleDto saleDto)
    {
        if (id != saleDto.Id)
        {
            return BadRequest("ID da URL n�o coincide com ID do objeto");
        }

        // Mapeia DTO para entidade e define o ID (assegurando que estamos atualizando a venda correta)
        var saleEntity = _mapper.Map<Sale>(saleDto);
        saleEntity.Id = id;

        bool updated = _saleService.UpdateSale(id, saleEntity);
        if (!updated)
        {
            return NotFound(); // N�o encontrou a venda para atualizar
        }

        return NoContent(); // Sucesso na atualiza��o (204 No Content)
    }

    // DELETE /sales/{id} - remove uma venda
    [HttpDelete("{id}")]
    public IActionResult DeleteSale(int id)
    {
        bool removed = _saleService.DeleteSale(id);
        if (!removed)
        {
            return NotFound();
        }
        return NoContent();
    }
}
