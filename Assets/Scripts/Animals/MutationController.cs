using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals
{
    /// <summary>
    /// <param name="Dominant"> Fixly mutate dominant gene with the specified value</param>
    /// <param name="Recessive"> Fixly mutate recessive gene with the specified value</param>
    /// <param name="Both"> Mutate both (dominant and recessive) gene with the same specified value</param>
    /// <param name="Singleton"> Mutate only one (randomly choosen) of the genes (dominant or recessive) with the specified value</param>
    /// <param name="Random"> choose one of the type of mutation (Dominant, Recessive, Bothe or Singleton) </param>
    /// </summary>
    public enum MutationType 
    { 
        Dominant, 
        Recessive, 
        Both, 
        Singleton, 
        Random
    };

    public class MutationController : MonoBehaviour
    {
        #region Configuration
        [SerializeField] MutationType  _mutationType   = MutationType.Random;
        [SerializeField] private float _mutationChance = 30f;
        [SerializeField] private int   _minValue       = 0;
        [SerializeField] private int   _maxValue       = 0;
        #endregion

        #region Private Members
        private AnimalBehaviourController     _currentAnimal;
        private static readonly System.Random _random = new();
        #endregion

        #region Unity Methods
        private void Start()
        {
            _currentAnimal = GetComponent<AnimalBehaviourController>();
        }
        #endregion

        #region API
        public Gene TryMutateGene(Gene gene)
        {
            Gene newGene = gene;

            if (_random.NextDouble() < _mutationChance)
            {
                Mutation(newGene);
            }

            return newGene;
        }
        #endregion

        #region Local Methods
        private void Mutation(Gene gene)
        {
            int newValue1 = _random.Next(_minValue, _maxValue);
            int randomMutationType;

            switch (_mutationType)
            {
                case MutationType.Random:
                    randomMutationType = _random.Next(4);
                    switch (randomMutationType)
                    {
                        case 0:
                            _mutationType = MutationType.Dominant;
                            break;
                        case 1:
                            _mutationType = MutationType.Recessive;
                            break;
                        case 2:
                            _mutationType = MutationType.Both;
                            break;
                        case 3:
                            _mutationType = MutationType.Singleton;
                            break;
                    }
                    Mutation(gene);
                    break;

                case MutationType.Dominant:
                    gene.FirstGeneValue += newValue1;
                    break;

                case MutationType.Recessive:
                    gene.SecondGeneValue += newValue1;
                    break;

                case MutationType.Both:
                    int newValue2 = _random.Next(_minValue, _maxValue);
                    gene.FirstGeneValue += newValue1;
                    gene.SecondGeneValue += newValue2;
                    break;

                case MutationType.Singleton:
                    randomMutationType = _random.Next(3);
                    switch (randomMutationType)
                    {
                        case 0:
                            _mutationType = MutationType.Dominant;
                            break;
                        case 1:
                            _mutationType = MutationType.Recessive;
                            break;
                        case 2:
                            _mutationType = MutationType.Both;
                            break;
                    }
                    break;
            }
        }
        #endregion

        #region Properties
        public MutationType MutationType
        {
            get => _mutationType;
            set => _mutationType = value;
        }
        public float MutationChance
        {
            get => _mutationChance;
            set => _mutationChance = value;
        }
        public int MinValue
        {
            get => _minValue;
            set => _minValue = value;
        }
        public int MaxValue
        {
            get => _minValue;
            set => _maxValue = value;
        }
        #endregion
    }
}