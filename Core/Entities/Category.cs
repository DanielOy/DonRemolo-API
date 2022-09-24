using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool ShowInHome { get; set; }
        
        [ForeignKey("ParentId")]
        public virtual Category ParentCategory { get; set; }
        public int? ParentId { get; set; }
    }
}
