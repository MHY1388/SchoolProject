
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.Services;

namespace WebLayer.Pages.Shared.Components;


public class SliderViewComponent : ViewComponent
{
    private readonly UnitOfWork db;

    public SliderViewComponent(UnitOfWork context) => db = context;

    public async Task<IViewComponentResult> InvokeAsync(List<PostDto> items)
    {
        var data = GetItemsAsync(items);
        return View(data);
    }

    private List<PostDto> GetItemsAsync(List<PostDto> items)
    {
        return items.Where(a=>a.IsSpecial && !a.IsDeleted).OrderByDescending(a=>a.Created).Take(5).ToList();
    }
}