namespace EFRelations.DTOs
{
    public class OneToManyDTO
    {
    }

    public class BlogDto
    {
        public int Id { get; set; } 
        public string? Title { get; set; }

        public List<PostDto>? Post { get; set; }
    }

    public class PostDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
       public int BlogId { get; set; }
    }
}
