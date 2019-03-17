namespace BalticAmadeusTask.Models
{
    public class UserFiltering
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int MaxElements { get; set; } = 50;
    }
}
