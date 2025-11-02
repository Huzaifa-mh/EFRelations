namespace EFRelations.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }

        //we have to make the navigation object as nullabe  
        public Profile? Profile { get; set; }

    }
}
