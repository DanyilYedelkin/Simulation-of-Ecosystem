using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals
{
    public class Mutation : MonoBehaviour
    {
        #region Configuration
        [SerializeField] private float _mutationAmount;
        [SerializeField] private float _mutationChance;
        #endregion

        #region Private Members
        private static readonly System.Random _random = new();
        #endregion

        #region API
        public int TryMutateGene(int gene)
        {
            int _gene = gene;
            if (_random.NextDouble() < _mutationChance)
            {
                Debug.Log("Gene getting mutated");
                double mutationval = RandomGaussian() * _mutationAmount;
                if (_gene < (_gene + mutationval)) return ++_gene;
                else { return --_gene; }
            }
            return _gene;
        }

        //Mutates the sensory radius value of a given entity
        public void AttemptSensoryMutation(GameObject entity)
        {
            SearchRadius searchRadius = entity.GetComponent<SensoryReference>().SensoryRadiusObj;
            int mutatedGeneNewVal = TryMutateGene(searchRadius.AnimalSearchRadius);
            searchRadius.AnimalSearchRadius = mutatedGeneNewVal;
            Debug.Log("NEW SENSORY RADIUS: " + mutatedGeneNewVal);
        }
        #endregion

        #region Local Methods
        private static float RandomGaussian()
        {
            float u1 = Convert.ToSingle(1 - _random.NextDouble());
            float u2 = Convert.ToSingle(1 - _random.NextDouble());
            float randStdNormal = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Sin(2 * Mathf.PI * u2);
            //Debug.Log("randomGaussian generated: " + randStdNormal);
            return randStdNormal;
        }
        #endregion
    }
}