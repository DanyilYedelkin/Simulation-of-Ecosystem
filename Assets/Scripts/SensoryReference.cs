using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals
{
    public class SensoryReference : MonoBehaviour
    {
        #region Configuration
        [SerializeField] private SearchRadius sensoryRadiusObj;
        #endregion


        #region Properties
        public SearchRadius SensoryRadiusObj => sensoryRadiusObj;
        #endregion
    }
}