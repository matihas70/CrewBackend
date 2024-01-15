using CrewBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrewBackend.Interfaces
{
    public interface IResponseService
    {
        IActionResult SendResponse<T>(ResponseModel<T> response);
    }
}
