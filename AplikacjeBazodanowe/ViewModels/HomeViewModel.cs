using System.Collections.Generic;
using AplikacjeBazodanowe.Models;

namespace AplikacjeBazodanowe.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Post> Posts { get; set; } = null!;
    public string CategoryImageUrl { get; set; } = null!;
    public string Category { get; set; } = null!;
}