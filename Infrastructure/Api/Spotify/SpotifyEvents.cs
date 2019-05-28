using System;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyApi
{
    public class SpotifyEvents
    {
        private const string SPOTIFY_HOST = "https://api.spotify.com/v1";
        private static readonly HttpClient client = new HttpClient();
        public SpotifyEvents(string token)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IDictionary<string,string>> GetAlbumsPerGenre(string genre)
        {
            string artist = string.Empty;
            IDictionary<string,string> albums = new Dictionary<string,string>();

            string Url = SPOTIFY_HOST + "/search?" + "q=genre:" + genre + "&type=artist&limit=50";
            var result = await client.GetAsync(Url);
            
            var jsonObject = result.Content.ReadAsStringAsync().Result;
            var artistsIds = this.GetIdsFormJson("(id\\\" : (\\\"[^\"]+\\\"),)", jsonObject);

            foreach(string str in artistsIds)
            {
                Url = SPOTIFY_HOST + "/artists/" + str + "/albums?include_groups=album";
                result = await client.GetAsync(Url);

                jsonObject = result.Content.ReadAsStringAsync().Result;
                var AlbumsNames = this.GetNamesFormJson("(name\\\" : (\\\"[^\"]+\\\"),)", jsonObject);

                artist = string.Empty;
                for (int i=0; i < AlbumsNames.Count(); i++)
                {
                    // this is because it get the name of artist for earch album.
                    if ((i+1)%2 == 0)
                    {
                        if (!albums.Keys.Contains(AlbumsNames.ElementAt(i)))
                        {
                            albums.Add(AlbumsNames.ElementAt(i),artist);
                        }
                    }
                    else if (artist == string.Empty)
                    {
                        artist = AlbumsNames.ElementAt(i);
                    }
                }

                //the only propose of this is save memory.
                if (albums.Count >= 50)
                    break;
            }

            return albums;
        }

        private IEnumerable<string> GetIdsFormJson(string patter, string jsonObject)
        {
            var Ids = Regex.Matches(jsonObject,patter);
            return Ids.Select(x=>x.Groups[2].Value.Replace("\"",string.Empty));
        }

        private IEnumerable<string> GetNamesFormJson(string patter, string jsonObject)
        {
            var Ids = Regex.Matches(jsonObject,patter);
            return Ids.Select(x=>x.Groups[2].Value.Replace("\"",string.Empty));
        }
    }
}