using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogSystem.Web.ViewModels
{
    using AutoMapper;

    using BlogSystem.Data.DbFirst;
    using BlogSystem.Web.Infrastructure.Mapping;
    public class TagViewModel : BaseViewModel, IMapFrom<PortfolioTag>
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}