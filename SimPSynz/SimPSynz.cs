/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimulationObjects;
using PopulationSynthesis.Utils;
using System.Diagnostics;
//using IPF;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {

            AgentType curTyp = new AgentType();
            curTyp = AgentType.HouseholdPersonComposite;
            CreateUsingSimulation(curTyp);

            Console.ReadLine();
        }


        private static void CreateUsingSimulation(AgentType currType)
        {
            if (currType == AgentType.Household)
            {
                CreateHhldViaSimulation();
            }
            else if (currType == AgentType.Person)
            {
                CreatePersonViaSimulation();
            }
            else if (currType == AgentType.HouseholdPersonComposite)
            {
               CreateHhldPersonCompositeViaSimulation();
            }
        }

        private static void CreateHhldPersonCompositeViaSimulation()
        {
            World currWorld = new World();
            Stopwatch watch = Stopwatch.StartNew();
            currWorld.Initialize(true, AgentType.HouseholdPersonComposite);
            watch.Stop();
            Console.WriteLine("Initialization: " + watch.ElapsedMilliseconds + "ms");
            watch.Restart();
            currWorld.CreateHoseholdCompositePopulationPool(
                Constants.DATA_DIR + "\\HouseholdComposite\\SyntheticHhldComposite.csv");
            watch.Stop();
            Console.WriteLine("Create Household Composite Population Pool: " + watch.ElapsedMilliseconds + "ms");
        }

        private static void CreatePersonViaSimulation()
        {
            World currWorld = new World();
            Stopwatch watch = Stopwatch.StartNew();
            currWorld.Initialize(true,AgentType.Person);
            watch.Stop();
            Console.WriteLine("Initialization: " + watch.ElapsedMilliseconds + "ms");
            watch.Restart();
            currWorld.CreatePersonPopulationPool(
                Constants.DATA_DIR + "\\Person\\SyntheticPerson_AgeHhldSizeEdu.csv");
            watch.Stop();
            Console.WriteLine("Create Person Population Pool: " + watch.ElapsedMilliseconds + "ms");
        }

        private static void CreateHhldViaSimulation()
        {
            World currWorld = new World();
            
            currWorld.Initialize(false,AgentType.Household);

            var runsListZero = new List<Dictionary<string, World.ZonalStat>>();
            var runsListOne = new List<Dictionary<string, World.ZonalStat>>();
            var runsListTwo = new List<Dictionary<string, World.ZonalStat>>();
            var runsListThree = new List<Dictionary<string, World.ZonalStat>>();

            for (int i = 0; i < 50; i++)
            {
                var CurrTotals = currWorld.ComputeCommuneMCStatsCars(
                                                i, (int) DateTime.Now.Ticks,
                                                Constants.DATA_DIR
                                     + "Household\\SyntheticHhld.csv", false);
                runsListZero.Add(CurrTotals[0]);
                runsListOne.Add(CurrTotals[1]);
                runsListTwo.Add(CurrTotals[2]);
                runsListThree.Add(CurrTotals[3]);
            }

            World.WriteMCStatsToFile(Constants.DATA_DIR +
                        "Household\\CommuneList.csv", runsListZero, 0);
            World.WriteMCStatsToFile(Constants.DATA_DIR +
                        "Household\\CommuneList.csv", runsListOne, 1);
            World.WriteMCStatsToFile(Constants.DATA_DIR +
                        "Household\\CommuneList.csv", runsListTwo, 2);
            World.WriteMCStatsToFile(Constants.DATA_DIR +
                        "Household\\CommuneList.csv", runsListThree, 3);
        }

        private static void CreateConditionalsFromSample()
        {
            ConditionalGenerator currGen 
                = new ConditionalGenerator();
            currGen.GenerateConditionals(
                Constants.DATA_DIR + "Person\\ConditionalExperiments"
                + "\\CH1004_Population_Census2000.csv",
                Constants.DATA_DIR + "Person\\ConditionalExperiments"
                +"\\CensusDimDesc.csv");
        }
    }
}
