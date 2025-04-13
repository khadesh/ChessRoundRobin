using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    class Match
    {
        public string White { get; set; }
        public string Black { get; set; }

        public override string ToString() => $"{White} (w) vs {Black} (b)";
    }

    static void Main()
    {
        Console.WriteLine("Enter player names (comma-separated):");
        var input = Console.ReadLine();
        var players = input.Split(',').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();

        if (players.Count < 2)
        {
            Console.WriteLine("Need at least 2 players.");
            return;
        }

        if (players.Count % 2 != 0)
        {
            players.Add("BYE"); // Dummy player for odd number of players
        }

        var totalRounds = players.Count - 1;
        var half = players.Count / 2;

        // Shuffle players
        var rnd = new Random();
        players = players.OrderBy(p => rnd.Next()).ToList();

        var whiteCount = new Dictionary<string, int>();
        var blackCount = new Dictionary<string, int>();
        foreach (var player in players)
        {
            whiteCount[player] = 0;
            blackCount[player] = 0;
        }

        var rounds = new List<List<Match>>();

        for (int round = 0; round < totalRounds; round++)
        {
            var roundMatches = new List<Match>();

            for (int i = 0; i < half; i++)
            {
                string p1 = players[i];
                string p2 = players[players.Count - 1 - i];

                if (p1 == "BYE" || p2 == "BYE")
                {
                    continue;
                }

                bool p1White = ShouldPlayWhite(p1, p2, whiteCount, blackCount);

                var match = new Match
                {
                    White = p1White ? p1 : p2,
                    Black = p1White ? p2 : p1
                };

                whiteCount[match.White]++;
                blackCount[match.Black]++;
                roundMatches.Add(match);
            }

            rounds.Add(roundMatches);

            // Rotate players (keep first fixed)
            var last = players[players.Count - 1];
            players.RemoveAt(players.Count - 1);
            players.Insert(1, last);
        }

        // Output
        for (int i = 0; i < rounds.Count; i++)
        {
            Console.WriteLine($"\nRound {i + 1}:");
            foreach (var match in rounds[i])
            {
                Console.WriteLine($"  {match}");
            }
        }
    }

    static bool ShouldPlayWhite(string p1, string p2, Dictionary<string, int> whiteCount, Dictionary<string, int> blackCount)
    {
        int p1Diff = whiteCount[p1] - blackCount[p1];
        int p2Diff = whiteCount[p2] - blackCount[p2];

        if (p1Diff < p2Diff) return true;
        if (p2Diff < p1Diff) return false;

        // Tie: randomly decide
        return new Random().Next(2) == 0;
    }
}
