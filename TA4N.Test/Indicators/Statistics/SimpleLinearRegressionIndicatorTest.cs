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
using TA4N.Test.FixtureData;

namespace TA4N.Test.Indicators.Statistics
{
    using TA4N.Indicators.Simple;
    //	using SimpleRegression = org.apache.commons.math3.stat.regression.SimpleRegression;
    using TA4N.Indicators.Statistics;
    using NUnit.Framework;

    public sealed class SimpleLinearRegressionIndicatorTest
    {
        private double[] _data;
        private IIndicator<Decimal> _closePrice;

        [SetUp]
        public void SetUp()
        {
            _data = new double[]
            {
                10,
                20,
                30,
                40,
                30,
                40,
                30,
                20,
                30,
                50,
                60,
                70,
                80
            };
            _closePrice = new ClosePriceIndicator(GenerateTimeSeries.From(_data));
        }

        [Test]
        public void NotComputedLinearRegression()
        {
            var linearReg = new SimpleLinearRegressionIndicator(_closePrice, 0);
            Assert.That(linearReg.GetValue(0).NaN, Is.True);
            Assert.That(linearReg.GetValue(1).NaN, Is.True);
            Assert.That(linearReg.GetValue(2).NaN, Is.True);
            linearReg = new SimpleLinearRegressionIndicator(_closePrice, 1);
            Assert.That(linearReg.GetValue(0).NaN, Is.True);
            Assert.That(linearReg.GetValue(1).NaN, Is.True);
            Assert.That(linearReg.GetValue(2).NaN, Is.True);
        }

        [Test]
        public void CalculateLinearRegressionWithLessThan2ObservationsReturnsNaN()
        {
            var reg = new SimpleLinearRegressionIndicator(_closePrice, 0);
            Assert.That(reg.GetValue(0).NaN, Is.True);
            Assert.That(reg.GetValue(3).NaN, Is.True);
            Assert.That(reg.GetValue(6).NaN, Is.True);
            Assert.That(reg.GetValue(9).NaN, Is.True);
            reg = new SimpleLinearRegressionIndicator(_closePrice, 1);
            Assert.That(reg.GetValue(0).NaN, Is.True);
            Assert.That(reg.GetValue(3).NaN, Is.True);
            Assert.That(reg.GetValue(6).NaN, Is.True);
            Assert.That(reg.GetValue(9).NaN, Is.True);
        }

        [Test]
        public void CalculateLinearRegressionOn4Observations()
        {
            var reg = new SimpleLinearRegressionIndicator(_closePrice, 4);
            Assert.That(reg.GetValue(1), Is.EqualTo(Decimal.ValueOf(20)));
            Assert.That(reg.GetValue(2), Is.EqualTo(Decimal.ValueOf(30)));
            //			SimpleRegression origReg = buildSimpleRegression(10, 20, 30, 40);
            Assert.That(reg.GetValue(3), Is.EqualTo(Decimal.ValueOf(40)));
            //			TaTestsUtils.AssertDecimalEquals(reg.GetValue(3), origReg.predict(3));
            //			origReg = buildSimpleRegression(30, 40, 30, 40);
            Assert.That(reg.GetValue(5), Is.EqualTo(Decimal.ValueOf(38))); //origReg.predict(3));
            //			origReg = buildSimpleRegression(30, 20, 30, 50);
            Assert.That(reg.GetValue(9), Is.EqualTo(Decimal.ValueOf(43))); //origReg.predict(3));
        }

        [Test]
        public void CalculateLinearRegression()
        {
            var values = new double[]
            {
                1,
                2,
                1.3,
                3.75,
                2.25
            };
            var indicator = new ClosePriceIndicator(GenerateTimeSeries.From(values));
            var reg = new SimpleLinearRegressionIndicator(indicator, 5);
            //			SimpleRegression origReg = buildSimpleRegression(values);
            Assert.That(reg.GetValue(4).ToDouble(), Is.EqualTo(2.91).Within(TaTestsUtils.TaOffset)); //origReg.predict(4));
        }
    /// <param name = "values"> values </param>
    /// <returns> a simple linear regression based on provided values </returns>
     //private static SimpleRegression buildSimpleRegression(params double[] values)
    //{
    //	SimpleRegression simpleReg = new SimpleRegression();
    //	for (int i = 0; i < values.Length; i++)
    //	{
    //		simpleReg.addData(i, values[i]);
    //	}
    //	return simpleReg;
    //}
    }
}