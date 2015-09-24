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

namespace Samplers
{
    class GibbsSampler : Sampler
    {
        uint agentIDCounter;
        public void SetAgentCounter(uint cntr)
        {
            agentIDCounter = cntr;
        }
        public uint GetAgentCounter()
        {
            return agentIDCounter;
        }

        private ImportanceSampler myImportantSampler;
        private MetropolisHasting myMHSampler;

        public GibbsSampler()
        {
            warmupTime = 0;
            samplingInterval = 0;
            agentIDCounter = 0;
            myImportantSampler = new ImportanceSampler();
            myMHSampler = new MetropolisHasting();
            Initialze();
        }

        public GibbsSampler(int warmup, int samplingIntv)
        {
            warmupTime = warmup;
            samplingInterval = samplingIntv;
            agentIDCounter = 0;
            Initialze();
        }

        public GibbsSampler(int warmup, int samplingIntv, int randSeed)
        {
            warmupTime = warmup;
            samplingInterval = samplingIntv;
            agentIDCounter = 0;
            Initialze(randSeed);
        }

        // the method assumes that the conditionals are full conditionals
        // The data processing has already been done
        // [BF] The method should be changed so that it can generate any
        //      kind of agents
        public List<SimulationObject> GenerateAgents(SpatialZone currZone, int numAgents,
                        SimulationObject initAgent, bool warmUpStatus,
                        List<ConditionalDistribution> mobelCond,
						OutputFileWriter currHhldWriter, OutputFileWriter currPersWriter)
        {
			
            switch(initAgent.GetAgentType())
            {
                case AgentType.Household:
	                return GenerateHousholds(currZone, numAgents,
                                    (Household)initAgent, warmUpStatus,
					mobelCond, currHhldWriter);
                case AgentType.Person:
                    return GeneratePersons(currZone, numAgents,
                                    (Person)initAgent, warmUpStatus,
					currHhldWriter);
                case AgentType.HouseholdPersonComposite:
				return GenerateHousholdsComposite(currZone, numAgents,
                                    (HouseholdPersonComposite)initAgent, warmUpStatus,
					currHhldWriter,currPersWriter);
            }
            return null;
        }


        private List<SimulationObject> GenerateHousholdsComposite(SpatialZone currZone, int numHousehold,
                        HouseholdPersonComposite initAgent, bool warmUpStatus,
                        OutputFileWriter currWriterHhld, OutputFileWriter currWriterPers)
        {
            int seltdDim = 0;
			int seltAgnt = 0;

            List<ConditionalDistribution> condHhldList = currZone.GetDataHhldCompositeCollectionsListH();
			List<ConditionalDistribution> condPerList = currZone.GetDataHhldCompositeCollectionsListP();

            var generatedAgents = new List<SimulationObject>();
            HouseholdPersonComposite prevAgent = initAgent;
			ImportanceSampler currImpSampler = new ImportanceSampler();
			Random rnd = new Random();

            int iter = 0;
            if (warmUpStatus == true)
            {
                iter = Constants.WARMUP_ITERATIONS;
            }
            else
            {
                iter = Constants.SKIP_ITERATIONS * numHousehold;
            }
            HouseholdPersonComposite newAgent;
            StringBuilder builderHhld = new StringBuilder();
			StringBuilder builderPers = new StringBuilder();

            for (int i = 0; i < iter; i++)
            {
				// with equal probablity select one of the hhld or persons
				seltAgnt = randGen.NextInRange(0, prevAgent.getPersons().Count());
				//Change Hhld object
				if (seltAgnt == 0) {
					seltdDim = randGen.NextInRange(0, condHhldList.Count - 1);
					ConditionalDistribution currDist = condHhldList[seltdDim];

					var currComm = currDist.GetCommulativeValue (
						prevAgent
						, currZone, -1);
					newAgent = (HouseholdPersonComposite)GenerateNextAgent (currComm, prevAgent,
						currDist.GetDimensionName (), -1);
				}
				//Change person object
				else {
					seltdDim = randGen.NextInRange(0, condPerList.Count - 1);
					ConditionalDistribution currDist = condPerList[seltdDim];

					// Select randomly one person from the collection, whose attribute is changed
					int personId = rnd.Next(0, prevAgent.getPersons().Count - 1);

					// Importance sampling for the Age
					if (currDist.GetDimensionName() == "Age")
					{
						newAgent = (HouseholdPersonComposite) currImpSampler.GetNextAgent(
							currZone.GetPersonAgeMarginal(),
							currDist, currDist.GetDimensionName(),
							prevAgent, currZone, personId);
					}
					else
					{
						var currComm = currDist.GetCommulativeValue (
							prevAgent
							, currZone, personId);
						newAgent = (HouseholdPersonComposite)GenerateNextAgent (currComm, prevAgent,
							currDist.GetDimensionName (), personId);
					}
					//Consistency check
					((HouseholdPersonComposite) newAgent).CheckSexConsisteny(currZone.GetPersonSexMarginal());
                }

                prevAgent = newAgent;
                if (warmUpStatus == false && (i % Constants.SKIP_ITERATIONS == 0))
                {
                    generatedAgents.Add(newAgent);
					builderHhld.Append(newAgent.getHousehold().GetAgentID()); builderHhld.Append(',');
					builderHhld.Append(newAgent.getHousehold().GetZoneID()); builderHhld.Append(',');
					builderHhld.Append(',');
					builderHhld.Append((int)newAgent.getHousehold().GetDwellingType()); builderHhld.Append(',');
					builderHhld.Append((int)newAgent.getHousehold().GetHhldSize()); builderHhld.Append(',');
					builderHhld.Append((int)newAgent.getHousehold().GetNumOfKids()); builderHhld.Append(',');
					builderHhld.Append((int)newAgent.getHousehold().GetNumOfCars()); builderHhld.Append(',');
                    int personNumber = 1;

                    foreach (Person person in newAgent.getPersons())
                    {
						builderPers.Append((int)newAgent.getHousehold().GetAgentID()); builderPers.Append(',');
						builderPers.Append(personNumber++); builderPers.Append(',');
						builderPers.Append((int)person.GetContAge()); builderPers.Append(',');
						builderPers.Append((int)person.GetSex()); builderPers.Append(',');
						builderPers.Append((int)person.GetDrivingLicense()); builderPers.Append(',');
						builderPers.Append('0'); builderPers.Append(',');
						builderPers.Append((int)person.GetEmploymentStatus()); builderPers.Append(',');
						builderPers.Append((int)person.GetOccupation()); builderPers.Append(',');
						builderPers.Append("0,0,0,0");

						currWriterPers.WriteToFile(builderPers.ToString());
						builderPers.Clear ();
                    }
                    currWriterHhld.WriteToFile(builderHhld.ToString());
                    builderHhld.Clear();
                }
            }
            return generatedAgents;
        }

