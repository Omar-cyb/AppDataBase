using System.ComponentModel.DataAnnotations;

namespace AplikacjeBazodanowe.ViewModels
{
  public class CommentViewModel
  {
      [Required]
      public int PostId { get; set; }
      [Required]
      public string? Message { get; set; }
      [Required]
      public int MainCommentId { get; set; }
  }
}
