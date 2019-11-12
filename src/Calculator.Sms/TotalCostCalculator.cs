using System;
using Calculator.Sms.Interfaces;

namespace Calculator.Sms
{
    public class TotalCostCalculator : ITotalCostCalculator
    {
        public decimal CalculateCost(Guid customerAccount, int monthNo, int yearNo)
        {
            throw new NotImplementedException();
        }
    }
}
