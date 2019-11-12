using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.Helpers;
using Calculator.Helpers.Interfaces;
using Calculator.Sms;
using FakeItEasy;
using Xunit;

namespace Calculator.Tests.Sms
{
    public class TotalCostCalculatorTests
    {
        private const int Month = 1;
        private const int Year = 2;

        private readonly Guid _customerAccount = new Guid("FB908C44-7AF1-4894-A0A5-860338468DFA");
        private readonly TotalCostCalculator _calculator;
        private readonly IAccountDetails _accountDetails;

        public TotalCostCalculatorTests()
        {
            _accountDetails = A.Fake<IAccountDetails>();
            A.CallTo(() =>
                _accountDetails.GetAccountPriceBands(_customerAccount)).Returns(GetAccountPriceBands());

            _calculator = new TotalCostCalculator(_accountDetails);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(200)]
        public void CalculateCost_FirstBand_ShouldReturnExpected(int textMessages)
        {
            // Assert
            var priceBands = GetAccountPriceBands().ToList();
            var priceBand = priceBands[0];

            A.CallTo(() => _accountDetails.NumberOfTextMessagesSentInMonth(_customerAccount, Month, Year))
                .Returns(textMessages);

            // Act
            var cost = _calculator.CalculateCost(_customerAccount, Month, Year);

            // Assert
            Assert.Equal(priceBand.PricePerTextMessage * textMessages, cost);
        }

        [Theory]
        [InlineData(201)]
        [InlineData(300)]
        [InlineData(500)]
        public void CalculateCost_SecondBand_ShouldReturnExpected(int textMessages)
        {
            // Assert
            var priceBands = GetAccountPriceBands().ToList();
            var firstPriceBand = priceBands[0];
            var secondPriceBand = priceBands[1];

            var firstBandCost = firstPriceBand.QtyTo * firstPriceBand.PricePerTextMessage;
            var secondBandCost = (textMessages - secondPriceBand.QtyFrom + 1) * secondPriceBand.PricePerTextMessage;
            var expectedCost = firstBandCost + secondBandCost;

            A.CallTo(() => _accountDetails.NumberOfTextMessagesSentInMonth(_customerAccount, Month, Year))
                .Returns(textMessages);

            // Act
            var cost = _calculator.CalculateCost(_customerAccount, Month, Year);

            // Assert
            Assert.Equal(expectedCost, cost);
        }

        [Theory]
        [InlineData(501)]
        [InlineData(700)]
        [InlineData(1000)]
        public void CalculateCost_ThirdBand_ShouldReturnExpected(int textMessages)
        {
            // Assert
            var priceBands = GetAccountPriceBands().ToList();
            var firstPriceBand = priceBands[0];
            var secondPriceBand = priceBands[1];
            var thirdPriceBand = priceBands[2];

            var firstBandCost = firstPriceBand.QtyTo * firstPriceBand.PricePerTextMessage;
            var secondBandCost = (secondPriceBand.QtyTo - secondPriceBand.QtyFrom + 1) * secondPriceBand.PricePerTextMessage;
            var thirdBandCost = (textMessages - thirdPriceBand.QtyFrom + 1) * thirdPriceBand.PricePerTextMessage;
            var expectedCost = firstBandCost + secondBandCost + thirdBandCost;

            A.CallTo(() => _accountDetails.NumberOfTextMessagesSentInMonth(_customerAccount, Month, Year))
                .Returns(textMessages);

            // Act
            var cost = _calculator.CalculateCost(_customerAccount, Month, Year);

            // Assert
            Assert.Equal(expectedCost, cost);
        }

        [Theory]
        [InlineData(1001)]
        [InlineData(10000)]
        [InlineData(int.MaxValue)]
        public void CalculateCost_FourthBand_ShouldReturnExpected(int textMessages)
        {
            // Assert
            var priceBands = GetAccountPriceBands().ToList();
            var firstPriceBand = priceBands[0];
            var secondPriceBand = priceBands[1];
            var thirdPriceBand = priceBands[2];
            var fourthPriceBand = priceBands[3];

            var firstBandCost = firstPriceBand.QtyTo * firstPriceBand.PricePerTextMessage;
            var secondBandCost = (secondPriceBand.QtyTo - secondPriceBand.QtyFrom + 1) * secondPriceBand.PricePerTextMessage;
            var thirdBandCost = (thirdPriceBand.QtyTo - thirdPriceBand.QtyFrom + 1) * thirdPriceBand.PricePerTextMessage;
            var fourthBandCost = (textMessages - fourthPriceBand.QtyFrom + 1) * fourthPriceBand.PricePerTextMessage;

            var expectedCost = firstBandCost + secondBandCost + thirdBandCost + fourthBandCost;

            A.CallTo(() => _accountDetails.NumberOfTextMessagesSentInMonth(_customerAccount, Month, Year))
                .Returns(textMessages);

            // Act
            var cost = _calculator.CalculateCost(_customerAccount, Month, Year);

            // Assert
            Assert.Equal(expectedCost, cost);
        }

        private static IEnumerable<PriceBand> GetAccountPriceBands()
        {
            return new List<PriceBand>
            {
                new PriceBand
                {
                    QtyFrom = 1,
                    QtyTo = 200,
                    PricePerTextMessage = 0.1m
                },
                new PriceBand
                {
                    QtyFrom = 201,
                    QtyTo = 500,
                    PricePerTextMessage = 0.08m
                },
                new PriceBand
                {
                    QtyFrom = 501,
                    QtyTo = 1000,
                    PricePerTextMessage = 0.06m
                },
                new PriceBand
                {
                    QtyFrom = 1001,
                    PricePerTextMessage = 0.03m
                }
            }.OrderBy(p => p.QtyFrom);
        }
    }
}
