using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminControllerBase : ControllerBase
    {
    }
}
