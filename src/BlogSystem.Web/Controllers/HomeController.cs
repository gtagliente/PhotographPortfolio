namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using BlogSystem.Data.DbFirst;
    using BlogSystem.Common;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.ViewModels;
    using BlogSystem.Web.ViewModels.Home;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;
    using System.Threading;
    using Google.Apis.Util.Store;
    using BlogSystem.Web.ViewModels.Pic;
    using Microsoft.Ajax.Utilities;

    public class HomeController : BaseController
    {
        //private readonly IDbRepository<Post> postsData;

        //public HomeController(IDbRepository<Post> postsData)
        //{
        //    this.postsData = postsData;
        //}



        private List<Pic> picsData;
        //private List<TagViewModel> tags;
        private List<PortfolioTag> tags;
        static string[] Scopes = {  DriveService.Scope.Drive, DriveService.Scope.DriveFile, DriveService.Scope.DriveAppdata, DriveService.Scope.DriveScripts, DriveService.Scope.DriveMetadata};
        //static string[] Scopes = { DriveService.Scope.DriveReadonly };

        static string ApplicationName = "Drive API .NET Quickstart";
        
        public  HomeController()
        {
            ModelState.Clear();
            PortfolioContext ctx = new PortfolioContext();
            var DbSetPortfolioImages = ctx.db.PortfolioImages;
            this.picsData = new List<Pic>();

            this.tags = ctx.db.PortfolioTags.ToList();
            //{ 
            //    new Pic { Path = "images/1-nature.jpg", Category = "nature" }
            //    , new Pic { Path = "images/10-people.jpg", Category = "people" }
            //    , new Pic { Path = "images/26-food.jpg", Category = "food" }
            //    , new Pic { Path = "images/18-computer.jpg", Category = "computer" }
            //    };
            DriveService service = GetService();

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";
            listRequest.Q = "parents in '18UA4HtxDyedVxvA_lOheE1FASCwY0MsA'";


            // List files.
            IList<Google.Apis.Drive.v3.Data.File> driveFiles = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            if (driveFiles != null && driveFiles.Count > 0)
            {
                foreach (var file in driveFiles)
                {
                    String fileId = file.Id;
                    MemoryStream outputStream = new MemoryStream();
                    service.Files.Get(fileId).Download(outputStream);
                    string base64content = Convert.ToBase64String(outputStream.ToArray());

                    picsData.Add(
                        new Pic
                        {
                            Name = DbSetPortfolioImages.Where(x => x.id == fileId).Select(x => x.filename).FirstOrDefault() ?? ""
                            ,
                            Category = DbSetPortfolioImages.Where(x => x.id == fileId).Select(x => x.category).FirstOrDefault() ?? ""
                            ,
                            SrcBase64 = "data:image/jpeg;base64," + base64content
                            ,
                            Ordinal = DbSetPortfolioImages.Where(x => x.id == fileId).Select(x => x.ordinal).FirstOrDefault() ?? 1000
                            ,
                            Id = file.Id
                        }); ;
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
                picsData.OrderBy(p => p.Ordinal);
            }
            else
            {
                Console.WriteLine("No files found.");
            }

        }



        private static DriveService GetService()
        {
            UserCredential credential;

            using (var stream =
                new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "credentials.json"), FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "token.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }

        //public ActionResult _Index(int page = 1, int perPage = GlobalConstants.DefaultPageSize)
        //{
        //    var pagesCount = (int) Math.Ceiling(this.postsData.All().Count() / (decimal) perPage);
        //    var postsPage = this.postsData.All().OrderByDescending(p => p.CreatedOn).Skip(perPage * (page - 1)).Take(perPage);
        //    var posts = this.Mapper.Map<PostConciseViewModel>(postsPage).ToList();

        //    var model = new IndexPageViewModel
        //    {
        //        Posts = posts,
        //        CurrentPage = page,
        //        PagesCount = pagesCount
        //    };

        //    return this.View(model);
        //}
        //[HttpPost]
        //public ActionResult UploadFiles(string foo,IEnumerable<HttpPostedFileBase> files)
        //{
        //    //HttpPostedFileBase file = Request.Files[0];

        //    //MemoryStream target = new MemoryStream();
        //    //file.InputStream.CopyTo(target);
        //    //byte[] data = target.ToArray();

        //    //System.IO.File.WriteAllBytes(Path.Combine("E:\\Test",file.FileName), data);
        //    PortfolioContext ctx = new PortfolioContext();
        //    ctx.AddImage();
        //    return Json("All files have been successfully stored.");
        //    //return RedirectToAction("Index", "Home");
        //}

        [HttpPost]
        public JsonResult GetTags()
        {
            return Json(tags.Select(t => t.value).ToList());
        }

        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult UploadFiles(PicPostModel post)
        {
            ModelState.Clear();
            
            PortfolioContext ctx = new PortfolioContext();
            var transaction = ctx.db.Database.BeginTransaction();
            try { 
                string id = UploadFileOnDrive(post.files.First()?.InputStream, post.files.First()?.FileName, post.files.First()?.ContentType, "18UA4HtxDyedVxvA_lOheE1FASCwY0MsA", post.Category);
                post.Id = id;
                ctx.AddImage(post);
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                transaction.Dispose();
            }
            return Json("All files have been successfully stored.");
        }

        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult UploadOrdinals(List<PicPostModel> dotnetList)
        {
            ModelState.Clear();

            PortfolioContext ctx = new PortfolioContext();
            var transaction = ctx.db.Database.BeginTransaction();
            try
            {
                ctx.UpdateOrdinals(dotnetList);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                transaction.Dispose();
            }

            return Json("All files have been successfully stored.");
        }


        public string UploadFileOnDrive(Stream file, string fileName, string fileMime, string folder, string fileDescription)
        {
            DriveService service = GetService();


            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = fileName;
            driveFile.Description = fileDescription;
            driveFile.MimeType = fileMime;
            driveFile.Parents = new string[] { folder };


            var request = service.Files.Create(driveFile, file, fileMime);
            request.Fields = "id";

            var response = request.Upload();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                throw response.Exception;

            return request.ResponseBody.Id;
        }


        //[HttpPost]
        //public ActionResult UploadFiles(PicPostModel pics)
        //{
        //    //HttpPostedFileBase file = Request.Files[0];

        //    //MemoryStream target = new MemoryStream();
        //    //file.InputStream.CopyTo(target);
        //    //byte[] data = target.ToArray();

        //    //System.IO.File.WriteAllBytes(Path.Combine("E:\\Test",file.FileName), data);
        //    PortfolioContext ctx = new PortfolioContext();
        //    ctx.AddImage();
        //    return Json("All files have been successfully stored.");
        //    //return RedirectToAction("Index", "Home");
        //}

         [HttpGet]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Index(int page = 1, int perPage = GlobalConstants.DefaultPageSize)
        {
            ModelState.Clear();
            //var pagesCount = (int)Math.Ceiling(this.postsData.All().Count() / (decimal)perPage);
            //var postsPage = this.postsData.All().OrderByDescending(p => p.CreatedOn).Skip(perPage * (page - 1)).Take(perPage);
            var picsPage = picsData.AsQueryable();
            var pics = this.Mapper.Map<BlogSystem.Web.ViewModels.Pic.PicViewModel>(picsPage).OrderBy(p => p.Ordinal).ToList();
            var viewtags = this.Mapper.Map<BlogSystem.Web.ViewModels.TagViewModel>(this.tags.AsQueryable()).ToList();
            var model = new IndexPageViewModel
            {
                Pics = pics,
                Tags = viewtags,
                CurrentPage = page,
                PagesCount = 1
            };
           
            return View(model);
        }
    }
}
