using System;

namespace Calculator.Sms.Interfaces
{
    public interface ITotalCostCalculator
    {
        /// <summary>
        /// Calculates the total cost for the month from text message usage
        /// </summary>
        /// <param name="customerAccount">The identifier of the customer account</param>
        /// <param name="monthNo">Number of the month</param>
        /// <param name="yearNo">Number of the year</param>
        /// <returns></returns>
        decimal CalculateCost(Guid customerAccount, int monthNo, int yearNo);
    }
}
