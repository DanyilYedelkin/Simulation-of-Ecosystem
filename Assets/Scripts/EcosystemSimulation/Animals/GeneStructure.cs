using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals
{
    public enum ParentType 
    { 
        Father, 
        Mother 
    };

    public class GeneStructure : MonoBehaviour
    {
        #region Configuration
        [SerializeField] private AnimalBehaviourController _currentAnimal;
        #endregion

        #region Private Members
        private Gene          _fatherGene           = null;
        private Gene          _motherGene           = null;

        // Mendelian Inheritance Logic
        private List<Gene>    _possibleGenotype     = new();
        private string        _stringFatherGenotype = "";
        private string        _stringMotherGenotype = "";
        #endregion

        #region Unity Methods
        private void Start()
        {
            if (_currentAnimal == null)
            {
                _currentAnimal = GetComponent<AnimalBehaviourController>();
            }
        }
        #endregion

        #region API
        public Gene MendelianInheritance(Gene fatherGene, Gene motherGene)
        {
            _fatherGene = fatherGene;
            _motherGene = motherGene;

            return Inherit();
        }
        #endregion

        #region Local Methods
        private Gene Inherit()
        {
            _stringFatherGenotype = GetStringGenotype(_fatherGene.GeneType);
            _stringMotherGenotype = GetStringGenotype(_motherGene.GeneType);

            GetAllPossibleGenotypes();

            int randomNumber = Random.Range(0, 4);

            return _possibleGenotype[randomNumber];
        }

        private void GetAllPossibleGenotypes()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Gene newGene = new();

                    // get genotype
                    newGene.GeneType = CombineGenotype(i, j);

                    // get animal's sex
                    newGene.ParentType = _currentAnimal.Sex ? ParentType.Father : ParentType.Mother;

                    // get values for allels
                    (int, int) alleleValue = GetAlleleValue(i, j);
                    newGene.FirstGeneValue = alleleValue.Item1;
                    newGene.SecondGeneValue = alleleValue.Item2;

                    _possibleGenotype.Add(newGene);
                }
            }
        }

        private (int, int) GetAlleleValue(int firstAllele, int secondAllele)
        {
            int firstValue, secondValue;

            char fatherAllele = _stringFatherGenotype[firstAllele];
            char motherAllele = _stringMotherGenotype[secondAllele];

            if (fatherAllele == 'A' && motherAllele == 'b')
            {
                firstValue = GetTheBestValue(_fatherGene);
                secondValue = (_motherGene.GeneType == Genotype.bb) ? GetTheBestValue(_motherGene) : _motherGene.SecondGeneValue;
            }
            else if (fatherAllele == 'b' && motherAllele == 'A')
            {
                firstValue = (_fatherGene.GeneType == Genotype.bb) ? GetTheBestValue(_fatherGene) : _fatherGene.SecondGeneValue;
                secondValue = GetTheBestValue(_motherGene);
            }
            else if (fatherAllele == 'A' && motherAllele == 'A')
            {
                firstValue = GetTheBestValue(_fatherGene);
                secondValue = GetTheBestValue(_motherGene);

                if (firstValue < secondValue)
                {
                    (secondValue, firstValue) = (firstValue, secondValue);
                }
            }
            else
            {
                firstValue = (_fatherGene.GeneType == Genotype.bb) ? GetTheBestValue(_fatherGene) : _fatherGene.SecondGeneValue;
                secondValue = (_motherGene.GeneType == Genotype.bb) ? GetTheBestValue(_motherGene) : _motherGene.SecondGeneValue;

                if (firstValue < secondValue)
                {
                    (secondValue, firstValue) = (firstValue, secondValue);
                }
            }

            return (firstValue, secondValue);
        }

        private int GetTheBestValue(Gene gene)
        {
            int value = 0;

            if (gene.GeneType == Genotype.AA)
            {
                value = gene.FirstGeneValue > gene.SecondGeneValue ? gene.FirstGeneValue : gene.SecondGeneValue;
            }
            else if (gene.GeneType == Genotype.Ab)
            {
                value = gene.FirstGeneValue;
            }
            else
            {
                value = gene.FirstGeneValue > gene.SecondGeneValue ? gene.FirstGeneValue : gene.SecondGeneValue;
            }

            return value;
        }

        private Genotype CombineGenotype(int firstAllele, int secondAllele)
        {
            string stringGenotype = CombineStringGenotype(firstAllele, secondAllele);

            switch (stringGenotype)
            {
                case "AA":
                    return Genotype.AA;

                case "Ab":
                    return Genotype.Ab;

                case "bb":
                    return Genotype.bb;

                default:
                    Debug.Log("Error in combining of parents' genotype!");
                    break;
            }

            return Genotype.Ab;
        }

        private string CombineStringGenotype(int firstAllele, int secondAllele)
        {
            string newStringGenotype;

            if (_stringFatherGenotype[firstAllele].ToString() == "A" && _stringMotherGenotype[secondAllele].ToString() == "b")
            {
                newStringGenotype = _stringFatherGenotype[firstAllele].ToString() + _stringMotherGenotype[secondAllele].ToString();
            }
            else if (_stringFatherGenotype[firstAllele].ToString() == "b" && _stringMotherGenotype[secondAllele].ToString() == "A")
            {
                newStringGenotype = _stringMotherGenotype[secondAllele].ToString() + _stringFatherGenotype[firstAllele].ToString();
            }
            else
            {
                newStringGenotype = _stringFatherGenotype[firstAllele].ToString() + _stringMotherGenotype[secondAllele].ToString();
            }

            return newStringGenotype;
        }

        private string GetStringGenotype(Genotype genotype)
        {
            if (genotype == Genotype.AA)
            {
                return "AA";
            }
            else if (genotype == Genotype.Ab)
            {
                return "Ab";
            }
            else
            {
                return "bb";
            }
        }

        private void FindBetterGeneValues(Gene gene)
        {

        }
        #endregion

        #region Properties
        public Gene FatherGene
        {
            get => _fatherGene;
            set => _fatherGene = value;
        }
        public Gene MotherGene
        {
            get => _motherGene;
            set => _motherGene = value;
        }
        #endregion
    }
}