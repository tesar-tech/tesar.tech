using BlazorStatic;
using TesarTechWebsite.Components;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddBlazorStaticService(opt => {
        opt.HotReloadEnabled = true;
        opt.IgnoredPathsOnContentCopy.Add("app.css");//pre-build version for tailwind

    }
    ).AddBlazorStaticContentService<BlogFrontMatter>()
    .AddBlazorStaticContentService<VdpFrontMatter>(
    opt => {
        opt.ContentPath = "Content/Video-Detail-Player";
        opt.PageUrl = "video-detail-player";
        opt.AddTagPagesFromPosts = false;
        opt.MediaFolderRelativeToContentPath = "media";
    }
    );

builder.Services.AddRazorComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.UseBlazorStaticGenerator(shutdownApp: !app.Environment.IsDevelopment());

app.Run();

public static class WebsiteKeys
{
    public const string GitHubMe = "https://github.com/tesar-tech/";
    public const string GitHubRepo = $"{GitHubMe}tesar-tech.github.io";
    public const string X = "https://x.com/";
    public const string Title = "Jan Tesař";
    public const string BlogPostStorageAddress = $"{GitHubRepo}/tree/main/Content/Blog";
    public const string BlogLead = "High performance in sport and programming";
}

public class VdpFrontMatter : IFrontMatter
{
    public List<string> Tags { get; set; } = [];
    public string Title { get; set; } = "";
    public string SuccessMessage { get; set; } = "";
    public string EmailText { get; set; } = "";
    public string MessageText { get; set; } = "";
    public string LegendText { get; set; } = "";
    public string SendText { get; set; } = "";
}
