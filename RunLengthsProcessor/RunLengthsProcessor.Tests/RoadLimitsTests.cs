using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RunLengthsProcessor;
namespace RunLengthsProcessor.Tests
{
    public class RoadLimitsTests
    {

        [Fact]
        public void GetRoadLimit()
        {
            // arrange 
            var hwy = "036B";
            var dir = 2;
            decimal fromMeasure = 34.7m;
            decimal toMeasure = 57.494m;
            
            // act 
            var data = new Massive.DynamicModel("DefaultConnection").Query("SELECT HWY, DIR, FROMMEASURE, TOMEASURE FROM RoadLimits ORDER BY HWY, DIR, FROMMEASURE, TOMEASURE");
            var roadLimits = new List<RoadLimit>();
            foreach (var d in data)
            {
                roadLimits.Add(new RoadLimit()
                {
                    HWY = d.HWY.ToString(),
                    DIR = int.Parse(d.DIR.ToString()),
                    FROMMEASURE = decimal.Parse(d.FROMMEASURE.ToString()),
                    TOMEASURE = decimal.Parse(d.TOMEASURE.ToString())
                });
            }
            roadLimits = roadLimits.Where(x => x.HWY == hwy && x.DIR == dir && x.FROMMEASURE >= fromMeasure && x.TOMEASURE <= toMeasure).ToList();
//            Assert.Equal(dir, data.First().HWY);
            Assert.True(roadLimits.Count() == 1);
            Assert.Equal(roadLimits.First().FROMMEASURE, fromMeasure);
            Assert.Equal(roadLimits.First().TOMEASURE, toMeasure);

        }
    }
}
