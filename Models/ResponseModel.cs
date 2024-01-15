using CrewBackend.Enums;
namespace CrewBackend.Models
{
    public class ResponseModel<T>
    {
        public T ResponseData { get; set; }
        public StatusEnum Status {  get; set; }
        public string Message { get; set; }
    }
}
