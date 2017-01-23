using ScorePredictor.EventData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorePredictor
{
    public class Predictor
    {
        private TeamData homeTeam;
        private TeamData awayTeam;
        private int maxScore;

        public Predictor(TeamData homeTeam, TeamData awayTeam, int maxScore)
        {
            this.homeTeam = homeTeam;
            this.awayTeam = awayTeam;
            this.maxScore = maxScore;
        }

        public double HomeWinProbability { get; private set; }
        public double DrawProbability { get; private set; }
        public double AwayWinProbability { get; private set; }

        public void CalcProbabilities()
        {
            int step = maxScore / 5; // redusing steps number for high maxScores
            if (step == 0)
            {
                step = 1;
            }

            for (int i = 0; i <= maxScore; i += step)
            {
                for (int j = 0; j <= maxScore; j += step)
                {
                    if (i > j)
                    {
                        HomeWinProbability += CalcProbability(i, j);
                    }
                    else if (i < j)
                    {
                        AwayWinProbability += CalcProbability(i, j);
                    }
                    else if (i == j)
                    {
                        DrawProbability += CalcProbability(i, j);
                    }  
                }
            }

            NormalizeProbabilities();
        }

        private void NormalizeProbabilities()
        {
            double norm = 1 / (HomeWinProbability + DrawProbability + AwayWinProbability);
            HomeWinProbability *= norm;
            DrawProbability *= norm;
            AwayWinProbability *= norm;
        }

        private double CalcProbability(int firstScore, int secondScore)
        {
            Func<int, int> factorial = null;
            factorial = x => x <= 1 ? 1 : x * factorial(x - 1);

            double homeProbability = Math.Pow(homeTeam.Rating, firstScore) * Math.Exp(-homeTeam.Rating) / factorial(firstScore);
            double awayProbability = Math.Pow(awayTeam.Rating, secondScore) * Math.Exp(-awayTeam.Rating) / factorial(secondScore);

            return CalcCorrectingCoefficient(firstScore, secondScore) * homeProbability * awayProbability;
        }

        private double CalcCorrectingCoefficient(int firstScore, int secondScore)
        {
            double coefficient = 1.0;
            double dependenceParameter = Math.Max(-1 / homeTeam.Rating, -1 / awayTeam.Rating) + Math.Min(1 / (homeTeam.Rating * awayTeam.Rating), 1.0);

            if (firstScore == 0 && secondScore == 0)
            {
                coefficient = 1 - (homeTeam.Rating * awayTeam.Rating * dependenceParameter);
            }
            else if (firstScore == 0 && secondScore == 1)
            {
                coefficient = 1 + (homeTeam.Rating * dependenceParameter);
            }
            else if (firstScore == 1 && secondScore == 0)
            {
                coefficient = 1 + (awayTeam.Rating * dependenceParameter);
            }
            else if (firstScore == 1 && secondScore == 1)
            {
                coefficient = 1 - dependenceParameter;
            }

            return coefficient;
        }
    }
}
