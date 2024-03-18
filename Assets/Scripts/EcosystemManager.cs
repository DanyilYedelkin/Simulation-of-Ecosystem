using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animals;

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
    [SerializeField] private bool _geneticEvolutionOfPrey;
    [SerializeField] private int _preyFoodCount;
    [SerializeField] private int _preyCount;
    [SerializeField] private int _predatorCount;

    [Space]
    [Header("Results of Simulation")]
    [SerializeField] private int _totalPreyFood;
    [SerializeField] private int _totalPrey;
    [SerializeField] private int _totalPredators;
    [SerializeField] private int _totalPreyFoodEaten;
    [SerializeField] private int _totalPreyDeath;
    [SerializeField] private int _totalPredatorDeath;

    [Space]
    [Header("Another Settings")]
    [SerializeField] private int _simulationSpeed;
    [SerializeField] private int _foodSpawnDelay;

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
        _preyFoodEdibility = new();
        SpawnEntities();
        _totalPrey = _preyCount;
        _totalPredators = _predatorCount;
        _totalPreyFood = _preyFoodCount;
        _nextFoodSpawn = _foodSpawnDelay;
    }

    private void Update()
    {
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
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(Random.Range(-100f, 100f), 100f, Random.Range(-100f, 100f)), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }
        else
        {
            Debug.LogWarning("Failed to find ground surface for spawn point. Try to restart the spawn.");
            return GetSpawnPointOnGround();
            //return Vector3.zero;
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
    public GameObject PreyPrefab             => _preyPrefab;
    public GameObject PredatorPrefab         => _predatorPrefab;
    public GameObject PreyParentObject       => _preyParentObject;
    public GameObject PredatorparentObject   => _predatorParentObject;
    public GameObject Water                  => _water;
    public bool       GeneticEvolutionOfPrey => _geneticEvolutionOfPrey;
    public int        SimulationSpeed        => _simulationSpeed;
    #endregion
}
