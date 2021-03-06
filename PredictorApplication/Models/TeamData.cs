﻿using ScorePredictor.EventData;
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
                this.Rating = this.attackRate / this.defenseRate;
            }
        }

        public double Rating { get; private set; }

        private double CalcHomeEffect(IEnumerable<TeamEvent> teamEvents)
        {
            double homeAttackScores = 0;
            double homedefenseScores = 0;
            double awayAttackScores = 0;
            double awaydefenseScores = 0;
            int homeGames = 1;
            int awayGames = 1;

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

            double homeAttack = homeAttackScores / homeGames;
            double awayAttack = awayAttackScores / awayGames;
            double homedefense = homedefenseScores / homeGames;
            double awaydefense = awaydefenseScores / awayGames;

            // TODO: resolve division by zero
            awayAttack = awayAttack == 0 ? 0.1 : awayAttack;
            awaydefense = awaydefense == 0 ? 0.1 : awaydefense;

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
