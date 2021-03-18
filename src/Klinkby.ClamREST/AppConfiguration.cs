using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net;

namespace Klinkby.ClamREST
{
    internal class AppConfiguration : IAppConfiguration
    {
        public string Host { get; set; }
        public IPAddress[] IPAdresses { get; set; }
        public int Port { get; set; }
        public long MaxFileSize { get; set; }

        internal static AppConfiguration FromConfiguration(IConfiguration c)
        {
            return new AppConfiguration
            {
                Host = c.GetValue<string>("HOST") ?? "localhost",
                IPAdresses = (
                    from s in (c.GetValue<string>("IPADDRESSES") ?? "").Split(',')
                    where !string.IsNullOrWhiteSpace(s)
                    select IPAddress.Parse(s)
                    ).ToArray(),
                Port = c.GetValue<int?>("PORT") ?? 3310,
                MaxFileSize = c.GetValue<long?>("MAXFILESIZE") ?? 26214400 // 25 MB default
            };
        }
    }
}