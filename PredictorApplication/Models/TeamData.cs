using ScorePredictor.EventData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorePredictor
{
    public class TeamData
    {
        private readonly double averageWeight = 0.0065;

        private double attackRate;
        private double defenseRate;

        public TeamData(IEnumerable<TeamEvent> teamEvents, bool isHome = false)
        {
            if (isHome)
            {
                this.CalcRates(teamEvents);
                this.Rating = this.attackRate * this.defenseRate * this.CalcHomeEffect(teamEvents);
            }
            else
            {
                this.CalcRates(teamEvents);
                this.Rating = this.attackRate * this.defenseRate;
            }
        }

        public double Rating { get; private set; }

        private double CalcHomeEffect(IEnumerable<TeamEvent> teamEvents)
        {
            int homeAttackScores = 0;
            int homedefenseScores = 0;
            int awayAttackScores = 0;
            int awaydefenseScores = 0;
            int homeGames = 0;
            int awayGames = 0;

            foreach (TeamEvent teamEvent in teamEvents)
            {
                if (teamEvent.IsHomeGame)
                {
                    homeAttackScores += teamEvent.AttackScore;
                    homedefenseScores += teamEvent.DefenseScore;
                    homeGames++;
                }
                else
                {
                    awayAttackScores += teamEvent.AttackScore;
                    awaydefenseScores += teamEvent.DefenseScore;
                    awayGames++;
                }
            }

            float homeAttack = homeAttackScores / homeGames;
            float awayAttack = awayAttackScores / awayGames;
            float homedefense = homedefenseScores / homeGames;
            float awaydefense = awaydefenseScores / awayGames;

            return ((homeAttack / awayAttack) + (homedefense / awaydefense)) / 2;
        }

        private void CalcRates(IEnumerable<TeamEvent> teamEvents)
        {
            long currentDate = DateTime.Now.Ticks;
            double weightSum = 0;
            double weight;

            foreach (TeamEvent teamEvent in teamEvents)
            {
                weight = Math.Exp(-this.averageWeight * new TimeSpan(currentDate - teamEvent.Date.Ticks).TotalDays);
                weightSum += weight;
                this.attackRate += teamEvent.AttackScore * weight;
                this.defenseRate += teamEvent.DefenseScore * weight;
            }

            this.attackRate /= weightSum;
            this.defenseRate /= weightSum;
        }
    }
}
