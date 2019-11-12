using System;
using System.Collections.Generic;

namespace Calculator.Helpers.Interfaces
{
    public interface IAccountDetails
    {
        /// <summary>
        /// Returns the number of text messages for the specified customer and month.
        /// </summary>
        /// <param name="customerAccount">Customer account ID</param>
        /// <param name="monthNo"></param>
        /// <param name="yearNo"></param>
        /// <returns>Number of the text messages sent.</returns>
        int NumberOfTextMessagesSentInMonth(Guid customerAccount, int monthNo, int yearNo);
        /// <summary>
        /// Used to get the price bands associated with the customer.
        /// </summary>
        /// <param name="customerAccount">Customer account ID</param>
        /// <returns>Price bands for the specified customer.</returns>
        IEnumerable<PriceBand> GetAccountPriceBands(Guid customerAccount);
    }
}