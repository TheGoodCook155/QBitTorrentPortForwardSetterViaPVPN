
using static System.Net.WebRequestMethods;

namespace QBitTorrentPortForwardSetterViaPVPN.Constants
{
    public static class QBitTorrentConstants
    {
        public static readonly string LoginEndpoint = "/api/v2/auth/login";

        public static readonly string SetPreferencesEndpoint = "/api/v2/app/setPreferences";

        public static readonly string BaseAddress = "http://localhost:8080";
    }
}
