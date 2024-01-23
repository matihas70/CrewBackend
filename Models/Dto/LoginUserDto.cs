namespace CrewBackend.Models
{
    public record LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool DontLogOut { get; set; }
    }
}
