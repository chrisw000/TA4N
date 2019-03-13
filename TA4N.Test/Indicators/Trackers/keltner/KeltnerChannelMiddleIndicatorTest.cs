using System.Collections.Generic;

/// <summary>
/// The MIT License (MIT)
/// 
/// Copyright (c) 2014-2016 Marc de Verdelhan & respective authors (see AUTHORS)
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy of
/// this software and associated documentation files (the "Software"), to deal in
/// the Software without restriction, including without limitation the rights to
/// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
/// the Software, and to permit persons to whom the Software is furnished to do so,
/// subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
/// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
/// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
/// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/// </summary>
namespace TA4N.Test.Indicators.Trackers.Keltner
{
    using NUnit.Framework;
    using TA4N.Indicators.Simple;
    using TA4N.Mocks;
    using TA4N.Indicators.Trackers.Keltner;

    public sealed class KeltnerChannelMiddleIndicatorTest
	{
        private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
            IList<Tick> ticks = new List<Tick>();
			ticks.Add(new MockTick(11577.43, 11670.75, 11711.47, 11577.35));
			ticks.Add(new MockTick(11670.90, 11691.18, 11698.22, 11635.74));
			ticks.Add(new MockTick(11688.61, 11722.89, 11742.68, 11652.89));
			ticks.Add(new MockTick(11716.93, 11697.31, 11736.74, 11667.46));
			ticks.Add(new MockTick(11696.86, 11674.76, 11726.94, 11599.68));
			ticks.Add(new MockTick(11672.34, 11637.45, 11677.33, 11573.87));
			ticks.Add(new MockTick(11638.51, 11671.88, 11704.12, 11635.48));
			ticks.Add(new MockTick(11673.62, 11755.44, 11782.23, 11673.62));
			ticks.Add(new MockTick(11753.70, 11731.90, 11757.25, 11700.53));
			ticks.Add(new MockTick(11732.13, 11787.38, 11794.15, 11698.83));
			ticks.Add(new MockTick(11783.82, 11837.93, 11858.78, 11777.99));
			ticks.Add(new MockTick(11834.21, 11825.29, 11861.24, 11798.46));
			ticks.Add(new MockTick(11823.70, 11822.80, 11845.16, 11744.77));
			ticks.Add(new MockTick(11822.95, 11871.84, 11905.48, 11822.80));
			ticks.Add(new MockTick(11873.43, 11980.52, 11982.94, 11867.98));
			ticks.Add(new MockTick(11980.52, 11977.19, 11985.97, 11898.74));
			ticks.Add(new MockTick(11978.85, 11985.44, 12020.52, 11961.83));
			ticks.Add(new MockTick(11985.36, 11989.83, 12019.53, 11971.93));
			ticks.Add(new MockTick(11824.39, 11891.93, 11891.93, 11817.88));
			ticks.Add(new MockTick(11892.50, 12040.16, 12050.75, 11892.50));
			ticks.Add(new MockTick(12038.27, 12041.97, 12057.91, 12018.51));
			ticks.Add(new MockTick(12040.68, 12062.26, 12080.54, 11981.05));
			ticks.Add(new MockTick(12061.73, 12092.15, 12092.42, 12025.78));
			ticks.Add(new MockTick(12092.38, 12161.63, 12188.76, 12092.30));
			ticks.Add(new MockTick(12152.70, 12233.15, 12238.79, 12150.05));
			ticks.Add(new MockTick(12229.29, 12239.89, 12254.23, 12188.19));
			ticks.Add(new MockTick(12239.66, 12229.29, 12239.66, 12156.94));
			ticks.Add(new MockTick(12227.78, 12273.26, 12285.94, 12180.48));
			ticks.Add(new MockTick(12266.83, 12268.19, 12276.21, 12235.91));
			ticks.Add(new MockTick(12266.75, 12226.64, 12267.66, 12193.27));
			ticks.Add(new MockTick(12219.79, 12288.17, 12303.16, 12219.79));
			ticks.Add(new MockTick(12287.72, 12318.14, 12331.31, 12253.24));
			ticks.Add(new MockTick(12389.74, 12212.79, 12389.82, 12176.31));
			ticks.Add(new MockTick(12211.81, 12105.78, 12221.12, 12063.43));

			_data = new MockTimeSeries(ticks);
		}
        
        [Test] 
		public void keltnerChannelMiddleIndicatorTest()
		{
			var km = new KeltnerChannelMiddleIndicator(new ClosePriceIndicator(_data), 14);

			TaTestsUtils.AssertDecimalEquals(km.GetValue(13), 11751.376);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(14), 11781.9285);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(15), 11807.9634);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(16), 11831.6269);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(17), 11852.7207);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(18), 11857.9486);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(19), 11882.2434);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(20), 11903.5403);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(21), 11924.7029);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(22), 11947.0292);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(23), 11975.6427);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(24), 12009.977);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(25), 12040.632);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(26), 12065.7864);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(27), 12093.4496);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(28), 12116.7483);
			TaTestsUtils.AssertDecimalEquals(km.GetValue(29), 12131.4005);
		}
	}
}