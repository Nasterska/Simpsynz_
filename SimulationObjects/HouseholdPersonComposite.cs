using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using PopulationSynthesis.Utils;


namespace SimulationObjects
{
    [Serializable]
    class HouseholdPersonComposite : SimulationObject
    {
        private static uint idCounter = 0;

        Household household;
        private List<Person> persons;

        //Sex
        private int count_male = 0;
        public int Count_male
        {
            get { return count_male; }
        }
        private int count_female = 0;
        public int Count_female
        {
            get { return count_female; }
        }

        //Age
        private int count_less11 = 0;
        public int Count_less11
        {
            get { return count_less11; }
        }

        private int count_11 = 0;
        public int Count_11
        {
            get { return count_11; }
        }

        private int count_14 = 0;
        public int Count_14
        {
            get { return count_14; }
        }

        private int count_16 = 0;
        public int Count_16
        {
            get { return count_16; }
        }

        private int count_18 = 0;
        public int Count_18
        {
            get { return count_18; }
        }

        private int count_26 = 0;
        public int Count_26
        {
            get { return count_26; }
        }

        private int count_31 = 0;
        public int Count_31
        {
            get { return count_31; }
        }

        private int count_41 = 0;
        public int Count_41
        {
            get { return count_41; }
        }

        private int count_51 = 0;
        public int Count_51
        {
            get { return count_51; }
        }

        private int count_55 = 0;
        public int Count_55
        {
            get { return count_55; }
        }

        private int count_65 = 0;
        public int Count_65
        {
            get { return count_65; }
        }

        //EmploymentStatus
        private int count_part_time = 0;
        public int Count_part_time
        {
            get { return count_part_time; }
        }

        private int count_part_time_home = 0;
        public int Count_part_time_home
        {
            get { return count_part_time_home; }
        }

        private int count_full_time = 0;
        public int Count_full_time
        {
            get { return count_full_time; }
        }

        private int count_full_time_home = 0;
        public int Count_full_time_home
        {
            get { return count_full_time_home; }
        }

        private int count_unemployed = 0;
        public int Count_unemployed
        {
            get { return count_unemployed; }
        }

        //DrivingLicense
        private int count_driving_license_yes = 0;
        public int Count_driving_license_yes
        {
            get { return count_driving_license_yes; }
        }

        private int count_driving_license_no = 0;
        public int Count_driving_license_no
        {
            get { return count_driving_license_no; }
        }

        //Occupation
        private int count_general_office = 0;
        public int Count_general_office
        {
            get { return count_general_office; }
        }

        private int count_clerical_manufacturing = 0;
        public int Count_clerical_manufacturing
        {
            get { return count_clerical_manufacturing; }
        }

        private int count_costruction_man = 0;
        public int Count_costruction_man
        {
            get { return count_costruction_man; }
        }

        private int count_professional_man = 0;
        public int Count_professional_man
        {
            get { return count_professional_man; }
        }

        private int count_retail = 0;
        public int Count_retail
        {
            get { return count_retail; }
        }

        private void updateCounts(Person person)
        {
            {
                switch (person.GetAge())
                {
                    case (Age.LessThanEleven):
                        {
                            count_less11++;
                            break;
                        }
                    case (Age.ElevenToThirteen):
                        {
                            count_11++;
                            break;
                        }
                    case (Age.FourteenToFifteen):
                        {
                            count_14++;
                            break;
                        }
                    case (Age.SixteenToSeventeen):
                        {
                            count_16++;
                            break;
                        }
                    case (Age.EighteenToTwentyFive):
                        {
                            count_18++;
                            break;
                        }
                    case (Age.TwentySixToThirty):
                        {
                            count_26++;
                            break;
                        }
                    case (Age.ThirtyOneToForty):
                        {
                            count_31++;
                            break;
                        }
                    case (Age.FortyOneToFifty):
                        {
                            count_41++;
                            break;
                        }
                    case (Age.FiftyOneToFiftyFour):
                        {
                            count_51++;
                            break;
                        }
                    case (Age.FiftyfiveToSixtyFour):
                        {
                            count_55++;
                            break;
                        }
                    case (Age.MoreThanSixtyFive):
                        {
                            count_65++;
                            break;
                        }
                    default:
                        break;
                }
                switch (person.GetEmploymentStatus())
                {
                    case (EmploymentStatus.PartTime):
                        count_part_time++;
                        break;
                    case (EmploymentStatus.FullTimeHome):
                        count_full_time_home++;
                        break;
                    case (EmploymentStatus.FullTime):
                        count_full_time++;
                        break;
                    case (EmploymentStatus.PartTimeHome):
                        count_part_time_home++;
                        break;
                    case (EmploymentStatus.Unemployed):
                        count_unemployed++;
                        break;
                    default:
                        break;
                }
                switch (person.GetSex())
                {
                    case (Sex.Male):
                        count_male++;
                        break;
                    case (Sex.Female):
                        count_female++;
                        break;
                    default:
                        break;
                }
                switch (person.GetDrivingLicense())
                {
                    case (DrivingLicense.Yes):
                        count_driving_license_yes++;
                        break;
                    case (DrivingLicense.No):
                        count_driving_license_no++;
                        break;
                    default:
                        break;

                }
                switch (person.GetOccupation())
                {
                    case (Occupation.GeneralOffice):
                        {
                            count_general_office++;
                            break;
                        }
                    case (Occupation.ClericalManufacturing):
                        {
                            count_clerical_manufacturing++;
                            break;
                        }
                    case (Occupation.TradesProfessional):
                        {
                            count_professional_man++;
                            break;
                        }
                    case (Occupation.TechnicalRetailSalesServiceNotEmployedUnknown):
                        {
                            count_retail++;
                            break;
                        }
                    case (Occupation.Construction):
                        {
                            count_costruction_man++;
                            break;
                        }
                }
            }
        }

