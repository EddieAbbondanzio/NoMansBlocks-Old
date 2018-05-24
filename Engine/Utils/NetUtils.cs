using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Voxelated.Utilities {
    /// <summary>
    /// Helper functions for tasks related to networking.
    /// </summary>
    public static class NetUtils {
        /// <summary>
        /// Attempts to convert a string into an IPEndPoint. In the
        /// event it fails null is returned.
        /// </summary>
        /// <param name="text">The string to attempt to parse an IP from.</param>
        /// <returns>The IPEndPoint if one was found.</returns>
        public static IPEndPoint ParseIPEndPoint(string text) {
            Uri uri;
            if (Uri.TryCreate(text, UriKind.Absolute, out uri)) {
                return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port < 0 ? 0 : uri.Port);
            }
            else if (Uri.TryCreate(String.Concat("tcp://", text), UriKind.Absolute, out uri)) {
                return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port < 0 ? 0 : uri.Port);
            }
            else if (Uri.TryCreate(String.Concat("tcp://", String.Concat("[", text, "]")), UriKind.Absolute, out uri)) {
                return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port < 0 ? 0 : uri.Port);
            }
            else
                return null;
        }
    }
}
