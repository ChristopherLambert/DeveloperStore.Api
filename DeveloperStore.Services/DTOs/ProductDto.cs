namespace DeveloperStore.Services.DTOs;
using System.Text.Json.Serialization;

public class ProductDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    [JsonPropertyName("rating")]
    public RatingDto Rating { get; set; } = new();
}

public class RatingDto
{
    [JsonPropertyName("rate")]
    public double Rate { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}
