using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppleAcademy.Api.Controllers;

public class BaseController : ControllerBase
{
    protected string UserId => User.FindFirstValue("guid") ?? string.Empty;
}
