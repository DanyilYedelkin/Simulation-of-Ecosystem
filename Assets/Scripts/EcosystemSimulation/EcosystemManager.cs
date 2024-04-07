using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animals;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class EcosystemManager : MonoBehaviour
{
    #region Configuration
    [Header("Prefabs of animals and food")]
    [SerializeField] private GameObject[] _preyFoodPrefab;
    [SerializeField] private GameObject   _preyPrefab;
    [SerializeField] private GameObject   _predatorPrefab;

    [Space]
    [Header("List of animals and food")]
    [SerializeField] private List<GameObject> _preyFoods;
    [SerializeField] private List<GameObject> _prey;
    [SerializeField] private List<GameObject> _predators;

    [Space]
    [Header("Configuration of animals and food")]
    [SerializeField] private bool  _geneticEvolutionOfPrey;
    [SerializeField] private bool  _geneticEvolutionOfPredators;
    [SerializeField] private float _secondsPerYear;
    [SerializeField] private int   _preyFoodCount;
    [SerializeField] private int   _preyCount;
    [SerializeField] private int   _predatorCount;

    [Space]
    [Header("Results of Simulation")]
    [SerializeField] private float _simulationRuntime = 0f;
    [SerializeField] private int   _totalPreyFood;
    [SerializeField] private int   _totalPrey;
    [SerializeField] private int   _totalPredators;
    [SerializeField] private int   _totalPreyFoodEaten;
    [SerializeField] private int   _totalPreyDeath;
    [SerializeField] private int   _totalPredatorDeath;

    [Space]
    [Header("Another Settings")]
    [SerializeField] private int            _simulationSpeed;
    [SerializeField] private int            _foodSpawnDelay;
    [SerializeField] private NavMeshSurface _navMeshSurface;

    [Space]
    [Header("GameObjects that hold predator and prey instances")]
    [SerializeField] private GameObject _preyParentObject;
    [SerializeField] private GameObject _predatorParentObject;
    [SerializeField] private GameObject _ground;
    [SerializeField] private GameObject _water;
    #endregion


    #region Private Members 
    private Dictionary<GameObject, Food> _preyFoodEdibility;
    private float _nextFoodSpawn = 0f;
    #endregion


    #region Unity Methods
    private void Start()
    {
        if (StateSettingsController.MainMenuStart)
        {
            PreyConfiguration();
            PredatorConfiguration();
            GeneralConfiguration();
        }

        _preyFoodEdibility = new();
        SpawnEntities();
        _totalPrey = _preyCount;
        _totalPredators = _predatorCount;
        _totalPreyFood = _preyFoodCount;
        _nextFoodSpawn = _foodSpawnDelay;
    }

    private void Update()
    {
        _simulationRuntime += Time.fixedDeltaTime * _simulationSpeed / _secondsPerYear;

        Time.timeScale = _simulationSpeed;

        if (Time.time > _nextFoodSpawn)
        {
            _nextFoodSpawn = (Time.time + _foodSpawnDelay);
            SpawnFoodBatch(1, _preyFoodPrefab, _ground.transform, _preyFoodEdibility);
            _totalPreyFood++;
        }
    }
    #endregion


    #region Local Methods
    private AgeState FindAgeState(string strAgeState)
    {
        switch (strAgeState.ToLower())
        {
            case "newborn":
                return AgeState.Newborn;
            case "infant":
                return AgeState.Infant;
            case "adolescent":
                return AgeState.Adolescent;
            case "young":
                return AgeState.Young;
            case "adult":
                return AgeState.Adult;
            case "old":
                return AgeState.Old;
            default:
                // error
                return AgeState.Adolescent;
        }
    }

    private void PreyConfiguration()
    {
        _geneticEvolutionOfPrey = StateSettingsController.PreyConfiguration.GeneticEvolution;
        _preyCount = StateSettingsController.PreyConfiguration.Count;

        if (_preyCount > 0)
        {
            AnimalBehaviourController preyController = _preyPrefab.GetComponent<AnimalBehaviourController>();
            if (preyController)
            {
                preyController.IdleSpeed = StateSettingsController.PreyConfiguration.IdleSpeed;
                preyController.FleeingSpeed = StateSettingsController.PreyConfiguration.FleeingSpeed;

                if (preyController.Gene == null)
                {
                    preyController.Gene = new();
                }
                preyController.Gene.GeneType = Genotype.Ab;
                preyController.Gene.FirstGeneValue = StateSettingsController.PreyConfiguration.FirstValue;
                preyController.Gene.SecondGeneValue = StateSettingsController.PreyConfiguration.SecondValue;
                preyController.SearchRadius.AnimalSearchRadius = StateSettingsController.PreyConfiguration.ViewDistance;
            }

            ReproductionController reproduction = _preyPrefab.GetComponent<ReproductionController>();
            if (reproduction)
            {
                reproduction.InheritanceCount = StateSettingsController.PreyConfiguration.InheritanceCount;
            }

            AgeController age = _preyPrefab.GetComponent<AgeController>();
            if (age)
            {
                age.CurrentAge = StateSettingsController.PreyConfiguration.CurrentAge;
                age.CurrentAgeState = FindAgeState(StateSettingsController.PreyConfiguration.CurrentAgeState);
                age.MaxAge = StateSettingsController.PreyConfiguration.MaxAge;
                age.InfantAge = StateSettingsController.PreyConfiguration.InfantAge;
                age.AdolescentAge = StateSettingsController.PreyConfiguration.AdolescentAge;
                age.YoungAge = StateSettingsController.PreyConfiguration.YoungAge;
                age.AdultAge = StateSettingsController.PreyConfiguration.AdultAge;
                age.OldAge = StateSettingsController.PreyConfiguration.OldAge;
            }
        }
    }

    private void PredatorConfiguration()
    {
        _geneticEvolutionOfPredators = StateSettingsController.PredatorConfiguration.GeneticEvolution;
        _predatorCount = StateSettingsController.PredatorConfiguration.Count;

        if (_predatorCount > 0)
        {
            AnimalBehaviourController predatorController = _predatorPrefab.GetComponent<AnimalBehaviourController>();
            if (predatorController)
            {
                predatorController.IdleSpeed = StateSettingsController.PredatorConfiguration.IdleSpeed;
                predatorController.FleeingSpeed = StateSettingsController.PredatorConfiguration.FleeingSpeed;

                if (predatorController.Gene == null)
                {
                    predatorController.Gene = new();
                }
                predatorController.Gene.GeneType = Genotype.Ab;
                predatorController.Gene.FirstGeneValue = StateSettingsController.PredatorConfiguration.FirstValue;
                predatorController.Gene.SecondGeneValue = StateSettingsController.PredatorConfiguration.SecondValue;
                predatorController.SearchRadius.AnimalSearchRadius = StateSettingsController.PredatorConfiguration.ViewDistance;
            }

            ReproductionController reproduction = _predatorPrefab.GetComponent<ReproductionController>();
            if (reproduction)
            {
                reproduction.InheritanceCount = StateSettingsController.PredatorConfiguration.InheritanceCount;
            }

            AgeController age = _predatorPrefab.GetComponent<AgeController>();
            if (age)
            {
                age.CurrentAge = StateSettingsController.PredatorConfiguration.CurrentAge;
                age.CurrentAgeState = FindAgeState(StateSettingsController.PredatorConfiguration.CurrentAgeState);
                age.MaxAge = StateSettingsController.PredatorConfiguration.MaxAge;
                age.InfantAge = StateSettingsController.PredatorConfiguration.InfantAge;
                age.AdolescentAge = StateSettingsController.PredatorConfiguration.AdolescentAge;
                age.YoungAge = StateSettingsController.PredatorConfiguration.YoungAge;
                age.AdultAge = StateSettingsController.PredatorConfiguration.AdultAge;
                age.OldAge = StateSettingsController.PredatorConfiguration.OldAge;
            }
        }
    }

    private void GeneralConfiguration()
    {
        _preyFoodCount = StateSettingsController.GeneralConfiguration.PreyFoodCount;
        _foodSpawnDelay = StateSettingsController.GeneralConfiguration.PreyFoodSpawnDelay;
        _simulationSpeed = StateSettingsController.GeneralConfiguration.SimulationSpeed;
        _secondsPerYear = StateSettingsController.GeneralConfiguration.SecondsPerYear;
    }

    private void SpawnEntities()
    {
        //Spawns predator, prey and food on opposite sides of the watering hole.
        SpawnEntityBatch(_predatorCount, _predatorPrefab, _predatorParentObject.transform, _predators);
        SpawnEntityBatch(_preyCount, _preyPrefab, _preyParentObject.transform, _prey);
        SpawnFoodBatch(_preyFoodCount, _preyFoodPrefab, _ground.transform, _preyFoodEdibility);
    }

    private void SpawnEntityBatch(int count, GameObject prefab, Transform parent, List<GameObject> list)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPoint = GetSpawnPointOnGround();
            GameObject instantiatedObject = Instantiate(prefab, spawnPoint, Quaternion.identity, parent);
            list.Add(instantiatedObject);
            instantiatedObject.SetActive(true);
        }
    }

    private void SpawnFoodBatch(int count, GameObject[] prefab, Transform parent, Dictionary<GameObject, Food> dict)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPoint = GetSpawnPointOnGround();
            GameObject instantiatedPlant = Instantiate(prefab[Random.Range(0, prefab.Length)], spawnPoint, Quaternion.identity, parent);
            dict.Add(instantiatedPlant, instantiatedPlant.GetComponent<Food>());
            instantiatedPlant.SetActive(true);
        }
    }

    private Vector3 GetSpawnPointOnGround()
    {
        Vector3 randomPoint = new Vector3(Random.Range(-100f, 100f), 10f, Random.Range(-100f, 100f));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, Mathf.Infinity, NavMesh.AllAreas)) 
        {
            return hit.position;
        }
        else
        {
            Debug.LogWarning("Failed to find ground surface for spawn point. Try to restart the spawn.");
            return GetSpawnPointOnGround(); 
        }
    }
    #endregion


    #region Properties
    public Dictionary<GameObject, Food> PreyFoodEdibility
    {
        get => _preyFoodEdibility;
        set => _preyFoodEdibility = value;
    }
    public List<GameObject> PreyFoods
    {
        get => _preyFoods;
        set => _preyFoods = value;
    }
    public List<GameObject> Prey
    {
        get => _prey;
        set => _prey = value;
    }
    public List<GameObject> Predators
    {
        get => _predators;
        set => _predators = value;
    }
    public int TotalPreyDeath
    {
        get => _totalPreyDeath;
        set => _totalPreyDeath = value;
    }
    public int TotalPredatorDeath
    {
        get => _totalPredatorDeath;
        set => _totalPredatorDeath = value;
    }
    public int TotalPrey
    {
        get => _totalPrey;
        set => _totalPrey = value;
    }
    public int TotalPredators
    {
        get => _totalPredators;
        set => _totalPredators = value;
    }
    public int TotalPreyFood
    {
        get => _totalPreyFood;
        set => _totalPreyFood = value;
    }
    public int TotalPreyFoodEaten
    {
        get => _totalPreyFoodEaten;
        set => _totalPreyFoodEaten = value;
    }
    public GameObject PreyPrefab                  => _preyPrefab;
    public GameObject PredatorPrefab              => _predatorPrefab;
    public GameObject PreyParentObject            => _preyParentObject;
    public GameObject PredatorparentObject        => _predatorParentObject;
    public GameObject Water                       => _water;
    public bool       GeneticEvolutionOfPrey      => _geneticEvolutionOfPrey;
    public bool       GeneticEvolutionOfPredators => _geneticEvolutionOfPredators;
    public int        SimulationSpeed             => _simulationSpeed;
    public float      SecondsPerAge               => _secondsPerYear;
    #endregion
}
