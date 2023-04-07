namespace Trucks.Commnon
{
    public static class CheckValidator
    {
        public const int MaxRegistrationnumberLength = 8;

        public const int MaxVinNumberLength = 17;

        public const int MinVinNumberLength = 17;

        public const int DespetcherNameMaxLengtth = 40;

        public const int DespatcherNameMinLength = 2;

        public const int ClientMaxNameLength = 40;

        public const int ClientMinNameLength = 3;

        public const int NationalityMaxLnegth = 40;

        public const int NationalityMinLength = 2;

        public const string TruckNumberRegex = @"[A-Z]{2}\d{4}[A-Z]{2}$";

        public const int TankMinCapacity = 950;

        public const int TankMaxCapacity = 1420;

        public const int MinCargoCapacity = 5000;

        public const int MaxCargoCapacity = 29000;

        public const int MinCargoValue = 0;

        public const int MaxCargoValue = 3;

        public const int MinMakeTypeValue = 0;

        public const int MaxMaveTypeValue = 4;
    }
}
