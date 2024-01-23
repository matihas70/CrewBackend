namespace CrewBackend.Models
{
    public record LoginOutput
    {
        public Guid guid { get; set; }
        public string token { get; set; }
    }
}
