using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PredictorBalancer.Models
{
    public class PredictorList
    {
        private static readonly string FILE_LOCATION = @"C:\Predictors\predictors.json";
        private IList<Predictor> predictors;

        public PredictorList()
        {
            this.ReadPredictors();
            if (this.predictors == null)
            {
                this.predictors = new List<Predictor>();
            }
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
            this.SavePredictors();
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
            this.SavePredictors();
        }

        private bool ReadPredictors()
        {
            string json = string.Empty;

            try
            {
                FileInfo fileInf = new FileInfo(FILE_LOCATION);
                if (fileInf.Exists)
                {
                    using (StreamReader sr = fileInf.OpenText())
                    {
                        json = sr.ReadToEnd();
                    }
                }

                this.predictors = (List<Predictor>)JsonConvert.DeserializeObject(json, typeof(List<Predictor>));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        private bool SavePredictors()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this.predictors, Formatting.Indented);

                FileInfo fileInf = new FileInfo(FILE_LOCATION);
                if (fileInf.Exists)
                {
                    fileInf.Delete();
                }

                using (StreamWriter sw = fileInf.CreateText())
                {
                    sw.Write(json);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}