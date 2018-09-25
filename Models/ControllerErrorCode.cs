using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Models
{
    public enum ControllerErrorCode
    {
        OK,
        NotConfirmed,
        UserNotFound,
        ProjectNotFound,
        WrongInputData,
        PermissionsDenied
    }
}
