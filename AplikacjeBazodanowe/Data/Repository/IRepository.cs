using System.Collections.Generic;
using System.Threading.Tasks;
using AplikacjeBazodanowe.Models;
using AplikacjeBazodanowe.Models.Comments;

namespace AplikacjeBazodanowe.Data.Repository;
public interface IRepository
{
    Post GetPost(int id);
    List<Post> GetAllPosts();
    List<Post> GetAllPosts(string Category);
    void RemovePost(int id);
    void AddPost(Post post);
    void UpdatePost(Post post);
    void AddSubcomment(SubComment comment);
    Task<bool> SaveChangesAsync();
}
