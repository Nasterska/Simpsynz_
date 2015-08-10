/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopulationSynthesis.Utils;

namespace SimulationObjects
{
        [Serializable]
    sealed class Person : SimulationObject
    {
        private static uint idCounter = 0;
        private Household household;
        public Household GetHhld()
        {
            return household;
        }
        public void SetHhld(Household Household)
        {
            household = Household;
        }
        private Age Age;
        public Age GetAge()
        {
            return Age;
        }
        public void SetAge(Age curAge)
        {
            Age = curAge;
        }
        private Sex Sex;
        public Sex GetSex()
        {
            return Sex;
        }
        public void SetSex(Sex curSex)
        {
            Sex = curSex;
        }
        private DrivingLicense DrivingLicense;
        public DrivingLicense GetDrivingLicense()
        {
            return DrivingLicense;
        }
        public void SetDrivingLicense(DrivingLicense curDL)
        {
            DrivingLicense = curDL;
        }
        private EducationLevel EducationLevel;
        public EducationLevel GetEducationLevel()
        {
            return EducationLevel;
        }
        public void SetEducationLevel(EducationLevel eduLvl)
        {
            EducationLevel = eduLvl;
        }
        private EmploymentStatus EmploymentStatus;
        public EmploymentStatus GetEmploymentStatus()
        {
            return EmploymentStatus;
        }
        public void SetEmploymentStatus(EmploymentStatus eStatus)
        {
            EmploymentStatus = eStatus;
        }
        private Occupation Occupation;
        public Occupation GetOccupation()
        {
            return Occupation;
        }
        public void SetOccupation(Occupation eOccu)
        {
            Occupation = eOccu;
        }
        private PublicTransitPass PublicTransitPass;  // done
        public PublicTransitPass GetPublicTransitPass()
        {
            return PublicTransitPass;
        }
        public void SetPublicTransitPass(PublicTransitPass PublicTrans)
        {
            PublicTransitPass = PublicTrans;
        }
   

        private string ZoneID;
        public string GetZoneID()
        {
            return ZoneID;
        }
        public void SetZoneID(string id)
        {
            ZoneID = id;
        }

        public Person()
        {
            Sex = Sex.Female;
            Age = Age.ThirtyOneToForty;
            EducationLevel = EducationLevel.primary;
            Type = AgentType.Person;
        }

        public Person(string currZone)
        {
            ZoneID = currZone;
            Age = Age.EighteenToTwentyFive;
            Sex = Sex.Male;
            EducationLevel = EducationLevel.primary;
            household = new Household();
            Occupation = Occupation.TradesProfessional;
            PublicTransitPass = PublicTransitPass.MetroPass;
            EmploymentStatus = EmploymentStatus.PartTime;
            DrivingLicense = DrivingLicense.No;
            Type = AgentType.Person;
        }

        private Person(Person original)
        {
            Type = AgentType.Person;
            //copy the values
            ZoneID = original.ZoneID;
            Age = original.Age;
            Sex = original.Sex;
            EducationLevel = original.EducationLevel;
            Occupation = original.Occupation;
            PublicTransitPass = original.PublicTransitPass;
            EmploymentStatus = original.EmploymentStatus;
            DrivingLicense = original.DrivingLicense;
            household = original.household;
        }

        [ThreadStatic]
        private static StringBuilder KeyBuilder;

        public override string GetNewJointKey(string baseDim)
        {
            var builder = KeyBuilder;
            if(builder == null)
            {
                KeyBuilder = builder = new StringBuilder();
            }
            builder.Clear();
            if(baseDim == "Age")
            {
                builder.Append((int)Sex);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EducationLevel);

                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)DrivingLicense);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EmploymentStatus);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Occupation);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)PublicTransitPass);
            }
            else if(baseDim == "Sex")
            {
                builder.Append((int)Age).ToString();
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EducationLevel);

                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)DrivingLicense);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EmploymentStatus);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Occupation);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)PublicTransitPass);
            }
            else if(baseDim == "EducationLevel")
            {
                builder.Append((int)Age);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Sex);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)DrivingLicense);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EmploymentStatus);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Occupation);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)PublicTransitPass);
            }
            else if (baseDim == "DrivingLicense")
            {
                builder.Append((int)Age);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Sex);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EducationLevel);

                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EmploymentStatus);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Occupation);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)PublicTransitPass);
            }
            else if (baseDim == "EmploymentStatus")
            {
                builder.Append((int)Age);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Sex);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EducationLevel);

                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)DrivingLicense);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Occupation);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)PublicTransitPass);
            }
            else if (baseDim == "Occupation")
            {
                builder.Append((int)Age);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Sex);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EducationLevel);

                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)DrivingLicense);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EmploymentStatus);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)PublicTransitPass);
                
            }
            else if (baseDim=="PublicTransitPass")
            {
                                builder.Append((int)Age);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Sex);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EducationLevel);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)Occupation);

                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)DrivingLicense);
                builder.Append(Constants.CONDITIONAL_DELIMITER);
                builder.Append((int)EmploymentStatus);
            }
            return builder.ToString();
        }

        public override SimulationObject CreateNewCopy(string baseDim,
            int baseDimVal, int personId)
        {
            Person copy = new Person(this);
            // apply the new change
            switch(baseDim)
            {
                case "Age":
                    copy.Age = (Age)baseDimVal;
                    break;
                case "Sex":
                    copy.Sex = (Sex)baseDimVal;
                    break;
                case "Occupation":
                    copy.Occupation = (Occupation)baseDimVal;
                    break;
                case "PublicTransitPass":
                    copy.PublicTransitPass = (PublicTransitPass)baseDimVal;
                    break;
                    
                case "EducationLevel":
                    copy.EducationLevel = (EducationLevel)baseDimVal;
                    break;
                 case "EmploymentStatus":
                    copy.EmploymentStatus = (EmploymentStatus)baseDimVal;
                    break;
                 case "DrivingLicense":
                    copy.DrivingLicense = (DrivingLicense)baseDimVal;
                    break;
                default:
                    return null;
            }
            return copy;
        }
        public  SimulationObject CreateNewCopy()
        {
            Person copy = new Person(this);
            return copy;
        }
    }
}
 