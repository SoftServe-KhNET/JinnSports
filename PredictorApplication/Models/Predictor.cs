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
            int step = this.maxScore / 5; // redusing steps number for high maxScores
            if (step == 0)
            {
                step = 1;
            }

            for (int i = 0; i <= this.maxScore; i += step)
            {
                for (int j = 0; j <= this.maxScore; j += step)
                {
                    if (i > j)
                    {
                        this.HomeWinProbability += this.CalcProbability(i, j);
                    }
                    else if (i < j)
                    {
                        this.AwayWinProbability += this.CalcProbability(i, j);
                    }
                    else if (i == j)
                    {
                        this.DrawProbability += this.CalcProbability(i, j);
                    }  
                }
            }

            this.NormalizeProbabilities();
        }

        private void NormalizeProbabilities()
        {
            double norm = 1 / (this.HomeWinProbability + this.DrawProbability + this.AwayWinProbability);
            this.HomeWinProbability *= norm;
            this.DrawProbability *= norm;
            this.AwayWinProbability *= norm;
        }

        private double CalcProbability(int firstScore, int secondScore)
        {
            Func<int, int> factorial = null;
            factorial = x => x <= 1 ? 1 : x * factorial(x - 1);

            double homeProbability = Math.Pow(this.homeTeam.Rating, firstScore) * Math.Exp(-this.homeTeam.Rating) / factorial(firstScore);
            double awayProbability = Math.Pow(this.awayTeam.Rating, secondScore) * Math.Exp(-this.awayTeam.Rating) / factorial(secondScore);

            return this.CalcCorrectingCoefficient(firstScore, secondScore) * homeProbability * awayProbability;
        }

        private double CalcCorrectingCoefficient(int firstScore, int secondScore)
        {
            double coefficient = 1.0;
            double dependenceParameter = Math.Max(-1 / this.homeTeam.Rating, -1 / this.awayTeam.Rating) + 
                                            Math.Min(1 / (this.homeTeam.Rating * this.awayTeam.Rating), 1.0);

            if (firstScore == 0 && secondScore == 0)
            {
                coefficient = 1 - (this.homeTeam.Rating * this.awayTeam.Rating * dependenceParameter);
            }
            else if (firstScore == 0 && secondScore == 1)
            {
                coefficient = 1 + (this.homeTeam.Rating * dependenceParameter);
            }
            else if (firstScore == 1 && secondScore == 0)
            {
                coefficient = 1 + (this.awayTeam.Rating * dependenceParameter);
            }
            else if (firstScore == 1 && secondScore == 1)
            {
                coefficient = 1 - dependenceParameter;
            }

            return coefficient;
        }
    }
}
