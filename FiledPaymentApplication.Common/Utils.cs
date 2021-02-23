using System;
using System.Globalization;

namespace FiledPaymentApplication.Common
{
    public static class Utils
    {
        public static bool DateFormatValidator(string date)
        {
            bool dateValid = DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateConvert);

            bool returnDateResponse = false;
            if (dateValid)
            {
                returnDateResponse = IsDateInThePast(dateConvert);
            }

            return returnDateResponse;
        }

        public static bool IsDateInThePast(DateTime date)
        {
            return date >= DateTime.Today;
        }

        public static bool IsCardNumberValid(string cardNumber)
        {
            int i, checkSum = 0;
            for (i = cardNumber.Length - 1; i >= 0; i -= 2)
                checkSum += (cardNumber[i] - '0');
            for (i = cardNumber.Length - 2; i >= 0; i -= 2)
            {
                int val = ((cardNumber[i] - '0') * 2);
                while (val > 0)
                {
                    checkSum += (val % 10);
                    val /= 10;
                }
            }
            return ((checkSum % 10) == 0);
        }
    }
}
