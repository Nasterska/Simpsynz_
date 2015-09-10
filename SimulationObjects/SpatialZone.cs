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

using PopulationSynthesis.Utils;

namespace SimulationObjects
{
    class SpatialZone
    {
        private string myName;
        public void SetName(string myNm)
        {
            myName = myNm;
        }
        public string GetName()
        {
            return myName;
        }

        private string myEPFLName;
        public void SetEPFLName(string myNm)
        {
            myEPFLName = myNm;
        }
        public string GetEPFLName()
        {
            return myEPFLName;
        }

        private double surf1;
        public void SetSurfaceOne(double s1)
        {
            surf1 = s1;
        }
        public double GetSurfaceOne()
        {
            return surf1;
        }

        private double surf2;
        public void SetSurfaceTwo(double s2)
        {
            surf2 = s2;
        }
        public double GetSurfaceTwo()
        {
            return surf2;
        }

        private double surf3;
        public void SetSurfaceThree(double s3)
        {
            surf3 = s3;
        }
        public double GetSurfaceThree()
        {
            return surf3;
        }

        private double surf4;
        public void SetSurfaceFour(double s4)
        {
            surf4 = s4;
        }
        public double GetSurfaceFour()
        {
            return surf4;
        }
        //private double myPerUntChosen;
        //public void SetPercUnitsChosen(double val)
        //{
        //    myPerUntChosen = val;
        //}
        public double GetApartmentPercent()
        {
            return (double) myDwellMarginal.GetValue("3")
                / ((double)myDwellMarginal.GetValue("0")
                + (double)myDwellMarginal.GetValue("1")
                + (double) myDwellMarginal.GetValue("2")
                + (double)myDwellMarginal.GetValue("3"));
        }

        ///////////////////////////////////
        // For Household Synthesis

        //public DiscreteCondDistribution censusPersonConditionals;

        public ModelDistribution modelUnivDegConditionals;
        public ModelDistribution modelIncConditionals;
        public ModelDistribution modelCarsConditionals;
        public ModelDistribution modelDwellConditionals;
        public ModelDistribution modelNumberOfPeopleConditionals; //
        public ModelDistribution modelKidsConditionals;//
        public ModelDistribution modelHouseHoldTypeConditionals;//

         
        public DiscreteMarginalDistribution myCarsMarginal;
        public DiscreteMarginalDistribution myDwellMarginal;
        public DiscreteMarginalDistribution myDwellMarginalCounts;
        public DiscreteMarginalDistribution myPersonMarginal;
        ///////////////////////////////////

        ///////////////////////////////////
        // For Person Synthesis
        public DiscreteMarginalDistribution myHhldSize2Marginal;
        public DiscreteMarginalDistribution mySexMarginal;
        public DiscreteMarginalDistribution myAgeMarginal;
        public DiscreteMarginalDistribution myEducationMarginal;
        public DiscreteMarginalDistribution myOccupationMarginal;

        public DiscreteCondDistribution myAgeConditional;
        public DiscreteCondDistribution mySexConditional;
        public DiscreteCondDistribution myHhldSizeConditional;
        public DiscreteCondDistribution myEduLevelConditional;

        public ModelDistribution modelAgeConditionals;//
      //  public ModelDistribution modelPublicTransitConditionals;//
        public ModelDistribution modelOccupationConditionals;//
        public ModelDistribution modelSexConditionals;//
        public ModelDistribution modelDriverLicenseConditionals;//
        public ModelDistribution modelEmploymentStatus;

        ///////////////////////////////////

