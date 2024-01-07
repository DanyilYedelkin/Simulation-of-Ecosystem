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
        [SerializeField] private float _idleSpeed    = 0f;
        [SerializeField] private float _fleeingSpeed = 0f;
        [SerializeField] private int   _litterCount  = 0;
        [SerializeField] private float _hunger;
        [SerializeField] private float _thirst;
        [SerializeField] private float _healthPoints;
        [SerializeField] private float _age;
        [Tooltip("true - male, false - female")]
        [SerializeField] private bool  _sex;

        [Space]
        [Header("Ecosystem Manager")]
        [SerializeField] private EcosystemManager _ecosystemManager;
        #endregion

        #region Private Members
        private NavMeshAgent _agent;
        #endregion

        #region Unity Methods
        public void OnValidate()
        {
            // handling user inputs to only valid values & hiding of optional values
            HandleInputs();
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _idleSpeed;
            _ecosystemManager = GameObject.FindGameObjectWithTag("EcosystemManager").GetComponent<EcosystemManager>();
            _sex = UnityEngine.Random.Range(0, 2) == 0 ? true : false;
        }
        #endregion

        #region API
        public void DeathIsInevitable()
        {
            /*if (_lifeSpan > 0)
            {
                _lifeSpan -= Time.fixedDeltaTime;
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
        #endregion

        #region Local Methods
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
        public int              LitterCount      => _litterCount;
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
        public float Age
        {
            get => _age;
            set => _age = value;
        }
        #endregion
    }
}