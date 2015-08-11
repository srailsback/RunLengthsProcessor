using System;
using System.Collections.Generic;
using System.Linq;

namespace RunLengthsProcessor
{
    public interface IRepo
    {
        IList<Condition> GetConditions();
        IList<RoadLimit> GetRoadLimits();
        void UpdateCondition(Condition condition);
    }

    public class Repo : IRepo
    {
        private IOutputHelper _output;
        private string _connection;
        private string _roadLimitsTable;
        private string _conditionsTable;
        private string _roadLimitsFields;
        private string _conditionFields;
        private string _conditionsOrderBy;
        private string _roadLimitsOrderBy;

        public Repo(IOutputHelper outputHelper)
        {
            this._connection = "DefaultConnection";
            this._output = outputHelper;
            this._roadLimitsTable = GetSetting("tables:roadLimits");
            this._conditionsTable = GetSetting("tables:conditions");
            this._roadLimitsFields = GetSetting("tables:roadLimits:columns");
            this._conditionFields = GetSetting("tables:conditions:columns");
            this._conditionsOrderBy = GetSetting("tables:conditions:orderBy");
            this._roadLimitsOrderBy = GetSetting("tables:roadLimits:orderBy");
        }

        private string GetSetting(string p)
        {
            return System.Configuration.ConfigurationManager.AppSettings[p].ToString();
        }

        public IList<Condition> GetConditions()
        {
            _output.Write("Getting all conditions");
            var sql = string.Format("SELECT {0} FROM {1} ORDER BY {2}", this._conditionFields, this._conditionsTable, this._conditionsOrderBy);
            var data = new Massive.DynamicModel(this._connection).Query(sql);
            var conditions = new List<Condition>();
            foreach(var d in data) {
                conditions.Add(new Condition() {
                    ID = int.Parse(d.ID.ToString()),
                    HWY = d.HWY.ToUpper().Trim(),
                    DIR = int.Parse(d.DIR.ToString()),
                    FROMMEASURE = decimal.Parse(d.FROMMEASURE.ToString()),
                    TOMEASURE = decimal.Parse(d.TOMEASURE.ToString()),
                    IRIAVG = decimal.Parse(d.IRIAVG.ToString())
                });
            }
            return conditions;
        }

        public IList<RoadLimit> GetRoadLimits()
        {
            _output.Write("Getting all road limits");
            var sql = string.Format("SELECT {0} FROM {1} ORDER BY {2}", this._roadLimitsFields, this._roadLimitsTable, this._roadLimitsOrderBy);
            var data = new Massive.DynamicModel(this._connection).Query(sql);
            var roadLimits = new List<RoadLimit>();
            foreach(var d in data) {
                roadLimits.Add(new RoadLimit() {
                    HWY = d.HWY.ToUpper().Trim(),
                    DIR = int.Parse(d.DIR.ToString()),
                    FROMMEASURE = decimal.Parse(d.FROMMEASURE.ToString()),
                    TOMEASURE = decimal.Parse(d.TOMEASURE.ToString())
                });
            }
            return roadLimits;
        }

        public void UpdateCondition(Condition condition)
        {
            _output.WriteLog(string.Format("Updating condition ID: {0}, HWY: {1}, DIR: {2}, FROMMEASURE: {3}, TOMEASURE: {4}",
                condition.ID.ToString(),
                condition.HWY, condition.DIR.ToString(),
                condition.FROMMEASURE.ToString(),
                condition.TOMEASURE.ToString()));

            var args = new List<object>();
            args.Add(condition.FROMMEASURE);
            args.Add(condition.TOMEASURE);
            args.Add(condition.ID);

            var placeholders = new List<string>();
            for (int i = 0; i < args.Count(); i++)
            {

                placeholders.Add(string.Format("@{0}", i));
            }

            var sql = "UPDATE Condition2015 SET FROMMEASURE = @0, TOMEASURE = @1 WHERE ID = @2";

            Massive.DynamicModel
                .Open(this._connection)
                .Execute(sql, args.ToArray());
        }

    }
}
