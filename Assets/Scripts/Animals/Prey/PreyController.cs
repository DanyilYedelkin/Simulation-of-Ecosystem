using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals.Prey
{
    public class PreyController : AnimalBehaviourController
    {
        #region Private Members
        private GameObject       _preyFood;
        private GameObject       _water;
        private List<GameObject> _predators;
        private Vector3          _runDirection;
        private bool             _wantToDrink    = false;
        #endregion


        #region Unity Methods
        protected override void Start()
        {
            base.Start();
            GetComponent<SensoryReference>().SensoryRadiusObj.AnimalSearchRadius = Gene.FirstGeneValue;
        }

        private void Update()
        {
            AnimalStateLogic();
            DeathIsInevitable();

            // food 
            HungerLogic();
            FoodLogic();

            // water
            ThirstLogic();
            DrinkLogic();

            // reproduction
            ReproductionNeed();
        }
        #endregion


        #region API
        public void FoodWasSeen(GameObject food)
        {
            if (Hunger > 50f && CurrentState != AnimalState.Fleeing && EcosystemManager.PreyFoodEdibility.ContainsKey(food) &&
                EcosystemManager.PreyFoodEdibility[food].IsEdible && CurrentState != AnimalState.Eating && CurrentState != AnimalState.LookingForMate)
            {
                CurrentState = AnimalState.RunningTowardsFood;
                Agent.destination = food.gameObject.transform.position;
                _preyFood = food;
            }
        }

        public void WaterWasSeen(GameObject water)
        {
            if (Thirst > 50f && CurrentState != AnimalState.Fleeing && CurrentState != AnimalState.Drinking && CurrentState != AnimalState.LookingForMate)
            {
                CurrentState = AnimalState.RunningTowardsWater;
                Agent.destination = water.gameObject.transform.position;
                _water = water;
                _wantToDrink = true;
            }
        }

        public void PredatorWasSeen(List<GameObject> predators)
        {
            CurrentState = AnimalState.Fleeing;
            _predators = predators;
        }

        public void PredatorWasLost()
        {
            CurrentState = AnimalState.Idle;
            _predators = null;
            Agent.ResetPath();
        }

        public override void RemoveAnimalFromManager()
        {
            EcosystemManager.Prey.Remove(this.gameObject);
            EcosystemManager.TotalPreyDeath++;
        }

        public override void AddAnimalToManager(GameObject animal)
        {
            EcosystemManager.Prey.Add(animal);
        }
        #endregion


        #region Local Methods
        private void AnimalStateLogic()
        {
            if (CurrentState != AnimalState.Fleeing)
            {
                Agent.speed = IdleSpeed;
            }

            if (CurrentState == AnimalState.Fleeing && _predators.Count != 0)
            {
                _runDirection = Vector3.zero;
                foreach (var predator in _predators)
                {
                    _runDirection += (predator.transform.position - transform.position).normalized;
                }
                Agent.speed = FleeingSpeed;
                Agent.destination = -1 * _runDirection + transform.position;
            }
        }

        private void HungerLogic()
        {
            if (Agent.velocity.magnitude > 0.2f)
            {
                Hunger += (0.05f * IdleSpeed) * Time.fixedDeltaTime;
            }
            else
            {
                Hunger += (0.05f * Time.fixedDeltaTime);
            }

            if (Hunger >= 100)
            {
                RemoveAnimalFromManager();
                Destroy(gameObject);
            }
        }

        private void FoodLogic()
        {
            if (_preyFood)
            {
                if (EcosystemManager.PreyFoodEdibility.ContainsKey(_preyFood) &&
                    EcosystemManager.PreyFoodEdibility[_preyFood].IsEdible)
                {
                    if (Vector3.Distance(transform.position, _preyFood.transform.position) < 0.5f)
                    {
                        EatFood();
                    }
                }
                else
                {
                    CurrentState = AnimalState.Idle;
                    _preyFood = null;
                }
            }
        }

        private void EatFood()
        {
            if (EcosystemManager.PreyFoodEdibility.ContainsKey(_preyFood) && 
                EcosystemManager.PreyFoodEdibility[_preyFood].IsEdible)
            {
                Hunger -= 25;
                EcosystemManager.PreyFoodEdibility[_preyFood].FoodIsEaten();

                // for testing
                EcosystemManager.PreyFoodEdibility.Remove(_preyFood);
                Destroy(_preyFood.gameObject);

                _preyFood = null;
                CurrentState = AnimalState.Idle;
            }
            CurrentState = AnimalState.Idle;
            _preyFood = null;
        }

        private void DrinkLogic()
        {
            if (_water && _wantToDrink)
            {
                // Проверяем, находится ли объект на границе коллайдера воды
                Collider waterCollider = _water.GetComponent<Collider>();
                if (waterCollider != null)
                {
                    Vector3 checkPoint = transform.position + Vector3.down * 0.1f; // Можно настроить высоту проверки
                    float radius = 0.5f; // Можно настроить радиус проверки

                    if (Physics.CheckSphere(checkPoint, radius, LayerMask.GetMask("Water"))) // Предполагается, что вода имеет свой слой "Water"
                    {
                        // Объект находится на границе воды
                        Debug.Log("Prey is near of water");
                        Agent.destination = gameObject.transform.position;
                        DrinkWater();
                    }
                }
            }
        }

        private void DrinkWater()
        {
            if (Thirst > 0)
            {
                CurrentState = AnimalState.Drinking;
                Thirst -= Time.fixedDeltaTime * 2f;

                Thirst = Thirst < 0 ? 0 : Thirst;
            }
            else
            {
                _wantToDrink = false;
                CurrentState = AnimalState.Idle;
            }
        }
        #endregion
    }
}