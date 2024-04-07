namespace MainMenu
{
    using UnityEngine;

    public class AnimalConfiguration
    {
        #region Properties
        public bool   GeneticEvolution { get; set; }
        public int    Count            { get; set; }
        public int    ViewDistance     { get; set; }
        public float  IdleSpeed        { get; set; }
        public float  FleeingSpeed     { get; set; }
        public int    InheritanceCount { get; set; }
        public int    FirstValue       { get; set; }
        public int    SecondValue      { get; set; }
        public float  CurrentAge       { get; set; }
        public string CurrentAgeState  { get; set; }
        public float  MaxAge           { get; set; }
        public float  InfantAge        { get; set; }
        public float  AdolescentAge    { get; set; }
        public float  YoungAge         { get; set; }
        public float  AdultAge         { get; set; }
        public float  OldAge           { get; set; }
        #endregion
    }

    public class GeneralConfiguration
    {
        #region Properties
        public int PreyFoodCount      { get; set; }
        public int PreyFoodSpawnDelay { get; set; }
        public int SimulationSpeed    { get; set; }
        public int SecondsPerYear     { get; set; }
        #endregion
    }

    public class SettingsController : MonoBehaviour
    {
        #region API

        #region Prey Configuration
        public void SetPreyGeneticEvoultion(bool enable)
        {
            PreyConfiguration.GeneticEvolution = enable;
        }

        public void SetPreyCount(string strNumber)
        {
            PreyConfiguration.Count = int.Parse(strNumber);
        }

        public void SetPreyViewDistance(string strNumber)
        {
            PreyConfiguration.ViewDistance = int.Parse(strNumber);
        }

        public void SetPreyIdleSpeed(string strNumber)
        {
            PreyConfiguration.IdleSpeed = float.Parse(strNumber);
        }

        public void SetPreyFleeingSpeed(string strNumber)
        {
            PreyConfiguration.FleeingSpeed = float.Parse(strNumber);
        }

        public void SetPreyInheritanceCount(string strNumber)
        {
            PreyConfiguration.InheritanceCount = int.Parse(strNumber);
        }

        public void SetPreyFirstValue(string strNumber)
        {
            PreyConfiguration.FirstValue = int.Parse(strNumber);
        }

        public void SetPreySecondValue(string strNumber)
        {
            PreyConfiguration.SecondValue = int.Parse(strNumber);
        }

        public void SetPreyCurrentAge(string strNumber)
        {
            PreyConfiguration.CurrentAge = float.Parse(strNumber);
        }

        public void SetPreyCurrentAgeState(string str)
        {
            PreyConfiguration.CurrentAgeState = str;
        }

        public void SetPreyMaxAge(string strNumber)
        {
            PreyConfiguration.MaxAge = float.Parse(strNumber);
        }

        public void SetPreyInfantAge(string strNumber)
        {
            PreyConfiguration.InfantAge = float.Parse(strNumber);
        }

        public void SetPreyAdolescentAge(string strNumber)
        {
            PreyConfiguration.AdolescentAge = float.Parse(strNumber);
        }

        public void SetPreyYoungAge(string strNumber)
        {
            PreyConfiguration.YoungAge = float.Parse(strNumber);
        }

        public void SetPreyAdultAge(string strNumber)
        {
            PreyConfiguration.AdultAge = float.Parse(strNumber);
        }

        public void SetPreyOldAge(string strNumber)
        {
            PreyConfiguration.OldAge = float.Parse(strNumber);
        }
        #endregion

        #region Predator Configuration
        public void SetPredatorGeneticEvoultion(bool enable)
        {
            PredatorConfiguration.GeneticEvolution = enable;
        }

        public void SetPredatorCount(string strNumber)
        {
            PredatorConfiguration.Count = int.Parse(strNumber);
        }

        public void SetPredatorViewDistance(string strNumber)
        {
            PredatorConfiguration.ViewDistance = int.Parse(strNumber);
        }

        public void SetPredatorIdleSpeed(string strNumber)
        {
            PredatorConfiguration.IdleSpeed = float.Parse(strNumber);
        }

        public void SetPredatorFleeingSpeed(string strNumber)
        {
            PredatorConfiguration.FleeingSpeed = float.Parse(strNumber);
        }

        public void SetPredatorInheritanceCount(string strNumber)
        {
            PredatorConfiguration.InheritanceCount = int.Parse(strNumber);
        }

        public void SetPredatorFirstValue(string strNumber)
        {
            PredatorConfiguration.FirstValue = int.Parse(strNumber);
        }

        public void SetPredatorSecondValue(string strNumber)
        {
            PredatorConfiguration.SecondValue = int.Parse(strNumber);
        }

        public void SetPredatorCurrentAge(string strNumber)
        {
            PredatorConfiguration.CurrentAge = float.Parse(strNumber);
        }

        public void SetPredatorCurrentAgeState(string str)
        {
            PredatorConfiguration.CurrentAgeState = str;
        }

        public void SetPredatorMaxAge(string strNumber)
        {
            PredatorConfiguration.MaxAge = float.Parse(strNumber);
        }

        public void SetPredatorInfantAge(string strNumber)
        {
            PredatorConfiguration.InfantAge = float.Parse(strNumber);
        }

        public void SetPredatorAdolescentAge(string strNumber)
        {
            PredatorConfiguration.AdolescentAge = float.Parse(strNumber);
        }

        public void SetPredatorYoungAge(string strNumber)
        {
            PredatorConfiguration.YoungAge = float.Parse(strNumber);
        }

        public void SetPredatorAdultAge(string strNumber)
        {
            PredatorConfiguration.AdultAge = float.Parse(strNumber);
        }

        public void SetPredatorOldAge(string strNumber)
        {
            PredatorConfiguration.OldAge = float.Parse(strNumber);
        }
        #endregion

        #region General Configuration
        public void SetPreyFoodCount(string strNumber)
        {
            GeneralConfiguration.PreyFoodCount = int.Parse(strNumber);
        }

        public void SetPreyFoodSpawnDelay(string strNumber)
        {
            GeneralConfiguration.PreyFoodSpawnDelay = int.Parse(strNumber);
        }

        public void SetSimulationSpeed(string strNumber)
        {
            GeneralConfiguration.SimulationSpeed = int.Parse(strNumber);
        }

        public void SetSecondsPerYear(string strNumber)
        {
            GeneralConfiguration.SecondsPerYear = int.Parse(strNumber);
        }
        #endregion

        #endregion

        #region Properties
        public AnimalConfiguration  PreyConfiguration     { get; set; } = new();
        public AnimalConfiguration  PredatorConfiguration { get; set; } = new();
        public GeneralConfiguration GeneralConfiguration  { get; set; } = new();
        #endregion
    }
}