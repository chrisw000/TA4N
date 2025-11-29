using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using NodaTime.Text;

/*
The MIT License (MIT)

Copyright (c) 2014-2016 Marc de Verdelhan & respective authors (see AUTHORS)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
namespace TA4N.Examples.Loaders
{
    using NodaTime;

    /// <summary>
    /// This class build a Ta4j time series from a CSV file containing ticks.
    /// </summary>
    public class CsvTicksLoader
    {
        public static void Main()
        {
            var series = CsvTicksLoader.LoadAppleIncSeries();

            Console.WriteLine("Series: " + series.Name + " (" + series.SeriesPeriodDescription + ")");
            Console.WriteLine("Number of ticks: " + series.TickCount);
            Console.WriteLine("First tick: \n" + "\tVolume: " + series.GetTick(0).Volume + "\n" + "\tOpen price: " + series.GetTick(0).OpenPrice + "\n" + "\tClose price: " + series.GetTick(0).ClosePrice);
        }

        /// <returns> a time series from Apple Inc. ticks.</returns>
        public static TimeSeries LoadAppleIncSeries()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = @"TA4N.Example.Resources.appleinc_ticks_from_20130101_usd.csv";

            List<dynamic> lines = null;
            var ticks = new List<Tick>();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                    using (var reader = new StreamReader(stream))
                    {
                        // Reading all lines of the CSV file using CsvHelper v30+ API
                        var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                        {
                            HasHeaderRecord = true
                        };
                        using (var csvReader = new CsvReader(reader, config))
                        {
                            lines = csvReader.GetRecords<dynamic>().ToList();
                        }
                    }
            }

            if (lines != null)
                foreach (var line in lines)
                {
                    var pattern = LocalDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd");

                    ParseResult<LocalDateTime> parseResult = pattern.Parse(line.date);
                    var date = parseResult.GetValueOrThrow();

                    double open = double.Parse(line.open);
                    double high = double.Parse(line.high);
                    double low = double.Parse(line.low);
                    double close = double.Parse(line.close);
                    double volume = double.Parse(line.volume);

                    ticks.Add(new Tick(date, open, high, low, close, volume));
                }

            return new TimeSeries("apple_ticks", ticks);
        }
    }
}
