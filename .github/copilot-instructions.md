# TA4N Copilot Instructions

## Project Overview
TA4N is a C# port of TA4J (a Java technical analysis library). It provides indicators, strategies, and analysis tools for algorithmic trading across financial markets (stocks, forex, crypto). Target frameworks: `.NET Framework 4.6.1` and `.NET Standard 2.0`.

## Architecture

### Core Data Model
The foundation consists of four interconnected types:

- **`Tick`** (`TA4N/Tick.cs`): A single time period's OHLCV data (open, high, low, close, volume). Defined by time period (1 day, 15 min, etc.) with `LocalDateTime` from NodaTime.
- **`TimeSeries`** (`TA4N/TimeSeries.cs`): An ordered sequence of `Tick` objects. Acts as the container for analysis and strategy execution. Can be sub-divided or capped at fixed size.
- **`Order`** (BUY/SELL types in `TA4N/Order.cs`): A trading action at a specific tick index with price and quantity.
- **`Trade`** (`TA4N/Trade.cs`): A pair of complementary orders (entry + exit). Entry type determines exit type (BUY→SELL, SELL→BUY).

### Processing Pipeline

1. **Load Data**: Create `TimeSeries` from ticks/trades (see `TA4N.Example/Loaders/CsvTicksLoader.cs`, `CsvTradesLoader.cs`)
2. **Build Indicators**: Implement `IIndicator<T>` to compute values across ticks
3. **Define Strategy**: Pair entry and exit `IRule` objects to create `Strategy`
4. **Run Strategy**: Call `timeSeries.Run(strategy)` → returns `TradingRecord` with all executed trades
5. **Analyze Results**: Apply `IAnalysisCriterion` implementations to `TradingRecord` for performance metrics

## Key Patterns

### Indicators (`TA4N/Indicators/`)
All indicators implement `IIndicator<T>` and extend `AbstractIndicator<T>` or `CachedIndicator<T>`:

- **`CachedIndicator<T>`**: Wraps calculation results in a list, avoiding recalculation. Extend and override `Calculate(int index)`.
  ```csharp
  public class SmaIndicator : CachedIndicator<Decimal> {
      protected override Decimal Calculate(int index) { /*compute SMA*/ }
  }
  ```
- Indicators accept other `IIndicator<T>` instances, enabling composition.
- Categories: `Trackers/` (SMA, EMA, RSI), `Oscillators/`, `Volatility/`, `Candles/`, `Volume/`, `Statistics/`, `Helpers/`.

### Rules & Strategies (`TA4N/Trading/Rules/`, `TA4N/Strategy.cs`)
- **`IRule`**: Evaluates to boolean at each tick. Supports `And()`, `Or()`, `Xor()`, `Negation()` combinators.
- **`Strategy`**: Pair of `IRule` (entry + exit) with optional `UnstablePeriod` to skip early signals.
- Rule types: `CrossedUpIndicatorRule`, `CrossedDownIndicatorRule`, `OverIndicatorRule`, `UnderIndicatorRule`, etc.
- Example (RSI2Strategy.cs): Combine SMA trend + RSI momentum for entry/exit signals.

### Analysis & Criteria (`TA4N/Analysis/`)
- **`TradingRecord`**: Accumulates orders/trades during strategy execution. Tracks entry/exit orders separately.
- **`IAnalysisCriterion`**: Computes performance metric (`double`) from `TimeSeries` + `TradingRecord`.
  - Implementations: `TotalProfitCriterion`, `NumberOfTradesCriterion`, `MaximumDrawdownCriterion`, etc.
  - `BetterThan(value1, value2)` allows ranking strategies.
- **`CashFlow`**: `IIndicator<Decimal>` tracking cumulative profit/loss per tick.

### JSON Serialization
- `Tick` and `TimeSeries` use `[JsonProperty]` annotations (Newtonsoft.Json).
- Only marked fields serialize; use `IJsonUtil` for custom logic if needed.

## Developer Workflows

### Build & Test
- **Build**: `dotnet build TA4N.sln` (or in Visual Studio)
- **Test**: `dotnet test TA4N.Test.csproj` (NUnit via NUnit3TestAdapter)
- **CI**: AppVeyor config (`appveyor.yml`) runs on every commit; NuGet package auto-generated on release

### Code Conventions
- Use `Decimal` type (custom wrapper in `TA4N/Decimal.cs`) for prices/percentages—avoids floating-point errors in trading math
- Logging via `LogWrapper.Factory?.CreateLogger<T>()` (Microsoft.Extensions.Logging)
- All classes MIT-licensed; preserve copyright headers
- Use `decimal` operations: `.Plus()`, `.DividedBy()`, `.Multiplied()`, `.Minus()` on `Decimal` instances

### Adding New Indicators
1. Create class extending `CachedIndicator<T>` in appropriate subfolder under `Indicators/`
2. Store dependent indicator in field, implement `Calculate(int index)`
3. Add tests to `TA4N.Test/Indicators/{Subfolder}/`

### Adding New Analysis Criteria
1. Extend `AbstractAnalysisCriterion` in `Analysis/Criteria/`
2. Implement `Calculate(TimeSeries, TradingRecord)` and `BetterThan(double, double)`
3. Example: `TotalProfitCriterion` sums close prices of all trades

## Integration Points

- **NodaTime**: Time representation (`LocalDateTime`, `Period`). Not DateTime—use conversions if needed.
- **Newtonsoft.Json**: Data serialization (configured at `TimeSeries` and `Tick` level)
- **NUnit**: Test framework; add tests alongside code in `TA4N.Test/`
- **External Data**: Load via CSV loaders (`TA4N.Example/Loaders/`); data format: `{DateTime},{Open},{High},{Low},{Close},{Volume}`

## Common Tasks

- **Create strategy**: Compose indicators → build `IRule` entry/exit conditions → wrap in `Strategy` (see `RSI2Strategy.cs`)
- **Backtest**: Load `TimeSeries`, run `strategy`, measure with criteria (see `StrategyAnalysis.cs`)
- **Walk-forward test**: Sub-divide `TimeSeries` by period, train/test segments (see `TA4N.Example/Walkforward/`)
