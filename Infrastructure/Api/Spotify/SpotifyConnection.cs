using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace SpotifyApi
{
    public class SpotifyConnection
    {
        private string ClientID = string.Empty;
        private string ClientSecret = string.Empty;
        private string SpotifyToken = string.Empty;
        private const string SPOTIFY_AUTH = "https://accounts.spotify.com/api/token";
        private static readonly HttpClient client = new HttpClient();

        public SpotifyConnection()
        {
            this.ClientID = Environment.GetEnvironmentVariable("MySpotifyClientID");
            this.ClientSecret = Environment.GetEnvironmentVariable("MySpotifyClientSecret");
        }

        public async Task<string> GetToken()
        {
            byte[] AccessKeys = Encoding.UTF8.GetBytes(this.ClientID + ":" + this.ClientSecret);

            // Headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(AccessKeys));

            // Form body
            List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
            requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

            // Call
            var response = await client.PostAsync(SPOTIFY_AUTH, requestBody);
            var tokenData = await response.Content.ReadAsStringAsync();
            tokenData = tokenData.Substring(tokenData.IndexOf(":\"") + 2);
            tokenData = tokenData.Substring(0, tokenData.IndexOf(",\"") - 1);
            return tokenData;
        }
    }
}