namespace Animals
{
    using UnityEngine;

    public enum AgeState
    {
        Newborn,
        Infant,
        Adolescent,
        Young,
        Adult,
        Old
    }

    public class AgeController : MonoBehaviour
    {
        #region Configuration
        [Header("General Info")]
        [SerializeField] private float    _currentAge = 0f;
        [SerializeField] private AgeState _currentAgeState;

        [Header("Age Configuration")]
        [SerializeField] private float _maxAge;
        [SerializeField] private float _infantAge;
        [SerializeField] private float _adolescentAge;
        [SerializeField] private float _youngAge;
        [SerializeField] private float _adultAge;
        [SerializeField] private float _oldAge;

        [Header("Other Configurations")]
        [SerializeField] private SearchRadius _animalSearchRadius;
        [SerializeField] private AnimalBehaviourController _animalBehaviour;
        #endregion

        #region Private Members
        // default animal stats
        private int _defaultSearchRadius;
        private float _defaultIdleSpeed;
        private float _defaultFleeingSpeed;
        #endregion

        #region Unity Methods
        private void Start()
        {
            if (_animalBehaviour == null)
            {
                _animalBehaviour = gameObject.GetComponent<AnimalBehaviourController>();
            }

            _defaultSearchRadius = _animalSearchRadius.AnimalSearchRadius;
            _defaultIdleSpeed = _animalBehaviour.IdleSpeed;
            _defaultFleeingSpeed = _animalBehaviour.FleeingSpeed;
        }

        private void Update()
        {
            _currentAge += Time.fixedDeltaTime * _animalBehaviour.EcosystemManager.SimulationSpeed / _animalBehaviour.EcosystemManager.SecondsPerAge;
            GetCurrentAgeState();
            UpdateAnimalStats();
        }
        #endregion

        #region API
        public bool CanReproduce()
        {
            return _currentAgeState == AgeState.Adolescent || _currentAgeState == AgeState.Young || _currentAgeState == AgeState.Adult;
        }

        public bool IsDiedByAge()
        {
            return _currentAge > _maxAge;
        }

        public bool IsChild()
        {
            return _currentAgeState == AgeState.Newborn || _currentAgeState == AgeState.Infant;
        }
        #endregion

        #region Local Methods
        private void GetCurrentAgeState()
        {
            AgeState[] ageStates = { AgeState.Newborn, AgeState.Infant, AgeState.Adolescent, AgeState.Young, AgeState.Adult, AgeState.Old };
            float[] ageThresholds = { _infantAge, _adolescentAge, _youngAge, _adultAge, _oldAge };

            for (int i = 0; i < ageThresholds.Length; i++)
            {
                if (_currentAge < ageThresholds[i])
                {
                    _currentAgeState = ageStates[i];
                    return;
                }
            }

            _currentAgeState = AgeState.Old;
        }

        private void UpdateAnimalStats()
        {
            float radiusMultiplier = 1f;
            float speedMultiplier = 1f;

            switch (_currentAgeState)
            {
                case AgeState.Newborn:
                    radiusMultiplier = 0.4f;
                    speedMultiplier = 0.4f;
                    gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    break;

                case AgeState.Infant:
                    radiusMultiplier = 0.6f;
                    speedMultiplier = 0.6f;
                    gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;

                case AgeState.Adolescent:
                case AgeState.Adult:
                    radiusMultiplier = 0.8f;
                    speedMultiplier = 0.8f;
                    gameObject.transform.localScale = Vector3.one;
                    break;

                case AgeState.Old:
                    radiusMultiplier = 0.7f;
                    speedMultiplier = 0.7f;
                    break;
            }

            _animalSearchRadius.AnimalSearchRadius = Mathf.RoundToInt(_defaultSearchRadius * radiusMultiplier);
            _animalBehaviour.IdleSpeed = _defaultIdleSpeed * speedMultiplier;
            _animalBehaviour.FleeingSpeed = _defaultFleeingSpeed * speedMultiplier;
        }
        #endregion

        #region Properties
        public AgeState CurrentAgeState
        {
            get => _currentAgeState;
            set => _currentAgeState = value;
        }
        public float CurrentAge
        {
            get => _currentAge;
            set => _currentAge = value;
        }
        public float MaxAge
        {
            get => _maxAge;
            set => _maxAge = value;
        }
        public float InfantAge
        {
            get => _infantAge;
            set => _infantAge = value;
        }
        public float AdolescentAge
        {
            get => _adolescentAge;
            set => _adolescentAge = value;
        }
        public float YoungAge
        {
            get => _youngAge;
            set => _youngAge = value;
        }
        public float AdultAge
        {
            get => _adultAge;
            set => _adultAge = value;
        }
        public float OldAge
        {
            get => _oldAge;
            set => _oldAge = value;
        }
        #endregion
    }
}