        public SpatialZone()
        {
            ///////////////////////////////////
            // For Household Synthesis

            //censusPersonConditionals = new DiscreteCondDistribution();

            modelIncConditionals = new ModelDistribution();
            modelIncConditionals.SetDimensionName("IncomeLevel");
            modelUnivDegConditionals = new ModelDistribution();
            modelUnivDegConditionals.SetDimensionName("NumWithUnivDeg");
            modelDwellConditionals = new ModelDistribution();
            modelDwellConditionals.SetDimensionName("DwellingType");
            modelCarsConditionals = new ModelDistribution();
            modelCarsConditionals.SetDimensionName("NumOfCars");
            modelNumberOfPeopleConditionals = new ModelDistribution();
            modelNumberOfPeopleConditionals.SetDimensionName("NumOfPeople");
            modelKidsConditionals = new ModelDistribution();
            modelKidsConditionals.SetDimensionName("NumOfKids");
            modelHouseHoldTypeConditionals = new ModelDistribution();
            modelHouseHoldTypeConditionals.SetDimensionName("HouseholdSize");
            

            myDwellMarginal = new DiscreteMarginalDistribution();
            myDwellMarginal.SetDimensionName("DwellingType");

            myDwellMarginalCounts = new DiscreteMarginalDistribution();
            myDwellMarginalCounts.SetDimensionName("DwellingType");

            myCarsMarginal = new DiscreteMarginalDistribution();
            myCarsMarginal.SetDimensionName("NumOfCars");

            myPersonMarginal = new DiscreteMarginalDistribution();
            myPersonMarginal.SetDimensionName("HouseholdSize");
            ///////////////////////////////////

            ///////////////////////////////////
            // For Person Synthesis
           /* modelPublicTransitConditionals = new ModelDistribution();
            modelPublicTransitConditionals.SetDimensionName("PublicTransitPass");*/

            myHhldSize2Marginal = new DiscreteMarginalDistribution();
            myHhldSize2Marginal.SetDimensionName("HouseholdSize2");

            mySexMarginal = new DiscreteMarginalDistribution();
            mySexMarginal.SetDimensionName("Sex");

            myAgeMarginal = new DiscreteMarginalDistribution();
            myAgeMarginal.SetDimensionName("Age");

            myEducationMarginal = new DiscreteMarginalDistribution();
            myEducationMarginal.SetDimensionName("EducationLevel");

            myOccupationMarginal = new DiscreteMarginalDistribution();
            myOccupationMarginal.SetDimensionName("Occupation");

            myAgeConditional = new DiscreteCondDistribution();
            myAgeConditional.SetDimensionName("Age");

            mySexConditional = new DiscreteCondDistribution();
            mySexConditional.SetDimensionName("Sex");

            myHhldSizeConditional = new DiscreteCondDistribution();
            myHhldSizeConditional.SetDimensionName("HouseholdSize2");

            myEduLevelConditional = new DiscreteCondDistribution();
            myEduLevelConditional.SetDimensionName("EducationLevel");

            modelAgeConditionals = new ModelDistribution();//
            modelAgeConditionals.SetDimensionName("Age");

            modelEmploymentStatus = new ModelDistribution();//
            modelEmploymentStatus.SetDimensionName("EmploymentStatus");

         /*   modelPublicTransitConditionals = new ModelDistribution();//
            modelPublicTransitConditionals.SetDimensionName("PublicTransitPass");*/

            modelOccupationConditionals = new ModelDistribution();//
            modelOccupationConditionals.SetDimensionName("Occupation");

            modelSexConditionals = new ModelDistribution();//
            modelSexConditionals.SetDimensionName("Sex");

            modelDriverLicenseConditionals = new ModelDistribution();//
            modelDriverLicenseConditionals.SetDimensionName("DrivingLicense");
        }

        private double averageIncome;
        public double GetAverageIncome()
        {
            return averageIncome;
        }
        public void SetAverageIncome(double avgInc)
        {
            averageIncome = avgInc;
        }
        private double percentHighEducated;
        public double GetPercentHighEducated()
        {
            return percentHighEducated;
        }
        public void SetPercentHighEducated(double perHighEdu)
        {
            percentHighEducated = perHighEdu;
        }
        private KeyValPair hhldControlTotal;
        public void SetHhldControlTotal(string key, uint val)
        {
            hhldControlTotal.Category = key;
            hhldControlTotal.Value = val;
        }

		public List<ConditionalDistribution> GetDataHhldCompositeCollectionsListP()
		{
			List<ConditionalDistribution> currColl = new List<ConditionalDistribution>();

			currColl.Add(modelEmploymentStatus);
			currColl.Add(modelAgeConditionals);
			//   currColl.Add(modelPublicTransitConditionals);
			currColl.Add(modelOccupationConditionals);
			currColl.Add(modelSexConditionals);
			currColl.Add(modelDriverLicenseConditionals);
			return currColl;
		}

        public List<ConditionalDistribution> GetDataHhldCompositeCollectionsListH()
        {
            List<ConditionalDistribution> currColl = new List<ConditionalDistribution>();
         //   currColl.Add(modelIncConditionals);
         //   currColl.Add(modelUnivDegConditionals);
            currColl.Add(modelDwellConditionals);
            currColl.Add(modelCarsConditionals);
         // currColl.Add(modelNumberOfPeopleConditionals); 
            currColl.Add(modelKidsConditionals);
         //   currColl.Add(modelHouseHoldTypeConditionals);           
            return currColl;
        }

