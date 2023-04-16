namespace Invoices.Common
{
    public static class GlobalConstants
    {
        public const int ProductNameMingength = 9;

        public const int ProductNameMaxgength = 30;

        public const decimal ProductMinPrice = 5.00M;

        public const decimal ProductMaxPrice = 1000.00M;

        public const int StreetNameMinLength = 10;
        
        public const int StreetNameMaxLength = 20;

        public const int CityMinLength = 5;

        public const int CityMaxLength = 15;

        public const int CountryMinLength = 5;

        public const int CountryMaxLength = 15;

        public const int InvoiceNumberMinValue = 1000000000;

        public const int InvoiceNumberMaxValue = 1500000000;

        public const int MinCurrencyValue = 0;

        public const int MaxCurrencyValue = 2;

        public const int ClientNameMinLength = 10;

        public const int ClientNameMaxLength = 25;

        public const int NumberVatMinLength = 10;

        public const int NumberVatMaxLength = 15;

        public const int CategoryTypeMinValue = 0;

        public const int CategoryTypeMaxValue = 4;
    }
}
