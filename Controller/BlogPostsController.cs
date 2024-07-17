using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ocp_blog.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ocp_blog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BlogPostsController> _logger;

        public BlogPostsController(ApplicationDbContext context, ILogger<BlogPostsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/BlogPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts()
        {
            _logger.LogInformation("Getting all blog posts");
            var list = await _context.BlogPosts.ToListAsync();
            return list;
        }

        // GET: api/BlogPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetBlogPost(int id)
        {
            _logger.LogInformation("Getting blog post with id {Id}", id);
            var blogPost = await _context.BlogPosts.FindAsync(id);

            if (blogPost == null)
            {
                _logger.LogWarning("Blog post with id {Id} not found", id);
                return NotFound();
            }

            return blogPost;
        }

        // POST: api/BlogPosts
        [HttpPost]
        public async Task<IActionResult> PostBlogPost([FromBody] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.Date = DateTime.SpecifyKind(blogPost.Date, DateTimeKind.Utc);
                _context.BlogPosts.Add(blogPost);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBlogPost), new { id = blogPost.Id }, blogPost);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/BlogPosts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogPost(int id, BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                _logger.LogWarning("Blog post id {Id} does not match the route id", id);
                return BadRequest();
            }

            _logger.LogInformation("Updating blog post with id {Id}", id);
            _context.Entry(blogPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(id))
                {
                    _logger.LogWarning("Blog post with id {Id} not found during update", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Concurrency error occurred while updating blog post with id {Id}", id);
                    throw;
                }
            }

            _logger.LogInformation("Blog post with id {Id} updated successfully", id);
            return NoContent();
        }

        // DELETE: api/BlogPosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            _logger.LogInformation("Deleting blog post with id {Id}", id);
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                _logger.LogWarning("Blog post with id {Id} not found", id);
                return NotFound();
            }

            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Blog post with id {Id} deleted successfully", id);
            return NoContent();
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
    }
}