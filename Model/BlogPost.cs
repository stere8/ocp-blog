using System.ComponentModel.DataAnnotations;

namespace ocp_blog.Model
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string GitHubUrl { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
    }
}
