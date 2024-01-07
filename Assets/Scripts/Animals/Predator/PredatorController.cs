using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals.Predator
{
    public class PredatorController : AnimalBehaviourController
    {
        #region Private Members
        private GameObject _prey;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (CurrentState == AnimalState.ChasingPrey && _prey != null)
            {
                Agent.destination = _prey.transform.position;
                EatPrey(_prey);
            }

            if (CurrentState == AnimalState.ChasingPrey && !_prey)
            {
                CurrentState = AnimalState.Idle;
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

            if (Hunger <= 0 || Thirst <= 0)
            {
                RemoveAnimalFromManager();
                Destroy(gameObject);
            }
        }
        #endregion

        #region API
        public void PreyWasSeen(GameObject prey)
        {
            CurrentState = AnimalState.ChasingPrey;
            _prey = prey;
        }

        public void PreyWasLost()
        {
            CurrentState = AnimalState.Idle;
            _prey = null;
        }

        public override void RemoveAnimalFromManager()
        {
            EcosystemManager.Predators.Remove(this.gameObject);
            EcosystemManager.TotalPredatorDeath++;
        }

        public override void AddAnimalToManager(GameObject animal)
        {
            EcosystemManager.Predators.Add(animal);
        }
        #endregion

        #region Local Methods
        private void EatPrey(GameObject prey)
        {
            if (Vector3.Distance(this.transform.position, prey.transform.position) < 2f)
            {
                EcosystemManager.Prey.Remove(prey);
                Destroy(prey);
                EcosystemManager.TotalPreyDeath++;

                CurrentState = AnimalState.Idle;

                AnimalBehaviourController preyBehaviour = prey.GetComponent<AnimalBehaviourController>();
                if (preyBehaviour != null)
                {
                    if (preyBehaviour.Age >= 1f && preyBehaviour.Age <= 15f)
                    {
                        Hunger += 50;
                    }
                    else
                    {
                        Hunger += 25;
                    }
                }
                else
                {
                    Debug.LogError("Prey does not have the AnimalBehaviourController component.");
                }
            }
        }
        #endregion
    }
}