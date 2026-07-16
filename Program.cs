using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBASim
{
    // ------------------------------------------------------------------
    //  PLAYER
    // ------------------------------------------------------------------
    public class Player
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int Scoring { get; set; }     // 0-99 rating
        public int Playmaking { get; set; }  // 0-99 rating
        public int Rebounding { get; set; }  // 0-99 rating
        public int Defense { get; set; }     // 0-99 rating
        public int Clutch { get; set; }      // 0-99 rating (affects 4th quarter)

        // Season accumulators
        public int GamesPlayed;
        public long TotalPoints;
        public long TotalAssists;
        public long TotalRebounds;

        public Player(string name, string pos, int scoring, int playmaking, int rebounding, int defense, int clutch)
        {
            Name = name; Position = pos;
            Scoring = scoring; Playmaking = playmaking;
            Rebounding = rebounding; Defense = defense; Clutch = clutch;
        }

        public double Overall => Math.Round((Scoring + Playmaking + Rebounding + Defense + Clutch) / 5.0, 1);

        public double PPG => GamesPlayed == 0 ? 0 : Math.Round((double)TotalPoints / GamesPlayed, 1);
        public double APG => GamesPlayed == 0 ? 0 : Math.Round((double)TotalAssists / GamesPlayed, 1);
        public double RPG => GamesPlayed == 0 ? 0 : Math.Round((double)TotalRebounds / GamesPlayed, 1);
    }

    // ------------------------------------------------------------------
    //  TEAM
    // ------------------------------------------------------------------
    public class Team
    {
        public string City { get; set; }
        public string Nickname { get; set; }
        public string Abbrev { get; set; }
        public string Conference { get; set; } // "East" or "West"
        public List<Player> Roster { get; set; } = new List<Player>();

        public string FullName => $"{City} {Nickname}";

        // Season record
        public int Wins;
        public int Losses;
        public long PointsFor;
        public long PointsAgainst;

        public double WinPct => (Wins + Losses) == 0 ? 0 : Math.Round((double)Wins / (Wins + Losses), 3);

        public double TeamStrength =>
            Roster.OrderByDescending(p => p.Overall).Take(8).Average(p => p.Overall);

        public Team(string city, string nick, string abbrev, string conf)
        {
            City = city; Nickname = nick; Abbrev = abbrev; Conference = conf;
        }
    }

    // ------------------------------------------------------------------
    //  GAME RESULT
    // ------------------------------------------------------------------
    public class GameResult
    {
        public Team Home, Away;
        public int HomeScore, AwayScore;
        public List<string> PlayByPlay = new List<string>();
        public Player HomeStar, AwayStar;
        public Team Winner => HomeScore > AwayScore ? Home : Away;
        public Team Loser => HomeScore > AwayScore ? Away : Home;
    }

    // ------------------------------------------------------------------
    //  LEAGUE DATA - Real NBA Teams & Real Star Players (2025-26 rosters)
    // ------------------------------------------------------------------
    public static class League
    {
        public static Random Rng = new Random();

        public static List<Team> BuildLeague()
        {
            var teams = new List<Team>();

            void Add(string city, string nick, string abbr, string conf, params Player[] players)
            {
                var t = new Team(city, nick, abbr, conf);
                t.Roster.AddRange(players);
                teams.Add(t);
            }

            // ---------------- EASTERN CONFERENCE ----------------
            Add("Boston", "Celtics", "BOS", "East",
                new Player("Jayson Tatum", "SF", 93, 82, 80, 84, 90),
                new Player("Jaylen Brown", "SG", 90, 70, 72, 82, 85),
                new Player("Derrick White", "PG", 78, 84, 60, 85, 80),
                new Player("Kristaps Porzingis", "C", 85, 55, 78, 78, 70),
                new Player("Jrue Holiday", "PG", 75, 78, 62, 90, 78));

            Add("New York", "Knicks", "NYK", "East",
                new Player("Jalen Brunson", "PG", 92, 88, 55, 72, 94),
                new Player("Karl-Anthony Towns", "C", 87, 65, 85, 68, 75),
                new Player("OG Anunoby", "SF", 78, 55, 68, 90, 76),
                new Player("Mikal Bridges", "SF", 76, 60, 60, 84, 74),
                new Player("Josh Hart", "SG", 70, 68, 82, 78, 70));

            Add("Milwaukee", "Bucks", "MIL", "East",
                new Player("Giannis Antetokounmpo", "PF", 94, 78, 92, 88, 93),
                new Player("Damian Lillard", "PG", 90, 85, 45, 68, 96),
                new Player("Khris Middleton", "SF", 78, 70, 60, 72, 80),
                new Player("Brook Lopez", "C", 74, 40, 70, 82, 65),
                new Player("Bobby Portis", "PF", 72, 45, 78, 60, 68));

            Add("Philadelphia", "76ers", "PHI", "East",
                new Player("Joel Embiid", "C", 95, 70, 88, 80, 88),
                new Player("Tyrese Maxey", "PG", 88, 80, 50, 74, 85),
                new Player("Paul George", "SF", 84, 68, 65, 82, 80),
                new Player("Andre Drummond", "C", 60, 30, 92, 65, 50),
                new Player("Kelly Oubre Jr.", "SF", 76, 45, 62, 70, 65));

            Add("Miami", "Heat", "MIA", "East",
                new Player("Bam Adebayo", "C", 78, 68, 84, 92, 78),
                new Player("Tyler Herro", "SG", 86, 72, 48, 62, 84),
                new Player("Jimmy Butler III", "SF", 85, 72, 68, 88, 96),
                new Player("Terry Rozier", "PG", 78, 70, 45, 68, 72),
                new Player("Duncan Robinson", "SG", 74, 40, 42, 55, 70));

            Add("Cleveland", "Cavaliers", "CLE", "East",
                new Player("Donovan Mitchell", "SG", 92, 75, 55, 76, 90),
                new Player("Darius Garland", "PG", 85, 84, 42, 65, 78),
                new Player("Evan Mobley", "PF", 80, 62, 85, 90, 76),
                new Player("Jarrett Allen", "C", 76, 40, 84, 80, 62),
                new Player("Max Strus", "SG", 72, 50, 50, 66, 68));

            Add("Indiana", "Pacers", "IND", "East",
                new Player("Tyrese Haliburton", "PG", 82, 96, 55, 70, 84),
                new Player("Pascal Siakam", "PF", 85, 68, 75, 76, 80),
                new Player("Myles Turner", "C", 78, 40, 76, 84, 68),
                new Player("Bennedict Mathurin", "SG", 78, 50, 55, 62, 70),
                new Player("Andrew Nembhard", "PG", 70, 72, 48, 78, 66));

            Add("Orlando", "Magic", "ORL", "East",
                new Player("Paolo Banchero", "PF", 88, 70, 78, 74, 82),
                new Player("Franz Wagner", "SF", 84, 68, 65, 78, 78),
                new Player("Jalen Suggs", "PG", 74, 68, 55, 88, 72),
                new Player("Wendell Carter Jr.", "C", 68, 50, 80, 72, 60),
                new Player("Desmond Bane", "SG", 84, 62, 55, 70, 78));

            Add("Atlanta", "Hawks", "ATL", "East",
                new Player("Trae Young", "PG", 90, 94, 40, 55, 88),
                new Player("Jalen Johnson", "SF", 80, 62, 78, 76, 74),
                new Player("Dyson Daniels", "SG", 62, 60, 60, 94, 60),
                new Player("Clint Capela", "C", 62, 30, 88, 74, 50),
                new Player("Zaccharie Risacher", "SF", 68, 45, 55, 68, 55));

            Add("Chicago", "Bulls", "CHI", "East",
                new Player("Coby White", "PG", 82, 68, 48, 62, 76),
                new Player("Josh Giddey", "PG", 72, 82, 72, 68, 68),
                new Player("Nikola Vucevic", "C", 78, 58, 82, 66, 68),
                new Player("Matas Buzelis", "PF", 70, 50, 60, 66, 60),
                new Player("Ayo Dosunmu", "SG", 68, 60, 46, 78, 62));

            Add("Detroit", "Pistons", "DET", "East",
                new Player("Cade Cunningham", "PG", 88, 90, 62, 72, 86),
                new Player("Jalen Duren", "C", 74, 42, 88, 76, 62),
                new Player("Tobias Harris", "PF", 74, 55, 60, 66, 68),
                new Player("Ausar Thompson", "SF", 68, 55, 70, 88, 60),
                new Player("Malik Beasley", "SG", 76, 42, 40, 58, 72));

            Add("New Orleans", "Pelicans", "NOP", "East",
                new Player("Zion Williamson", "PF", 90, 60, 82, 70, 80),
                new Player("Trey Murphy III", "SF", 78, 50, 55, 74, 72),
                new Player("Dejounte Murray", "PG", 82, 78, 55, 86, 76),
                new Player("Herbert Jones", "SF", 62, 48, 60, 92, 58),
                new Player("Yves Missi", "C", 58, 30, 82, 72, 48));

            Add("Toronto", "Raptors", "TOR", "East",
                new Player("Scottie Barnes", "SF", 80, 78, 78, 84, 78),
                new Player("Immanuel Quickley", "PG", 78, 74, 48, 66, 74),
                new Player("RJ Barrett", "SG", 78, 60, 60, 66, 74),
                new Player("Jakob Poeltl", "C", 66, 45, 86, 76, 55),
                new Player("Gradey Dick", "SG", 68, 45, 42, 55, 60));

            Add("Brooklyn", "Nets", "BKN", "East",
                new Player("Cam Thomas", "SG", 84, 55, 42, 55, 78),
                new Player("Nic Claxton", "C", 66, 40, 80, 82, 55),
                new Player("Michael Porter Jr.", "SF", 78, 45, 65, 60, 68),
                new Player("Ben Simmons", "PG", 55, 82, 75, 88, 55),
                new Player("Noah Clowney", "PF", 62, 40, 62, 62, 50));

            Add("Charlotte", "Hornets", "CHA", "East",
                new Player("LaMelo Ball", "PG", 88, 92, 58, 60, 82),
                new Player("Brandon Miller", "SF", 80, 55, 60, 74, 72),
                new Player("Miles Bridges", "PF", 78, 50, 68, 62, 70),
                new Player("Mark Williams", "C", 68, 35, 82, 70, 55),
                new Player("Josh Green", "SG", 62, 50, 48, 76, 55));

            Add("Washington", "Wizards", "WAS", "East",
                new Player("Alex Sarr", "C", 68, 45, 74, 74, 55),
                new Player("Bilal Coulibaly", "SF", 66, 55, 58, 78, 58),
                new Player("Jordan Poole", "SG", 78, 68, 42, 55, 68),
                new Player("Kyshawn George", "SF", 62, 48, 55, 66, 55),
                new Player("CJ McCollum", "SG", 78, 62, 40, 60, 74));

            // ---------------- WESTERN CONFERENCE ----------------
            Add("Oklahoma City", "Thunder", "OKC", "West",
                new Player("Shai Gilgeous-Alexander", "PG", 95, 82, 55, 82, 94),
                new Player("Jalen Williams", "SF", 84, 70, 65, 84, 80),
                new Player("Chet Holmgren", "C", 82, 55, 78, 90, 72),
                new Player("Luguentz Dort", "SG", 65, 45, 55, 94, 60),
                new Player("Isaiah Hartenstein", "C", 62, 55, 86, 76, 55));

            Add("Denver", "Nuggets", "DEN", "West",
                new Player("Nikola Jokic", "C", 92, 96, 92, 78, 92),
                new Player("Jamal Murray", "PG", 86, 78, 45, 68, 90),
                new Player("Michael Porter Jr.", "SF", 80, 45, 68, 62, 70),
                new Player("Aaron Gordon", "PF", 78, 55, 70, 80, 76),
                new Player("Christian Braun", "SG", 68, 48, 55, 76, 62));

            Add("Minnesota", "Timberwolves", "MIN", "West",
                new Player("Anthony Edwards", "SG", 92, 74, 62, 82, 90),
                new Player("Rudy Gobert", "C", 62, 35, 92, 96, 55),
                new Player("Julius Randle", "PF", 82, 62, 74, 62, 74),
                new Player("Jaden McDaniels", "SF", 68, 48, 60, 90, 62),
                new Player("Mike Conley", "PG", 68, 78, 42, 74, 74));

            Add("Dallas", "Mavericks", "DAL", "West",
                new Player("Kyrie Irving", "PG", 90, 82, 45, 70, 92),
                new Player("Anthony Davis", "PF", 90, 60, 88, 90, 84),
                new Player("Klay Thompson", "SG", 78, 42, 42, 62, 78),
                new Player("Daniel Gafford", "C", 66, 30, 80, 78, 55),
                new Player("Cooper Flagg", "SF", 80, 68, 72, 82, 78));

            Add("Los Angeles", "Lakers", "LAL", "West",
                new Player("LeBron James", "SF", 90, 88, 78, 76, 94),
                new Player("Luka Doncic", "PG", 94, 92, 72, 62, 92),
                new Player("Austin Reaves", "SG", 78, 74, 48, 68, 76),
                new Player("Rui Hachimura", "PF", 74, 42, 62, 60, 66),
                new Player("Jaxson Hayes", "C", 60, 30, 74, 68, 50));

            Add("LA Clippers", "Clippers", "LAC", "West",
                new Player("Kawhi Leonard", "SF", 90, 65, 68, 92, 88),
                new Player("James Harden", "PG", 86, 92, 55, 65, 88),
                new Player("Ivica Zubac", "C", 68, 40, 88, 74, 58),
                new Player("Norman Powell", "SG", 82, 45, 42, 62, 76),
                new Player("Derrick Jones Jr.", "SF", 68, 40, 62, 84, 60));

            Add("Phoenix", "Suns", "PHX", "West",
                new Player("Devin Booker", "SG", 92, 82, 50, 68, 92),
                new Player("Kevin Durant", "PF", 93, 72, 68, 76, 90),
                new Player("Bradley Beal", "SG", 82, 62, 45, 62, 78),
                new Player("Jusuf Nurkic", "C", 62, 45, 82, 68, 52),
                new Player("Grayson Allen", "SG", 76, 45, 40, 68, 74));

            Add("Sacramento", "Kings", "SAC", "West",
                new Player("De'Aaron Fox", "PG", 88, 78, 45, 74, 84),
                new Player("Domantas Sabonis", "C", 78, 78, 92, 68, 76),
                new Player("DeMar DeRozan", "SF", 86, 62, 50, 62, 88),
                new Player("Keegan Murray", "SF", 76, 42, 60, 68, 66),
                new Player("Malik Monk", "SG", 78, 62, 40, 55, 76));

            Add("Golden State", "Warriors", "GSW", "West",
                new Player("Stephen Curry", "PG", 94, 84, 45, 65, 96),
                new Player("Draymond Green", "PF", 62, 82, 78, 92, 74),
                new Player("Jimmy Butler III", "SF", 82, 68, 66, 84, 90),
                new Player("Jonathan Kuminga", "PF", 78, 45, 62, 66, 68),
                new Player("Buddy Hield", "SG", 76, 42, 38, 55, 70));

            Add("Houston", "Rockets", "HOU", "West",
                new Player("Alperen Sengun", "C", 82, 80, 80, 72, 76),
                new Player("Jalen Green", "SG", 84, 55, 45, 66, 74),
                new Player("Kevin Durant", "PF", 90, 68, 66, 74, 88),
                new Player("Amen Thompson", "SG", 74, 62, 68, 88, 68),
                new Player("Fred VanVleet", "PG", 76, 78, 42, 78, 78));

            Add("Memphis", "Grizzlies", "MEM", "West",
                new Player("Ja Morant", "PG", 90, 85, 55, 70, 90),
                new Player("Jaren Jackson Jr.", "PF", 82, 45, 74, 90, 78),
                new Player("Desmond Bane", "SG", 82, 60, 55, 68, 76),
                new Player("Zach Edey", "C", 68, 30, 90, 70, 52),
                new Player("Marcus Smart", "PG", 68, 68, 50, 90, 72));

            Add("San Antonio", "Spurs", "SAS", "West",
                new Player("Victor Wembanyama", "C", 90, 68, 90, 96, 82),
                new Player("De'Aaron Fox", "PG", 86, 76, 45, 74, 82),
                new Player("Devin Vassell", "SG", 78, 55, 48, 74, 72),
                new Player("Stephon Castle", "PG", 74, 70, 55, 82, 68),
                new Player("Harrison Barnes", "SF", 70, 45, 55, 65, 66));

            Add("Portland", "Trail Blazers", "POR", "West",
                new Player("Deni Avdija", "SF", 80, 68, 74, 78, 76),
                new Player("Shaedon Sharpe", "SG", 82, 55, 55, 70, 74),
                new Player("Scoot Henderson", "PG", 74, 75, 48, 72, 66),
                new Player("Donovan Clingan", "C", 66, 35, 84, 82, 55),
                new Player("Jerami Grant", "PF", 76, 45, 55, 68, 70));

            Add("Utah", "Jazz", "UTA", "West",
                new Player("Lauri Markkanen", "PF", 84, 45, 68, 62, 76),
                new Player("Walker Kessler", "C", 60, 35, 86, 84, 50),
                new Player("Keyonte George", "PG", 76, 68, 42, 60, 68),
                new Player("Isaiah Collier", "PG", 66, 70, 52, 70, 58),
                new Player("Svi Mykhailiuk", "SF", 64, 40, 42, 55, 58));

            return teams;
        }
    }

    // ------------------------------------------------------------------
    //  GAME ENGINE
    // ------------------------------------------------------------------
    public static class GameEngine
    {
        static string[] dunkCalls = { "throws it DOWN", "flushes it home", "posterizes the defender", "slams it with authority" };
        static string[] threeCalls = { "buries a three", "splashes it from deep", "drills a triple", "nails it from downtown" };
        static string[] clutchCalls = { "ICE COLD in the clutch", "delivers in crunch time", "comes up big when it matters" };
        static string[] defenseCalls = { "swats the shot away", "comes up with a huge steal", "shuts the door defensively" };

        public static GameResult SimulateGame(Team home, Team away, bool verbose = false)
        {
            var result = new GameResult { Home = home, Away = away };
            int homeScore = 0, awayScore = 0;

            // Home court advantage nudge
            double homeStrength = home.TeamStrength + 2.5;
            double awayStrength = away.TeamStrength;

            var homeTop = home.Roster.OrderByDescending(p => p.Scoring).First();
            var awayTop = away.Roster.OrderByDescending(p => p.Scoring).First();
            result.HomeStar = homeTop;
            result.AwayStar = awayTop;

            // Simulate 4 quarters
            for (int q = 1; q <= 4; q++)
            {
                int homeQ = SimulateQuarter(home, homeStrength, q == 4);
                int awayQ = SimulateQuarter(away, awayStrength, q == 4);
                homeScore += homeQ;
                awayScore += awayQ;

                result.PlayByPlay.Add($"  End of Q{q}: {home.Abbrev} {homeScore} - {awayScore} {away.Abbrev}");
            }

            // Overtime if tied
            int otCount = 0;
            while (homeScore == awayScore && otCount < 4)
            {
                otCount++;
                int homeOT = SimulateQuarter(home, homeStrength, true) / 2;
                int awayOT = SimulateQuarter(away, awayStrength, true) / 2;
                homeScore += homeOT;
                awayScore += awayOT;
                result.PlayByPlay.Add($"  End of OT{otCount}: {home.Abbrev} {homeScore} - {awayScore} {away.Abbrev}");
            }

            result.HomeScore = homeScore;
            result.AwayScore = awayScore;

            // Update stat lines (rough distribution across box score)
            DistributeStats(home, homeScore);
            DistributeStats(away, awayScore);

            // Update records
            home.PointsFor += homeScore; home.PointsAgainst += awayScore;
            away.PointsFor += awayScore; away.PointsAgainst += homeScore;
            if (homeScore > awayScore) { home.Wins++; away.Losses++; }
            else { away.Wins++; home.Losses++; }

            return result;
        }

        static int SimulateQuarter(Team t, double strength, bool clutchQuarter)
        {
            double clutchBoost = clutchQuarter ? t.Roster.Average(p => p.Clutch) / 40.0 : 0;
            double basePoints = 24 + (strength - 78) * 0.6 + clutchBoost;
            double variance = League.Rng.NextDouble() * 10 - 5;
            int points = (int)Math.Round(basePoints + variance);
            return Math.Max(14, points);
        }

        static void DistributeStats(Team t, int teamScore)
        {
            var ranked = t.Roster.OrderByDescending(p => p.Scoring).ToList();
            double[] shareWeights = { 0.28, 0.24, 0.19, 0.16, 0.13 };
            for (int i = 0; i < ranked.Count && i < shareWeights.Length; i++)
            {
                var p = ranked[i];
                p.GamesPlayed++;
                int pts = (int)Math.Round(teamScore * shareWeights[i] * (0.85 + League.Rng.NextDouble() * 0.3));
                p.TotalPoints += Math.Max(0, pts);
                p.TotalAssists += (int)Math.Round(p.Playmaking / 100.0 * (2 + League.Rng.NextDouble() * 6));
                p.TotalRebounds += (int)Math.Round(p.Rebounding / 100.0 * (2 + League.Rng.NextDouble() * 8));
            }
        }

        public static string RandomCall(string[] arr) => arr[League.Rng.Next(arr.Length)];
    }

    // ------------------------------------------------------------------
    //  PROGRAM / SEASON DRIVER
    // ------------------------------------------------------------------
    class Program
    {
        static List<Team> teams;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            PrintBanner();

            teams = League.BuildLeague();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n================ MAIN MENU ================");
                Console.WriteLine("1. Simulate a single exhibition game");
                Console.WriteLine("2. Simulate a FULL 82-game regular season");
                Console.WriteLine("3. Show current standings");
                Console.WriteLine("4. Show league MVP race (top scorers)");
                Console.WriteLine("5. Run the Playoffs & crown a champion");
                Console.WriteLine("6. Team roster lookup");
                Console.WriteLine("7. Quit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": PlayExhibitionGame(); break;
                    case "2": SimulateFullSeason(); break;
                    case "3": ShowStandings(); break;
                    case "4": ShowMvpRace(); break;
                    case "5": RunPlayoffs(); break;
                    case "6": ShowRoster(); break;
                    case "7": running = false; break;
                    default: Console.WriteLine("Not a valid choice, try again."); break;
                }
            }

            Console.WriteLine("\nThanks for playing NBA Season Simulator 2026! 🏀");
        }

        static void PrintBanner()
        {
            Console.WriteLine(@"
  _   _ ____    _         ____  _                 _       _             
 | \ | | __ )  / \       / ___|(_)_ __ ___  _   _| | __ _| |_ ___  _ __ 
 |  \| |  _ \ / _ \  ____\___ \| | '_ ` _ \| | | | |/ _` | __/ _ \| '__|
 | |\  | |_) / ___ \|_____|__) | | | | | | | |_| | | (_| | || (_) | |   
 |_| \_|____/_/   \_\    |____/|_|_| |_| |_|\__,_|_|\__,_|\__\___/|_|   
                                                                          
             W E L C O M E   T O   T H E   2 0 2 6   S E A S O N
");
        }

        static Team PickTeam(string prompt)
        {
            Console.WriteLine(prompt);
            for (int i = 0; i < teams.Count; i++)
                Console.WriteLine($"  {i + 1,2}. {teams[i].FullName} ({teams[i].Abbrev}) - {teams[i].Conference}");
            Console.Write("Enter number: ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= teams.Count)
                return teams[idx - 1];
            Console.WriteLine("Invalid pick, defaulting to a random team.");
            return teams[League.Rng.Next(teams.Count)];
        }

        static void PlayExhibitionGame()
        {
            Team home = PickTeam("\nPick the HOME team:");
            Team away = PickTeam("\nPick the AWAY team:");
            if (home == away) { Console.WriteLine("A team can't play itself! Picking a random opponent instead."); away = teams.Where(t => t != home).OrderBy(_ => League.Rng.Next()).First(); }

            Console.WriteLine($"\n🏀 TIP-OFF: {away.FullName} @ {home.FullName}\n");
            var result = GameEngine.SimulateGame(home, away, verbose: true);
            foreach (var line in result.PlayByPlay) Console.WriteLine(line);

            Console.WriteLine($"\nFINAL: {home.Abbrev} {result.HomeScore} - {result.AwayScore} {away.Abbrev}");
            Console.WriteLine($"🏆 Winner: {result.Winner.FullName}!");
            Console.WriteLine($"⭐ Star performers: {result.HomeStar.Name} ({home.Abbrev}) & {result.AwayStar.Name} ({away.Abbrev})");
        }

        static void SimulateFullSeason()
        {
            Console.WriteLine("\nSimulating full 82-game regular season for all 30 teams... 🏀");
            // Reset season stats
            foreach (var t in teams) { t.Wins = 0; t.Losses = 0; t.PointsFor = 0; t.PointsAgainst = 0; foreach (var p in t.Roster) { p.GamesPlayed = 0; p.TotalPoints = 0; p.TotalAssists = 0; p.TotalRebounds = 0; } }

            // Build a simple round-robin-ish schedule: each team plays ~82 games vs random opponents
            var schedule = new List<(Team, Team)>();
            foreach (var t in teams)
            {
                var opponents = teams.Where(o => o != t).OrderBy(_ => League.Rng.Next()).ToList();
                for (int i = 0; i < 41; i++) // 41 home games
                {
                    var opp = opponents[i % opponents.Count];
                    schedule.Add((t, opp));
                }
            }
            schedule = schedule.OrderBy(_ => League.Rng.Next()).ToList();

            int gameCount = 0;
            foreach (var (h, a) in schedule)
            {
                GameEngine.SimulateGame(h, a);
                gameCount++;
                if (gameCount % 100 == 0) Console.Write(".");
            }

            Console.WriteLine($"\nSeason complete! {gameCount} games simulated across the league.");
            ShowStandings();
        }

        static void ShowStandings()
        {
            Console.WriteLine("\n===================== EASTERN CONFERENCE =====================");
            PrintConferenceStandings("East");
            Console.WriteLine("\n===================== WESTERN CONFERENCE =====================");
            PrintConferenceStandings("West");
        }

        static void PrintConferenceStandings(string conf)
        {
            var ranked = teams.Where(t => t.Conference == conf)
                               .OrderByDescending(t => t.WinPct)
                               .ThenByDescending(t => t.PointsFor - t.PointsAgainst)
                               .ToList();
            Console.WriteLine($"{"#",-3}{"Team",-24}{"W",-5}{"L",-5}{"PCT",-8}{"DIFF",-6}");
            int rank = 1;
            foreach (var t in ranked)
            {
                long diff = t.PointsFor - t.PointsAgainst;
                string diffStr = (diff >= 0 ? "+" : "") + diff;
                Console.WriteLine($"{rank,-3}{t.FullName,-24}{t.Wins,-5}{t.Losses,-5}{t.WinPct,-8:0.000}{diffStr,-6}");
                rank++;
            }
        }

        static void ShowMvpRace()
        {
            var allPlayers = teams.SelectMany(t => t.Roster).Where(p => p.GamesPlayed > 0).ToList();
            if (!allPlayers.Any()) { Console.WriteLine("\nNo games played yet this season — simulate a season first (option 2)!"); return; }

            Console.WriteLine("\n================ MVP RACE (Top 10 Scorers) ================");
            Console.WriteLine($"{"Player",-26}{"Team",-6}{"PPG",-7}{"APG",-7}{"RPG",-7}");
            var top = allPlayers.OrderByDescending(p => p.PPG).Take(10);
            foreach (var p in top)
            {
                var team = teams.First(t => t.Roster.Contains(p));
                Console.WriteLine($"{p.Name,-26}{team.Abbrev,-6}{p.PPG,-7}{p.APG,-7}{p.RPG,-7}");
            }

            var mvp = allPlayers.OrderByDescending(p => p.PPG * 1.0 + p.APG * 1.2 + p.RPG * 1.1).First();
            var mvpTeam = teams.First(t => t.Roster.Contains(mvp));
            Console.WriteLine($"\n🏆 Your projected 2026 MVP: {mvp.Name} ({mvpTeam.FullName}) — {mvp.PPG} PPG / {mvp.APG} APG / {mvp.RPG} RPG");
        }

        static void RunPlayoffs()
        {
            bool hasSeasonData = teams.Any(t => t.Wins + t.Losses > 0);
            if (!hasSeasonData)
            {
                Console.WriteLine("\nNo regular season played yet — seeding playoffs by roster strength instead.");
            }

            var east = SeedConference("East", hasSeasonData);
            var west = SeedConference("West", hasSeasonData);

            Console.WriteLine("\n🌟 ===================== NBA PLAYOFFS ===================== 🌟");
            Console.WriteLine("\n-- Eastern Conference Playoffs --");
            Team eastChamp = RunBracket(east);
            Console.WriteLine($"\n👑 Eastern Conference Champion: {eastChamp.FullName}!");

            Console.WriteLine("\n-- Western Conference Playoffs --");
            Team westChamp = RunBracket(west);
            Console.WriteLine($"\n👑 Western Conference Champion: {westChamp.FullName}!");

            Console.WriteLine("\n🏆 ===================== NBA FINALS ===================== 🏆");
            Team champion = PlaySeries(eastChamp, westChamp, "NBA FINALS", 7);
            Console.WriteLine($"\n🎉🎉🎉 THE {champion.FullName.ToUpper()} ARE YOUR 2026 NBA CHAMPIONS! 🎉🎉🎉");
        }

        static List<Team> SeedConference(string conf, bool useRecord)
        {
            var pool = teams.Where(t => t.Conference == conf).ToList();
            var seeded = useRecord
                ? pool.OrderByDescending(t => t.WinPct).Take(8).ToList()
                : pool.OrderByDescending(t => t.TeamStrength).Take(8).ToList();
            return seeded;
        }

        // Standard 1v8, 2v7, 3v6, 4v5 bracket down to a conference champ
        static Team RunBracket(List<Team> seeds)
        {
            var round = new List<Team>(seeds);
            int roundNum = 1;
            string[] roundNames = { "First Round", "Semifinals", "Conference Finals" };

            while (round.Count > 1)
            {
                Console.WriteLine($"\n  -- {roundNames[Math.Min(roundNum - 1, roundNames.Length - 1)]} --");
                var next = new List<Team>();
                for (int i = 0; i < round.Count / 2; i++)
                {
                    Team high = round[i];
                    Team low = round[round.Count - 1 - i];
                    Team winner = PlaySeries(high, low, $"{high.Abbrev} vs {low.Abbrev}", 7);
                    next.Add(winner);
                }
                round = next;
                roundNum++;
            }
            return round.First();
        }

        // Best-of-N playoff series simulation with a running game log
        static Team PlaySeries(Team a, Team b, string label, int bestOf)
        {
            int winsNeeded = bestOf / 2 + 1;
            int aWins = 0, bWins = 0;
            Console.WriteLine($"\n📣 {label} (best of {bestOf})");
            int game = 1;
            while (aWins < winsNeeded && bWins < winsNeeded)
            {
                // Alternate home court, "a" is higher seed and gets extra home games
                bool aHome = (game <= 2) || (game == 5) || (game == 7) || (game == 3 && false);
                Team home = aHome ? a : b;
                Team away = aHome ? b : a;
                var result = GameEngine.SimulateGame(home, away);

                if (result.Winner == a) aWins++; else bWins++;

                Console.WriteLine($"  Game {game}: {home.Abbrev} {result.HomeScore} - {result.AwayScore} {away.Abbrev}  " +
                                   $"({result.Winner.Abbrev} win)  [Series: {a.Abbrev} {aWins} - {bWins} {b.Abbrev}]");
                game++;
            }

            Team seriesWinner = aWins > bWins ? a : b;
            Console.WriteLine($"  ✅ {seriesWinner.FullName} win the series {Math.Max(aWins, bWins)}-{Math.Min(aWins, bWins)}!");
            return seriesWinner;
        }

        static void ShowRoster()
        {
            Team t = PickTeam("\nWhich team's roster do you want to see?");
            Console.WriteLine($"\n===== {t.FullName} ({t.Abbrev}) — {t.Conference}ern Conference =====");
            Console.WriteLine($"{"Player",-26}{"Pos",-5}{"SCO",-5}{"PLM",-5}{"REB",-5}{"DEF",-5}{"CLU",-5}{"OVR",-5}");
            foreach (var p in t.Roster.OrderByDescending(p => p.Overall))
            {
                Console.WriteLine($"{p.Name,-26}{p.Position,-5}{p.Scoring,-5}{p.Playmaking,-5}{p.Rebounding,-5}{p.Defense,-5}{p.Clutch,-5}{p.Overall,-5}");
            }
            Console.WriteLine($"\nTeam Strength Rating: {Math.Round(t.TeamStrength, 1)}");
        }
    }
}