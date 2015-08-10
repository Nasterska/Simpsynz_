/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

namespace SimulationObjects
{
    public enum AgentType
    {
        Household = 0,
        Person = 1,
        HouseholdPersonComposite = 2
    }

    /// <summary>
    /// Household attributes
    /// </summary>

    public enum HouseholdSize
    {
        SingleAdult = 0,
        OneAdultOneChild = 1,
        Twoadults = 2,
        TwoAdultsChildren = 3,
        ThreeOrMoreAdults = 4,
        ThreeOrMoreAdultsChildren = 5
    }

    public enum DwellingType
    {
        House = 0,
        Apartment = 1,
        Townhouse = 2
    };

    public enum NumOfCars
    {
        NoCar = 0,
        OneCar = 1,
        TwoCars = 2,
        ThreeOrMore = 3
    };

    public enum NumOfWorkers
    {
        None = 0,
        One = 1,
        TwoOrMore = 2
    };

    public enum NumOfKids
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        FourOrMore = 4
    };

    public enum NumOfPeople
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        fiveOrMore = 5
    };

    public enum NumWithUnivDeg
    {
        None = 0,
        One = 1,
        TwoOrMore = 2
    }

    public enum IncomeLevel
    {
        ThirtyOrLess = 0,
        ThirtyToSevetyFive = 1,
        SeventyFiveToOneTwentyFive = 2,
        OneTwentyFiveToTwoHundred = 3,
        TwohundredOrMore = 4
    }

    /// <summary>
    /// Person attributes
    /// </summary>

    public enum HouseholdType
    {
        SinglePersonHhld = 0,
        FamilyHhld = 1,
        NonFamilyHhld = 2,
        Collectivehhld = 3
    };

    public enum Sex
    {
        Male = 0,
        Female = 1
    };

    public enum MaritalStatus
    {
        Single = 0,
        Married = 1,
        Widowed = 2,
        Divorced = 3
    };

    public enum EducationLevel
    {
        none = 0,
        primary = 1,
        secondary = 2,
        tertiary = 3
    };

    public enum EmploymentStatus
    {
        Unemployed = 0,
        FullTime = 1,
        FullTimeHome = 2,
        PartTime = 3,
        PartTimeHome = 4,
    };


    public enum Age
    {
        LessThanEleven = 0,
        ElevenToThirteen = 1,
        FourteenToFifteen = 2,
        SixteenToSeventeen = 3,
        EighteenToTwentyFive = 4,
        TwentySixToThirty = 5,
        ThirtyOneToForty = 6,
        FortyOneToFifty = 7,
        FiftyOneToFiftyFour = 8,
        FiftyfiveToSixtyFour = 9,
        MoreThanSixtyFive = 10
    };

    public enum DrivingLicense
    {
        Yes = 0,
        No = 1,
        Unkown = 2
    };

    public enum Occupation
    {
        GeneralOffice = 0,
        ClericalManufacturing = 1,
        Construction = 2,
        TradesProfessional = 3,
        Management = 4,
        TechnicalRetailSalesServiceNotEmployedUnknown = 5
    }

    public enum PublicTransitPass 
    {
        None = 0,
        MetroPass = 1,
        GOTransitPass = 2,
        ComboOrDualPass = 3,
        OtherAgencyPass = 4,
        Unknown = 5
    }

}