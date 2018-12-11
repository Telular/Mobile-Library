using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Telular.Tracking
{
    public enum ExceptionType
    {
        [Description("Display Only")]
        DisplayOnly = 1,
        [Description("Log Only")]
        LogOnly = 2,
        [Description("Display And Log")]
        DisplayAndLog =3
    }
    public enum ErrorType
    {
        [Description("Service Error")]
        ServiceError = 1,
        [Description("Connectivity Error")]
        ConnectivityError = 2
    }
    public class ExceptionHandler : IExceptionHandler
    {
        private Action<string, string, string> MessageAction;
        private const string DefaultDialogTitle = "System Error";
        private const string DefaultButtonText = "System Error";

        public ExceptionHandler(Action<string, string, string> messageAction)
        {
            this.MessageAction = messageAction;
        }

        public async Task HandleException(string eventName, ExceptionType type, Exception ex, string dialogTile = null, string userMessage = null, string buttonText = "Ok")
        {
            switch (type)
            {
                case ExceptionType.DisplayAndLog:
                    DisplayUserMessage(dialogTile, userMessage, buttonText);
                    LogException(eventName, userMessage);
                    break;
                case ExceptionType.DisplayOnly:
                    DisplayUserMessage(dialogTile, userMessage, buttonText);
                    break;
                case ExceptionType.LogOnly:
                    LogException(eventName, userMessage);
                    break;
            }
        }

        public async Task DisplayErrorCondition(ErrorType type, string errorMessage, string dialogTile = null, string buttonText = "Ok")
        {
            string title = "";
            if (dialogTile == null)
            {
                switch (type)
                {
                    case ErrorType.ConnectivityError:
                        title = "Cannot Connect to service";
                        break;
                    case ErrorType.ServiceError:
                        title = "Error with Service";
                        break;
                }
            }
            else
                title = dialogTile;
            DisplayUserMessage(title, errorMessage, buttonText);
        }

        /// <summary>
        /// Wrapper to Log Exception.  Want this displayed as logging an analytic event instead of an exception
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventDetails"></param>
        public void LogAnalyticEvent(string eventName,string eventDetails)
        {
            LogException(eventName, eventDetails);
        }

        #region Private Methods
        private void DisplayUserMessage(string title, string message, string buttonText)
        {
            MessageAction(title, message, buttonText);
        }

        private void LogException(string eventName, Exception ex)
        {
            Analytics.TrackEvent(ex.Message);
        }

        private void LogException(string eventName, string message)
        {
            Dictionary<string, string> displayProperties = new Dictionary<string, string>();
            displayProperties.Add(eventName, message);
            Analytics.TrackEvent(eventName, displayProperties);
        }
        #endregion
    }
}
