using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Animals
{
    public class AnimalBehaviourController : MonoBehaviour
    {
        #region Configuration
        [Header("Animal State")]
        [SerializeField] private AnimalState _currentState = AnimalState.Idle;

        [Space]
        [Header("General Animal Settings")]
        [SerializeField] private float _idleSpeed        = 0f;
        [SerializeField] private float _fleeingSpeed     = 0f;
        [SerializeField] private float _hunger           = 0f;
        [SerializeField] private float _thirst           = 0f;
        [SerializeField] private float _reproductiveNeed = 0f;
        [SerializeField] private float _healthPoints     = 100;
        [SerializeField] private float _age              = 0;
        [Tooltip("true - male, false - female")]
        [SerializeField] private bool  _sex              = false;
        [SerializeField] private Gene  _gene;

        [Space]
        [Header("Gene configuration")]
        [SerializeField] private Genotype _genotype;
        [SerializeField] private int      _firstValue;
        [SerializeField] private int      _secondValue;

        [Space]
        [Header("Ecosystem Manager")]
        [SerializeField] private EcosystemManager _ecosystemManager;
        #endregion


        #region Private Members
        private NavMeshAgent _agent;
        private GameObject   _mateAnimal;
        #endregion


        #region Unity Methods
        public void OnValidate()
        {
            // handling user inputs to only valid values & hiding of optional values
            HandleInputs();
        }

        protected virtual void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _idleSpeed;
            _ecosystemManager = GameObject.FindGameObjectWithTag("EcosystemManager").GetComponent<EcosystemManager>();
            _sex = UnityEngine.Random.Range(0, 2) == 0 ? true : false;

            GenerateGene();
        }
        #endregion


        #region API
        public void DeathIsInevitable()
        {
            /*if (_healthPoints > 0)
            {
                _healthPoints -= Time.fixedDeltaTime;
            }
            else
            {
                RemoveAnimalFromManager();
                Destroy(this.gameObject);
            }*/
        }

        public virtual void RemoveAnimalFromManager()
        {
            throw new NotImplementedException();
        }

        public virtual void AddAnimalToManager(GameObject animal)
        {
            throw new NotImplementedException();
        }

        public void ThirstLogic()
        {
            if (CurrentState == AnimalState.Drinking)
            {
                return;
            }

            if (Agent.velocity.magnitude > 0.2f)
            {
                Thirst += (0.05f * IdleSpeed) * Time.fixedDeltaTime;
            }
            else
            {
                Thirst += (0.05f * Time.fixedDeltaTime);
            }

            if (Thirst >= 100)
            {
                RemoveAnimalFromManager();
                Destroy(gameObject);
            }
        }

        public void ReproductionNeed()
        {
            if (Agent.velocity.magnitude > 0.2f)
            {
                ReproductiveNeed += (0.05f * IdleSpeed) * Time.fixedDeltaTime;
            }
            else
            {
                ReproductiveNeed += (0.05f * Time.fixedDeltaTime);
            }
        }
        #endregion


        #region Local Methods
        private void GenerateGene()
        {
            if (_gene == null)
            {
                _gene = new();
                System.Random random = new System.Random();

                _gene.GeneType = Genotype.Ab;
                _gene.FirstGeneValue = random.Next(10, 21);
                _gene.SecondGeneValue = random.Next(5, 13);
                _gene.ParentType = _sex ? ParentType.Father : ParentType.Mother;
            }

            _firstValue = _gene.FirstGeneValue;
            _secondValue = _gene.SecondGeneValue;
            _genotype = _gene.GeneType;
        }

        private void HandleInputs()
        {
            // handle hunger rate 
            _hunger = Mathf.Clamp(_hunger, 0, 100);

            // handle thirst rate
            _thirst = Mathf.Clamp(_thirst, 0, 100);

            // handle health points 
            _healthPoints = Mathf.Clamp(_healthPoints, 0, 100);

            // handle age
            _age = Mathf.Clamp(_age, 0f, 30f);
        }
        #endregion
        

        #region Properties
        public EcosystemManager EcosystemManager => _ecosystemManager;
        public float            IdleSpeed        => _idleSpeed;
        public float            FleeingSpeed     => _fleeingSpeed;
        public bool             Sex              => _sex;
        public AnimalState CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }
        public NavMeshAgent Agent
        {
            get => _agent;
            set => _agent = value;
        }
        public float Hunger
        {
            get => _hunger;
            set => _hunger = value;
        }
        public float Thirst
        {
            get => _thirst;
            set => _thirst = value;
        }
        public float ReproductiveNeed
        {
            get => _reproductiveNeed;
            set => _reproductiveNeed = value;
        }
        public float Age
        {
            get => _age;
            set => _age = value;
        }
        public Gene Gene
        {
            get => _gene;
            set => _gene = value;
        }
        public GameObject MateAnimal
        {
            get => _mateAnimal;
            set => _mateAnimal = value;
        }
        #endregion
    }
}