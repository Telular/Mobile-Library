using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telular.Tracking
{
    public interface IExceptionHandler
    {
        Task HandleException(string eventName, ExceptionType type, Exception ex, string dialogTile = null, string userMessage = null, string buttonText = "Ok");
        Task DisplayErrorCondition(ErrorType type, string errorMessage, string dialogTile = null, string buttonText = "Ok");
        void LogAnalyticEvent(string eventName, string eventDetails);
    }
}
