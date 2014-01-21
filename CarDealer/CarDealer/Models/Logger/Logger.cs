using System;

namespace CarDealerProject.Models.Logger
{
    public class Logger
    {
        public static string LogError(string messagePrefix, Exception ex)
        {
            //TODO LOG codes for easy log
            //TODO Logging with log4net or other
            var result = ReplaceTroubleMakerChars(messagePrefix + ex.Message);
            return result;
        }

        public static string LogWarning(string messagePrefix)
        {
            //TODO Logging with log4net or other
            var result = ReplaceTroubleMakerChars(messagePrefix);
            return messagePrefix;
        }

        private static string ReplaceTroubleMakerChars(string input)
        {
            return input.Replace("\n", "  ").Replace("\r", "  ");
        }
    }
}