using DTO = QuakeLog.Domain.DTO;
using Enums = QuakeLog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuakeLog.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public static class LogMocks
    {
        public static T Moq<T>(this T obj, Action<T> action)
        {
            action(obj);

            return obj;
        }

        public static List<DTO.Games> GetListGames()
        {
            return new List<DTO.Games>()
            {
                GetGame(1)
            };
        }

        public static DTO.Games GetGame(int id)
        {
            return new DTO.Games(id)
            {
                Players = GetListPlayers(),
                Kills = GetListKillPlayers(),
                TotalKills = GetListKillPlayers().Count
            };
        }

        public static List<DTO.Player> GetListPlayers()
        {
            return new List<DTO.Player>()
            {
                GetPlayer(1, "Robson"),
                GetPlayer(3, "John"),
                GetPlayer(4),
            };
        }

        public static DTO.Player GetPlayer(int id, string name = "")
        {
            return new DTO.Player(id, name);
        }

        public static List<DTO.KillPlayer> GetListKillPlayers()
        {
            return new List<DTO.KillPlayer>() {
                GetKillPlayers(GetPlayer(1, "Robson"), GetPlayer(3, "John"), Enums.MeansOfDeath.MOD_ROCKET),
                GetKillPlayers(GetPlayer(1, "Robson"), GetPlayer(3, "John"), Enums.MeansOfDeath.MOD_ROCKET),
                GetKillPlayers(GetPlayer(3, "John"), GetPlayer(3, "John"), Enums.MeansOfDeath.MOD_ROCKET),
                GetKillPlayers(GetPlayer(1022, "Wolrd"), GetPlayer(3, "John"), Enums.MeansOfDeath.MOD_TRIGGER_HURT)
            };
        }

        public static DTO.KillPlayer GetKillPlayers(DTO.Player killer, DTO.Player killed, Enums.MeansOfDeath meansOfDeath)
        {
            return new DTO.KillPlayer(killer, killed, meansOfDeath);
        }

        public static List<DTO.KillMeans> GetListKillMeans()
        {
            return new List<DTO.KillMeans>() {
                GetKillMeans(Enums.MeansOfDeath.MOD_ROCKET, 1),
                GetKillMeans(Enums.MeansOfDeath.MOD_TRIGGER_HURT, 1)
            };
        }

        public static DTO.KillMeans GetKillMeans(Enums.MeansOfDeath meansOfDeath, int count)
        {
            return new DTO.KillMeans(meansOfDeath, count);
        }

        public static IEnumerable<string> LinesFile()
        {
            return new List<string>() {
                "  0:00 InitGame: \\sv_floodProtect\\1\\sv_maxPing\\0\\sv_minPing\\0\\sv_maxRate\\10000\\sv_minRate\\0\\sv_hostname\\Code Miner Server\\g_gametype\\0\\sv_privateClients\\2\\sv_maxclients\\16\\sv_allowDownload\\0\\dmflags\\0\\fraglimit\\20\\timelimit\\15\\g_maxGameClients\\0\\capturelimit\\8\\version\\ioq3 1.36 linux-x86_64 Apr 12 2009\\protocol\\68\\mapname\\q3dm17\\gamename\\baseq3\\g_needpass\\0",
                "  0:25 ClientConnect: 2",
                "  0:25 ClientUserinfoChanged: 2 n\\Dono da Bola\\t\\0\\model\\sarge/krusade\\hmodel\\sarge/krusade\\g_redteam\\g_blueteam\\c1\\5\\c2\\5\\hc\\95\\w\\0\\l\\0\\tt\\0\\tl\\0",
                "  0:27 ClientUserinfoChanged: 2 n\\Mocinha\\t\\0\\model\\sarge\\hmodel\\sarge\\g_redteam\\g_blueteam\\c1\\4\\c2\\5\\hc\\95\\w\\0\\l\\0\\tt\\0\\tl\\0",
                "  0:27 ClientBegin: 2",
                "  0:29 Item: 2 weapon_rocketlauncher",
                "  0:35 Item: 2 item_armor_shard",
                "  0:35 Item: 2 item_armor_shard",
                "  0:35 Item: 2 item_armor_shard",
                "  0:35 Item: 2 item_armor_combat",
                "  0:38 Item: 2 item_armor_shard",
                "  0:38 Item: 2 item_armor_shard",
                "  0:38 Item: 2 item_armor_shard",
                "  0:55 Item: 2 item_health_large",
                "  0:56 Item: 2 weapon_rocketlauncher",
                "  0:57 Item: 2 ammo_rockets",
                "  0:59 ClientConnect: 3",
                "  0:59 ClientUserinfoChanged: 3 n\\Isgalamido\\t\\0\\model\\xian/default\\hmodel\\xian/default\\g_redteam\\g_blueteam\\c1\\4\\c2\\5\\hc\\100\\w\\0\\l\\0\\tt\\0\\tl\\0",
                "  1:01 ClientUserinfoChanged: 3 n\\Isgalamido\\t\\0\\model\\uriel/zael\\hmodel\\uriel/zael\\g_redteam\\g_blueteam\\c1\\5\\c2\\5\\hc\\100\\w\\0\\l\\0\\tt\\0\\tl\\0",
                "  1:01 ClientBegin: 3",
                "  1:02 Item: 3 weapon_rocketlauncher",
                "  1:04 Item: 2 item_armor_shard",
                "  1:04 Item: 2 item_armor_shard",
                "  1:04 Item: 2 item_armor_shard",
                "  1:06 ClientConnect: 4",
                "  1:06 ClientUserinfoChanged: 4 n\\Zeh\\t\\0\\model\\sarge/default\\hmodel\\sarge/default\\g_redteam\\g_blueteam\\c1\\5\\c2\\5\\hc\\100\\w\\0\\l\\0\\tt\\0\\tl\\0",
                "  1:08 Kill: 3 2 6: Isgalamido killed Mocinha by MOD_ROCKET",
                "  1:08 ClientUserinfoChanged: 4 n\\Zeh\\t\\0\\model\\sarge/default\\hmodel\\sarge/default\\g_redteam\\g_blueteam\\c1\\1\\c2\\5\\hc\\100\\w\\0\\l\\0\\tt\\0\\tl\\0",
                "  1:08 ClientBegin: 4",
                "  1:10 Item: 3 item_armor_shard",
                "  1:10 Item: 3 item_armor_shard",
                "  1:10 Item: 3 item_armor_shard",
                "  1:10 Item: 3 item_armor_combat",
                "  1:11 Item: 4 weapon_shotgun",
                "  1:11 Item: 4 ammo_shells",
                "  1:16 Item: 4 item_health_large",
                "  1:18 Item: 4 weapon_rocketlauncher",
                "  1:18 Item: 4 ammo_rockets",
                "  1:26 Kill: 1022 4 22: <world> killed Zeh by MOD_TRIGGER_HURT",
                "  1:26 ClientUserinfoChanged: 2 n\\Dono da Bola\\t\\0\\model\\sarge\\hmodel\\sarge\\g_redteam\\g_blueteam\\c1\\4\\c2\\5\\hc\\95\\w\\0\\l\\0\\tt\\0\\tl\\0",
                "  1:26 Item: 3 weapon_railgun",
                "  1:29 Item: 2 weapon_rocketlauncher",
                "  1:29 Item: 3 weapon_railgun",
                "  1:32 Item: 3 weapon_railgun",
                "  1:32 Kill: 1022 4 22: <world> killed Zeh by MOD_TRIGGER_HURT",
                "  1:35 Item: 2 item_armor_shard",
                "  1:35 Item: 2 item_armor_shard",
                "  1:35 Item: 2 item_armor_shard",
                "  1:35 Item: 3 weapon_railgun",
                "  1:38 Item: 2 item_health_large",
                "  1:38 Item: 3 weapon_railgun",
                "  1:41 Kill: 1022 2 19: <world> killed Dono da Bola by MOD_FALLING",
                "  1:41 Item: 3 weapon_railgun",
                "  1:43 Item: 2 ammo_rockets",
                "  1:44 Item: 2 weapon_rocketlauncher",
                "  1:46 Item: 2 item_armor_shard",
                "  1:47 Item: 2 item_armor_shard",
                "  1:47 Item: 2 item_armor_shard",
                "  1:47 ShutdownGame:",
                "  1:47 ------------------------------------------------------------"
            };
        }
    }
}
