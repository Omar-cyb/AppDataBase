using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AplikacjeBazodanowe.Models;
using AplikacjeBazodanowe.Models.Comments;

namespace AplikacjeBazodanowe.Data.Repository;
public class Repository : IRepository
{
    private AppDbContext _ctx;
    public Repository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public void AddPost(Post post)
    {
        _ctx.Posts.Add(post);
    }

    public List<Post> GetAllPosts()
    {
        return _ctx.Posts.ToList();
    }

    public List<Post> GetAllPosts(string Category)
    {
        return _ctx.Posts
            .Where(post => post.Category.ToLower() == Category.ToLower())
            .ToList();
    }

    public Post GetPost(int id)
    {
        return _ctx.Posts
            .Include(p => p.MainComments)
            .ThenInclude(p => p.SubComments)
            .FirstOrDefault(p => p.Id == id);
    }

    public void RemovePost(int id)
    {
        _ctx.Posts.Remove(GetPost(id));
    }

    public void UpdatePost(Post post)
    {
        _ctx.Posts.Update(post);
    }

    public async Task<bool> SaveChangesAsync()
    {
        if (await _ctx.SaveChangesAsync() > 0)
        {
            return true;
        }
        return false;
    }

    public void AddSubcomment(SubComment comment)
    {
        _ctx.SubComments.Add(comment);
    }
}
