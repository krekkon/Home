using System;

namespace CarDealerProject.Models.Logger
{
    public class Logger
    {
        public static string LogError(string messagePrefix, Exception ex)
        {
            //TODO Logging with log4net or other
            return messagePrefix + ex.Message;
        }
    }
}