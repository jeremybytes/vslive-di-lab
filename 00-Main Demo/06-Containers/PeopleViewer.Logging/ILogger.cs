using System;
using System.Threading.Tasks;

namespace PeopleViewer.Logging
{
    public interface ILogger
    {
        Task LogException(Exception ex);
        Task LogMessage(string message);
    }
}