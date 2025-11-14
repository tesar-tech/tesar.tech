using System.Globalization;
using BlazorStatic;
using TesarTechWebsite.Components;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddBlazorStaticService(opt =>
{
  opt.IgnoredPathsOnContentCopy.Add("app.css");//pre-build version for tailwind
  opt.SiteUrl = "https://tesar.tech";
  opt.ShouldGenerateSitemap = true;
}
    )
       .AddBlazorStaticContentService<BlogFrontMatter>(opt =>
       {

         opt.AfterContentParsedAndAddedAction = (service, contentService) =>
         {
           contentService.Posts.ForEach(post =>
           {
             if (post.Url.Split('_', 2) is [var datePart, var rest]
                 && DateTime.TryParseExact(datePart, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                 DateTimeStyles.None, out var published))
             {
               post.FrontMatter.Published = published;
             }
           });
           service.Options.PagesToGenerate.ForEach(page => { });
         };
       }) //
    .AddBlazorStaticContentService<VdpFrontMatter>(
    opt =>
    {
      opt.ContentPath = "Content/Video-Detail-Player";
      opt.PageUrl = "video-detail-player";
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
  public const string GitHubRepo = $"{GitHubMe}tesar.tech";
  public const string X = "https://x.com/";
  public const string Xme = $"{X}tesar_tech";
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
