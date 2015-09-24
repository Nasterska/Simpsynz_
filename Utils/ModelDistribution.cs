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

namespace PopulationSynthesis.Utils
{
    class ModelDistribution : ConditionalDistribution
    {
        public ModelDistribution()
        {
            SetDistributionType(false);
        }
        //TODO:
        //***Update the methode GetValue()***

       /* public override double GetValue(string dim,string cat 
            , HouseholdPersonComposite composite, SpatialZone curZ)
        {
            string cat = fullKey.Substring(0,
                    fullKey.IndexOf(Constants.CATEGORY_DELIMITER) - 1);
            string key = fullKey.Substring(
                   fullKey.IndexOf(Constants.CATEGORY_DELIMITER) + 1
                , fullKey.Length - 1);
            return GetValue(cat, dim, key, curZ);
        }*/
        public override double GetValue(string dimension,
                        string category, SimulationObject composite_person, 
                                SpatialZone curZ)
        {
            //string procdKey = ProcessKey(key);
            // [BF] For now here it will always be income or education
            //HouseHold
            if(dimension == "IncomeLevel")
            {
                return (double) ComputeIncomeProbablities(
                    category, (HouseholdPersonComposite)composite_person, curZ);
            }
            else if (dimension == "NumOfWorkers")
            {
                return (double) ComputeEducationProbablities(
                    category, (HouseholdPersonComposite)composite_person, curZ);
            }
            else if (dimension == "DwellingType")
            {
                return (double)ComputeDwellingTypeProbablities(
                    category, (HouseholdPersonComposite)composite_person, curZ);
            }
            else if (dimension == "NumOfKids")
            {
                return (double)ComputeNumberOfChildreProbablities(
                     category, (HouseholdPersonComposite)composite_person);
            }
            else if (dimension == "HouseholdSize")
            {
                return (double)ComputeHouseholdTypeProbablities(
                                    category, (HouseholdPersonComposite)composite_person);
            }
            else if (dimension == "NumOfPeople")
            {
                return (double)ComputeNumberOfPeopleProbablities(
                                    category, (HouseholdPersonComposite)composite_person);
            }
            else if (dimension == "NumOfCars")
            {
                return (double)ComputeNumberOfVehiclesProbablities(
                                    category, (HouseholdPersonComposite)composite_person, curZ);
            }
            else if (dimension == "PublicTransitPass")
            {
                return (double)ComputePublicTransitPassProbablities(
                                    category, (HouseholdPersonComposite)composite_person);
            }
                
            //Person
            else if (dimension == "Sex")
            {
                return (double)ComputeSexProbablities(
                                                  category, (Person)composite_person);
            }
            else if (dimension == "DrivingLicense")
            {
                return (double)ComputeDriverLisenceProbablities(
                                                  category, (Person)composite_person);
            }
            else if (dimension == "Occupation")
            {
                return (double)ComputeOccupationProbablities(
                                                  category, (Person)composite_person, curZ);
            }
            else if (dimension == "EmploymentStatus")
            {
                return (double)ComputeEmploymentStatusProbablities(
                                                  category, (Person)composite_person);
            }
            else if (dimension == "Age")
            {
                return (double)ComputeAgeProbablities(
                                                  category, (Person)composite_person);
            }

            return 0;
        }

        private List<KeyValPair> GetCommValue(string dimension
                                , SimulationObject composite_person,
                                SpatialZone curZ, int personId)
        {
            //Household
            if (dimension == "IncomeLevel")
            {
                return ComputeIncomeCommulative((HouseholdPersonComposite)composite_person, curZ);
            }
            else if (dimension == "NumWithUnivDeg")
            {
                return ComputeEducationCommulative((HouseholdPersonComposite)composite_person, curZ);
            }
            else if (dimension == "DwellingType")
            {
                return ComputeDwellingTypeCommulative((HouseholdPersonComposite)composite_person, curZ);
            }
            else if (dimension == "NumOfKids")
            {
                return ComputeNumberOfChildrenCommulative((HouseholdPersonComposite)composite_person);
            }
            else if (dimension == "HouseholdSize")
            {
                return ComputeHouseholdTypeCommulative((HouseholdPersonComposite)composite_person);
            }
            else if (dimension == "NumOfPeople")
            {
                return ComputeNumberOfPeopleCommulative((HouseholdPersonComposite)composite_person);
            }
            else if (dimension == "NumOfCars")
            {
                return ComputeNumberOfVehiclesCommulative((HouseholdPersonComposite)composite_person, curZ);
            }
            else if (dimension == "PublicTransitPass")
            {
                return ComputePublicTransitPassCommulative((HouseholdPersonComposite)composite_person);
            }
            //Person
            else if (dimension == "Sex")
            {
                return ComputeSexCommulative(((HouseholdPersonComposite)composite_person).getPersons().ElementAt(personId));
            }
            else if (dimension == "DrivingLicense")
            {
                return ComputeDriverLisenceCommulative(((HouseholdPersonComposite)composite_person).getPersons().ElementAt(personId));
            }
            else if (dimension == "Occupation")
            {
                return ComputeOccupationCommulative(((HouseholdPersonComposite)composite_person).getPersons().ElementAt(personId), curZ);
            }
            else if (dimension == "EmploymentStatus")
            {
                return ComputeEmploymentStatusCommulative(((HouseholdPersonComposite)composite_person).getPersons().ElementAt(personId));
            }
            else if (dimension == "Age")
            {
                return ComputeAgeCommulative(((HouseholdPersonComposite)composite_person).getPersons().ElementAt(personId));
            }
            
            return null;
        }

        public override List<KeyValPair> GetCommulativeValue(SimulationObject composite,
                                            SpatialZone curZ, int personId)
        {
            return GetCommValue(GetDimensionName(), composite, curZ, personId);
        }

