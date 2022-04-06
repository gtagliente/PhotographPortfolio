namespace BlogSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using BlogSystem.Web.ViewModels;
    using BlogSystem.Web.ViewModels.Pic;

    public class IndexPageViewModel : PaginationViewModel
    {
        //public IEnumerable<PostConciseViewModel> Posts { get; set; }
        public IEnumerable<PicViewModel> Pics { get; set; }
        public IEnumerable<TagViewModel> Tags { get; set; }
    }
}