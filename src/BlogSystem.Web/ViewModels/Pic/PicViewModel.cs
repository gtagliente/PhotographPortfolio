
namespace BlogSystem.Web.ViewModels.Pic
{
    using AutoMapper;

    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure.Mapping;
    public class PicViewModel : BaseViewModel, IMapFrom<Pic>
    {
        public string Path { get; set; }
        public string Category { get; set; }

        public string SrcBase64 { get; set; }

        public string Name { get; set; }

        public int Ordinal { get; set; }

        public string Id { get; set; }
    }
}