        private List<SimulationObject> GenerateHousholds(SpatialZone currZone, int numHousehold,
                      Household initAgent, bool warmUpStatus,
                      List<ConditionalDistribution> mobelCond,
                      OutputFileWriter currWriter)
        {
            int seltdDim = 0;
            List<ConditionalDistribution> condList = currZone.GetDataHhldCollectionsList();
            for (int i = 0; i < mobelCond.Count; i++)
            {
                condList.Add(mobelCond[i]);
            }
            var generatedAgents = new List<SimulationObject>();
            Household prevAgent = initAgent;

            ImportanceSampler currImpSampler = new ImportanceSampler();
            MetropolisHasting currMHSampler = new MetropolisHasting();
            int iter = 0;
            if (warmUpStatus == true)
            {
                iter = Constants.WARMUP_ITERATIONS;
            }
            else
            {
                iter = Constants.SKIP_ITERATIONS * numHousehold;
            }
            Household newAgent = new Household();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < iter; i++)
            {
                seltdDim = randGen.NextInRange(0, condList.Count - 1);

                ConditionalDistribution currDist = condList[seltdDim];

                // If the selected distribution is dwelling/cars
                // call important sampling

                /*if (currDist.GetDimensionName() == "DwellingType")
                {
                    newAgent = currImpSampler.GetNextAgent(currZone.myDwellMarginal,
                        currDist, currDist.GetDimensionName(), 
                        prevAgent, currZone);
                }
                else if (currDist.GetDimensionName() == "NumOfCars")
                {
                    newAgent = currImpSampler.GetNextAgent(currZone.myCarsMarginal,
                        currDist, currDist.GetDimensionName(),
                        prevAgent, currZone);
                }*/

                // If the selected distribution is income
                // call MH
                //                else if (((ConditionalDistribution)condList[seltdDim])
                //                                .GetDimensionName() == "IncomeLevel")
                //                {
                //                    newAgent = myMHSampler.GetNextAgent((ModelDistribution)currDist,
                //                            currDist.GetDimensionName(), prevAgent, currZone);
                //                }
                if (currDist.GetDimensionName() == "HouseholdSize")
                {
                    newAgent = (Household)currImpSampler.GetNextAgent(
                        currZone.GetHousholdSizeDist(),
                        currDist, currDist.GetDimensionName(),
                        prevAgent, currZone,0);
                }
                else
                {
                    var currComm = currDist.GetCommulativeValue(
                        prevAgent.GetNewJointKey(currDist.GetDimensionName())
                        , currZone);
                    newAgent = (Household)GenerateNextAgent(currComm, prevAgent,
                        currDist.GetDimensionName(),0);
                }

                prevAgent = newAgent;
                if (warmUpStatus == false && (i % Constants.SKIP_ITERATIONS == 0))
                {
                    generatedAgents.Add(newAgent);
                    uint currIncome = IncomeConvertor.GetEuroIncome((uint)
                                        newAgent.GetIncome());
                    builder.Append(newAgent.GetZoneID()); builder.Append(',');
                    builder.Append(currZone.GetEPFLName()); builder.Append(',');
                    builder.Append((int)newAgent.GetHhldSize()); builder.Append(',');
                    builder.Append((int)newAgent.GetNumOfWorkers()); builder.Append(',');
                    builder.Append((int)newAgent.GetNumOfKids()); builder.Append(',');
                    builder.Append((int)newAgent.GetNumOfUnivDegree()); builder.Append(',');
                    builder.Append((int)newAgent.GetIncomeLevel()); builder.Append(',');
                    builder.Append((int)newAgent.GetNumOfCars()); builder.Append(',');
                    builder.Append((int)newAgent.GetDwellingType());
                    currWriter.WriteToFile(builder.ToString());
                    builder.Clear();
                }
            }
            return generatedAgents;
        }

