namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Common;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PlayerStatistic
    {
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }

        public virtual Game Game { get; set; }


        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }


        [MaxLength(GlobalConstants.MaxScoredGoalsNumberPerGame)]
        public int ScoredGoals { get; set; }

        [MaxLength(GlobalConstants.MaxAssistsPerGameFromPlayer)]
        public int Assists { get; set; }

        [MaxLength(GlobalConstants.MaxMinutesPlayed)]
        public byte MinutesPlayed { get; set; }
    }
}
