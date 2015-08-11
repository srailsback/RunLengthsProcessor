using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelExporter;

namespace RunLengthsProcessor
{
    class Program
    {
        private static INLogger _logger;
        private static IRepo _repo;
        private static OutputHelper _output;

        static void Main(string[] args)
        {
            try
            {

                // DI
                IKernel _kernal = new StandardKernel();
                _kernal.Bind<INLogger>().To<NLogger>().InSingletonScope();
                _kernal.Bind<IRepo>().To<Repo>().InSingletonScope();
                _kernal.Bind<IOutputHelper>().To<OutputHelper>().InSingletonScope();
                _logger = _kernal.Get<NLogger>();
                _repo = _kernal.Get<Repo>();
                _output = _kernal.Get<OutputHelper>();

                //ValidateRunLengths();
                var duplicates = ValidateIRIAVG();

                var export = new ExcelExport().AddSheet("Duplicates", duplicates.ToArray());
                export.ExportTo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), System.Configuration.ConfigurationManager.AppSettings["excel:exportFileName"].ToString()));
            }
            catch (Exception ex)
            {
                _output.Write(string.Format("Error: {0}", ex.Message), true);
            }
            Console.WriteLine("Done. Press any key to exist.");
            Console.ReadKey();
        }


        private static List<Condition> ValidateIRIAVG()
        {

            var duplicates = new List<Condition>();
            var conditions = _repo.GetConditions();
            var roadLimits = _repo.GetRoadLimits();
            foreach (var roadLimit in roadLimits)
            {
                _output.Write(string.Format("HWY {0}, DIR {1}, FROMMEASURE {2}, TOMEASURE {3}", roadLimit.HWY, roadLimit.DIR, roadLimit.FROMMEASURE, roadLimit.TOMEASURE));
                var collections = conditions.Where(x =>
                    x.HWY == roadLimit.HWY &&
                    x.DIR == roadLimit.DIR &&
                    x.FROMMEASURE >= roadLimit.FROMMEASURE &&
                    x.TOMEASURE <= roadLimit.TOMEASURE);

                foreach (var current in collections)
                {
                    var next = collections.GetNext<Condition>(current);
                    if (next != null)
                    {
                        if (current.IRIAVG == next.IRIAVG)
                        {
                            _output.Write(string.Format("Duplicate found!! ID {0}, HWY {1}, DIR {2}, FROMMEASURE {3}, TOMEASURE {4}", next.ID, next.HWY, next.DIR, next.FROMMEASURE, next.TOMEASURE));
                            duplicates.Add(current);
                            duplicates.Add(next);
                        }
                    }
                }
            }
            return duplicates;
        }

        private static void ValidateRunLengths()
        {
            // get conditions and roadlimits
            var conditions = _repo.GetConditions();
            var roadLimits = _repo.GetRoadLimits();

            foreach (var roadLimit in roadLimits)
            {
                var runs = conditions.Where(x =>
                    x.HWY == roadLimit.HWY &&
                    x.DIR == roadLimit.DIR &&
                    x.FROMMEASURE >= roadLimit.FROMMEASURE && 
                    x.TOMEASURE <= roadLimit.TOMEASURE);

                Console.WriteLine("HWY {0} DIR {1} FROMMEASURE {2} TOMEASURE {3} RUNS {4}", roadLimit.HWY, roadLimit.DIR, roadLimit.FROMMEASURE, roadLimit.TOMEASURE, runs.Count());

                if (runs.Count() > 0)
                {
                    var maxToMeasure = runs.Max(x => x.TOMEASURE);

                    foreach (var run in runs)
                    {
                        int fromMeasurePrecision = DecimalHelper.GetDecimalPlaces(run.FROMMEASURE);
                        int toMeasurePrecision = DecimalHelper.GetDecimalPlaces(run.TOMEASURE);

                        // all frommeasure should be 10ths w/ precision or 1000ths
                        if (fromMeasurePrecision > 1)
                        {
                            run.FROMMEASURE = Math.Round(run.FROMMEASURE, 1);
                        }

                        // all tomeasures should be 10ths w/ precision of 1000ths
                        // except for the last tomeasure in a run, it should be in the 1000ths
                        if (run.TOMEASURE < maxToMeasure)
                        {
                            // round these to 1/10ths
                            run.TOMEASURE = Math.Round(run.TOMEASURE, 1);
                        }
                        else
                        {
                            // round these to 1/1000ths
                            run.TOMEASURE = Math.Round(run.TOMEASURE, 3);
                        }

                        // update db
                        _repo.UpdateCondition(run);
                    }
                }
            }
        }
    }
}
