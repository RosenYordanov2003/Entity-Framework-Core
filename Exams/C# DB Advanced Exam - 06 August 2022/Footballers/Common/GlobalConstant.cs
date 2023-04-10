namespace Footballers.Common
{
    public static class GlobalConstant
    {
        public const int MinFootballerNameLength = 2;

        public const int MaxFootballerNameLength = 40;

        public const int TeamNameMinLength = 3;

        public const int TeamNameMaxLength = 40;

        public const int TeamNationalityMinLength = 2;

        public const int TeamNationalityMaxLength = 40;

        public const string TeamNameRegex = @"^[a-zA-Z0-9 .-]{3,40}$";

        public const int CoachNameMinLength = 2;

        public const int CoachNameMaxLength = 40;

        public const int MinSkillTypeValue = 0;

        public const int MaxSkillTypeValue = 4;

        public const int MinPositionValue = 0;

        public const int MaxPositionValue = 3;
    }
}