        public HouseholdPersonComposite(Household hhld)
        {
            SetAgentType(AgentType.HouseholdPersonComposite);
            household = hhld;
            persons = new List<Person>();

            fillHouseholdWithPersons();

            foreach (Person person in persons)
            {
                person.SetHhld(household);
                updateCounts(person);
            }

        }

        void fillHouseholdWithPersons()
        {
            switch(household.GetHhldSize())
            {
                case(HouseholdSize.SingleAdult):
                case(HouseholdSize.OneAdultOneChild):
                    addPerson(new Person());
                    break;
                case(HouseholdSize.Twoadults):
                case(HouseholdSize.TwoAdultsChildren):
                    for (int i = 0; i < 2; i++)
                        addPerson(new Person());
                    break;
                case (HouseholdSize.ThreeOrMoreAdults):
                case (HouseholdSize.ThreeOrMoreAdultsChildren):
                    for (int i = 0; i < 3; i++)
                        addPerson(new Person());
                    break;
               default:
                    break;
            }
        }

		public void CheckConsistency()
		{
			CheckSexConsisteny ();
			CheckAgeConsistency ();
		}

		void CheckSexConsisteny()
		{
			Random myrand = new Random ();
			double r = 0.0;
			//Sex
			if (persons.Count () > 1) {
				if (persons [1].GetSex () == persons [2].GetSex ()) {
					r = myrand.NextDouble ();
					if (r < 0.5) {
						persons [1].SetSex(Sex.Male);
						persons [2].SetSex(Sex.Female);
					} else {
						persons [1].SetSex(Sex.Female);
						persons [2].SetSex(Sex.Male);
					}
				}
			}else if (persons.Count () == 1) {
				r = myrand.NextDouble ();
				if (r < 0.49) {
					persons [1].SetSex(Sex.Male);
				} else {
					persons [1].SetSex(Sex.Female);
				}
			}
		}

		void CheckAgeConsistency()
		{
			
		}

       /* public HouseholdPersonComposite(HouseholdPersonComposite HhPersonMediator)
        {
            SetAgentType(AgentType.HouseholdPersonMediator);
            household = HhPersonMediator.household.CreateNewCopy();
            persons = new List<Person>(HhPersonMediator.persons);
            foreach (Person person in persons)
                updateCounts(person);
        }*/

        /*Add only, it does not replace a value if it already exists.
         */
        public void addPerson(Person p)
        {
            persons.Add(p);
            p.SetHhld(household);
        }

        public  Household getHousehold()
        {
            return household;
        }

        public List<Person> getPersons()
        { 
            return persons;
        }

        public HouseholdPersonComposite CreateNewCopy()
        {
           /* MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, this);
            m.Position = 0;
            HouseholdPersonComposite myCopy = (HouseholdPersonComposite)b.Deserialize(m);
            return myCopy;*/

            HouseholdPersonComposite myCopy = (HouseholdPersonComposite)this.MemberwiseClone();
            myCopy.persons = persons.ConvertAll(person => (Person)person.CreateNewCopy());
            myCopy.household = household.CreateNewCopy();

            return myCopy;
        }
        public override SimulationObject CreateNewCopy(string baseDim,
			int baseDimVal, int agentIndex/*from -1 to person (count-1). if -1 then it is a hhld object else person*/)
        {
            
            HouseholdPersonComposite myCopy = (HouseholdPersonComposite)this.MemberwiseClone(); //TODO: changed copy method -MN
            myCopy.persons = persons.ConvertAll(person => (Person)person.CreateNewCopy());
            myCopy.household = household.CreateNewCopy();
			//change hhld
			if (agentIndex == -1) {
				if (baseDim == "DwellingType")
				{
					myCopy.household.SetDwellingType((DwellingType)baseDimVal);
				}
				else if (baseDim == "NumOfCars")
				{
					myCopy.household.SetNumOfCars((NumOfCars)baseDimVal);
				}
				else if (baseDim == "NumOfKids")
				{
					myCopy.household.SetNumOfKids((NumOfKids)baseDimVal);
				}
				else
				{
					return null;
				}
			}
			//change person at index agentid
			else {
				if (baseDim == "Sex")
				{

					myCopy.getPersons().ElementAt(agentIndex).SetSex((Sex)baseDimVal);
				}
				else if (baseDim == "Occupation")
				{

					myCopy.getPersons().ElementAt(agentIndex).SetOccupation((Occupation)baseDimVal);
				}
				else if (baseDim == "DrivingLicense")
				{

					myCopy.getPersons().ElementAt(agentIndex).SetDrivingLicense((DrivingLicense)baseDimVal);
				}
				else if (baseDim == "Age")
				{

					myCopy.getPersons().ElementAt(agentIndex).SetAge((Age)baseDimVal);
				}
				else if (baseDim == "EmploymentStatus")
				{

					myCopy.getPersons().ElementAt(agentIndex).SetEmploymentStatus((EmploymentStatus)baseDimVal);
				}
				else if (baseDim == "EducationLevel")
				{

					myCopy.getPersons().ElementAt(agentIndex).SetEducationLevel((EducationLevel)baseDimVal);
				}
				else
				{
					return null;
				}
			}

            return myCopy;
        }
    }
}




