using System;
using System.Linq;
using Calculator.Helpers.Interfaces;
using Calculator.Sms.Interfaces;

namespace Calculator.Sms
{
    public class TotalCostCalculator : ITotalCostCalculator
    {
        private readonly IAccountDetails _accountDetails;

        public TotalCostCalculator(IAccountDetails accountDetails)
        {
            _accountDetails = accountDetails;
        }

        public decimal CalculateCost(Guid customerAccount, int monthNo, int yearNo)
        {
            var totalTextMessages = _accountDetails.NumberOfTextMessagesSentInMonth(customerAccount, monthNo, yearNo);
            var priceBands = _accountDetails.GetAccountPriceBands(customerAccount);

            return priceBands
                .Where(priceBand => totalTextMessages >= priceBand.QtyFrom)
                .Select(priceBand =>
                {
                    var quantityToUse = priceBand.QtyTo ?? int.MaxValue;
                    var totalMessagesInPriceBand = totalTextMessages > quantityToUse
                        ? (quantityToUse - priceBand.QtyFrom) + 1
                        : (totalTextMessages - priceBand.QtyFrom) + 1;
                    return totalMessagesInPriceBand * priceBand.PricePerTextMessage;
                }).Sum();
        }
    }
}
