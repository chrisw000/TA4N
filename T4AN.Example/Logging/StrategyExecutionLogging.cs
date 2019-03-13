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
namespace TA4N.Examples.logging
{

	using LoggerContext = ch.qos.logback.classic.LoggerContext;
	using JoranConfigurator = ch.qos.logback.classic.joran.JoranConfigurator;
	using JoranException = ch.qos.logback.core.joran.spi.JoranException;
	using Strategy = TA4N.Strategy;
	using TimeSeries = TA4N.TimeSeries;
	using LoggerFactory = org.slf4j.LoggerFactory;
	using CsvTradesLoader = TA4N.Examples.loaders.CsvTradesLoader;
	using CCICorrectionStrategy = TA4N.Examples.strategies.CCICorrectionStrategy;

	/// <summary>
	/// Strategy execution logging example.
	/// <para>
	/// </para>
	/// </summary>
	public class StrategyExecutionLogging
	{

		private static readonly URL LOGBACK_CONF_FILE = typeof(StrategyExecutionLogging).ClassLoader.getResource("logback-traces.xml");

		/// <summary>
		/// Loads the Logback configuration from a resource file.
		/// Only here to avoid polluting other examples with logs. Could be replaced by a simple logback.xml file in the resource folder.
		/// </summary>
		private static void loadLoggerConfiguration()
		{
			LoggerContext context = (LoggerContext) LoggerFactory.ILoggerFactory;
			context.reset();

			JoranConfigurator configurator = new JoranConfigurator();
			configurator.Context = context;
			try
			{
				configurator.doConfigure(LOGBACK_CONF_FILE);
			} catch (JoranException je)
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
				Logger.getLogger(typeof(StrategyExecutionLogging).FullName).log(Level.SEVERE, "Unable to load Logback configuration", je);
			}
		}

		public static void Main(string[] args)
		{
			// Loading the Logback configuration
			loadLoggerConfiguration();

			// Getting the time series
			TimeSeries series = CsvTradesLoader.loadBitstampSeries();

			// Building the trading strategy
			Strategy strategy = CCICorrectionStrategy.buildStrategy(series);

			// Running the strategy
			series.run(strategy);
		}
	}

}