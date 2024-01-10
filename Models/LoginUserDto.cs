namespace CrewBackend.Models
{
    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool DontLogOut { get; set; }
    }
}
