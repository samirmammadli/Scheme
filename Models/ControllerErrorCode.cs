using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Models
{
    public enum ControllerErrorCode
    {
        NotConfirmed,
        UserNotFound,
        AlradyConfirmed,
        ProjectNotFound,
        WrongInputData,
        PermissionsDenied
    }
}
