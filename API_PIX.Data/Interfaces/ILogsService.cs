using API_PIX.Domain.Error;
using API_PIX.Domain.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_PIX.Data.Interfaces
{
    public interface ILogsService
    {
        HandledException HandleException(Exception ex, string errorTitle, string errorMessage, string origin);
        Log HandleLog(Log l);
    }
}