        // Income
        private double ComputeIncomeProbablities(string category,
                                            HouseholdPersonComposite composite,
                                            SpatialZone curZ)
        {
            var valList = GetUtilityValuesForIncome(composite, curZ);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]);
            if (int.Parse(category) ==
                (int) IncomeLevel.ThirtyOrLess)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int) IncomeLevel.ThirtyToSevetyFive)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int) IncomeLevel.SeventyFiveToOneTwentyFive)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)IncomeLevel.OneTwentyFiveToTwoHundred)
            {
                return ((double)valList[3] / logsum);
            }
            else if (int.Parse(category) ==
                (int)IncomeLevel.TwohundredOrMore)
            {
                return ((double)valList[4] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeIncomeCommulative(HouseholdPersonComposite composite,
                                          SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForIncome(composite, curZ);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4];
            currPair.Category = "0";//IncomeLevel.ThirtyOrLess.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.ThirtyToSevetyFive.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.SeventyFiveToOneTwentyFive.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add( currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.OneTwentyFiveToTwoHundred.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "4";//IncomeLevel.TwohundredOrMore.ToString();
            currPair.Value = comVal + (double)valList[4] / utilSum;
            comList.Add(currPair);
            return comList;
        }
        private List<double> GetUtilityValuesForIncome(HouseholdPersonComposite composite,
                                            SpatialZone curZ)
        {
            Household hhld = composite.getHousehold().CreateNewCopy();
            String key = hhld.GetNewJointKey(GetDimensionName());
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(5);

            currValues.Add(1.00);
            currValues.Add(Math.Exp(-0.859 + 0.000783 * curZ.GetAverageIncome()
                            / Constants.BFRANC_TO_EURO
                         + 1.16 * Int16.Parse(curKeys[4])
                         + 1.15 * Int16.Parse(curKeys[1])));
            currValues.Add(Math.Exp(-4.57 + 0.674 * Int16.Parse(curKeys[3])
                         + 0.0012 * curZ.GetAverageIncome() / Constants.BFRANC_TO_EURO
                         + 1.87 * Int16.Parse(curKeys[4])
                         + 0.409 * Int16.Parse(curKeys[5])
                         + 2.2 * Int16.Parse(curKeys[1])));
            currValues.Add(Math.Exp(-8.11 + 1.38 * Int16.Parse(curKeys[3])
                         + 0.00157 * curZ.GetAverageIncome()
                         / Constants.BFRANC_TO_EURO
                         + 2.22 * Int16.Parse(curKeys[4])
                         + 0.415 * Int16.Parse(curKeys[5])
                         + 2.33 * Int16.Parse(curKeys[1])));
            currValues.Add(Math.Exp(-10.5 + 1.61 * Int16.Parse(curKeys[3])
                         + 0.0016 * curZ.GetAverageIncome()
                         / Constants.BFRANC_TO_EURO
                         + 3.04 * Int16.Parse(curKeys[4])
                         + 0.415 * Int16.Parse(curKeys[5])
                         + 1.64 * Int16.Parse(curKeys[1])));
            return currValues;
        }
        // Education
        private double ComputeEducationProbablities(string category,
                                            HouseholdPersonComposite composite,
                                            SpatialZone curZ)
        {
            var valList = GetUtilityValuesForEducation(composite, curZ);
            double logsum = ((double)valList[0]
                            + (double)valList[1]
                            + (double)valList[2]);
            if (int.Parse(category) == (int)NumWithUnivDeg.None)
            {
                return ((double)valList[0]/logsum);
            }
            else if (int.Parse(category) == (int)NumWithUnivDeg.One)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) == (int)NumWithUnivDeg.TwoOrMore)
            {
                return ((double)valList[2] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeEducationCommulative(HouseholdPersonComposite composite,
                                          SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForEducation(composite, curZ);
            double utilSum = (double)valList[0] + (double)valList[1]
                         + (double)valList[2];
            KeyValPair currPair = new KeyValPair();
            currPair.Category = "0";//NumWithUnivDeg.None.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//NumWithUnivDeg.One.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//NumWithUnivDeg.TwoOrMore.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comList.Add(currPair);
            return comList;
        }

        private List<double> GetUtilityValuesForEducation(HouseholdPersonComposite composite,
                                            SpatialZone curZ)
        {
            Household hhld = composite.getHousehold().CreateNewCopy();
            String key = hhld.GetNewJointKey(GetDimensionName());
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            List<double> currValues = new List<double>(3);
            currValues.Add(1.00);
            currValues.Add(Math.Exp(-2.96 + 0.238 * Int16.Parse(curKeys[4])
                               + 3.34 * curZ.GetPercentHighEducated()
                               + 0.24 * Int16.Parse(curKeys[3])
                               + 0.393 * Int16.Parse(curKeys[1])));
            currValues.Add(Math.Exp(-7.19 + 0.701 * Int16.Parse(curKeys[4])
                   + 4.34 * curZ.GetPercentHighEducated()
                   + 1.09 * Int16.Parse(curKeys[3])
                   + 0.851 * Int16.Parse(curKeys[1])));
            return currValues;
        }
        // Car
        private double ComputeCarProbablities(string category,
                                            HouseholdPersonComposite composite,
                                            SpatialZone curZ)
        {
            var valList = GetUtilityValuesForCar(composite, curZ);
            
            double logsum = ((double)valList[0]
                            + (double)valList[1]
                            + (double)valList[2]
                            + (double)valList[3]);

            if (int.Parse(category) == (int)NumOfCars.NoCar)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) == (int)NumOfCars.OneCar)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) == (int)NumOfCars.TwoCars)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) == (int)NumOfCars.ThreeOrMore)
            {
                return ((double)valList[3] / logsum);
            }
            return 0.00;
        }
        private List<KeyValPair> ComputeCarCommulative(HouseholdPersonComposite composite,
                                          SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForCar(composite, curZ);
            double utilSum = (double)valList[0] + (double)valList[1]
                         + (double)valList[2] + (double)valList[3];
            KeyValPair currPair = new KeyValPair();
            currPair.Category = "0";//No Car
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//1 Car
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2"; //2 Cars
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3"; //3 or more Cars
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comList.Add(currPair);

            return comList;
        }

        private List<double> GetUtilityValuesForCar(HouseholdPersonComposite composite,
                                            SpatialZone curZ)
        {
            Household hhld = composite.getHousehold().CreateNewCopy();
            String key = hhld.GetNewJointKey(GetDimensionName());
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(4);
            currValues.Add(1.00);
            double dwellNotApartment = 0.00;
            if (Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 0.841;
            }
            double incParam = 0.00;
            if (Int16.Parse(curKeys[3]) == 2)
            {
                incParam = 0.858;
            }
            else if (Int16.Parse(curKeys[3]) > 2)
            {
                incParam = 0.978;
            }
            double childParam = 0.00;
            if (Int16.Parse(curKeys[2]) != 0)
            {
                childParam = 0.457;
            }
            currValues.Add(Math.Exp(-2.75 + 0.504 * Int16.Parse(curKeys[4])
                               + 0.00105 * curZ.GetAverageIncome()
                               / Constants.BFRANC_TO_EURO
                               + 0.437 * Int16.Parse(curKeys[1])
                               + 0.498 * curZ.GetPercentHhldWOneCar()
                               + dwellNotApartment
                               + incParam
                               + childParam));
            incParam = 0.00;
            if (Int16.Parse(curKeys[3]) == 2)
            {
                incParam = 1.87;
            }
            else if (Int16.Parse(curKeys[3]) > 2)
            {
                incParam = 2.43;
            }
            childParam = 0.00;
            if (Int16.Parse(curKeys[2]) != 0)
            {
                childParam = 0.800;
            }
            dwellNotApartment = 0.00;
            if (Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 1.86;
            }
            currValues.Add(Math.Exp(-7.02 + 0.933 * Int16.Parse(curKeys[4])
                   + 0.00126 * curZ.GetAverageIncome()
                   / Constants.BFRANC_TO_EURO
                   + 1.24 * Int16.Parse(curKeys[1])
                   + 2.13 * curZ.GetPercentHhldWTwoCar()
                   + dwellNotApartment
                   + incParam
                   + childParam));
            incParam = 0.00;
            if (Int16.Parse(curKeys[3]) == 2)
            {
                incParam = 1.42;
            }
            else if (Int16.Parse(curKeys[3]) > 2)
            {
                incParam = 3.24;
            }

            dwellNotApartment = 0.00;
            if (Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 2.66;
            }
            currValues.Add(Math.Exp(-10.1 + 1.07 * Int16.Parse(curKeys[4])
                   + 0.00126 * curZ.GetAverageIncome()
                   / Constants.BFRANC_TO_EURO
                   + 1.60 * Int16.Parse(curKeys[1])
                   + 14.1 * curZ.GetPercentHhldWThreeCar()
                   + dwellNotApartment
                   + incParam));
            return currValues;
        }

        /*private ArrayList GetUtilityValuesForCar(string key,
                                            SpatialZone curZ)
        {
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            ArrayList currValues = new ArrayList(4);
            currValues.Add(1.00);
            double dwellNotApartment = 0.00;
            if( Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 0.841;
            }
            double incParam = 0.00;
            if(Int16.Parse(curKeys[3])==2)
            {
                incParam = 0.858;
            }
            else if (Int16.Parse(curKeys[3])>2)
            {
                incParam = 0.978;
            }
            double childParam = 0.00;
            if(Int16.Parse(curKeys[2])!=0)
            {
                childParam = 0.457;
            }
            currValues.Add(Math.Exp(-2.75 + 0.504 * Int16.Parse(curKeys[4])
                               + 0.00105 * curZ.GetAverageIncome()
                               / Constants.BFRANC_TO_EURO
                               + 0.437 * Int16.Parse(curKeys[1])
                               + 0.00105 * curZ.GetNumHhldWOneCar()
                               + dwellNotApartment
                               + incParam
                               + childParam));
            incParam=0.00;
            if (Int16.Parse(curKeys[3]) == 2)
            {
                incParam = 1.87;
            }
            else if (Int16.Parse(curKeys[3]) > 2)
            {
                incParam = 2.43;
            }
            childParam = 0.00;
            if (Int16.Parse(curKeys[2]) != 0)
            {
                childParam = 0.801;
            }
            dwellNotApartment = 0.00;
            if (Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 1.86;
            }
            currValues.Add(Math.Exp(-7.02 + 0.933 * Int16.Parse(curKeys[4])
                   + 0.00126 * curZ.GetAverageIncome()
                   / Constants.BFRANC_TO_EURO
                   + 1.24 * Int16.Parse(curKeys[1])
                   + 0.0045 * curZ.GetNumHhldWTwoCar()
                   + dwellNotApartment
                   + incParam
                   + childParam));
            incParam = 0.00;
            if (Int16.Parse(curKeys[3]) == 2)
            {
                incParam = 1.42;
            }
            else if (Int16.Parse(curKeys[3]) > 2)
            {
                incParam = 3.24;
            }
             
            dwellNotApartment = 0.00;
            if (Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 2.66;
            }
            currValues.Add(Math.Exp(-10.1 + 1.07 * Int16.Parse(curKeys[4])
                   + 0.00126 * curZ.GetAverageIncome()
                   / Constants.BFRANC_TO_EURO
                   + 1.60 * Int16.Parse(curKeys[1])
                   + 0.0298 * curZ.GetNumHhldWThreeCar()
                   + dwellNotApartment
                   + incParam));

            return currValues;

        }*/
        //******************************************************************
        // Dwelling
      /*  private double ComputeDwellingProbablities(string category,
                                    string procdKey,
                                    SpatialZone curZ)
        {
            var valList = GetUtilityValuesForDwelling(procdKey, curZ);

            double logsum = ((double)valList[0]
                            + (double)valList[1]
                            + (double)valList[2]
                            + (double)valList[3]);

            if (int.Parse(category) == (int)DwellingType.Separate)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) == (int)DwellingType.SemiAttached)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) == (int)DwellingType.Attached)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) == (int)DwellingType.Apartments)
            {
                return ((double)valList[3] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeDwellingCommulative(string procdKey,
                                  SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForDwelling(procdKey, curZ);
            double utilSum = (double)valList[0] + (double)valList[1]
                         + (double)valList[2] + (double)valList[3];
            KeyValPair currPair = new KeyValPair();
            currPair.Category = "0";//Seperate
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//Semi detached
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2"; //Attached
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3"; //Apartment
            currPair.Value = comVal + (double)valList[3] / utilSum;

            comList.Add(currPair);

            return comList;
        }*/

        //private ArrayList GetUtilityValuesForDwelling(string key,
        //                            SpatialZone curZ)
        //{
        //    string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
        //    ArrayList currValues = new ArrayList(4);
        //    currValues.Add(1.00);

        //    double childParam = 0.00;
        //    if (Int16.Parse(curKeys[2]) != 0)
        //    {
        //        childParam = 0.484;
        //    }
        //    currValues.Add(Math.Exp(1.52 - 0.000549 * curZ.GetAverageIncome()
        //                       / Constants.BFRANC_TO_EURO
        //                       - 0.572 * Int16.Parse(curKeys[5])
        //                       + childParam));
        //    double incParam=0.00;
        //    if (Int16.Parse(curKeys[3]) > 2 )
        //    {
        //        incParam = 0.664;
        //    }

        //    if (Int16.Parse(curKeys[2]) != 0)
        //    {
        //        childParam = 0.430;
        //    }
        //    currValues.Add(Math.Exp(5.95 - 0.00208 * curZ.GetAverageIncome()
        //                       / Constants.BFRANC_TO_EURO
        //                       + 0.266 * Int16.Parse(curKeys[4])
        //                       - 1.16 * Int16.Parse(curKeys[5])
        //                       + incParam + childParam));
        //    if (Int16.Parse(curKeys[3]) == 2)
        //    {
        //        incParam = -0.681;
        //    }

        //    if (Int16.Parse(curKeys[2]) != 0)
        //    {
        //        childParam = -0.642;
        //    }
        //    currValues.Add(Math.Exp(4.52 - 0.00233 * curZ.GetAverageIncome()
        //                        / Constants.BFRANC_TO_EURO
        //                       + 0.811 * Int16.Parse(curKeys[4])
        //                       - 1.89 * Int16.Parse(curKeys[5])
        //                       + 5.77 * curZ.GetApartmentPercent()
        //                       + incParam + childParam));
        //    return currValues;
        //}
        //*****************************************************************************
      /*  private List<double> GetUtilityValuesForDwelling(string key,
                            SpatialZone curZ)
        {
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(4);
            int hhldSz = int.Parse(curKeys[0]);
            double b_surf = 0.00;
            if (hhldSz == 2)
            {
                b_surf = 0.0146;
            }
            else if( hhldSz == 3)
            {
                b_surf = 0.0194;
            }
            else if (hhldSz > 3)
            {
                b_surf = 0.0249;
            }
            double currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("0");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            currValues.Add(Math.Exp(currWtDwell 
                               + b_surf * curZ.GetSurfaceOne()));

            currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("1");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            int numCars = int.Parse(curKeys[5]);
            int incLvl = int.Parse(curKeys[3]);

            currValues.Add(Math.Exp(0.423 + currWtDwell
                               - 0.279 * numCars
                               + b_surf * curZ.GetSurfaceTwo()));

            currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("2");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            currValues.Add(Math.Exp(0.870 + currWtDwell
                               - 0.593 * numCars
                               + b_surf * curZ.GetSurfaceThree()));

            currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("3");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            currValues.Add(Math.Exp(1.20 + currWtDwell
                               - 0.9482 * numCars
                               + b_surf * curZ.GetSurfaceFour()));
            return currValues;
        }
        */
        /*private ArrayList GetUtilityValuesForDwelling(string key,
                            SpatialZone curZ)
        {
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            ArrayList currValues = new ArrayList(4);
            int hhldSz = int.Parse(curKeys[0]);
            double b_surf = 0.00;
            if (hhldSz == 2)
            {
                b_surf = 0.0122;
            }
            else if( hhldSz == 3)
            {
                b_surf = 0.0168;
            }
            else if (hhldSz > 4)
            {
                b_surf = 0.0234;
            }
            double currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("0");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            currValues.Add(Math.Exp(currWtDwell 
                               + b_surf * curZ.GetSurfaceOne()));

            currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("1");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            int numCars = int.Parse(curKeys[5]);
            int incLvl = int.Parse(curKeys[3]);
            double b_inc = 0.00;
            if (incLvl > 2)
            {
                b_inc = 0.584;
            }
            currValues.Add(Math.Exp(0.377 + currWtDwell
                               - 0.19 * numCars
                               - b_inc
                               + b_surf * curZ.GetSurfaceTwo()));

            currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("2");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            currValues.Add(Math.Exp(0.838 + currWtDwell
                               - 0.591 * numCars
                               + b_surf * curZ.GetSurfaceThree()));

            currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("3");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            b_inc = 0.00;
            if (incLvl == 2)
            {
                b_inc = 0.392;
            }
            else if (incLvl > 2)
            {
                b_inc = 0.721;
            }
            currValues.Add(Math.Exp(1.17 + currWtDwell
                               - 0.792 * numCars
                               - b_inc
                               + b_surf * curZ.GetSurfaceFour()));
            return currValues;
        }*/
        // Income
        private double ComputeHouseholdTypeProbablities(string category,
                                            HouseholdPersonComposite composite)
        {
            var valList = GetUtilityValuesForHouseholdType(composite);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]
                                     + (double)valList[5]);
            if (int.Parse(category) ==
                (int)HouseholdSize.SingleAdult)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)HouseholdSize.OneAdultOneChild)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)HouseholdSize.Twoadults)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)HouseholdSize.TwoAdultsChildren)
            {
                return ((double)valList[3] / logsum);
            }
            else if (int.Parse(category) ==
                (int)HouseholdSize.ThreeOrMoreAdults)
            {
                return ((double)valList[4] / logsum);
            }
            else if (int.Parse(category) ==
                 (int)HouseholdSize.ThreeOrMoreAdultsChildren)
            {
                return ((double)valList[5] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeHouseholdTypeCommulative(HouseholdPersonComposite composite)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForHouseholdType(composite);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]
                                     + (double)valList[5];
            currPair.Category = "0";//IncomeLevel.SingleAdult.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.OneAdultOneChild.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.Twoadults.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.TwoAdultsChildren.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "4";//IncomeLevel.ThreeOrMoreAdults.ToString();
            currPair.Value = comVal + (double)valList[4] / utilSum;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "5";//IncomeLevel.ThreeOrMoreAdultsChildren.ToString();
            currPair.Value = comVal + (double)valList[5] / utilSum;
            comList.Add(currPair);
            return comList;
        }

        private List<double> GetUtilityValuesForHouseholdType(
            HouseholdPersonComposite composite)
        {
            Household hhld = composite.getHousehold().CreateNewCopy();
            //String key = hhld.GetNewJointKey(GetDimensionName());
            //string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(6);
			int myNumOfCars = (int) hhld.GetNumOfCars();
			int dwellType = (int) hhld.GetDwellingType();
			int hhldSize = (int) hhld.GetHhldSize();

            currValues.Add(1); // Math.Exp(0)

            int twoCars=0;
            if(myNumOfCars == 2)
                twoCars = 1;
            else
                twoCars = 0;
            int apart=0;
            int town=0;
            if(dwellType == 1) // apart
            {   
                town = 0;
                apart = 1;
            }
            else if(dwellType == 3) // town
            {
                town = 1;
                apart = 0;
            }
            else
            {
                town = 0;
                apart = 0;
            }

            int  h_male_1 = 0;
            int  h_sixteen_1Plus = 0;
            int  h_eighteen_1 = 0;
            int  h_twentysix_1 = 0;
            int  h_thirtyone_1 = 0;
            int  h_fiftyone_1 = 0;
            int  h_sixtyfivePlus_1 = 0;
            int  h_PT_1 = 0;
            int  h_homeFT_1Plus = 0;
            int  h_homePT_1Plus = 0;

            if (composite.Count_18 == 1)
                h_eighteen_1 = 1;
            else
                h_eighteen_1 = 0;
            if (composite.Count_26 == 1)
                h_twentysix_1 = 1;
            else
                h_twentysix_1 = 0;
            if (composite.Count_31 == 1)
                h_thirtyone_1 = 1;
            else
                h_thirtyone_1 = 0;
            if (composite.Count_51 == 1)
                h_fiftyone_1 = 1;
            else
                h_fiftyone_1 = 0;
            if (composite.Count_65 == 1)
                h_sixtyfivePlus_1 = 1;
            else
                h_sixtyfivePlus_1 = 0;
            if (composite.Count_part_time == 1)
                h_PT_1 = 1;
            else
                h_PT_1 = 0;

            currValues.Add(Math.Exp(-2.81 + 0.30 * twoCars - 0.615 * apart + 0.467 * town + 1.97 * h_eighteen_1
                + 1.42 * h_twentysix_1 + 2 * h_thirtyone_1 - 1.07 * h_fiftyone_1 - 4.71 * h_sixtyfivePlus_1 
                + 0.525 * h_PT_1) );

            int oneCar, threeOrMoreCars = 0;
            if(myNumOfCars == 1)
                oneCar = 1;
            else
                oneCar = 0; 
            if(myNumOfCars == 3)
                threeOrMoreCars = 1;
            else
                threeOrMoreCars = 0;

            if (composite.Count_16 >= 1)
                h_sixteen_1Plus = 1;
            else
                h_sixteen_1Plus = 0;
            if (composite.Count_male == 1)
                h_male_1 = 1;
            else
                h_male_1 = 0;
            if (composite.Count_full_time_home >= 1)
                h_homeFT_1Plus = 1;
            else 
                h_homeFT_1Plus = 0;
            if (composite.Count_part_time_home >= 1)
                h_homePT_1Plus = 1;
            else
                h_homePT_1Plus = 0;
            int h_eNE_1 = Convert.ToInt32(composite.Count_unemployed == 1); ;

            currValues.Add(Math.Exp(-1.14 + 0.573 * oneCar + 2.66 * twoCars + 2.13 * threeOrMoreCars
                - 0.689 * apart - 0.259 * town + 4.26 * h_sixteen_1Plus + 1.87 * h_eighteen_1
                + 0.899 * h_twentysix_1 - 0.0815 * h_thirtyone_1 - 1.01 * h_sixtyfivePlus_1 + 2.17 * h_male_1
                + 0.112 * h_homeFT_1Plus + 0.415 * h_homePT_1Plus
                + -0.668 * h_eNE_1 + 0.729 * h_PT_1 ) );

            int threePlusCars = 0;
            if (myNumOfCars >= 3)
                threePlusCars = 1;
            else
                threePlusCars = 0;
            int h_thirtyone_2Plus = 0;
            if (composite.Count_31 >= 2)
                h_thirtyone_2Plus = 1;
            else
                h_thirtyone_2Plus = 0;

            currValues.Add(Math.Exp(-1.99  * 1.00 + 1.00 * oneCar + 3.45 * twoCars + 2.26 * threePlusCars
                + -1.15 * apart + -0.292 * town + 3.95 * h_sixteen_1Plus + 1.72 * h_eighteen_1
                + 1.69 * h_twentysix_1 + 2.40 * h_thirtyone_1 + 3.77 * h_thirtyone_2Plus
                + -0.526 * h_fiftyone_1 + -4.02 * h_sixtyfivePlus_1 + -0.747 * h_male_1 + 0.264 * h_homeFT_1Plus
                + 0.474 * h_homePT_1Plus + 0.765 * h_PT_1 ) );

            int h_fiftyfive_2Plus = 0;
            if (composite.Count_55 >= 2)
                h_fiftyfive_2Plus = 1;
            else
                h_fiftyfive_2Plus = 0;

            currValues.Add(Math.Exp(-1.62 * 1.00 + 0.934 * oneCar + 3.55 * twoCars + 4.22 * threePlusCars
                + -1.38 * apart + -0.401 * town + 7.86 * h_sixteen_1Plus + 4.18 * h_eighteen_1 + 2.03 * h_twentysix_1
                + 1.12 * h_thirtyone_1 + 0.971 * h_thirtyone_2Plus + 1.01 * h_fiftyone_1 + 1.08 * h_fiftyfive_2Plus
                + -0.727 * h_sixtyfivePlus_1 + -0.583 * h_male_1 + 0.339 * h_homeFT_1Plus + 0.509 * h_homePT_1Plus
                + -0.529 * h_eNE_1 + 1.30 * h_PT_1 ) );

            currValues.Add(Math.Exp(-2.14 * 1.00 + 1.01 * oneCar + 3.62 * twoCars + 3.80 * threePlusCars 
                + -1.30 * apart+ -0.300 * town + 7.37 * h_sixteen_1Plus + 3.36 * h_eighteen_1 
                + 1.42 * h_twentysix_1 + 2.06 * h_thirtyone_1 + 2.85 * h_thirtyone_2Plus + 0.356 * h_fiftyone_1 
                + -0.854 * h_sixtyfivePlus_1 + -1.39 * h_male_1 + 0.597 * h_homeFT_1Plus + 0.638 * h_homePT_1Plus 
                + -2.70 * h_eNE_1 + 1.27 * h_PT_1 ) );

            return currValues;
        }

        private double ComputeNumberOfChildreProbablities(string category,
                                           HouseholdPersonComposite composite)
        {
            var valList = GetUtilityValuesForNumberOfChildren(composite);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]);
            if (int.Parse(category) ==
                (int)NumOfKids.None)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfKids.One)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfKids.Two)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfKids.Three)
            {
                return ((double)valList[3] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfKids.FourOrMore)
            {
                return ((double)valList[4] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeNumberOfChildrenCommulative(HouseholdPersonComposite composite)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForNumberOfChildren(composite);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4];
            currPair.Category = "0";//IncomeLevel.None.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.One.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.Two.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.Three.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "4";//IncomeLevel.FourOrMore.ToString();
            currPair.Value = comVal + (double)valList[4] / utilSum;
            comList.Add(currPair);

            return comList;
        }

        private List<double> GetUtilityValuesForNumberOfChildren(
           HouseholdPersonComposite composite)
        {
            Household hhld = composite.getHousehold().CreateNewCopy();
            //String key = hhld.GetNewJointKey(GetDimensionName());
            //string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(5);
			int myNumOfCars = (int) hhld.GetNumOfCars();
			int dwellType = (int) hhld.GetDwellingType();
			int hhldSize = (int) hhld.GetHhldSize();

            currValues.Add(1); // Math.Exp(0)

            int h_eleven_1Plus = Convert.ToInt32(composite.Count_11 >= 1);
            int h_fourteen_1Plus = Convert.ToInt32(composite.Count_14 >= 1);
            int h_sixteen_1Plus = Convert.ToInt32(composite.Count_16 >= 1);
            int h_eighteen_1 = Convert.ToInt32(composite.Count_18 == 1);
            int h_eighteen_2Plus = Convert.ToInt32(composite.Count_11 >= 2);
            int h_twentysix_1 = Convert.ToInt32(composite.Count_26 == 1);
            int h_twentysix_2Plus = Convert.ToInt32(composite.Count_26 >= 2);
            int h_thirtyone_1 = Convert.ToInt32(composite.Count_31 == 1);
            int h_thirtyone_2Plus = Convert.ToInt32(composite.Count_31 >= 2);
            int h_fortyone_1 = Convert.ToInt32(composite.Count_41 == 1);
            int h_fortyone_2Plus = Convert.ToInt32(composite.Count_41 >= 2);
            int h_fiftyfive_1 = Convert.ToInt32(composite.Count_55 == 1);
            int h_fiftyfive_2Plus = Convert.ToInt32(composite.Count_55 >= 2);
            int h_sixtyfivePlus_1 = Convert.ToInt32(composite.Count_65 == 1);
            int h_sixtyfivePlus_2Plus = Convert.ToInt32(composite.Count_65 >= 2);
            int h_lYes_1 = Convert.ToInt32(composite.Count_driving_license_yes == 1);
            int h_lYes_2 = Convert.ToInt32(composite.Count_driving_license_yes == 2);
            int h_lYes_3 = Convert.ToInt32(composite.Count_driving_license_yes == 3);
            int h_lYes_4Plus = Convert.ToInt32(composite.Count_driving_license_yes >= 4);
            int h_lNo_4Plus = Convert.ToInt32(composite.Count_driving_license_no >= 4);
            int apart = Convert.ToInt32(dwellType == 1);
            int oneCar = Convert.ToInt32(myNumOfCars == 1);
            int twoCars = Convert.ToInt32(myNumOfCars == 2);
            int h_male_1 = Convert.ToInt32(composite.Count_male == 1);
            int h_male_2 = Convert.ToInt32(composite.Count_male == 2);

            currValues.Add(Math.Exp(-3.30 * 1.0 + 0.555 * h_eleven_1Plus + -0.370 * h_fourteen_1Plus
                + -0.521 * h_sixteen_1Plus + -0.357 * h_eighteen_1 + -0.443 * h_eighteen_2Plus
                + 0.895 * h_twentysix_1 + 2.19 * h_twentysix_2Plus + 1.94 * h_thirtyone_1
                + 3.43 * h_thirtyone_2Plus + 1.24 * h_fortyone_1 + 2.16 * h_fortyone_2Plus
                + -0.562 * h_fiftyfive_1 + -1.40 * h_fiftyfive_2Plus + -1.01 * h_sixtyfivePlus_1
                + -1.61 * h_sixtyfivePlus_2Plus + 0.143 * h_lYes_1 + 0.472 * h_lYes_2
                + -0.221 * h_lYes_3 + -0.482 * h_lYes_4Plus + 1.30 * h_lNo_4Plus
                + -0.279 * apart + 0.110 * oneCar + 0.162 * twoCars + -0.752 * h_male_1
                + 0.438 * h_male_2));

            int h_fiftyone_1 = Convert.ToInt32(composite.Count_51 == 1);
            int h_fiftyone_2Plus =  Convert.ToInt32(composite.Count_male >= 2);
            int town = Convert.ToInt32(dwellType == 2);
            int threePlusCars = Convert.ToInt32(myNumOfCars >= 3);

            currValues.Add(Math.Exp(-3.48 * 1.00 + -1.12 * h_eleven_1Plus
                + -1.90 * h_fourteen_1Plus + -2.04 * h_sixteen_1Plus + -1.26 * h_eighteen_1
                + -2.04 * h_eighteen_2Plus + 0.684 * h_twentysix_1 + 1.74 * h_twentysix_2Plus
                + 2.30 * h_thirtyone_1 + 3.92 * h_thirtyone_2Plus + 1.26 * h_fortyone_1
                + 2.17 * h_fortyone_2Plus + 0.577 * h_fiftyone_1 + -1.35 * h_fiftyone_2Plus
                + -1.23 * h_fiftyfive_1 + -2.34 * h_fiftyfive_2Plus + -1.68 * h_sixtyfivePlus_1
                + -2.83 * h_sixtyfivePlus_2Plus + 0.396 * h_lYes_1 + 1.27 * h_lYes_2
                + 0.438 * h_lYes_3 + 0.426 * h_lYes_4Plus + 3.35 * h_lNo_4Plus
                + -0.697 * apart + -0.278 * town + 0.425 * oneCar + 0.638 * twoCars
                + 0.413 * threePlusCars + -2.32 * h_male_1 + -0.295 * h_male_2));

            currValues.Add(Math.Exp(-4.27 * 1.00 + -2.14 * h_eleven_1Plus + -2.89 * h_fourteen_1Plus
                + -2.98 * h_sixteen_1Plus + -2.10 * h_eighteen_1 + -3.70 * h_eighteen_2Plus
                + 1.55 * h_thirtyone_1 + 2.04 * h_thirtyone_2Plus + 0.212 * h_fortyone_1
                + -1.83 * h_fiftyone_1 + -3.79 * h_fiftyone_2Plus + -2.44 * h_fiftyfive_1
                + -4.83 * h_fiftyfive_2Plus + -3.04 * h_sixtyfivePlus_1 + -5.38 * h_sixtyfivePlus_2Plus
                + 1.18 * h_lYes_1 + 3.15 * h_lYes_2 + 3.21 * h_lYes_3 + 4.07 * h_lYes_4Plus
                + 6.19 * h_lNo_4Plus + -0.861 * apart + -0.303 * town + 0.129 * twoCars
                + -3.12 * h_male_1 + -0.924 * h_male_2));

            currValues.Add(Math.Exp(-6.25 * 1.00 + -0.379 * h_eleven_1Plus + -0.743 * h_fourteen_1Plus
                + -1.08 * h_sixteen_1Plus + -0.627 * h_eighteen_1 + -1.02 * h_eighteen_2Plus
                + 2.01 * h_twentysix_1 + 3.16 * h_twentysix_2Plus + 3.39 * h_thirtyone_1
                + 6.11 * h_thirtyone_2Plus + 1.98 * h_fortyone_1 + 3.33 * h_fortyone_2Plus
                + -1.09 * h_fiftyfive_1 + -2.33 * h_fiftyfive_2Plus + -1.43 * h_sixtyfivePlus_1
                + -1.50 * h_sixtyfivePlus_2Plus + -0.926 * h_lYes_1 + -1.45 * h_lYes_2
                + -2.72 * h_lYes_3 + -2.87 * h_lYes_4Plus + -0.600 * apart + 1.01 * oneCar
                + 1.27 * twoCars + 1.29 * threePlusCars + -4.02 * h_male_1 + -1.71 * h_male_2));

            return currValues;
        }

        private double ComputeNumberOfPeopleProbablities(string category,
                                   HouseholdPersonComposite composite)
        {
            var valList = GetUtilityValuesForNumberOfPeople(composite);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]
                                     + (double)valList[5]);
            if (int.Parse(category) ==
                (int)NumOfPeople.None)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfPeople.One)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfPeople.Two)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfPeople.Three)
            {
                return ((double)valList[3] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfPeople.Four)
            {
                return ((double)valList[4] / logsum);
            }
            else if (int.Parse(category) ==
                 (int)NumOfPeople.fiveOrMore)
            {
                return ((double)valList[5] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeNumberOfPeopleCommulative(HouseholdPersonComposite composite)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForNumberOfPeople(composite);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4];
            currPair.Category = "0";//IncomeLevel.None.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.One.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.Two.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.Three.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "4";//IncomeLevel.FourOrMore.ToString();
            currPair.Value = comVal + (double)valList[4] / utilSum;
            comList.Add(currPair);

            return comList;
        }

        private List<double> GetUtilityValuesForNumberOfPeople(
      HouseholdPersonComposite composite)
        {
            Household hhld = composite.getHousehold().CreateNewCopy();
            //String key = hhld.GetNewJointKey(GetDimensionName());
            //string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(5);
			int myNumOfCars = (int) hhld.GetNumOfCars();
			int dwellType = (int) hhld.GetDwellingType();
			int hhldSize = (int) hhld.GetHhldSize();


            currValues.Add(1);

            int h_sixteen_1Plus = Convert.ToInt32(composite.Count_16 >= 1);
            int h_eighteen_1 = Convert.ToInt32(composite.Count_18 == 1);
            int h_twentysix_1 = Convert.ToInt32(composite.Count_26 == 1);
            int h_fiftyone_1 = Convert.ToInt32(composite.Count_51 == 1);
            int h_homeFT_1Plus = Convert.ToInt32(composite.Count_full_time_home >= 1);
            int h_PT_1 = Convert.ToInt32(composite.Count_part_time == 1);
            int h_officeCler_1 = Convert.ToInt32(composite.Count_clerical_manufacturing == 1);
            int h_manConst_1 = Convert.ToInt32(composite.Count_costruction_man == 1);
            int h_profMan_1 = Convert.ToInt32(composite.Count_professional_man == 1);
            int h_retail_1 = Convert.ToInt32(composite.Count_retail == 1);


            currValues.Add(Math.Exp(0.0900 * 1.00 + 4.16 * h_sixteen_1Plus + 1.62 * h_eighteen_1
                + 0.745 * h_twentysix_1 + 0.185 * h_fiftyone_1 + 0.568 * h_homeFT_1Plus + 0.656 * 
                + 0.500 * h_officeCler_1 + 0.879 * h_manConst_1 + 0.173 * h_profMan_1
                + 0.422 * h_retail_1));

            int h_twentysix_2Plus = Convert.ToInt32(composite.Count_26 >= 2);
            int h_thirtyone_1 = Convert.ToInt32(composite.Count_31 == 1);
            int h_lYes_3 = Convert.ToInt32(composite.Count_driving_license_yes == 3);

            currValues.Add(Math.Exp(-1.67 * 1.00 + 6.22 * h_sixteen_1Plus + 2.96 * h_eighteen_1
                + 1.55 * h_twentysix_1 + 0.939 * h_twentysix_2Plus + 1.25 * h_thirtyone_1
                + 0.513 * h_fiftyone_1 + 1.91 * h_lYes_3 + 0.721 * h_homeFT_1Plus + 0.823 * h_PT_1
                + 0.866 * h_officeCler_1 + 1.35 * h_manConst_1 + 0.540 * h_profMan_1
                + 0.793 * h_retail_1));

            int h_thirtyone_2Plus = Convert.ToInt32(composite.Count_31 >= 2);
            int h_female_3 = Convert.ToInt32(composite.Count_female == 3);
            int h_retail_2 = Convert.ToInt32(composite.Count_retail == 2);

            currValues.Add(Math.Exp(-2.40 * 1.00 + 7.44 * h_sixteen_1Plus + 3.35 * h_eighteen_1 
                + 1.60 * h_twentysix_1 + 1.53 * h_twentysix_2Plus + 1.63 * h_thirtyone_1
                + 2.18 * h_thirtyone_2Plus + 0.664 * h_fiftyone_1 + 3.36 * h_female_3 
                + 0.794 * h_homeFT_1Plus + 0.924 * h_PT_1 + 0.962 * h_officeCler_1 + 1.66 * h_manConst_1 
                + 0.635 * h_profMan_1 + 0.859 * h_retail_1 + 1.02 * h_retail_2));

            int h_oNE_4Plus = Convert.ToInt32(composite.Count_unemployed >= 4);

            currValues.Add(Math.Exp(-4.56 * 1.00 + 7.71 * h_sixteen_1Plus + 3.03 * h_eighteen_1 
                + 1.92 * h_twentysix_1 + 2.57 * h_twentysix_2Plus + 1.71 * h_thirtyone_1 
                + 2.42 * h_thirtyone_2Plus + 0.860 * h_fiftyone_1 + 0.889 * h_lYes_3 + 3.94 * h_female_3 
                + 1.18 * h_homeFT_1Plus + 1.30 * h_PT_1 + 1.26 * h_officeCler_1 + 2.01 * h_manConst_1 
                + 0.691 * h_profMan_1 + 1.11 * h_retail_1 + 1.60 * h_retail_2 + 4.72 * h_oNE_4Plus));

            return currValues;
        }

        private double ComputeNumberOfVehiclesProbablities(string category,
                           HouseholdPersonComposite composite, SpatialZone curZ)
        {
            var valList = GetUtilityValuesForNumberOfVehicles(composite, curZ);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]);
            if (int.Parse(category) ==
                (int)NumOfCars.NoCar)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfCars.OneCar)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfCars.TwoCars)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)NumOfCars.ThreeOrMore)
            {
                return ((double)valList[3] / logsum);
            }

            return 0.00;
        }

        private List<KeyValPair> ComputeNumberOfVehiclesCommulative(
            HouseholdPersonComposite composite, SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForNumberOfVehicles(composite, curZ);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3];
            currPair.Category = "0";//IncomeLevel.None.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.One.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.Two.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.Three.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            return comList;
        }

        private List<double> GetUtilityValuesForNumberOfVehicles(
      HouseholdPersonComposite composite,SpatialZone curZ)
        {
            Household hhld = composite.getHousehold().CreateNewCopy();
            //String key = hhld.GetNewJointKey(GetDimensionName());
            //string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(4);
			int myNumOfCars = (int) hhld.GetNumOfCars();
			int dwellType = (int) hhld.GetDwellingType();
			int hhldSize = (int) hhld.GetHhldSize();

            currValues.Add(1);

            int apart = Convert.ToInt32(dwellType== 1);
            int town = Convert.ToInt32(dwellType == 2);

            double z_oneCar = curZ.GetCarMarginal().GetValue("1");

            int oneAdult = Convert.ToInt32(hhldSize == 0);
            int twoAdults = Convert.ToInt32(hhldSize == 2);
            int twoAdultsChildren = Convert.ToInt32(hhldSize == 3);
            int twoPlusAdults = Convert.ToInt32(hhldSize == 2 || hhldSize == 4);
            int h_lYes_1 = Convert.ToInt32(composite.Count_driving_license_yes == 1);
            int h_lYes_2 = Convert.ToInt32(composite.Count_driving_license_yes == 2);
            int h_lYes_3 = Convert.ToInt32(composite.Count_driving_license_yes == 3);
            int h_lNo_1 = Convert.ToInt32(composite.Count_driving_license_no == 1);
            int h_lNo_2 = Convert.ToInt32(composite.Count_driving_license_no == 2);
            int h_lNo_3 = Convert.ToInt32(composite.Count_driving_license_no == 3);
            int h_lNo_4Plus = Convert.ToInt32(composite.Count_driving_license_no >= 4);
            int h_FT_1 = Convert.ToInt32(composite.Count_full_time == 1);
            int h_FT_2 = Convert.ToInt32(composite.Count_full_time == 2);
            int h_FT_3Plus = Convert.ToInt32(composite.Count_full_time >= 3);
            int h_homeFT_1Plus = Convert.ToInt32(composite.Count_full_time_home >= 1);
            int h_homePT_1Plus = Convert.ToInt32(composite.Count_full_time_home >= 1);
            int h_eNE_1 = Convert.ToInt32(composite.Count_unemployed == 1);
            int h_eNE_2 = Convert.ToInt32(composite.Count_unemployed == 2);
            int h_eNE_3 = Convert.ToInt32(composite.Count_unemployed == 3);
            int h_eNE_4Plus = Convert.ToInt32(composite.Count_unemployed >= 4);
            int h_PT_1 = Convert.ToInt32(composite.Count_part_time == 1);
            int h_officeCler_1 = Convert.ToInt32(composite.Count_clerical_manufacturing == 1);
            int h_manConst_1 = Convert.ToInt32(composite.Count_costruction_man == 1);
            int h_manConst_2Plus = Convert.ToInt32(composite.Count_costruction_man >= 2);
            int h_profMan_1 = Convert.ToInt32(composite.Count_professional_man == 1);
            int h_profMan_2Plus = Convert.ToInt32(composite.Count_professional_man >= 2);
            int h_retail_1 = Convert.ToInt32(composite.Count_retail == 1);
            int h_retail_2 = Convert.ToInt32(composite.Count_retail == 2);
            int h_retail_3Plus = Convert.ToInt32(composite.Count_retail >= 3);
            int h_zero_1 = Convert.ToInt32(composite.Count_less11 == 1);
            int h_zero_2 = Convert.ToInt32(composite.Count_less11 == 2);
            int h_zero_3 = Convert.ToInt32(composite.Count_less11 == 3);
            int h_eleven_1Plus = Convert.ToInt32(composite.Count_11 >= 1);
            int h_fourteen_1Plus = Convert.ToInt32(composite.Count_14 >= 1);
            int h_sixteen_1Plus = Convert.ToInt32(composite.Count_16 >= 1);
            int h_eighteen_1 = Convert.ToInt32(composite.Count_18 == 1);
            int h_eighteen_2Plus = Convert.ToInt32(composite.Count_18 >= 2);
            int h_twentysix_1 = Convert.ToInt32(composite.Count_26 == 1);
            int h_twentysix_2Plus = Convert.ToInt32(composite.Count_26 >= 2);
            int h_thirtyone_1 = Convert.ToInt32(composite.Count_31 == 1);
            int h_thirtyone_2Plus = Convert.ToInt32(composite.Count_31 >= 2);
            int h_fortyone_1 = Convert.ToInt32(composite.Count_41 == 1);
            int h_fortyone_2Plus = Convert.ToInt32(composite.Count_41 >= 2);
            int h_fiftyone_1 = Convert.ToInt32(composite.Count_51 == 1);
            int h_fiftyone_2Plus = Convert.ToInt32(composite.Count_51 >= 2);
            int h_fiftyfive_1 = Convert.ToInt32(composite.Count_55 == 1);
            int h_fiftyfive_2Plus = Convert.ToInt32(composite.Count_55 >= 2);
            int h_sixtyfivePlus_1 = Convert.ToInt32(composite.Count_65 >= 1);
            int h_sixtyfivePlus_2Plus = Convert.ToInt32(composite.Count_65 >= 2);
            int h_male_1 = Convert.ToInt32(composite.Count_male == 1);
            int h_male_3 = Convert.ToInt32(composite.Count_male == 3);
            int h_female_2 = Convert.ToInt32(composite.Count_female == 2);

            currValues.Add(Math.Exp(2.48 * 1.00 + -1.19 * apart + -0.185 * town
                + -2.28 * z_oneCar + -4.09 * oneAdult + -4.25 * twoAdults + 0.197 * twoAdultsChildren
                + -4.08 * twoPlusAdults + 3.25 * h_lYes_1 + 3.00 * h_lYes_2 + 2.27 * h_lYes_3
                + -1.40 * h_lNo_1 + -2.88 * h_lNo_2 + -3.97 * h_lNo_3 + -5.66 * h_lNo_4Plus
                + 0.583 * h_FT_1 + 0.985 * h_FT_2 + 1.17 * h_FT_3Plus + 0.571 * h_homeFT_1Plus
                + 0.257 * h_homePT_1Plus + 0.468 * h_eNE_1 + 1.03 * h_eNE_2 + 1.40 * h_eNE_3
                + 1.81 * h_eNE_4Plus + 0.125 * h_PT_1 + 0.251 * h_officeCler_1 + 0.655 * h_manConst_1
                + 0.891 * h_manConst_2Plus + 0.440 * h_profMan_1 + 0.400 * h_profMan_2Plus
                + 0.368 * h_retail_1 + 0.560 * h_retail_2 + 1.28 * h_retail_3Plus
                + -3.05 * h_zero_1 + -1.84 * h_zero_2 + -1.07 * h_zero_3 + 1.16 * h_eleven_1Plus
                + 1.21 * h_fourteen_1Plus + 1.09 * h_sixteen_1Plus + 0.505 * h_eighteen_1
                + 0.973 * h_eighteen_2Plus + 0.328 * h_twentysix_1 + 0.915 * h_twentysix_2Plus
                + 0.564 * h_thirtyone_1 + 1.16 * h_thirtyone_2Plus
                + 0.664 * h_fortyone_1 + 1.46 * h_fortyone_2Plus + 0.754 * h_fiftyone_1
                + 1.66 * h_fiftyone_2Plus + 0.855 * h_fiftyfive_1 + 2.11 * h_fiftyfive_2Plus
                + 1.25 * h_sixtyfivePlus_1 + 2.63 * h_sixtyfivePlus_2Plus + 0.0863 * h_male_1
                + 0.200 * h_male_3 + -0.127 * h_female_2));

            double z_twoCars = curZ.GetCarMarginal().GetValue("2");
                     
            int oneAdultChildren = Convert.ToInt32(hhldSize == 3);
            int h_male_2 = Convert.ToInt32(composite.Count_male == 2);
            int h_male_4Plus = Convert.ToInt32(composite.Count_male >= 4);
            int h_female_3 = Convert.ToInt32(composite.Count_female == 3);
            int h_female_4Plus = Convert.ToInt32(composite.Count_female >= 4);

            currValues.Add(Math.Exp(-0.577 * 1.00 + -2.39 * apart + -0.858 * town + 5.45 * z_twoCars
                + -5.05 * oneAdult + 0.460 * oneAdultChildren + -5.13 * twoAdults + 0.308 * twoAdultsChildren
                + -4.94 * twoPlusAdults + 1.28 * h_lYes_1 + 3.11 * h_lYes_2 + 2.49 * h_lYes_3
                + -2.06 * h_lNo_1 + -4.02 * h_lNo_2 + -5.62 * h_lNo_3 + -7.96 * h_lNo_4Plus
                + 1.09 * h_FT_1 + 1.91 * h_FT_2 + 1.93 * h_FT_3Plus + 0.898 * h_homeFT_1Plus 
                +0.501 * h_homePT_1Plus + 0.450 * h_eNE_1 + 0.891 * h_eNE_2 + 1.26 * h_eNE_3
                + 1.51 * h_eNE_4Plus + 0.268 * h_PT_1 + 0.289 * h_officeCler_1 + 0.883 * h_manConst_1
                + 0.965 * h_manConst_2Plus + 0.652 * h_profMan_1 + 0.621 * h_profMan_2Plus
                + 0.512 * h_retail_1 + 0.736 * h_retail_2 + 1.32 * h_retail_3Plus + -3.76 * h_zero_1
                + -2.26 * h_zero_2 + -1.38 * h_zero_3 + 1.45 * h_eleven_1Plus + 1.40 * h_fourteen_1Plus
                + 0.935 * h_sixteen_1Plus + 0.163 * h_eighteen_1 + 0.433 * h_eighteen_2Plus
                + 0.281 * h_twentysix_1 + 0.870 * h_twentysix_2Plus + 0.480 * h_thirtyone_1
                + 1.15 * h_thirtyone_2Plus + 0.702 * h_fortyone_1 + 1.71 * h_fortyone_2Plus
                + 0.961 * h_fiftyone_1 + 2.11 * h_fiftyone_2Plus + 1.15 * h_fiftyfive_1 
                + 2.68 * h_fiftyfive_2Plus + 1.45 * h_sixtyfivePlus_1 + 2.97 * h_sixtyfivePlus_2Plus
                + 0.900 * h_male_1 + 1.21 * h_male_2 + 1.85 * h_male_3 + 2.28 * h_male_4Plus
                + 0.328 * h_female_2 + 0.817 * h_female_3 + 1.35 * h_female_4Plus));

            double z_threePlusCars = curZ.GetCarMarginal().GetValue("3");

            currValues.Add(Math.Exp(-1.19 * 1.00 + -3.18 * apart + -1.55 * town + 15.2 * z_threePlusCars
                + -5.01 * oneAdult + -5.35 * twoAdults + -4.90 * twoPlusAdults + 1.09 * h_lYes_2
                + 1.93 * h_lYes_3 + -2.52 * h_lNo_1 + -4.78 * h_lNo_2 + -6.69 * h_lNo_3
                + -9.39 * h_lNo_4Plus + 1.18 * h_FT_1 + 2.15 * h_FT_2 + 2.67 * h_FT_3Plus + 1.18 * h_homeFT_1Plus
                + 0.669 * h_homePT_1Plus + 0.239 * h_eNE_1 + 0.537 * h_eNE_2 + 0.877 * h_eNE_3
                + 1.10 * h_eNE_4Plus + 0.250 * h_PT_1 + 0.312 * h_officeCler_1 + 1.02 * h_manConst_1
                + 1.10 * h_manConst_2Plus + 0.790 * h_profMan_1 + 0.769 * h_profMan_2Plus
                + 0.539 * h_retail_1 + 0.786 * h_retail_2 + 1.37 * h_retail_3Plus + -3.64 * h_zero_1
                + -2.18 * h_zero_2 + -1.16 * h_zero_3 + 1.28 * h_eleven_1Plus + 1.32 * h_fourteen_1Plus
                + 0.303 * h_sixteen_1Plus + 0.343 * h_twentysix_1 + 0.841 * h_twentysix_2Plus
                + 0.572 * h_thirtyone_1 + 1.27 * h_thirtyone_2Plus + 0.857 * h_fortyone_1 + 2.16 * h_fortyone_2Plus
                + 1.31 * h_fiftyone_1 + 2.87 * h_fiftyone_2Plus + 1.46 * h_fiftyfive_1
                + 3.36 * h_fiftyfive_2Plus + 1.70 * h_sixtyfivePlus_1 + 3.34 * 2.63 + 1.46 * h_male_1
                + 2.04 * h_male_2 + 3.00 * h_male_3 + 4.02 * h_male_4Plus + 0.594 * h_female_2
                + 1.45 * h_female_3 + 2.39 * h_female_4Plus));

            return currValues;
        }

        private double ComputeDwellingTypeProbablities(string category,
                                    HouseholdPersonComposite composite, SpatialZone curZ)
        {
            var valList = GetUtilityValuesForDwellingType(composite, curZ);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]);
            if (int.Parse(category) ==
                (int)DwellingType.House)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)DwellingType.Apartment)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)DwellingType.Townhouse)
            {
                return ((double)valList[2] / logsum);
            }

            return 0.00;
        }

        private List<KeyValPair> ComputeDwellingTypeCommulative(
            HouseholdPersonComposite composite, SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForDwellingType(composite, curZ);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2];
            currPair.Category = "0";//IncomeLevel.House.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.Apartment.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.Townhouse.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);
            
            return comList;
        }

        private List<double> GetUtilityValuesForDwellingType(
     HouseholdPersonComposite composite, SpatialZone curZ)
        {
            Household hhld = composite.getHousehold().CreateNewCopy();
            //String key = hhld.GetNewJointKey(GetDimensionName());
            //string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(3);
			int myNumOfCars = (int) hhld.GetNumOfCars();
			int dwellType = (int) hhld.GetDwellingType();
			int hhldSize = (int) hhld.GetHhldSize();

            currValues.Add(1);

            int oneCar = Convert.ToInt32(myNumOfCars == 1);
            int twoCars = Convert.ToInt32(myNumOfCars == 2);
            int threePlusCars = Convert.ToInt32(myNumOfCars >= 3);
            int oneAdult = Convert.ToInt32(hhldSize == 3);

            // doubt
            double z_apart = curZ.GetDwellingMarginals().GetValue("1");

            int oneAdultChildren = Convert.ToInt32(hhldSize == 1);
            int twoAdults = Convert.ToInt32(hhldSize == 2);
            int h_twentysix_2Plus = Convert.ToInt32(composite.Count_26 >= 2);
            int h_thirtyone_1 = Convert.ToInt32(composite.Count_31 == 1);
            int twoPlusAdults = Convert.ToInt32(hhldSize == 2 || hhldSize == 4);
            int h_eleven_1Plus = Convert.ToInt32(composite.Count_11 >= 1);
            int h_eighteen_2Plus = Convert.ToInt32(composite.Count_18 >= 2);
            int h_fortyone_1 = Convert.ToInt32(composite.Count_41 == 1);
            int h_fortyone_2Plus = Convert.ToInt32(composite.Count_41 >= 2);
            int h_eighteen_1 = Convert.ToInt32(composite.Count_18 == 1);
            int h_twentysix_1 = Convert.ToInt32(composite.Count_26 == 1);
            int h_fiftyone_1 = Convert.ToInt32(composite.Count_51 == 1);
            int h_thirtyone_2Plus = Convert.ToInt32(composite.Count_31 == 2);
            int h_fiftyfive_2Plus = Convert.ToInt32(composite.Count_55 == 2);
            int h_sixtyfivePlus_1 = Convert.ToInt32(composite.Count_65 == 1);
            int h_male_1 = Convert.ToInt32(composite.Count_male == 1);
            int h_male_2 = Convert.ToInt32(composite.Count_male == 2);
            int h_female_4Plus = Convert.ToInt32(composite.Count_female >= 4);
            int h_male_3 = Convert.ToInt32(composite.Count_male == 3);
            int h_male_4Plus = Convert.ToInt32(composite.Count_male >= 4);
            int h_female_1 = Convert.ToInt32(composite.Count_female == 1);
            int h_fiftyfive_1 = Convert.ToInt32(composite.Count_55 == 1);
            int h_FT_1 = Convert.ToInt32(composite.Count_full_time == 1);
            int h_homeFT_1Plus = Convert.ToInt32(composite.Count_full_time >= 1);
            int h_FT_2 = Convert.ToInt32(composite.Count_full_time == 2);
            int h_FT_3Plus = Convert.ToInt32(composite.Count_full_time >= 3);
            int h_eNE_3 = Convert.ToInt32(composite.Count_unemployed == 3);
            int h_homePT_1Plus = Convert.ToInt32(composite.Count_part_time_home >= 1);
            int h_eNE_1 = Convert.ToInt32(composite.Count_unemployed == 1);
            int h_eNE_2 = Convert.ToInt32(composite.Count_unemployed == 2);
            int h_eNE_4Plus = Convert.ToInt32(composite.Count_unemployed >= 4);
            int h_officeCler_1 = Convert.ToInt32(composite.Count_clerical_manufacturing == 1);
            int h_officeCler_2Plus = Convert.ToInt32(composite.Count_clerical_manufacturing >= 2);
            int h_manConst_1 = Convert.ToInt32(composite.Count_costruction_man == 1);
            int h_manConst_2Plus = Convert.ToInt32(composite.Count_costruction_man >= 2);
            int h_profMan_1 = Convert.ToInt32(composite.Count_professional_man == 1);
            int h_profMan_2Plus = Convert.ToInt32(composite.Count_professional_man >= 2);
            int h_retail_1 = Convert.ToInt32(composite.Count_retail == 1);
            int h_retail_2 = Convert.ToInt32(composite.Count_retail == 2);
            int h_retail_3Plus = Convert.ToInt32(composite.Count_retail >= 3);
            int h_lYes_1 = Convert.ToInt32(composite.Count_driving_license_yes == 1);
            int h_lYes_3 = Convert.ToInt32(composite.Count_driving_license_yes == 3);
            int h_lYes_4Plus = Convert.ToInt32(composite.Count_driving_license_yes >= 4);
            int h_lNo_1 = Convert.ToInt32(composite.Count_driving_license_no == 1);
            int h_lNo_2 = Convert.ToInt32(composite.Count_driving_license_no == 2);

            currValues.Add(Math.Exp(0.772 * 1.00 + -1.06 * oneCar + -2.30 * twoCars + -3.18 * threePlusCars
                + 4.04 * z_apart + -0.312 * oneAdult + 0.180 * oneAdultChildren + -0.375 * twoAdults
                + -0.396 * twoPlusAdults + 0.246 * h_eleven_1Plus + 0.672 * h_eighteen_1 + 1.15 * h_eighteen_2Plus
                + 0.688 * h_twentysix_1 + 1.16 * h_twentysix_2Plus + 0.641 * h_thirtyone_1 + 0.786 * h_thirtyone_2Plus
                + 0.227 * h_fortyone_1 + 0.391 * h_fortyone_2Plus + 0.117 * h_fiftyone_1 + 0.0771 * h_fiftyfive_1
                + -0.109 * h_fiftyfive_2Plus + -0.124 * h_sixtyfivePlus_1 + -0.131 * h_male_1
                + -0.184 * h_male_2 + -0.234 * h_male_3 + -0.227 * h_male_4Plus + 0.0740 * h_female_1
                + -0.269 * h_female_4Plus + -0.212 * h_FT_1 + -0.528 * h_FT_2 + -0.717 * h_FT_3Plus
                + -0.537 * h_homeFT_1Plus + -0.330 * h_homePT_1Plus + -0.560 * h_eNE_1 + -1.21 * h_eNE_2
                + -1.43 * h_eNE_3 + -1.62 * h_eNE_4Plus + -0.541 * h_officeCler_1 + -1.03 * h_officeCler_2Plus
                + -0.293 * h_manConst_1 + -0.618 * h_manConst_2Plus + -0.746 * h_profMan_1
                + -1.60 * h_profMan_2Plus + -0.315 * h_retail_1 + -0.671 * h_retail_2 + -1.21 * h_retail_3Plus
                + 0.168 * h_lYes_1 + -0.324 * h_lYes_3 + -0.675 * h_lYes_4Plus + -0.110 * h_lNo_1
                + -0.140 * h_lNo_2));

            // doubt
            double z_town = curZ.GetDwellingMarginals().GetValue("2");

            int h_zero_1 = Convert.ToInt32(composite.Count_less11 == 1);
            int h_zero_4Plus =Convert.ToInt32(composite.Count_less11 >= 4);
            int h_fiftyone_2Plus = Convert.ToInt32(composite.Count_51 >= 2);
            int h_PT_1 = Convert.ToInt32(composite.Count_part_time == 1);

             currValues.Add(Math.Exp(-1.33 * 1.00 + -0.351 * oneCar + -1.08 * twoCars + -1.93  * threePlusCars 
                 + 11.8 * z_town + -0.747 * oneAdult + 0.429 * oneAdultChildren + -0.337  * twoAdults 
                 + 0.172 * h_zero_1 + 0.347 * h_zero_4Plus +  0.212 * h_eleven_1Plus + 0.598 * h_eighteen_1 
                 + 0.960 * h_eighteen_2Plus + 0.548 * h_twentysix_1 + 1.17 * h_twentysix_2Plus 
                 + 0.506 * h_thirtyone_1 + 0.796 * h_thirtyone_2Plus + 0.273 * h_fortyone_1 
                 + 0.388 * h_fortyone_2Plus + 0.143 * h_fiftyone_1 + 0.185 * h_fiftyone_2Plus 
                 + 0.215 * h_fiftyfive_1 + -0.211 * h_sixtyfivePlus_1 + -0.375 * h_male_1 
                 + -0.359 * h_male_2 + -0.392 * h_male_3 + -0.299 * h_male_4Plus 
                 + -0.0706 * h_female_1 + 0.0664 * h_FT_1 + -0.161 * h_FT_3Plus + -0.274 * h_homeFT_1Plus 
                 + -0.484 * h_eNE_1 + -0.951 * h_eNE_2 + -1.01 * h_eNE_3 + -1.07 * h_eNE_4Plus 
                 + -0.100 * h_PT_1 + -0.160 * h_officeCler_1 + -0.564 * h_officeCler_2Plus 
                 + -0.259 * h_manConst_1 + -0.511 * h_manConst_2Plus + -0.234 * h_profMan_1 
                 + -0.651 * h_profMan_2Plus + -0.0924  * h_retail_1 + -0.310 * h_retail_2 + -0.625 * h_retail_3Plus 
                 + 0.225 * h_lYes_1 + -0.395 * h_lYes_3 + -0.787 * h_lYes_4Plus + -0.176 * h_lNo_1 
                 + -0.135 * h_lNo_2));

            return currValues;
        }


        private double ComputePublicTransitPassProbablities(string category,
                           HouseholdPersonComposite composite)
        {
            var valList = GetUtilityValuesForPublicTransitPass(composite);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]
                                     + (double)valList[5]);
            if (int.Parse(category) ==
                (int)PublicTransitPass.None)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)PublicTransitPass.MetroPass)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)PublicTransitPass.GOTransitPass)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)PublicTransitPass.ComboOrDualPass)
            {
                return ((double)valList[3] / logsum);
            }
            else if (int.Parse(category) ==
                (int)PublicTransitPass.OtherAgencyPass)
            {
                return ((double)valList[4] / logsum);
            }
            else if (int.Parse(category) ==
                 (int)PublicTransitPass.Unknown)
            {
                return ((double)valList[5] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputePublicTransitPassCommulative(HouseholdPersonComposite composite)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForPublicTransitPass(composite);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]
                                     + (double)valList[5];
            currPair.Category = "0";//IncomeLevel.None.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.MetroPass.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.GOTransitPass.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.ComboOrDualPass.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "4";//IncomeLevel.OtherAgencyPass.ToString();
            currPair.Value = comVal + (double)valList[4] / utilSum;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "5";//IncomeLevel.Unknown.ToString();
            currPair.Value = comVal + (double)valList[5] / utilSum;
            comList.Add(currPair);

            return comList;
        }
        private List<double> GetUtilityValuesForPublicTransitPass(
                 HouseholdPersonComposite composite)
        {
            Household hhld = composite.getHousehold().CreateNewCopy();
            //String key = hhld.GetNewJointKey(GetDimensionName());
            //string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(6);
			int myNumOfCars = (int) hhld.GetNumOfCars();
			int dwellType = (int) hhld.GetDwellingType();
			int hhldSize = (int) hhld.GetHhldSize();

            currValues.Add(1);

            int oneCar = Convert.ToInt32(myNumOfCars == 1);
            int twoCars = Convert.ToInt32(myNumOfCars == 2);
            int threePlusCars = Convert.ToInt32(myNumOfCars >= 3);
            int oneAdult = Convert.ToInt32(hhldSize == 3);
            int oneAdultChildren = Convert.ToInt32(hhldSize == 1);
            int twoAdults = Convert.ToInt32(hhldSize == 2);
            int h_twentysix_2Plus = Convert.ToInt32(composite.Count_26 >= 2);
            int h_thirtyone_1 = Convert.ToInt32(composite.Count_31 == 1);
            int twoPlusAdults = Convert.ToInt32(hhldSize == 2 || hhldSize == 4);
            int h_eleven_1Plus = Convert.ToInt32(composite.Count_11 >= 1);
            int h_eighteen_2Plus = Convert.ToInt32(composite.Count_18 >= 2);
            int h_fortyone_1 = Convert.ToInt32(composite.Count_41 == 1);
            int h_fortyone_2Plus = Convert.ToInt32(composite.Count_41 >= 2);
            int h_eighteen_1 = Convert.ToInt32(composite.Count_18 == 1);
            int h_twentysix_1 = Convert.ToInt32(composite.Count_26 == 1);
            int h_fiftyone_1 = Convert.ToInt32(composite.Count_51 == 1);
            int h_thirtyone_2Plus = Convert.ToInt32(composite.Count_31 == 2);
            int h_fiftyfive_2Plus = Convert.ToInt32(composite.Count_55 == 2);
            int h_sixtyfivePlus_1 = Convert.ToInt32(composite.Count_65 == 1);
            int h_male_1 = Convert.ToInt32(composite.Count_male == 1);
            int h_male_2 = Convert.ToInt32(composite.Count_male == 2);
            int h_female_4Plus = Convert.ToInt32(composite.Count_female >= 4);
            int h_male_3 = Convert.ToInt32(composite.Count_male == 3);
            int h_male_4Plus = Convert.ToInt32(composite.Count_male >= 4);
            int h_female_1 = Convert.ToInt32(composite.Count_female == 1);
            int h_fiftyfive_1 = Convert.ToInt32(composite.Count_55 == 1);
            int h_FT_1 = Convert.ToInt32(composite.Count_full_time == 1);
            int h_homeFT_1Plus = Convert.ToInt32(composite.Count_full_time >= 1);
            int h_FT_2 = Convert.ToInt32(composite.Count_full_time == 2);
            int h_FT_3Plus = Convert.ToInt32(composite.Count_full_time >= 3);
            int h_eNE_3 = Convert.ToInt32(composite.Count_unemployed == 3);
            int h_homePT_1Plus = Convert.ToInt32(composite.Count_part_time_home >= 1);
            int h_eNE_1 = Convert.ToInt32(composite.Count_unemployed == 1);
            int h_eNE_2 = Convert.ToInt32(composite.Count_unemployed == 2);
            int h_eNE_4Plus = Convert.ToInt32(composite.Count_unemployed >= 4);
            int h_officeCler_1 = Convert.ToInt32(composite.Count_clerical_manufacturing == 1);
            int h_officeCler_2Plus = Convert.ToInt32(composite.Count_clerical_manufacturing >= 2);
            int h_manConst_1 = Convert.ToInt32(composite.Count_costruction_man == 1);
            int h_manConst_2Plus = Convert.ToInt32(composite.Count_costruction_man >= 2);
            int h_profMan_1 = Convert.ToInt32(composite.Count_professional_man == 1);
            int h_retail_1 = Convert.ToInt32(composite.Count_retail == 1);
            int h_retail_2 = Convert.ToInt32(composite.Count_retail == 2);
            int h_retail_3Plus = Convert.ToInt32(composite.Count_retail >= 3);
            int h_lYes_1 = Convert.ToInt32(composite.Count_driving_license_yes == 1);
            int h_lNo_2 = Convert.ToInt32(composite.Count_driving_license_no == 2);
            int h_fiftyone_2Plus = Convert.ToInt32(composite.Count_51 >= 2);
            int h_PT_1 = Convert.ToInt32(composite.Count_part_time == 1);
            int apart = Convert.ToInt32(dwellType == 1);
            int town = Convert.ToInt32(dwellType == 2);
            int twoAdultsChildren = Convert.ToInt32(hhldSize == 3);
            int h_zero_3 = Convert.ToInt32(composite.Count_less11 == 3);
            int h_fourteen_1Plus = Convert.ToInt32(composite.Count_14 >= 1);
            int h_sixteen_1Plus = Convert.ToInt32(composite.Count_16 >= 1);
            int h_sixtyfivePlus_2Plus = Convert.ToInt32(composite.Count_65 >= 2);
            int h_female_2 = Convert.ToInt32(composite.Count_female == 2);
            int h_female_3 = Convert.ToInt32(composite.Count_female == 3);
            int h_lYes_2 = Convert.ToInt32(composite.Count_driving_license_yes == 2);

            currValues.Add(Math.Exp(-1.37 * 1.00 + -1.45 * apart + -0.0995 * town + 1.14 * oneCar
                + 2.18 * twoCars + 3.05 * threePlusCars + 0.214 * oneAdult + 0.160 * twoAdults + -0.125 * twoPlusAdults
                + 0.356 * h_zero_3 + 0.109 * h_eleven_1Plus + -0.267 * h_eighteen_1 + -0.566 * h_eighteen_2Plus
                + -0.517 * h_twentysix_1 + -0.500 * h_twentysix_2Plus + -0.500 * h_thirtyone_1 + -1.02 * h_thirtyone_2Plus
                + -0.747 * h_fortyone_1 + -1.41 * h_fortyone_2Plus + -0.576 * h_fiftyone_1 + -1.25 * h_fiftyone_2Plus
                + -0.816 * h_fiftyfive_1 + -1.45 * h_fiftyfive_2Plus + -0.828 * h_sixtyfivePlus_1 + -1.54 * h_sixtyfivePlus_2Plus
                + -0.0828 * h_male_2 + 0.222 * h_female_1 + 0.178 * h_female_2 + 0.229 * h_female_3 + -0.523 * h_FT_1
                + -0.854 * h_FT_2 + -1.04 * h_FT_3Plus + -0.508 * h_homeFT_1Plus + -0.699 * h_homePT_1Plus + 0.199 * h_eNE_1
                + 0.334 * h_eNE_2 + 0.230 * h_eNE_3 + -0.256 * h_PT_1 + 0.127 * h_officeCler_1 + 0.361 * h_officeCler_2Plus
                + 0.575 * h_manConst_1 + 0.565 * h_manConst_2Plus + 0.387 * h_retail_1 + 0.680 * h_retail_2 + 0.907 * h_retail_3Plus
                + -0.343 * h_lYes_1 + -0.212 * h_lYes_2 + -0.0865 * h_lNo_2));

            currValues.Add(Math.Exp(-1.37 * 1.00 + -1.45 * apart + -0.0995 * town + 1.14 * oneCar + 2.18 * twoCars
                + 3.05 * threePlusCars + 0.214 * oneAdult + 0.160 * twoAdults + -0.125 * twoPlusAdults + 0.356 * h_zero_3
                + 0.109 * h_eleven_1Plus + -0.267 * h_eighteen_1 + -0.566 * h_eighteen_2Plus + -0.517 * h_twentysix_1
                + -0.500 * h_twentysix_2Plus + -0.500 * h_thirtyone_1 + -1.02 * h_thirtyone_2Plus
                + -0.747 * h_fortyone_1 + -1.41 * h_fortyone_2Plus + -0.576 * h_fiftyone_1 + -1.25 * h_fiftyone_2Plus
                + -0.816 * h_fiftyfive_1 + -1.45 * h_fiftyfive_2Plus + -0.828 * h_sixtyfivePlus_1 + -1.54 * h_sixtyfivePlus_2Plus
                + -0.0828 * h_male_2 + 0.222 * h_female_1 + 0.178 * h_female_2 + 0.229 * h_female_3 + -0.523 * h_FT_1
                + -0.854 * h_FT_2 + -1.04 * h_FT_3Plus + -0.508 * h_homeFT_1Plus + -0.699 * h_homePT_1Plus
                + 0.199 * h_eNE_1 + 0.334 * h_eNE_2 + 0.230 * h_eNE_3 + -0.256 * h_PT_1 + 0.127 * h_officeCler_1
                + 0.361 * h_officeCler_2Plus + 0.575 * h_manConst_1 + 0.565 * h_manConst_2Plus + 0.387 * h_retail_1
                + 0.680 * h_retail_2 + 0.907 * h_retail_3Plus + -0.343 * h_lYes_1 + -0.212 * h_lYes_2 + -0.0865 * h_lNo_2));

            currValues.Add(Math.Exp(-1.47 * 1.00 + -0.481 * apart + 0.529 * town + 1.03 * oneCar + 1.79 * twoCars
                + 2.20 * threePlusCars + -0.241 * oneAdult + -0.252 * twoAdults + -0.106 * twoAdultsChildren
                + -0.303 * twoPlusAdults + 0.141 * h_zero_3 + 0.219 * h_eleven_1Plus + 0.202 * h_fourteen_1Plus
                + 0.146 * h_sixteen_1Plus + 0.161 * h_twentysix_2Plus + 0.0627 * h_thirtyone_1 + -0.153 * h_fortyone_1
                + -0.376 * h_fortyone_2Plus + -0.224 * h_fiftyone_1 + -0.452 * h_fiftyone_2Plus + -0.303 * h_fiftyfive_1
                + -0.497 * h_fiftyfive_2Plus + -0.221 * h_sixtyfivePlus_1 + -0.547 * h_sixtyfivePlus_2Plus + -0.0548 * h_male_2
                + 0.222 * h_female_1 + 0.179 * h_female_2 + 0.207 * h_female_3 + 0.153 * h_female_4Plus + -0.277 * h_FT_1
                + -0.508 * h_FT_2 + -0.626 * h_FT_3Plus + -0.574 * h_homeFT_1Plus + -0.627 * h_homePT_1Plus + 0.0627 * h_eNE_2
                + -0.235 * h_PT_1 + 0.151 * h_officeCler_1 + 0.338 * h_officeCler_2Plus + 0.529 * h_manConst_1
                + 0.860 * h_manConst_2Plus + -0.0583 * h_profMan_1 + 0.313 * h_retail_1 + 0.480 * h_retail_2
                + 0.629 * h_retail_3Plus + -0.326 * h_lYes_1 + -0.242 * h_lYes_2));

            int h_zero_2 = Convert.ToInt32(composite.Count_less11 == 2);
            int h_lNo_3 = Convert.ToInt32(composite.Count_driving_license_no == 3);
            int h_lNo_4Plus = Convert.ToInt32(composite.Count_driving_license_no >= 4);
            int h_lNo_1 = Convert.ToInt32(composite.Count_driving_license_no == 1);

            currValues.Add(Math.Exp(-1.75 * 1.00 + -0.675 * apart + 0.807 * town + 1.44 * oneCar + 2.67 * twoCars
                + 3.50 * threePlusCars + -0.156 * twoAdults + -0.310 * twoPlusAdults + 0.228 * h_zero_2 + 0.415 * h_zero_3
                + 0.238 * h_eleven_1Plus + 0.135 * h_fourteen_1Plus + -0.483 * h_eighteen_1 + -0.770 * h_eighteen_2Plus
                + -0.544 * h_twentysix_1 + -0.704 * h_twentysix_2Plus + -0.634 * h_thirtyone_1 + -1.16 * h_thirtyone_2Plus
                + -0.774 * h_fortyone_1 + -1.45 * h_fortyone_2Plus + -0.873 * h_fiftyone_1
                + -1.51 * h_fiftyone_2Plus + -0.953 * h_fiftyfive_1 + -1.51 * h_fiftyfive_2Plus + -0.708 * h_sixtyfivePlus_1
                + -1.30 * h_sixtyfivePlus_2Plus + -0.0968 * h_male_2 + 0.348 * h_female_1 + 0.332 * h_female_2 + 0.297 * h_female_3
                + 0.438 * h_female_4Plus + -0.300 * h_FT_1 + -0.433 * h_FT_2 + -0.827 * h_FT_3Plus + -0.187 * h_homeFT_1Plus
                + -0.334 * h_homePT_1Plus + 0.0763 * h_eNE_2 + -0.135 * h_PT_1 + 0.0968 * h_manConst_1 + 0.173 * h_retail_1
                + 0.299 * h_retail_2 + 0.448 * h_retail_3Plus + -0.566 * h_lYes_1 + -0.282 * h_lYes_2
                + -0.145 * h_lNo_1 + -0.263 * h_lNo_2 + -0.469 * h_lNo_3 + -0.648 * h_lNo_4Plus));

            int h_lYes_3 = Convert.ToInt32(composite.Count_driving_license_yes == 3);

            currValues.Add(Math.Exp(-0.440 * 1.00 + -0.897 * apart + 0.117 * town + 0.475 * oneCar + 1.19 * twoCars
                + 1.88 * threePlusCars + 0.197 * twoAdults + -0.194 * h_eighteen_1 + -0.255 * h_eighteen_2Plus + -0.228 * h_twentysix_1
                + -0.221 * h_twentysix_2Plus + -0.383 * h_thirtyone_1 + -0.688 * h_thirtyone_2Plus + -0.675 * h_fortyone_1
                + -1.20 * h_fortyone_2Plus + -0.622 * h_fiftyone_1 + -1.24 * h_fiftyone_2Plus + -0.670 * h_fiftyfive_1
                + -1.21 * h_fiftyfive_2Plus + -0.817 * h_sixtyfivePlus_1 + -1.50 * h_sixtyfivePlus_2Plus + -0.0778 * h_male_2
                + 0.0531 * h_female_1 + -0.672 * h_FT_1 + -1.05 * h_FT_2 + -1.38 * h_FT_3Plus + -0.782 * h_homeFT_1Plus
                + -0.724 * h_homePT_1Plus + 0.0577 * h_eNE_1 + 0.137 * h_eNE_2 + -0.191 * h_PT_1 + 0.694 * h_manConst_1
                + 0.782 * h_manConst_2Plus + -0.0655 * h_profMan_1 + 0.334 * h_retail_1 + 0.530 * h_retail_2
                + 0.686 * h_retail_3Plus + -0.268 * h_lYes_1 + -0.284 * h_lYes_2 + -0.137 * h_lYes_3));

            return currValues;
        }

        private double ComputeSexProbablities(string category,
                                   Person person)
        {
            var valList = GetUtilityValuesForSex(person);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]);
            if (int.Parse(category) ==
                (int)Sex.Male)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)Sex.Female)
            {
                return ((double)valList[1] / logsum);
            }
           
            return 0.00;
        }

        private List<KeyValPair> ComputeSexCommulative(Person person)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForSex(person);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1];

            currPair.Category = "0";//IncomeLevel.Male.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.Female.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;

            return comList;
        }

        private List<double> GetUtilityValuesForSex(Person person)
        {
            var currValues = new List<double>(2);

            currValues.Add(1);

            int twoAdultsChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.TwoAdultsChildren);
            int twoPlusAdults = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.Twoadults
                || person.GetHhld().GetHhldSize() == HouseholdSize.ThreeOrMoreAdults);
            int twoPlusAdultsChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.TwoAdultsChildren
                || person.GetHhld().GetHhldSize() == HouseholdSize.ThreeOrMoreAdultsChildren);
            int threePlusCars = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.ThreeOrMore);
            int lNo = Convert.ToInt32(person.GetDrivingLicense() == DrivingLicense.No);
            int homeFT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.FullTimeHome);
            int homePT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.PartTimeHome);
            int PT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.PartTime);
            int town = Convert.ToInt32(person.GetHhld().GetDwellingType() == DwellingType.Townhouse);
            int manConst = Convert.ToInt32(person.GetOccupation() == Occupation.ManufacturingConstructionTrades);
            int profMan = Convert.ToInt32(person.GetOccupation() == Occupation.ProfessionalManagementTechnical);
            int retail = Convert.ToInt32(person.GetOccupation() == Occupation.RetailSalesService);
            int oNE = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.Unemployed);
            int eleven = Convert.ToInt32(person.GetAge() == Age.ElevenToThirteen);
            int sixteen = Convert.ToInt32(person.GetAge() == Age.SixteenToSeventeen);
            int eighteen = Convert.ToInt32(person.GetAge() == Age.EighteenToTwentyFive);
            int twentysix = Convert.ToInt32(person.GetAge() == Age.TwentySixToThirty);
            int fortyone = Convert.ToInt32(person.GetAge() == Age.FortyOneToFifty);
            int fiftyone = Convert.ToInt32(person.GetAge() == Age.FiftyOneToFiftyFour);
            int fiftyfive = Convert.ToInt32(person.GetAge() == Age.FiftyfiveToSixtyFour);
            int sixtyfivePlus = Convert.ToInt32(person.GetAge() == Age.MoreThanSixtyFive);

            currValues.Add(Math.Exp(0.436 * 1.00 + -0.261 * twoAdultsChildren + -0.174 * twoPlusAdults
                + -0.222 * twoPlusAdultsChildren + 0.0472 * threePlusCars + 0.443 * lNo + -0.267 * homeFT
                + 0.530 * homePT + 0.900 * PT + 0.0508 * town + -2.33 * manConst + -0.733 * profMan
                + -0.824 * retail + -0.329 * oNE + -0.451 * eleven + -0.354 * sixteen + -0.154 * eighteen
                + 0.371 * twentysix + 0.428 * fortyone + 0.326 * fiftyone + 0.216 * fiftyfive + -0.0818 * sixtyfivePlus));

            return currValues;
        }

        private double ComputeDriverLisenceProbablities(string category,
                                 Person person)
        {
            var valList = GetUtilityValuesForDriverLicense(person);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]);
            if (int.Parse(category) ==
                (int)DrivingLicense.Yes)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)DrivingLicense.No)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)DrivingLicense.Unkown)
            {
                return ((double)valList[2] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeDriverLisenceCommulative(Person person)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForDriverLicense(person);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1];
            currPair.Category = "0";//IncomeLevel.yes.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.no.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            return comList;
        }

        private List<double> GetUtilityValuesForDriverLicense(Person person)
        {
            var currValues = new List<double>(2);

            currValues.Add(1);

            int twoAdultsChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.TwoAdultsChildren);
            int twoPlusAdultsChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.TwoAdultsChildren
                || person.GetHhld().GetHhldSize() == HouseholdSize.ThreeOrMoreAdultsChildren);
            int homeFT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.FullTimeHome);
            int homePT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.PartTimeHome);
            int PT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.PartTime);
            int apart = Convert.ToInt32(person.GetHhld().GetDwellingType() == DwellingType.Apartment);
            int manConst = Convert.ToInt32(person.GetOccupation() == Occupation.ManufacturingConstructionTrades);
            int retail = Convert.ToInt32(person.GetOccupation() == Occupation.RetailSalesService);
            int sixteen = Convert.ToInt32(person.GetAge() == Age.SixteenToSeventeen);
            int eighteen = Convert.ToInt32(person.GetAge() == Age.EighteenToTwentyFive);
            int twentysix = Convert.ToInt32(person.GetAge() == Age.TwentySixToThirty);
            int fortyone = Convert.ToInt32(person.GetAge() == Age.FortyOneToFifty);

            int oneAdultChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.OneAdultOneChild);
            int twoAdults = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.Twoadults);
            int twoPlusAdults = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.ThreeOrMoreAdults
                || person.GetHhld().GetHhldSize() == HouseholdSize.Twoadults);
            int oneCar = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.OneCar);
            int twoCars = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.TwoCars);

            currValues.Add(Math.Exp(-0.833 * 1.00 + 2.17 * oneAdultChildren + -0.103 * twoAdults + 1.85 * twoAdultsChildren
                + 1.02 * twoPlusAdults + 2.17 * twoPlusAdultsChildren + 0.693 * apart + -0.392 * oneCar + -0.743 * twoCars
                + -1.35 * homeFT + -1.26 * homePT + -0.220 * PT + -1.95 * manConst + -1.22 * retail + 0.811 * sixteen
                + -0.621 * eighteen + -1.52 * twentysix + -1.80 * fortyone));

            return currValues;
        }

        private double ComputeOccupationProbablities(string category,
                                  Person person, SpatialZone curZ)
        {
            var valList = GetUtilityValuesForOccupation(person, curZ);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]);
            if (int.Parse(category) ==
                (int)Occupation.GeneralOfficeClerical)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)Occupation.ManufacturingConstructionTrades)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)Occupation.ProfessionalManagementTechnical)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)Occupation.RetailSalesService)
            {
                return ((double)valList[3] / logsum);
            }
            else if (int.Parse(category) ==
                (int)Occupation.NotEmployed)
            {
                return ((double)valList[4] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeOccupationCommulative(Person person, SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForOccupation(person, curZ);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4];
            currPair.Category = "0";//IncomeLevel.GeneralOffice.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.ClericalManufacturing.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.Construction.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.TradesProfessional.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "4";//IncomeLevel.Management.ToString();
            currPair.Value = comVal + (double)valList[4] / utilSum;
            comList.Add(currPair);

            return comList;
        }

        private List<double> GetUtilityValuesForOccupation(Person person, SpatialZone curZ)
        {
            var currValues = new List<double>(5);

            currValues.Add(1);

            int twoAdultsChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.TwoAdultsChildren);
            int twoPlusAdultsChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.TwoAdultsChildren
                || person.GetHhld().GetHhldSize() == HouseholdSize.ThreeOrMoreAdultsChildren);
            int homeFT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.FullTimeHome);
            int homePT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.PartTimeHome);
            int PT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.PartTime);
            int apart = Convert.ToInt32(person.GetHhld().GetDwellingType() == DwellingType.Apartment);
            int eighteen = Convert.ToInt32(person.GetAge() == Age.EighteenToTwentyFive);
            int twentysix = Convert.ToInt32(person.GetAge() == Age.TwentySixToThirty);
            int fortyone = Convert.ToInt32(person.GetAge() == Age.FortyOneToFifty);
            int fiftyone = Convert.ToInt32(person.GetAge() == Age.FiftyOneToFiftyFour);
            int fiftyfive = Convert.ToInt32(person.GetAge() == Age.FiftyfiveToSixtyFour);
            int thirtyone = Convert.ToInt32(person.GetAge() == Age.ThirtyOneToForty);
            int threePlusCars = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.ThreeOrMore);

            double z_manConst = curZ.GetPersonOccuMarginal().GetValue("1");

            int oneAdultChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.OneAdultOneChild);
            int twoAdults = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.Twoadults);
            int twoPlusAdults = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.ThreeOrMoreAdults
                || person.GetHhld().GetHhldSize() == HouseholdSize.Twoadults);
            int oneCar = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.OneCar);
            int twoCars = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.TwoCars);

            currValues.Add(Math.Exp(-1.95 * 1.00 + -0.746 * oneAdultChildren + 0.280 * twoAdults + 0.594 * twoAdultsChildren
                + 0.556 * twoPlusAdults + 0.827 * twoPlusAdultsChildren + 0.247 * apart + 0.0751 * oneCar + 0.0867 * threePlusCars
                + 2.13 * homeFT + 0.237 * homePT + 0.841 * PT + -0.106 * eighteen + -0.123 * twentysix + -0.137 * thirtyone
                + 0.145 * fiftyone + 0.140 * fiftyfive + 16.1 * z_manConst));

            int town = Convert.ToInt32(person.GetHhld().GetDwellingType() == DwellingType.Townhouse);
            int fourteen = Convert.ToInt32(person.GetAge() == Age.FourteenToFifteen);
            int sixteen = Convert.ToInt32(person.GetAge() == Age.SixteenToSeventeen);
            int sixtyfivePlus = Convert.ToInt32(person.GetAge() == Age.MoreThanSixtyFive);

            double z_profMan = curZ.GetPersonOccuMarginal().GetValue("2");

            currValues.Add(Math.Exp(-0.648 * 1.00 + -0.0753 * twoAdults + 0.121 * twoAdultsChildren + -0.243 * twoPlusAdults
                + -0.110 * twoPlusAdultsChildren + -0.265 * apart + -0.147 * town + 0.271 * oneCar + 0.337 * twoCars + 0.303 * threePlusCars
                + 2.31 * homeFT + -1.02 * fourteen + -0.866 * sixteen + -0.389 * eighteen + 0.0738 * thirtyone + 0.229 * sixtyfivePlus
                + 7.22 * z_profMan));

            int eleven = Convert.ToInt32(person.GetAge() == Age.ElevenToThirteen);
            double z_retail = curZ.GetPersonOccuMarginal().GetValue("3");

            currValues.Add(Math.Exp(-1.05 * 1.00 + 0.308 * oneAdultChildren + 0.205 * twoAdults + 0.479 * twoAdultsChildren + 0.379 * twoPlusAdults
                + 0.781 * twoPlusAdultsChildren + 0.220 * apart + 0.108 * town + -0.152 * oneCar + -0.245 * twoCars + -0.27 * threePlusCars
                + 2.85 * homeFT + 1.48 * homePT + 2.30 * PT + 3.90 * eleven + 2.20 * fourteen + 2.22 * sixteen + 0.507 * eighteen
                + 0.160 * twentysix + 0.0563 * fiftyone + 0.129 * fiftyfive + 0.398 * sixtyfivePlus + 6.32 * z_retail));

            
            double z_oNE = curZ.GetPersonOccuMarginal().GetValue("4");

            currValues.Add(Math.Exp(0.743 * 1.00 + 2.37 * oneAdultChildren + 0.298 * twoAdults + 2.47 * twoAdultsChildren + 0.437 * twoPlusAdults
                + 2.12 * twoPlusAdultsChildren + 0.114 * apart + -0.0672 * town + -0.63 * oneCar + -1.08 * twoCars + -1.33 * threePlusCars
                + 8.71 * eleven + 5.34 * fourteen + 3.87 * sixteen + 0.969 * eighteen + -0.842 * twentysix + -1.70 * thirtyone
                + -0.528 * fiftyone + 0.517 * fiftyfive + 3.08 * sixtyfivePlus + 0.854 * z_oNE));

            return currValues;
        }

        private double ComputeEmploymentStatusProbablities(string category,
                                  Person person)
        {
            var valList = GetUtilityValuesForEmploymentStatus(person);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]
                                     + (double)valList[5]);
            if (int.Parse(category) ==
                (int)EmploymentStatus.Unemployed)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)EmploymentStatus.FullTime)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)EmploymentStatus.FullTimeHome)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)EmploymentStatus.PartTime)
            {
                return ((double)valList[3] / logsum);
            }
            else if (int.Parse(category) ==
                (int)EmploymentStatus.PartTimeHome)
            {
                return ((double)valList[4] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeEmploymentStatusCommulative(Person person)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForEmploymentStatus(person);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4];
            currPair.Category = "0";//IncomeLevel.Unemployed.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.FullTime.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.FullTimeHome.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.PartTime.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "4";//IncomeLevel.PartTimeHome.ToString();
            currPair.Value = comVal + (double)valList[4] / utilSum;
            comList.Add(currPair);

            return comList;
        }

        private List<double> GetUtilityValuesForEmploymentStatus(Person person)
        {
            var currValues = new List<double>(5);

            currValues.Add(1);

            int twoAdultsChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.TwoAdultsChildren);
            int twoPlusAdults = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.Twoadults
                || person.GetHhld().GetHhldSize() == HouseholdSize.ThreeOrMoreAdults);
            int threePlusCars = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.ThreeOrMore);
            int lNo = Convert.ToInt32(person.GetDrivingLicense() == DrivingLicense.No);
            int apart = Convert.ToInt32(person.GetHhld().GetDwellingType() == DwellingType.Apartment);
            int town = Convert.ToInt32(person.GetHhld().GetDwellingType() == DwellingType.Townhouse);
            int profMan = Convert.ToInt32(person.GetOccupation() == Occupation.ProfessionalManagementTechnical);
            int retail = Convert.ToInt32(person.GetOccupation() == Occupation.RetailSalesService);


            int eighteen = Convert.ToInt32(person.GetAge() == Age.EighteenToTwentyFive);
            int twentysix = Convert.ToInt32(person.GetAge() == Age.TwentySixToThirty);
            int thirtyone = Convert.ToInt32(person.GetAge() == Age.ThirtyOneToForty);
            int female = Convert.ToInt32(person.GetSex() == Sex.Female);
            int twoAdults = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.Twoadults);
            int oneCar = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.OneCar);
            int twoPlusAdultsChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.TwoAdultsChildren
                || person.GetHhld().GetHhldSize() == HouseholdSize.ThreeOrMoreAdultsChildren);

            currValues.Add(Math.Exp(-2.34 * 1.00 - 0.122 * twoAdults + -0.335 * twoAdultsChildren + -0.453 * twoPlusAdults + -0.248 * twoPlusAdultsChildren
                + -0.236 * apart + -0.323 * town + 0.104 * oneCar + 0.126 * threePlusCars + 0.522 * profMan + 0.802 * retail
                + -1.32 * eighteen + -0.711 * twentysix + -0.325 * thirtyone + -0.252 * female + 0.247 * lNo));


            int manConst = Convert.ToInt32(person.GetOccupation() == Occupation.ManufacturingConstructionTrades);
            int eleven = Convert.ToInt32(person.GetAge() == Age.ElevenToThirteen);
            int fiftyone = Convert.ToInt32(person.GetAge() == Age.FiftyOneToFiftyFour);
            int sixteen = Convert.ToInt32(person.GetAge() == Age.SixteenToSeventeen);

            currValues.Add(Math.Exp(-3.25 * 1.00 + -0.430 * twoAdultsChildren + -0.573 * twoPlusAdults + -0.456 * twoPlusAdultsChildren
                + -0.249 * town + 0.0881 * oneCar + -0.757 * manConst + 0.204 * profMan + 0.351 * retail + 8.78 * eleven + 3.50 * sixteen
                + -0.367 * eighteen + -1.06 * twentysix + -0.865 * thirtyone + -0.508 * fiftyone + 0.542 * female));

            int fourteen = Convert.ToInt32(person.GetAge() == Age.FourteenToFifteen);
                
            currValues.Add(Math.Exp(2.01 * 1.00 + -0.689 * twoPlusAdults + -0.0915 * twoPlusAdultsChildren + 0.0922 * apart
                + -0.240 * town + 0.398 * oneCar + -0.126 * threePlusCars + -17.7 * manConst + -17.9 * profMan + 14.6 * eleven
                + 5.15 * fourteen + 6.33 * sixteen + 0.646 * eighteen + -1.15 * twentysix + -1.45 * thirtyone + -1.31 * fiftyone
                + -0.280 * female + 2.15 * lNo + -19.6 * retail));

            currValues.Add(Math.Exp(-2.69 * 1.00 + 0.116 * twoAdults + -0.232 * twoAdultsChildren + 0.0514 * twoPlusAdults + 0.188 * apart
                + -0.103 * town + 0.0953 * oneCar + -0.164 * threePlusCars + -0.194 * manConst + 0.0903 * profMan + 0.914 * retail
                + 3.93 * fourteen + 5.99 * sixteen + 1.74 * eighteen + -0.317 * thirtyone + -0.392 * fiftyone + 0.853 * female
                + 0.462 * lNo + 7.15 * eleven));

            return currValues;
        }

        private double ComputeAgeProbablities(string category,
                                 Person person)
        {
            var valList = GetUtilityValuesForAge(person);
            //double logsum = ((double)valList[0]
            //                         + (double)valList[1]
            //                         + (double)valList[2]
            //                         + (double)valList[3]
			double logsum = ((double)valList[3]
                                     + (double)valList[4]
                                     + (double)valList[5]
                                     + (double)valList[6]
                                     + (double)valList[7]
                                     + (double)valList[8]
                                     + (double)valList[9]
                                     + (double)valList[10]);
            if (int.Parse(category) ==
                (int)Age.LessThanEleven)
            {
				return 0.00;
                //return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int)Age.ElevenToThirteen)
            {
				return 0.00;
                //return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int)Age.FourteenToFifteen)
            {
				return 0.00;
                //return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)Age.SixteenToSeventeen)
            {
                return ((double)valList[3] / logsum);
            }
            else if (int.Parse(category) ==
                (int)Age.EighteenToTwentyFive)
            {
                return ((double)valList[4] / logsum);
            }
            else if (int.Parse(category) ==
                 (int)Age.TwentySixToThirty)
            {
                return ((double)valList[5] / logsum);
            }
            else if (int.Parse(category) ==
                 (int)Age.ThirtyOneToForty)
            {
                return ((double)valList[6] / logsum);
            }
           else if (int.Parse(category) ==
                 (int)Age.FortyOneToFifty)
            {
                return ((double)valList[7] / logsum);
            }
            else if (int.Parse(category) ==
                 (int)Age.FiftyOneToFiftyFour)
            {
                return ((double)valList[8] / logsum);
            }
             else if (int.Parse(category) ==
                 (int)Age.FiftyfiveToSixtyFour)
            {
                return ((double)valList[9] / logsum);
            }
            else if (int.Parse(category) ==
                 (int)Age.MoreThanSixtyFive)
            {
                return ((double)valList[10] / logsum);
            }

            return 0.00;
        }

        private List<KeyValPair> ComputeAgeCommulative(Person person)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForAge(person);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]
                                     + (double)valList[5]
                                     + (double)valList[6]
                                     + (double)valList[7]
                                     + (double)valList[8]
                                     + (double)valList[9]
                                     + (double)valList[10];
            currPair.Category = "0";//IncomeLevel.LessThanEleven.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.ElevenToThirteen.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.FourteenToFifteen.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.SixteenToSeventeen.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "4";//IncomeLevel.EighteenToTwentyFive.ToString();
            currPair.Value = comVal + (double)valList[4] / utilSum;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "5";//IncomeLevel.TwentySixToThirty.ToString();
            currPair.Value = comVal + (double)valList[5] / utilSum;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "6";//IncomeLevel.ThirtyOneToForty.ToString();
            currPair.Value = comVal + (double)valList[6] / utilSum;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "7";//IncomeLevel.FortyOneToFifty.ToString();
            currPair.Value = comVal + (double)valList[7] / utilSum;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "8";//IncomeLevel.FiftyOneToFiftyFour.ToString();
            currPair.Value = comVal + (double)valList[8] / utilSum;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "9";//IncomeLevel.FiftyfiveToSixtyFour.ToString();
            currPair.Value = comVal + (double)valList[9] / utilSum;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "10";//IncomeLevel.MoreThanSixtyFive.ToString();
            currPair.Value = comVal + (double)valList[10] / utilSum;
            comList.Add(currPair);

            return comList;
        }

        private List<double> GetUtilityValuesForAge(Person person)
        {
            var currValues = new List<double>(11);

            currValues.Add(1);

            int twoAdultsChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.TwoAdultsChildren);
            int oneCar = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.OneCar);
            int twoCars = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.TwoCars);
            int female = Convert.ToInt32(person.GetSex() == Sex.Female);
            int threePlusCars = Convert.ToInt32(person.GetHhld().GetNumOfCars() == NumOfCars.ThreeOrMore);

            currValues.Add(Math.Exp(-0.591 * 1.00 + -4.20 * twoAdultsChildren + 0.235 * oneCar + 0.271 * twoCars + -0.268 * threePlusCars
                + 0.153 * female));

            int twoAdults = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.Twoadults);
            int manConst = Convert.ToInt32(person.GetOccupation() == Occupation.ManufacturingConstructionTrades);

            currValues.Add(Math.Exp(-0.916 * 1.00 + 1.27 * twoAdults + -4.87 * twoAdultsChildren + 0.227 * oneCar + 0.214 * twoCars
                + -0.138 * threePlusCars + 0.945 * manConst + 0.159 * female));

            int eNE = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.Unemployed);

            currValues.Add(Math.Exp(2.92 * 1.00 + 1.44 * twoAdults + -5.21 * twoAdultsChildren + 0.152 * oneCar
                + 0.0860 * twoCars + -3.99 * eNE + -1.63 * manConst + 0.271 * female));

            int oneAdultChildren = Convert.ToInt32(person.GetHhld().GetHhldSize() == HouseholdSize.OneAdultOneChild);

            currValues.Add(Math.Exp(5.51 * 1.00 + -2.57 * oneAdultChildren + 2.31 * twoAdults + -4.19 * twoAdultsChildren
                + -0.598 * oneCar + -0.934 * twoCars + -0.106 * threePlusCars + -5.60 * eNE + 0.508 * female));

            int homeFT = Convert.ToInt32(person.GetEmploymentStatus() == EmploymentStatus.FullTimeHome);

            currValues.Add(Math.Exp(4.90 * 1.00 + -0.750 * oneAdultChildren + 3.72 * twoAdults + -1.45 * twoAdultsChildren
                + -0.880 * oneCar + -1.28 * twoCars + -0.309 * threePlusCars + 1.33 * homeFT + -6.66 * eNE
                + 0.381 * manConst + 0.950 * female));

            currValues.Add(Math.Exp(5.58 * 1.00 + -0.350 * oneAdultChildren + 3.55 * twoAdults + -0.381 * twoAdultsChildren
                + -0.706 * oneCar + -1.21 * twoCars + -1.10 * threePlusCars + 1.79 * homeFT + -6.91 * eNE
                + 0.476 * manConst + 1.14 * female));

            currValues.Add(Math.Exp(6.40 * 1.00 + -1.48 * oneAdultChildren + 3.13 * twoAdults + -1.98 * twoAdultsChildren
                + -0.639 * oneCar + -1.09 * twoCars + -1.17 * threePlusCars + 1.97 * homeFT + -7.04 * eNE
                + 0.567 * manConst + 0.978 * female));

            currValues.Add(Math.Exp(5.42 * 1.00 + -2.80 * oneAdultChildren + 3.71 * twoAdults + -3.73 * twoAdultsChildren
                + -0.796 * oneCar + -1.20 * twoCars + -0.655 * threePlusCars + 2.05 * homeFT + -6.74 * eNE
                + 0.600 * manConst + 0.891 * female));

            currValues.Add(Math.Exp(5.70 * 1.00 + -4.1 * oneAdultChildren + 4.44 * twoAdults + -4.58 * twoAdultsChildren
                + -0.717 * oneCar + -1.09 * twoCars + -0.424 * threePlusCars + 2.17 * homeFT + -5.83 * eNE
                + 0.496 * manConst + 0.813 * female));

            currValues.Add(Math.Exp(4.85 * 1.00 + -5.75 * oneAdultChildren + 4.80 * twoAdults + -5.69 * twoAdultsChildren
                + -0.902 * oneCar + -1.75 * twoCars + -1.44 * threePlusCars + 2.23 * homeFT + -3.44 * eNE
                + 0.513 * female));

            return currValues;
        }
     }
}
