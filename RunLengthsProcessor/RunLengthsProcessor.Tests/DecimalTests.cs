using System;
using Xunit;

namespace RunLengthsProcessor.Tests
{
    public class DecimalTests
    {
        [Fact]
        public void Is10ths()
        {
            // arrange 
            decimal value = 0.100m;

            // act 
            int decimals = DecimalHelper.GetDecimalPlaces(value);

            // assert
            Assert.Equal(1, decimals);
        }

        [Fact]
        public void Is100ths()
        {
            // arrange 
            decimal value = 0.12m;

            // act 
            int decimals = DecimalHelper.GetDecimalPlaces(value);

            // assert
            Assert.Equal(2, decimals);
        }

        [Fact]
        public void Is1000ths()
        {
            // arrange 
            decimal value = 0.123m;

            // act 
            int decimals = DecimalHelper.GetDecimalPlaces(value);

            // assert
            Assert.Equal(3, decimals);
        }

        [Fact]
        public void Is10thsPrecision()
        {
            // arrange 
            decimal value = 0.100m;

            // act 
            int decimals = DecimalHelper.GetDecimalPlaces(value);

            // assert
            Assert.Equal(1, decimals);
        }

        [Fact]
        public void Is100thsPrecision()
        {
            // arrange 
            decimal value = 0.1230m;

            // act 
            int decimals = DecimalHelper.GetDecimalPlaces(value);

            // assert
            Assert.Equal(3, decimals);
        }
    }
}