		public List<ConditionalDistribution> GetDataHhldCompositeCollectionsListPH()
		{
			List<ConditionalDistribution> currColl = new List<ConditionalDistribution>();
			//   currColl.Add(modelIncConditionals);
			//   currColl.Add(modelUnivDegConditionals);
			currColl.Add(modelDwellConditionals);
			currColl.Add(modelCarsConditionals);
			// currColl.Add(modelNumberOfPeopleConditionals); 
			currColl.Add(modelKidsConditionals);
			//   currColl.Add(modelHouseHoldTypeConditionals);           


			currColl.Add(modelEmploymentStatus);
			currColl.Add(modelAgeConditionals);
			//   currColl.Add(modelPublicTransitConditionals);
			currColl.Add(modelOccupationConditionals);
			currColl.Add(modelSexConditionals);
			currColl.Add(modelDriverLicenseConditionals);
			return currColl;
		}

        public List<ConditionalDistribution> GetDataHhldCollectionsList()
        {
            List<ConditionalDistribution> currColl = new List<ConditionalDistribution>();
            currColl.Add(modelIncConditionals);
            currColl.Add(modelUnivDegConditionals);
            currColl.Add(modelDwellConditionals);
            currColl.Add(modelCarsConditionals);
            return currColl;
        }

        public List<ConditionalDistribution> GetPersonDataCollectionsList()
        {
            List<ConditionalDistribution> currColl = new List<ConditionalDistribution>();
            currColl.Add(myAgeConditional);
            currColl.Add(mySexConditional);
            currColl.Add(myHhldSizeConditional);
            currColl.Add(myEduLevelConditional);
            return currColl;
        }

        public DiscreteMarginalDistribution GetHousholdSizeDist()
        {
            return myPersonMarginal;
        }
        public int GetNumHhldWOneCar()
        {
            return (int) myCarsMarginal.GetValue("1");
        }
        public int GetNumHhldWTwoCar()
        {
            return (int) myCarsMarginal.GetValue("2");
        }
        public int GetNumHhldWThreeCar()
        {
            return (int) myCarsMarginal.GetValue("3");
        }

        public double GetPercentHhldWOneCar()
        {
            double sum = myCarsMarginal.GetValue("1")
                    + myCarsMarginal.GetValue("2")
                    + myCarsMarginal.GetValue("3");
            if (sum > 0.00)
            {
                return myCarsMarginal.GetValue("1") / sum;
            }
            return 0.00;
        }
        public double GetPercentHhldWTwoCar()
        {
            double sum = myCarsMarginal.GetValue("1")
                    + myCarsMarginal.GetValue("2")
                    + myCarsMarginal.GetValue("3");
            if (sum > 0.00)
            {
                return myCarsMarginal.GetValue("2") / sum;
            }
            return 0.00;
        }
        public double GetPercentHhldWThreeCar()
        {
            double sum = myCarsMarginal.GetValue("1")
                    + myCarsMarginal.GetValue("2")
                    + myCarsMarginal.GetValue("3");
            if (sum > 0.00)
            {
                return myCarsMarginal.GetValue("3") / sum;
            }
            return 0.00;
        }

        public DiscreteMarginalDistribution GetCarMarginal()
        {
            return myCarsMarginal;
        }
        public DiscreteMarginalDistribution GetDwellingMarginals()
        {
            return myDwellMarginal;
        }
        public DiscreteMarginalDistribution GetDwellingMarginalsByCount()
        {
            return myDwellMarginalCounts;
        }
        public DiscreteMarginalDistribution GetPersonHhldSizeMarginal()
        {
            return myHhldSize2Marginal;
        }
        public DiscreteMarginalDistribution GetPersonSexMarginal()
        {
            return mySexMarginal;
        }
        public DiscreteMarginalDistribution GetPersonAgeMarginal()
        {
            return myAgeMarginal;
        }
        public DiscreteMarginalDistribution GetPersonEduMarginal()
        {
            return myEducationMarginal;
        }
        public DiscreteMarginalDistribution GetPersonOccuMarginal()
        {
            return myOccupationMarginal;
        }
    }
}
