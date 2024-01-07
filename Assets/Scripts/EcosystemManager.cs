using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemManager : MonoBehaviour
{
    #region Configuration
    [Header("Prefabs of animals and food")]
    [SerializeField] private GameObject _preyFoodPrefab;
    [SerializeField] private GameObject _preyPrefab;
    [SerializeField] private GameObject _predatorPrefab;

    [Space]
    [Header("List of animals and food")]
    [SerializeField] private List<GameObject> _preyFoods;
    [SerializeField] private List<GameObject> _prey;
    [SerializeField] private List<GameObject> _predators;

    [Space]
    [Header("Configuration of animals and food")]
    [SerializeField] private int _preyFoodCount;
    [SerializeField] private int _preyCount;
    [SerializeField] private int _predatorCount;

    [Space]
    [Header("Results of Simulation")]
    [SerializeField] private int _totalPrey;
    [SerializeField] private int _totalPredators;
    [SerializeField] private int _totalPreyDeath;
    [SerializeField] private int _totalPredatorDeath;

    [Space]
    [Header("Another Settings")]
    [SerializeField] private int _simulationSpeed;

    [Space]
    [Header("GameObjects that hold predator and prey instances")]
    [SerializeField] private GameObject _preyParentObject;
    [SerializeField] private GameObject _predatorParentObject;
    [SerializeField] private GameObject _ground;
    [SerializeField] private GameObject _water;
    #endregion


    #region Private Members 
    private Dictionary<GameObject, Animals.SearchRadius> _sensoryRadiusOfEntities;
    private Dictionary<GameObject, Food> _preyFoodEdibility;
    #endregion


    #region Unity Methods
    private void Start()
    {
        _preyFoodEdibility = new();
        _sensoryRadiusOfEntities = new();
        SpawnEntities();
        _totalPrey = _preyCount;
        _totalPredators = _predatorCount;
    }

    private void Update()
    {
        Time.timeScale = _simulationSpeed;
    }
    #endregion


    #region Local Methods
    private void SpawnEntities()
    {
        //Spawns predator and prey on opposite sides of the watering hole.
        for (int i = 0; i < _predatorCount; i++)
        {
            GameObject instantiatedObject = Instantiate(_predatorPrefab, 
                new Vector3(Random.Range(0f, 10f), _predatorPrefab.transform.position.y, Random.Range(0f, 10f)), Quaternion.identity, _predatorParentObject.transform);
            _predators.Add(instantiatedObject);
            instantiatedObject.SetActive(true);
        }

        for (int i = 0; i < _preyCount; i++)
        {
            GameObject instantiatedObject = Instantiate(_preyPrefab, 
                new Vector3(Random.Range(0f, 10f), _preyPrefab.transform.position.y, Random.Range(0f, 10f)), Quaternion.identity, _preyParentObject.transform);
            _sensoryRadiusOfEntities.Add(instantiatedObject, instantiatedObject.GetComponent<Animals.SensoryReference>().SensoryRadiusObj);
            _prey.Add(instantiatedObject);
            instantiatedObject.SetActive(true);
        }

        for (int i = 0; i < _preyFoodCount; i++)
        {
            GameObject instantiatedPlant;
            if (i % 2 == 0)
            {
                instantiatedPlant = Instantiate(_preyFoodPrefab, 
                    new Vector3(Random.Range(0f, 10f), _preyFoodPrefab.transform.position.y, Random.Range(0f, 10f)), Quaternion.identity, _ground.transform);
                _preyFoodEdibility.Add(instantiatedPlant, instantiatedPlant.GetComponent<Food>());
            }
            else
            {
                instantiatedPlant = Instantiate(_preyFoodPrefab, 
                    new Vector3(Random.Range(0f, 10f), _preyFoodPrefab.transform.position.y, Random.Range(0f, 10f)), Quaternion.identity, _ground.transform);
                _preyFoodEdibility.Add(instantiatedPlant, instantiatedPlant.GetComponent<Food>());
            }
            instantiatedPlant.SetActive(true);
        }
    }
    #endregion


    #region Properties
    public Dictionary<GameObject, Animals.SearchRadius> SensoryRadiusOfEntities
    {
        get => _sensoryRadiusOfEntities;
        set => _sensoryRadiusOfEntities = value;
    }
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
    public GameObject PreyPrefab           => _preyPrefab;
    public GameObject PredatorPrefab       => _predatorPrefab;
    public GameObject PreyParentObject     => _preyParentObject;
    public GameObject PredatorparentObject => _predatorParentObject;
    public GameObject Water                => _water;
    #endregion
}
