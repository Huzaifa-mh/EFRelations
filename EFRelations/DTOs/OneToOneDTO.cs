namespace EFRelations.DTOs
{
    public class OneToOneDTO
    {
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }

        //nested dto
        public ProfileDto Profile { get; set; }
    }

    public class ProfileDto
    {
        public int Id { get; set; }
        public string? Bio { get; set; }

        public string? Username { get; set; }

    }
}
