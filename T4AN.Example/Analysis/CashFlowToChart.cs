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
namespace ta4jexamples.analysis
{

	using Indicator = eu.verdelhan.ta4j.Indicator;
	using Strategy = eu.verdelhan.ta4j.Strategy;
	using Decimal = eu.verdelhan.ta4j.Decimal;
	using Tick = eu.verdelhan.ta4j.Tick;
	using TimeSeries = eu.verdelhan.ta4j.TimeSeries;
	using TradingRecord = eu.verdelhan.ta4j.TradingRecord;
	using CashFlow = eu.verdelhan.ta4j.analysis.CashFlow;
	using ClosePriceIndicator = eu.verdelhan.ta4j.indicators.simple.ClosePriceIndicator;
	using ChartFactory = org.jfree.chart.ChartFactory;
	using ChartPanel = org.jfree.chart.ChartPanel;
	using JFreeChart = org.jfree.chart.JFreeChart;
	using DateAxis = org.jfree.chart.axis.DateAxis;
	using NumberAxis = org.jfree.chart.axis.NumberAxis;
	using XYPlot = org.jfree.chart.plot.XYPlot;
	using StandardXYItemRenderer = org.jfree.chart.renderer.xy.StandardXYItemRenderer;
	using Minute = org.jfree.data.time.Minute;
	using TimeSeriesCollection = org.jfree.data.time.TimeSeriesCollection;
	using ApplicationFrame = org.jfree.ui.ApplicationFrame;
	using RefineryUtilities = org.jfree.ui.RefineryUtilities;
	using CsvTradesLoader = ta4jexamples.loaders.CsvTradesLoader;
	using MovingMomentumStrategy = ta4jexamples.strategies.MovingMomentumStrategy;

	/// <summary>
	/// This class builds a graphical chart showing the cash flow of a strategy.
	/// </summary>
	public class CashFlowToChart
	{

		/// <summary>
		/// Builds a JFreeChart time series from a Ta4j time series and an indicator. </summary>
		/// <param name="tickSeries"> the ta4j time series </param>
		/// <param name="indicator"> the indicator </param>
		/// <param name="name"> the name of the chart time series </param>
		/// <returns> the JFreeChart time series </returns>
		private static org.jfree.data.time.TimeSeries buildChartTimeSeries(TimeSeries tickSeries, Indicator<Decimal> indicator, string name)
		{
			org.jfree.data.time.TimeSeries chartTimeSeries = new org.jfree.data.time.TimeSeries(name);
			for (int i = 0; i < tickSeries.TickCount; i++)
			{
				Tick tick = tickSeries.getTick(i);
				chartTimeSeries.add(new Minute(tick.EndTime.toDate()), indicator.getValue(i).toDouble());
			}
			return chartTimeSeries;
		}

		/// <summary>
		/// Adds the cash flow axis to the plot. </summary>
		/// <param name="plot"> the plot </param>
		/// <param name="dataset"> the cash flow dataset </param>
		private static void addCashFlowAxis(XYPlot plot, TimeSeriesCollection dataset)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.jfree.chart.axis.NumberAxis cashAxis = new org.jfree.chart.axis.NumberAxis("Cash Flow Ratio");
			NumberAxis cashAxis = new NumberAxis("Cash Flow Ratio");
			cashAxis.AutoRangeIncludesZero = false;
			plot.setRangeAxis(1, cashAxis);
			plot.setDataset(1, dataset);
			plot.mapDatasetToRangeAxis(1, 1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.jfree.chart.renderer.xy.StandardXYItemRenderer cashFlowRenderer = new org.jfree.chart.renderer.xy.StandardXYItemRenderer();
			StandardXYItemRenderer cashFlowRenderer = new StandardXYItemRenderer();
			cashFlowRenderer.setSeriesPaint(0, Color.blue);
			plot.setRenderer(1, cashFlowRenderer);
		}

		/// <summary>
		/// Displays a chart in a frame. </summary>
		/// <param name="chart"> the chart to be displayed </param>
		private static void displayChart(JFreeChart chart)
		{
			// Chart panel
			ChartPanel panel = new ChartPanel(chart);
			panel.FillZoomRectangle = true;
			panel.MouseWheelEnabled = true;
			panel.PreferredSize = new Dimension(1024, 400);
			// Application frame
			ApplicationFrame frame = new ApplicationFrame("Ta4j example - Cash flow to chart");
			frame.ContentPane = panel;
			frame.pack();
			RefineryUtilities.centerFrameOnScreen(frame);
			frame.Visible = true;
		}

		public static void Main(string[] args)
		{

			// Getting the time series
			TimeSeries series = CsvTradesLoader.loadBitstampSeries();
			// Building the trading strategy
			Strategy strategy = MovingMomentumStrategy.buildStrategy(series);
			// Running the strategy
			TradingRecord tradingRecord = series.run(strategy);
			// Getting the cash flow of the resulting trades
			CashFlow cashFlow = new CashFlow(series, tradingRecord);

			/// <summary>
			/// Building chart datasets
			/// </summary>
			TimeSeriesCollection datasetAxis1 = new TimeSeriesCollection();
			datasetAxis1.addSeries(buildChartTimeSeries(series, new ClosePriceIndicator(series), "Bitstamp Bitcoin (BTC)"));
			TimeSeriesCollection datasetAxis2 = new TimeSeriesCollection();
			datasetAxis2.addSeries(buildChartTimeSeries(series, cashFlow, "Cash Flow"));

			/// <summary>
			/// Creating the chart
			/// </summary>
			JFreeChart chart = ChartFactory.createTimeSeriesChart("Bitstamp BTC", "Date", "Price", datasetAxis1, true, true, false); // generate URLs? -  generate tooltips? -  create legend? -  data -  y-axis label -  x-axis label -  title
			XYPlot plot = (XYPlot) chart.Plot;
			DateAxis axis = (DateAxis) plot.DomainAxis;
			axis.DateFormatOverride = new SimpleDateFormat("MM-dd HH:mm");

			/// <summary>
			/// Adding the cash flow axis (on the right)
			/// </summary>
			addCashFlowAxis(plot, datasetAxis2);

			/// <summary>
			/// Displaying the chart
			/// </summary>
			displayChart(chart);
		}
	}

}