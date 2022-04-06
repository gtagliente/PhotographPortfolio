
namespace BlogSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BlogSystem.Data.Contracts.Models;

    public class Pic:  BaseModel<int>
    {
        //[Required]
        public string Path { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string SrcBase64 { get; set; }

        public string Name { get; set; }

        public int Ordinal { get; set; }

        [Required]
        public new string Id { get; set; }
    }
}
