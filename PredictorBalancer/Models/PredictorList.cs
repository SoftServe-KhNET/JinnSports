using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictorBalancer.Models
{
    public class PredictorList
    {
        private IList<Predictor> predictors;

        public PredictorList()
        {
            this.predictors = new List<Predictor>();
        }

        public void Add(string baseUrl, string controllerUrn, int timeoutSec)
        {
            int id = 0;

            if (this.predictors.Count > 0)
            {
                id = this.predictors.Max(p => p.Id) + 1;
            }

            Predictor predictor = new Predictor(baseUrl, controllerUrn, timeoutSec);
            predictor.Id = id;

            this.predictors.Add(predictor);
        }

        public ICollection<Predictor> GetAll()
        {
            return this.predictors;
        }

        public Predictor GetById(int id)
        {
            return this.predictors.FirstOrDefault(p => p.Id == id);
        }

        public void Delete(int id)
        {
            Predictor predictor = this.predictors.FirstOrDefault(p => p.Id == id);
            this.predictors.Remove(predictor);
        }
    }
}