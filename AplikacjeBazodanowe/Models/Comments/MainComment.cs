using System.Collections.Generic;

namespace AplikacjeBazodanowe.Models.Comments;
public class MainComment : Comment
{
    public List<SubComment>? SubComments { get; set; }
}
