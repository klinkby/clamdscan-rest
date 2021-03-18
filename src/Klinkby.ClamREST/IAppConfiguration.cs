using System.Net;

namespace Klinkby.ClamREST
{
    public interface IAppConfiguration
    {
        string Host { get; set; }
        IPAddress[] IPAdresses { get; set; }
        long MaxFileSize { get; set; }
        int Port { get; set; }
    }
}