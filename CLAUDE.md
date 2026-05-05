# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Discord bot for rolling dice in tabletop play (the "Hellfrost" / D&D-adjacent system used by the project owner and friends). Hosted on a Raspberry Pi on the owner's home LAN. .NET 10, Discord.Net 3.x, EF Core + SQLite for persistent statistics.

## Commands

```bash
# Build everything (solution at repo root)
dotnet build

# Run all tests (xUnit + Shouldly)
dotnet test

# Run a single test
dotnet test --filter "FullyQualifiedName~ResultMessage_KeepLowest_WithPositiveModifier_ShowsModifier_And_Equation"

# Run the bot locally (needs DISCORD_BOT_TOKEN env var)
DISCORD_BOT_TOKEN=... dotnet run --project DiscordBot

# Override DB path (defaults to dicestats.db in CWD)
DICE_BOT_DB_PATH=/path/to/file.db DISCORD_BOT_TOKEN=... dotnet run --project DiscordBot

# Production publish for Raspberry Pi 5 (arm64, self-contained)
dotnet publish DiscordBot -c Release -r linux-arm64 --self-contained true -o /home/zarcton/published-bot/
```

## Architecture

### Slash command dispatch

`Program.cs` uses `Microsoft.Extensions.Hosting`. It registers `IDbContextFactory<StatisticsDbContext>` (SQLite), `IRollRecorder`, and `DiscordCommandHandler` as an `IHostedService`. `EnsureCreatedAsync` is called once on startup (no migrations yet — when the schema needs to evolve, switch to migrations).

`DiscordCommandHandler` owns the `DiscordSocketClient`, defines all slash commands in `CreateSlashCommands()`, and dispatches incoming `SocketSlashCommand`s through four instance dictionaries (`_diceCommandHanders`, `_effectCommandHandlers`, `_characterCommandHandlers`, `_utilityCommandHandlers`). Each dictionary maps a command name from `Constants` to a static handler in `DiscordBot.Handlers`. The dictionaries are instance fields so they can capture injected dependencies (`IRollRecorder`, `IDbContextFactory<StatisticsDbContext>`) in their lambdas.

When adding a new slash command:
1. Add a name constant to `Handlers/Constants.cs`.
2. Add a static handler class under `Handlers/`.
3. Register the command in `CreateSlashCommands()` and add a dispatch entry to one of the dictionaries.

### Rollers vs. Handlers

- `Rollers/` contains the dice/effect/character logic. Stateless. Static methods, no Discord types.
- `Handlers/` adapts `SocketSlashCommand` to the rollers, replies via `command.RespondAsync`, then records to stats.
- `Parsers/` converts strings/options into roller input.
- `Models/` holds `RollDiceCommand` (the parsed-and-rolled value used by both the renderer and the recorder) and `MessageDto`.

`DiceRoller.ParseAndRollDice` returns `DiceRollResult(string Message, IReadOnlyList<RollDiceCommand> Rolls)` — both the rendered reply and the underlying rolls so the handler can record them.

### Dice notation

Parsed by `DiceRollParser`: `NdMk<keep>[h|l][+/-mod]`, multiple commands chained with `&`. Examples: `4d6k3h+2`, `3d20 & 2d6k1l+5`. Whitespace is stripped. The README's claims about `d100`/`d%`/percentile dice and bare `d6` defaulting to `1d6` are **not** supported — README is stale.

### Statistics layer (`Statistics/`)

- `Models/Roll` + `Models/RollDie` — one `Roll` per parsed command segment; `Standard` rolls additionally get one `RollDie` per individual die with `Kept` and `Position`.
- `Models/RollType` — `Standard` / `Effect` / `Character`. Effect/Character rolls only record `RollSubType` + `ResultText`; their internal dice are not surfaced (the rollers return pre-formatted strings).
- `StatisticsDbContext` — indexes on `UserId`, `GuildId`, `Timestamp`, `DieType`. `RollType` stored as string.
- `IRollRecorder` / `RollRecorder` — uses `IDbContextFactory<StatisticsDbContext>` to create a fresh context per call. Always wraps in try/catch + log; **recording must never throw** because handlers call it after the user has already received a response.
- Hidden rolls (`/roll_hidden`) are recorded with no flag — they count toward stats.

### `/stats` command

Implemented in `Handlers/StatsCommandHandler.cs` (queries + formatting in one place). Subcommands: `distribution <die>`, `crits` (d20), `top`, `streaks` (d20), `hours` (UTC). All scoped to the current guild via `GuildId`. Each subcommand has an optional `public:bool` flag — replies are ephemeral by default. Output is a code-block bar chart/table; truncated to ~1900 chars to stay under Discord's 2000-char limit.

`UserDisplayName` is snapshotted into each `Roll` row at insert time. Stats render the latest known display name per `UserId` (Discord snowflake is the stable key; names can change).

## Deployment notes

- DB lives at `/home/zarcton/dicebot-data/dicestats.db` on the Pi, set via `Environment=DICE_BOT_DB_PATH=...` in `/etc/systemd/system/discord-bot.service.d/override.conf`. Keep the DB **outside** `/home/zarcton/published-bot/` so re-publishing can never wipe it.
- The deploy script (`update_bot.sh` on the Pi) pulls, publishes, and `systemctl` cycles the service. The bot itself is named `discord-bot` in systemd.
- `Microsoft.EntityFrameworkCore.Sqlite` ships its own native `e_sqlite3.so` — no system `sqlite3` install needed for the bot. Install `sqlite3` only for CLI inspection.

## Stale doc

`pp.md` (privacy policy) currently states the bot stores no data permanently — outdated since stats persistence landed. Worth updating when convenient.
