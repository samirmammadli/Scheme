using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Models
{
    public enum ControllerErrorCode
    {
        AccountOrPasswordWrong,
        AccountNotFound,
        NotConfirmed,
        UserNotFound,
        AlreadyConfirmed,
        ProjectNotFound,
        WrongInputData,
        PermissionsDenied,
        WrongRegCode,
        ExpiredCode,
        EmailAlreadyExists,
        SprintNotFound,
        ColumnNotFound, 
        CardNotFound
    }
}
