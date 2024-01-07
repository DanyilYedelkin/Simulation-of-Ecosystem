using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals.Prey
{
    public class PreyController : AnimalBehaviourController
    {
        #region Private Members
        private GameObject       _preyFood;
        private List<GameObject> _predators;
        private Vector3          _runDirection;
        #endregion

        #region Unity Methods
        private void Update()
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

            DeathIsInevitable();

            if (Agent.velocity.magnitude > 0.2f)
            {
                Hunger -= (0.05f * IdleSpeed) * Time.fixedDeltaTime;
            }
            else
            {
                Hunger -= (0.05f * Time.fixedDeltaTime);
            }

            if (Hunger <= 0)
            {
                RemoveAnimalFromManager();
                Destroy(gameObject);
            }
        }
        #endregion

        #region API
        public void FoodWasSeen(GameObject food)
        {
            if (Hunger < 50f && CurrentState != AnimalState.Fleeing && EcosystemManager.PreyFoodEdibility.ContainsKey(food) &&
                EcosystemManager.PreyFoodEdibility[food].IsEdible && CurrentState != AnimalState.Eating)
            {
                CurrentState = AnimalState.RunningTowardsFood;
                Agent.destination = food.gameObject.transform.position;
                _preyFood = food;
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
        private void EatFood()
        {
            if (EcosystemManager.PreyFoodEdibility.ContainsKey(_preyFood) && 
                EcosystemManager.PreyFoodEdibility[_preyFood].IsEdible)
            {
                Hunger += 25;
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
        #endregion
    }
}