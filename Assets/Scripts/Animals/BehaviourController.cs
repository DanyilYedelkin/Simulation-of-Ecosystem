using System;
using UnityEngine;
using UnityEngine.AI;

namespace Animals
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BehaviourController : MonoBehaviour
    {
        #region Configuration
        [Tooltip("Indicates whether the object is currently running.")]
        [SerializeField] private bool  _isRunning;
        [Tooltip("Indicates whether the object is currently chasing something or someone.")]
        [SerializeField] private bool  _isChasing;
        [Tooltip("Indicates whether the object is currently in the process of eating.")]
        [SerializeField] private bool  _isEating;
        [Tooltip("Represents the hunger level of the object, typically ranging from 0 to 100.")]
        [SerializeField] private float _hunger       = 100f;
        [Tooltip("Represents the maximum lifespan of the object in some unit of time (e.g., seconds).")]
        [SerializeField] private float _lifeSpan     = 1000f;
        [Tooltip("The speed at which the object moves when walking.")]
        [SerializeField] private float _walkingSpeed = 6f;
        [Tooltip("The speed at which the object moves when running (default value might be set elsewhere).")]
        [SerializeField] private float _runningSpeed;
        [Tooltip("The number of litters or offspring produced by the object (an integer count).")]
        [SerializeField] private int   _litterCount;


        [SerializeField] private EcosystemManager _ecosystemManager;
        #endregion

        #region Private Members
        private NavMeshAgent _agent;
        #endregion

        #region Unity Methods
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _walkingSpeed;
        }
        #endregion

        #region API
        public void DeathIsInevitable()
        {
            if (_lifeSpan > 0)
            {
                _lifeSpan -= Time.fixedDeltaTime;
            }
            else
            {
                RemoveAnimalFromManager();
                Destroy(this.gameObject);
            }
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

        #region Properties
        public bool IsRunning
        {
            get => _isRunning;
            set => _isRunning = value;
        }
        public bool IsChasing
        {
            get => _isChasing;
            set => _isChasing = value;
        }
        public bool IsEating
        {
            get => _isEating;
            set => _isEating = value;
        }
        public int LitterCount
        {
            get => _litterCount;
            set => _litterCount = value;
        }
        public float Hunger
        {
            get => _hunger;
            set => _hunger = value;
        }
        public EcosystemManager EcosystemManager => _ecosystemManager;
        public NavMeshAgent Agent => _agent;
        public float LifeSpan => _lifeSpan;
        public float WalkingSpeed => _walkingSpeed;
        public float RunningSpeed => _runningSpeed;
        #endregion
    }
}