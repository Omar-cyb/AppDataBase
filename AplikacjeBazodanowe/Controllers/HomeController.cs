using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AplikacjeBazodanowe.Models.Comments;
using AplikacjeBazodanowe.Data.FileManager;
using AplikacjeBazodanowe.Data.Repository;
using AplikacjeBazodanowe.ViewModels;

namespace AplikacjeBazodanowe.Controllers;
public class HomeController : Controller
{
    private readonly IRepository _repo;
    private readonly IFileManager _fileManager;

    private readonly Dictionary<string, string> _categoryPhotos;


    public HomeController(IRepository repo, IFileManager fileManager, IConfiguration config)
    {
        _repo = repo;
        _fileManager = fileManager;
        var staticPath = config["Path:Static"];

        _categoryPhotos = new Dictionary<string, string>
        {
            { "Programowanie", "photo02.jpg" },
            { "Podroz", "photo01.jpg" },
            { "Fotografia", "photo03.jpg" },
            { "Default", "photo04.jpg" }
        }
        .Select(entry => {
            return new KeyValuePair<string, string>(
                    entry.Key,
                    Path.Combine(staticPath, entry.Value)
                    .Replace('\\', '/')
                    .Replace("wwwroot", "..")
                );
            })
        .ToDictionary(entry => entry.Key, entry => entry.Value);
    }

    private string GetCategoryPhotoUrl(string category)
    {
        if (string.IsNullOrEmpty(category) || !_categoryPhotos.ContainsKey(category))
            return _categoryPhotos["Default"];
        
        return _categoryPhotos[category];
    }


    public IActionResult Index(string category)
    {
        var posts = string.IsNullOrEmpty(category) ? _repo.GetAllPosts() : _repo.GetAllPosts(category);
        return View(
            new HomeViewModel
            {
                Posts = posts,
                CategoryImageUrl = GetCategoryPhotoUrl(category),
                Category = string.IsNullOrEmpty(category) ? "Strona główna" : category
            });
    }
    public IActionResult Post(int id)
    {
        var post = _repo.GetPost(id);
        return View(post);
    }

    [HttpGet("/Image/{imageName}")]
    [ResponseCache(CacheProfileName = "Monthly")]
    public IActionResult Image(string imageName)
    {
        var mime = imageName.Substring(imageName.LastIndexOf('.') + 1);
        return new FileStreamResult(_fileManager.ImageStream(imageName), $"image/{mime}");
    }

    [HttpPost]
    public async Task<IActionResult> Comment(CommentViewModel vm)
    {
        if(!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Post), new { id = vm.PostId });
        }

        var post = _repo.GetPost(vm.PostId);

        if(vm.MainCommentId == 0)
        {
            post.MainComments = post.MainComments ?? new List<MainComment>();
            post.MainComments.Add(new MainComment
            {
                Message = vm.Message,
                Created = DateTime.Now
            });

            _repo.UpdatePost(post);
        }
        else
        {
            var comment = new SubComment
            {
                MainCommentId = vm.MainCommentId,
                Message = vm.Message,
                Created = DateTime.Now
            };

            _repo.AddSubcomment(comment);

        }

        await _repo.SaveChangesAsync();

        return RedirectToAction(nameof(Post), new { id = vm.PostId });
    }
}
