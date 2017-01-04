using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JinnSports.BLL.Matcher
{
    public class NamingMatcher
    {
        private readonly IUnitOfWork unit;

        public NamingMatcher(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        /// <summary>
        /// Returns Team with given name in case this is exist in DB or it is brand new Team. In other cases adds naming conflicts to Conformities collection.
        /// </summary>
        /// <param name="inputTeam"></param>
        /// <returns></returns>
        public List<Conformity> ResolveNaming(Team inputTeam) //add typisation and locals
        {
            List<Conformity> conformities = new List<Conformity>();
            IEnumerable<Team> teams = this.unit.GetRepository<Team>().Get(filter: (t) => t.SportType.Name == inputTeam.SportType.Name, includeProperties: "Names,SportType");

            Team simpleCheckTeam = this.SimpleCheck(teams, inputTeam.Name);
            if (simpleCheckTeam != null)
            {
                return null;
            }

            string preparedInputName = inputTeam.Name.ToUpper().Replace(".", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);
            List<Team> matches = new List<Team>();

            double comparingResult = this.Compairing(teams, preparedInputName, out matches);

            if (comparingResult == 100 && matches.Count == 1)
            {
                TeamName newName = new TeamName();
                newName.Name = inputTeam.Name;
                matches[0].Names.Add(newName);
                this.unit.GetRepository<Team>().Update(matches[0]);
                this.unit.SaveChanges();
                return null;
            }
            else if (comparingResult < 50)
            {
                inputTeam.Names.Add(new TeamName { Name = inputTeam.Name });
                this.unit.GetRepository<Team>().Insert(inputTeam);
                this.unit.SaveChanges();
                return null;
            }
            else
            {
                foreach (Team team in matches)
                {
                    Conformity conf = new Conformity();
                    conf.InputName = inputTeam.Name;
                    conf.ExistedName = team.Name;
                    conformities.Add(conf);
                }
                                
                return conformities;
            }
        }

        private Team SimpleCheck(IEnumerable<Team> teams, string inputName)
        {
            
            foreach (Team team in teams)
            {
                foreach (TeamName teamName in team.Names)
                {
                    if (teamName.Name == inputName)
                    {
                        //exist in db;
                        return team;
                    }
                }
            }
            return null;
        }

        private double Compairing(IEnumerable<Team> teams, string inputName, out List<Team> matches)
        {
            List<Team> tempMatches = new List<Team>();
            double bestSuit = 0;

            foreach (Team team in teams)
            {
                foreach (TeamName teamName in team.Names)
                {
                    string preparedBaseName = teamName.Name.ToUpper().Replace("-", string.Empty).Replace(".", string.Empty).Replace(" ", string.Empty);
                    double numberOfSuitableLetters = this.LiteralComparing(preparedBaseName, inputName);

                    if (numberOfSuitableLetters == 0)
                    {
                        continue;
                    }

                    double currentSuit;

                    if (preparedBaseName.Length > inputName.Length)
                    {
                        currentSuit = numberOfSuitableLetters / inputName.Length * 100;
                    }
                    else
                    {
                        currentSuit = numberOfSuitableLetters / preparedBaseName.Length * 100;
                    }

                    if (currentSuit > bestSuit && currentSuit > 50)
                    {
                        tempMatches.Clear();
                        tempMatches.Add(team);
                        bestSuit = currentSuit;
                    }
                    else if (currentSuit == bestSuit && currentSuit > 50)
                    {
                        tempMatches.Add(team);
                    }
                }
            }
            matches = tempMatches;
            return bestSuit;
        }

        private int LiteralComparing(string baseName, string inputName)
        {
            if (baseName.Contains(inputName) || inputName.Contains(baseName))
            {
                return Math.Min(baseName.Length, inputName.Length);
            }

            int comparingResult = 0;
            int reversedComparingResult = 0;

            StringBuilder comparer = new StringBuilder();

            for (int i = 0; i < inputName.Length; i++)
            {
                comparer.Append(inputName[i]);

                if (!baseName.Contains(comparer.ToString()))
                {
                    comparer.Remove(comparer.Length - 1, 1);
                    break;
                }
            }

            comparingResult = comparer.Length;
            comparer.Clear();

            for (int i = inputName.Length - 1; i >= 0; i--)
            {
                comparer.Insert(0, inputName[i]);

                if (!baseName.Contains(comparer.ToString()))
                {
                    comparer.Remove(0, 1);
                    break;
                }
            }

            reversedComparingResult = comparer.Length;

            return Math.Max(comparingResult, reversedComparingResult);
        }
    }
}
