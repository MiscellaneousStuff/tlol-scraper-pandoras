using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Flurl;
using Flurl.Http;

namespace LCU;

public class LcuClient
{
    private int _riotPort;
    private string _riotToken;
    
    private int _clientPort;
    private string _clientToken;

    public LcuClient(int riotPort, string riotToken, int clientPort, string clientToken)
    {
        _riotPort = riotPort;
        _riotToken = riotToken;
        _clientPort = clientPort;
        _clientToken = clientToken;
    }

    public string GetRegion()
    {
        var result = GetRequest(_riotPort, _riotToken, "/riotclient/region-locale") as IDictionary<string, object>;
        return result["region"] as string;
    }

    public LcuLobbyPlayers GetLobbyPlayers()
    {
        return GetRequest<LcuLobbyPlayers>(_riotPort, _riotToken, "/chat/v5/participants/champ-select");
    }

    public void SendChat(string cid, string message)
    {
        var url = $"/lol-chat/v1/conversations/{cid}/messages";
        var request = new {type = "chat", body = message};
        GeBaseRequest(_clientPort, _clientToken, url)
            .PostJsonAsync(request)
            .GetAwaiter()
            .GetResult();
    }

    private dynamic GetRequest(int port, string token, string path)
    {
        return GeBaseRequest(port, token, path)
            .GetJsonAsync()
            .Result;
    }
    
    private T GetRequest<T>(int port, string token, string path)
    {
        return GeBaseRequest(port, token, path)
            .GetJsonAsync<T>()
            .Result;
    }

    private IFlurlRequest GeBaseRequest(int port, string token, string path)
    {
        var url = new Url
        {
            Scheme = "https",
            Host = "127.0.0.1", 
            Port = port,
            Path = path
        };

        return url.WithHeader("Authorization", $"Basic {token}")
            .WithClient(new FlurlClient(new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain,
                    errors) => true
            })));
    }

    public static LcuClient? Create()
    {
        var process = GetProcessByName("LeagueClientUx");
        if (process is null)
        {
            return null;
        }

        var commandLine = GetCommandLine(process);

        if (string.IsNullOrWhiteSpace(commandLine))
        {
            return null;
        }

        var riotPortStr = GetString(commandLine, "--riotclient-app-port=", "\" \"--no-rads");
        var riotToken = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
            .GetBytes("riot:" + GetString(commandLine, "--riotclient-auth-token=", "\" \"--riotclient")));

        var clientPortStr = GetString(commandLine, "--app-port=", "\" \"--install");
        var clientToken = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
            .GetBytes("riot:" + GetString(commandLine, "--remoting-auth-token=", "\" \"--respawn-command=LeagueClient.exe")));

        if (!int.TryParse(riotPortStr, out var riotPort) || !int.TryParse(clientPortStr, out var clientPort))
        {
            return null;
        }

        return new LcuClient(riotPort, riotToken, clientPort, clientToken);
    }
    
    private static string GetString(string text, string from, string to)
    {
        string pattern = Regex.Escape(from) + "(.*?)" + Regex.Escape(to);
        Match match = Regex.Match(text, pattern);

        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return string.Empty;
    }
    
    static Process? GetProcessByName(string processName)
    {
        return  Process.GetProcesses().FirstOrDefault(process => process.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase));
    }
    
    public static bool IsSupported()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }
    
    static string? GetCommandLine(Process process)
    {
        if (!IsSupported())
        {
            Console.WriteLine("Windows is only supported");
            return string.Empty;
        }
        
#pragma warning disable CA1416
        using var searcher = new ManagementObjectSearcher(
            $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}");
        using var collection = searcher.Get();
        foreach (var obj in collection)
        {
            return obj["CommandLine"].ToString();
        }
#pragma warning restore CA1416
        
        return string.Empty;
    }
}