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
        private double defenceRate;

        public TeamData(IEnumerable<TeamEvent> teamEvents, bool isHome = false)
        {
            if (isHome)
            {
                CalcRates(teamEvents);
                Rating = attackRate * defenceRate * CalcHomeEffect(teamEvents);
            }
            else
            {
                CalcRates(teamEvents);
                Rating = attackRate * defenceRate;
            }
        }

        public double Rating { get; private set; }

        private double CalcHomeEffect(IEnumerable<TeamEvent> teamEvents)
        {
            int homeAttackScores = 0;
            int homeDefenceScores = 0;
            int awayAttackScores = 0;
            int awayDefenceScores = 0;
            int homeGames = 0;
            int awayGames = 0;

            foreach (TeamEvent teamEvent in teamEvents)
            {
                if (teamEvent.IsHomeGame)
                {
                    homeAttackScores += teamEvent.AttackScore;
                    homeDefenceScores += teamEvent.DefenceScore;
                    homeGames++;
                }
                else
                {
                    awayAttackScores += teamEvent.AttackScore;
                    awayDefenceScores += teamEvent.DefenceScore;
                    awayGames++;
                }
            }

            float homeAttack = homeAttackScores / homeGames;
            float awayAttack = awayAttackScores / awayGames;
            float homeDefence = homeDefenceScores / homeGames;
            float awayDefence = awayDefenceScores / awayGames;

            return ((homeAttack / awayAttack) + (homeDefence / awayDefence)) / 2;
        }

        private void CalcRates(IEnumerable<TeamEvent> teamEvents)
        {
            long currentDate = DateTime.Now.Ticks;
            double weightSum = 0;
            double weight;

            foreach (TeamEvent teamEvent in teamEvents)
            {
                weight = Math.Exp(-averageWeight * new TimeSpan(currentDate - teamEvent.Date.Ticks).TotalDays);
                weightSum += weight;
                attackRate += teamEvent.AttackScore * weight;
                defenceRate += teamEvent.DefenceScore * weight;
            }

            attackRate /= weightSum;
            defenceRate /= weightSum;
        }
    }
}
