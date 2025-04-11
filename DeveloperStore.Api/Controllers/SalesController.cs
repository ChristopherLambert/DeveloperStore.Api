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
        var sales = _saleService.GetAllSales();              // Recupera todas as vendas (entidades de domínio)
        var salesDto = _mapper.Map<IEnumerable<SaleDto>>(sales);  // Mapeia para DTOs de saída
        return Ok(salesDto);
    }

    // GET /sales/{id} - obtém uma venda pelo ID
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
        // Mapeia o DTO de entrada para a entidade de domínio
        var saleEntity = _mapper.Map<Sale>(saleDto);

        // Chama o serviço para adicionar a nova venda (o serviço aplica regras de desconto conforme necessário)
        var createdSale = _saleService.AddSale(saleEntity);
        // Após criação, mapeia de volta para DTO para retornar ao cliente
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
            return BadRequest("ID da URL não coincide com ID do objeto");
        }

        // Mapeia DTO para entidade e define o ID (assegurando que estamos atualizando a venda correta)
        var saleEntity = _mapper.Map<Sale>(saleDto);
        saleEntity.Id = id;

        bool updated = _saleService.UpdateSale(id, saleEntity);
        if (!updated)
        {
            return NotFound(); // Não encontrou a venda para atualizar
        }

        return NoContent(); // Sucesso na atualização (204 No Content)
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
