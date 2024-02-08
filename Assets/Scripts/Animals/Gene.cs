using System.Collections;
using System.Runtime.Serialization;

namespace Animals
{
    /// <summary>
    /// <param name="AA"> a gene, which has 2 Dominant alleles </param>
    /// <param name="Ab"> a gene, which has 1 Dominant and 1 Recessive alleles </param>
    /// <param name="bb"> a gene, which has 2 Recessive alleles </param>
    /// </summary>
    public enum Genotype
    {
        [EnumMember(Value = "AA")]
        AA = 1,
        [EnumMember(Value = "Ab")]
        Ab = 2,
        [EnumMember(Value = "bb")]
        bb = 3,
    };

    public class Gene
    {
        #region Properties
        public ParentType ParentType      { get; set; }
        public Genotype   GeneType        { get; set; }
        public int        FirstGeneValue  { get; set; }
        public int        SecondGeneValue { get; set; }
        #endregion
    }
}