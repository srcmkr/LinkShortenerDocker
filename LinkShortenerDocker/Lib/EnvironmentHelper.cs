using System;

namespace LinkShortenerDocker.Lib
{
    public static class EnvironmentHelpers
    {
        /// <summary>
        /// Reads all three kinds of environment variables in the order Process (first) => User => Machine (last)
        /// </summary>
        /// <param name="varName">Name of the environment variable</param>
        /// <returns>If evar exists, returns it, else returns empty string</returns>
        public static string GetEnvironmentVariable(string varName)
        {
            varName = varName.ToUpper();

            var processVar = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Process);
            if (!string.IsNullOrEmpty(processVar)) return processVar;

            var userVar = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.User);
            if (!string.IsNullOrEmpty(userVar)) return userVar;

            var machineVar = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrEmpty(machineVar)) return machineVar;

            return string.Empty;
        }
    }
}
