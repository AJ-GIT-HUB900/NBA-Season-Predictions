
# NBA Season Simulator 2026

![Image](championship.jpg)

A fun, single-file C# console app that simulates an entire NBA season — real teams, real star players, real drama. Play an exhibition game, grind through an 82-game regular season, chase the MVP race, or run the full playoff bracket all the way to an NBA Finals champion.

## Features

- **All 30 real NBA teams**, organized by conference, with real 2025-26 rosters
- **Real star players** (Tatum, Giannis, Luka, SGA, Jokic, Wembanyama, Curry, and dozens more) rated across five attributes: Scoring, Playmaking, Rebounding, Defense, and Clutch
- **Quarter-by-quarter game engine** with overtime support when games are tied
- **Realistic box scores** — points, assists, and rebounds are distributed across each roster based on player ratings
- **Full 82-game season simulation** with live standings, win %, and point differential
- **MVP race tracker** ranking players by season-long scoring, assists, and rebounds
- **Complete playoff bracket** — First Round → Semifinals → Conference Finals → best-of-7 NBA Finals — with a full game-by-game series log
- **Roster lookup** to inspect any team's players and overall ratings
- Simple menu-driven interface, no external dependencies

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or later recommended)

### Run it

```bash
git clone https://github.com/your-username/nba-season-simulator.git
cd nba-season-simulator
dotnet run
```

Or compile directly:

```bash
csc NBASimulator.cs
NBASimulator.exe
```

## Usage

On launch, you'll get a menu:

```
1. Simulate a single exhibition game
2. Simulate a FULL 82-game regular season
3. Show current standings
4. Show league MVP race (top scorers)
5. Run the Playoffs & crown a champion
6. Team roster lookup
7. Quit
```

Pick a number and follow the prompts. A typical session:

1. Run option **2** to simulate the regular season
2. Check option **3** for standings and option **4** for the MVP race
3. Run option **5** to seed the playoffs by record and simulate the full bracket to a champion

## How the Simulation Works

Each player has five 0–99 ratings that combine into an overall rating. Team strength is the average overall of a team's top 8 players. Each quarter's score is generated from team strength plus a home-court bump and some randomness, with player Clutch ratings giving a boost in the 4th quarter and overtime. Box score stats (points/assists/rebounds) are then distributed across the roster proportional to each player's role and ratings.

## Project Structure

```
NBASimulator.cs   # Entire game: player/team models, game engine, season & playoff logic, CLI menu
```

Everything lives in a single file for easy portability — drop it into any C# console project and run.

## Contributing

Pull requests are welcome! Ideas for extensions:
- Injuries and fatigue over a season
- Trades and a GM mode
- Save/load season state to a file
- ASCII scoreboard / live play-by-play mode
- Historical team rosters (e.g., simulate classic eras)
