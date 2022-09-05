using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AplikacjeBazodanowe.Data.FileManager;
using AplikacjeBazodanowe.Data.Repository;
using AplikacjeBazodanowe.Models;
using AplikacjeBazodanowe.ViewModels;

namespace AplikacjeBazodanowe.Controllers;

[Authorize(Roles = "Admin")]
public class PanelController : Controller
{
    private IRepository _repo;
    private IFileManager _fileManager;
    public PanelController(IRepository repo, IFileManager fileManager)
    {
        _repo = repo;
        _fileManager = fileManager;
    }

    public IActionResult Index()
    {
        var posts = _repo.GetAllPosts();
        return View(posts);
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id.HasValue)
        {
            var post = _repo.GetPost(id.Value);
            var postVM = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                CurrentImage = post.Image,
                Description = post.Description,
                Category = post.Category,
                Tags = post.Tags
            };
            return View(postVM);
        }

        return View(new PostViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PostViewModel postVM)
    {
        var post = new Post
        {
            Id = postVM.Id,
            Title = postVM.Title,
            Body = postVM.Body,
            Description = postVM.Description,
            Category = postVM.Category,
            Tags = postVM.Tags
        };

        if(postVM.Image == null)
        {
            if (string.IsNullOrEmpty(postVM.CurrentImage))
                post.Image = _fileManager.DefaultImage;
            else
                post.Image = postVM.CurrentImage;
        }
        else
        {
            if (!string.IsNullOrEmpty(postVM.CurrentImage))
            {
                _fileManager.RemoveImage(postVM.CurrentImage);
            }
            post.Image = await _fileManager.SaveImageAsync(postVM.Image);
        }

        if (post.Id > 0)
        {
            _repo.UpdatePost(post);
        }
        else
        {
            _repo.AddPost(post);
        }

        if (await _repo.SaveChangesAsync())
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View(post);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Remove(int id)
    {
        var post = _repo.GetPost(id);

        if (!string.IsNullOrEmpty(post.Image))
        {
            _fileManager.RemoveImage(post.Image);
        }

        _repo.RemovePost(id);
        await _repo.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
