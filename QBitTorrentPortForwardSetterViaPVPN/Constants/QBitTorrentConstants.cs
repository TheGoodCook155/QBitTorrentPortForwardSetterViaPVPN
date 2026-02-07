
using static System.Net.WebRequestMethods;

namespace QBitTorrentPortForwardSetterViaPVPN.Constants
{
    public static class QBitTorrentConstants
    {
        public static readonly string LoginEndpoint = "http://localhost:8080/api/v2/auth/login";

        public static readonly string SetPreferencesEndpoint = "http://localhost:8080/api/v2/app/setPreferences";
    }
}
