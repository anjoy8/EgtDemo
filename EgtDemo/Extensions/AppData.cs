using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgtDemo.Extensions
{

    public class AppData
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public Appset AppSet { get; set; }
        public string MainDB { get; set; }
        public bool MutiDBEnabled { get; set; }
        public bool CQRSEnabled { get; set; }
        public DB[] DBS { get; set; }
        public JWT JWT { get; set; }
        public Startup Startup { get; set; }
        public Middleware Middleware { get; set; }
        public Ipratelimiting IpRateLimiting { get; set; }
    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public Debug Debug { get; set; }
        public Console Console { get; set; }
        public Log4net Log4Net { get; set; }
    }

    public class Debug
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
    }

    public class Console
    {
        public Loglevel1 LogLevel { get; set; }
    }

    public class Loglevel1
    {
        public string Default { get; set; }
    }

    public class Log4net
    {
        public string Name { get; set; }
    }

    public class Appset
    {
        public Rediscachingaop RedisCachingAOP { get; set; }
        public Memorycachingaop MemoryCachingAOP { get; set; }
        public Logaop LogAOP { get; set; }
        public Tranaop TranAOP { get; set; }
        public Sqlaop SqlAOP { get; set; }
        public string Date { get; set; }
        public bool SeedDBEnabled { get; set; }
        public bool SeedDBDataEnabled { get; set; }
        public string Author { get; set; }
    }

    public class Rediscachingaop
    {
        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
    }

    public class Memorycachingaop
    {
        public bool Enabled { get; set; }
    }

    public class Logaop
    {
        public bool Enabled { get; set; }
    }

    public class Tranaop
    {
        public bool Enabled { get; set; }
    }

    public class Sqlaop
    {
        public bool Enabled { get; set; }
    }

    public class JWT
    {
        public string Secret { get; set; }
        public string SecretFile { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

    public class Startup
    {
        public Cors Cors { get; set; }
        public Appconfigalert AppConfigAlert { get; set; }
        public string ApiName { get; set; }
        public Identityserver4 IdentityServer4 { get; set; }
    }

    public class Cors
    {
        public string IPs { get; set; }
    }

    public class Appconfigalert
    {
        public bool Enabled { get; set; }
    }

    public class Identityserver4
    {
        public bool Enabled { get; set; }
        public string AuthorizationUrl { get; set; }
        public string ApiName { get; set; }
    }

    public class Middleware
    {
        public Requestresponselog RequestResponseLog { get; set; }
        public Iplog IPLog { get; set; }
        public Recordalllogs RecordAllLogs { get; set; }
        public Signalr SignalR { get; set; }
    }

    public class Requestresponselog
    {
        public bool Enabled { get; set; }
    }

    public class Iplog
    {
        public bool Enabled { get; set; }
    }

    public class Recordalllogs
    {
        public bool Enabled { get; set; }
    }

    public class Signalr
    {
        public bool Enabled { get; set; }
    }

    public class Ipratelimiting
    {
        public bool EnableEndpointRateLimiting { get; set; }
        public bool StackBlockedRequests { get; set; }
        public string RealIpHeader { get; set; }
        public string ClientIdHeader { get; set; }
        public object[] IpWhitelist { get; set; }
        public string[] EndpointWhitelist { get; set; }
        public string[] ClientWhitelist { get; set; }
        public int HttpStatusCode { get; set; }
        public Generalrule[] GeneralRules { get; set; }
    }

    public class Generalrule
    {
        public string Endpoint { get; set; }
        public string Period { get; set; }
        public int Limit { get; set; }
    }

    public class DB
    {
        public string ConnId { get; set; }
        public int DBType { get; set; }
        public bool Enabled { get; set; }
        public int HitRate { get; set; }
        public string Connection { get; set; }
        public string ProviderName { get; set; }
        public string OracleConnection_other1 { get; set; }
    }

}
