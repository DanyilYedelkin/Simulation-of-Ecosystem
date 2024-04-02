using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals.Predator
{
    public class PredatorController : AnimalBehaviourController
    {
        #region Private Members
        private GameObject _prey;
        private GameObject _water;
        private bool       _wantToDrink = false;
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            base.Start();
            if (EcosystemManager.GeneticEvolutionOfPredators)
            {
                FleeingSpeed = Gene.FirstGeneValue;
            }
        }

        private void Update()
        {
            AnimalStateLogic();
            DeathIsInevitable();

            // food 
            HungerLogic();

            // water
            ThirstLogic();
            DrinkLogic();

            // reproduction
            ReproductionNeed();
        }
        #endregion

        #region API
        public void WaterWasSeen(GameObject water)
        {
            if (Thirst > 50f && CurrentState != AnimalState.ChasingPrey && CurrentState != AnimalState.Drinking && CurrentState != AnimalState.LookingForMate)
            {
                CurrentState = AnimalState.RunningTowardsWater;
                Agent.destination = water.gameObject.transform.position;
                _water = water;
                _wantToDrink = true;
            }
        }

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
            EcosystemManager.TotalPredators--;
            EcosystemManager.TotalPredatorDeath++;
        }

        public override void AddAnimalToManager(GameObject animal)
        {
            EcosystemManager.Predators.Add(animal);
        }
        #endregion

        #region Local Methods
        private void AnimalStateLogic()
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
        }

        private void HungerLogic()
        {
            if (Agent.velocity.magnitude > 0.2f)
            {
                Hunger += (0.05f * IdleSpeed) * Time.fixedDeltaTime * EcosystemManager.SimulationSpeed;
            }
            else
            {
                Hunger += (0.05f * Time.fixedDeltaTime) * EcosystemManager.SimulationSpeed;
            }

            if (Hunger >= 100)
            {
                RemoveAnimalFromManager();
                Destroy(gameObject);
            }
        }

        private void DrinkLogic()
        {
            if (_water && _wantToDrink)
            {
                Collider waterCollider = _water.GetComponent<Collider>();
                if (waterCollider != null)
                {
                    Vector3 checkPoint = transform.position + Vector3.down * 0.1f; 
                    float radius = 0.5f; 

                    if (Physics.CheckSphere(checkPoint, radius, LayerMask.GetMask("Water"))) 
                    {
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
                Thirst -= Time.fixedDeltaTime * 2f * EcosystemManager.SimulationSpeed;

                Thirst = Thirst < 0 ? 0 : Thirst;
            }
            else
            {
                _wantToDrink = false;
                CurrentState = AnimalState.Idle;
            }
        }

        private void EatPrey(GameObject prey)
        {
            if (Vector3.Distance(this.transform.position, prey.transform.position) < 2f)
            {
                EcosystemManager.Prey.Remove(prey);
                Destroy(prey);
                EcosystemManager.TotalPreyDeath++;
                EcosystemManager.TotalPrey--;

                CurrentState = AnimalState.Idle;

                AnimalBehaviourController preyBehaviour = prey.GetComponent<AnimalBehaviourController>();
                if (preyBehaviour != null)
                {
                    if (preyBehaviour.AgeController.IsChild())
                    {
                        Hunger -= 50;
                    }
                    else
                    {
                        Hunger -= 25;
                    }

                    Hunger = Hunger < 0 ? 0 : Hunger;
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