        private List<SimulationObject> GeneratePersons(SpatialZone currZone, int numPerson,
                            Person initAgent, bool warmUpStatus,
                            OutputFileWriter currWriter)
        {
            int seltdDim = 0;
            List<ConditionalDistribution> condList = currZone.GetPersonDataCollectionsList();
            var generatedAgents = new List<SimulationObject>();
            Person prevAgent = initAgent;
            ImportanceSampler currImpSampler = new ImportanceSampler();

            int iter = 0;
            if(warmUpStatus == true)
            {
                iter = Constants.WARMUP_ITERATIONS;
            }
            else
            {
                iter = Constants.SKIP_ITERATIONS * numPerson;
            }
            Person newAgent = new Person();
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < iter; i++)
            {
                seltdDim = randGen.NextInRange(0, condList.Count - 1);

                DiscreteCondDistribution currDist =
                    (DiscreteCondDistribution)condList[seltdDim];
                if(currDist.GetDimensionName() == "Sex")
                {
                    newAgent = (Person)currImpSampler.GetNextAgent(
                                currZone.mySexMarginal,
                                currDist, currDist.GetDimensionName(),
                                (SimulationObject)prevAgent, currZone,0);
                }
                /*else if (currDist.GetDimensionName() == "EducationLevel")
                {
                    newAgent = (Person)currImpSampler.GetNextAgent(
                                currZone.myEducationMarginal,
                                currDist, currDist.GetDimensionName(),
                                (SimulationObject)prevAgent, currZone);
                }*/
                else
                {
                    List<KeyValPair> currComm = currDist.GetCommulativeValue(
                         prevAgent.GetNewJointKey(currDist.GetDimensionName())
                            , currZone);
                    newAgent = (Person)GenerateNextAgent(currComm,
                            (SimulationObject)prevAgent,
                            currDist.GetDimensionName(),0);
                }

                prevAgent = newAgent;
                if(warmUpStatus == false && (i % Constants.SKIP_ITERATIONS == 0))
                {
                    //generatedAgents.Add(newAgent);
                    builder.Append((int)newAgent.GetAge()); builder.Append(',');
                    builder.Append(newAgent.GetZoneID()); builder.Append(',');
                    builder.Append((int)newAgent.GetSex()); builder.Append(',');
                    //builder.Append((int)newAgent.GetHhldSize()); builder.Append(',');
                    builder.Append((int)newAgent.GetEducationLevel());
                    currWriter.WriteToFile(builder.ToString());
                    builder.Clear();
                }
            }
            return generatedAgents;
        }

        // Should generate a deep copy of self
        private SimulationObject GenerateNextAgent(List<KeyValPair> curCom,
            SimulationObject prvAgnt, string genDim, int personId)
        {
			double currMax = (double)
                ((KeyValPair)curCom[curCom.Count - 1]).Value;
            if (currMax != 0.00)
            {
                double randVal = randGen.NextDoubleInRange(0, currMax);
                for (int i = 0; i < curCom.Count; i++)
                {
                    if (randVal <= ((KeyValPair)curCom[i]).Value)
                    {
                        SimulationObject copy = prvAgnt.CreateNewCopy(genDim, i, personId);

                        return copy;
                    }
                }
                throw new Exception();
            }
            else
            {
                return prvAgnt.CreateNewCopy(genDim,
                    randGen.NextInRange(0, (curCom.Count - 1)), personId);
            }
            return null;
        }

    }
}
