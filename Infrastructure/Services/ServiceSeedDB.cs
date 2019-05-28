using System;
using System.Collections.Generic;
using Domain.Model;
using SpotifyApi;

public class ServiceSeedDB
{
    public static List<CashBack> GetCashBack()
    {
        /*
         Gênero Domingo Segunda Terça Quarta Quinta Sexta Sábado
         POP     25%     7%      6%    2%      10%   15%    20%
         MPB     30%     5%      10%   15%     20%   25%    30%
         CLASSIC 35%     3%      5%    8%      13%   18%    25%
         ROCK    40%     10%     15%   15%     15%   20%    40%
         */
        int CountPk = 1;
        List<CashBack> cbList = new List<CashBack>();

        // POP
        cbList.AddRange(new List<CashBack> {
            new CashBack { CashBackId = CountPk++, Weekday=0, Genre="POP", value=25 },
            new CashBack { CashBackId = CountPk++, Weekday=1, Genre="POP", value=7 },
            new CashBack { CashBackId = CountPk++, Weekday=2, Genre="POP", value=6 },
            new CashBack { CashBackId = CountPk++, Weekday=3, Genre="POP", value=2 },
            new CashBack { CashBackId = CountPk++, Weekday=4, Genre="POP", value=10 },
            new CashBack { CashBackId = CountPk++, Weekday=5, Genre="POP", value=15 },
            new CashBack { CashBackId = CountPk++, Weekday=6, Genre="POP", value=20 }
            });

        // MPB
        cbList.AddRange(new List<CashBack> {
            new CashBack { CashBackId = CountPk++, Weekday=0, Genre="MPB", value=30 },
            new CashBack { CashBackId = CountPk++, Weekday=1, Genre="MPB", value=5 },
            new CashBack { CashBackId = CountPk++, Weekday=2, Genre="MPB", value=10 },
            new CashBack { CashBackId = CountPk++, Weekday=3, Genre="MPB", value=15 },
            new CashBack { CashBackId = CountPk++, Weekday=4, Genre="MPB", value=20 },
            new CashBack { CashBackId = CountPk++, Weekday=5, Genre="MPB", value=25 },
            new CashBack { CashBackId = CountPk++, Weekday=6, Genre="MPB", value=30 }
            });

        // CLASSIC
        cbList.AddRange(new List<CashBack> {
            new CashBack { CashBackId = CountPk++, Weekday=0, Genre="CLASSIC", value=35 },
            new CashBack { CashBackId = CountPk++, Weekday=1, Genre="CLASSIC", value=3 },
            new CashBack { CashBackId = CountPk++, Weekday=2, Genre="CLASSIC", value=5 },
            new CashBack { CashBackId = CountPk++, Weekday=3, Genre="CLASSIC", value=8 },
            new CashBack { CashBackId = CountPk++, Weekday=4, Genre="CLASSIC", value=13 },
            new CashBack { CashBackId = CountPk++, Weekday=5, Genre="CLASSIC", value=18 },
            new CashBack { CashBackId = CountPk++, Weekday=6, Genre="CLASSIC", value=25 }
            });

        // ROCK
        cbList.AddRange(new List<CashBack> {
            new CashBack { CashBackId = CountPk++, Weekday=0, Genre="ROCK", value=40 },
            new CashBack { CashBackId = CountPk++, Weekday=1, Genre="ROCK", value=10 },
            new CashBack { CashBackId = CountPk++, Weekday=2, Genre="ROCK", value=15 },
            new CashBack { CashBackId = CountPk++, Weekday=3, Genre="ROCK", value=15 },
            new CashBack { CashBackId = CountPk++, Weekday=4, Genre="ROCK", value=15 },
            new CashBack { CashBackId = CountPk++, Weekday=5, Genre="ROCK", value=20 },
            new CashBack { CashBackId = CountPk++, Weekday=6, Genre="ROCK", value=40 }
            });

            return cbList;

    }
    public static IEnumerable<Albums> GetAlbums()
    {
        int CountPk = 1;
        Random random = new Random(30);
        List<Albums> AlbumsEntity = new List<Albums>();

        SpotifyConnection sApi = new SpotifyConnection();
        SpotifyEvents sEvents = new SpotifyEvents(sApi.GetToken().Result);

        IDictionary<string, string> SpotifyAlbunsPOP = sEvents.GetAlbumsPerGenre("POP").Result;

        foreach (string key in SpotifyAlbunsPOP.Keys)
        {
            AlbumsEntity.Add(new Albums { AlbumsId = CountPk, Name = key, Artist = SpotifyAlbunsPOP[key], Genre = "POP", Price = (decimal)random.Next(15,80)});
            CountPk++;
        }


        IDictionary<string, string> SpotifyAlbunsMPB = sEvents.GetAlbumsPerGenre("MPB").Result;

        foreach (string key in SpotifyAlbunsMPB.Keys)
        {
            AlbumsEntity.Add(new Albums { AlbumsId = CountPk, Name = key, Artist = SpotifyAlbunsMPB[key], Genre = "MPB", Price = (decimal)random.Next(15,80) });
            CountPk++;
        }

        IDictionary<string, string> SpotifyAlbunsCLASSIC = sEvents.GetAlbumsPerGenre("CLASSIC").Result;

        foreach (string key in SpotifyAlbunsCLASSIC.Keys)
        {
            AlbumsEntity.Add(new Albums { AlbumsId = CountPk, Name = key, Artist = SpotifyAlbunsCLASSIC[key], Genre = "CLASSIC", Price = (decimal)random.Next(15,80) });
            CountPk++;
        }

        IDictionary<string, string> SpotifyAlbunsROCK = sEvents.GetAlbumsPerGenre("ROCK").Result;
        foreach (string key in SpotifyAlbunsROCK.Keys)
        {
            AlbumsEntity.Add(new Albums { AlbumsId = CountPk, Name = key, Artist = SpotifyAlbunsROCK[key], Genre = "ROCK", Price = (decimal)random.Next(15,80) });
            CountPk++;
        }

        return AlbumsEntity;
    }
}