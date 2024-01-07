using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals
{
    [RequireComponent(typeof(SphereCollider))]
    public class SearchRadius : MonoBehaviour
    {
        #region Configuration
        [SerializeField] private SphereCollider _searchRadiusCollider;
        [SerializeField] private int _searchRadius;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (_searchRadiusCollider && _searchRadiusCollider.radius != _searchRadius)
            {
                _searchRadiusCollider.radius = _searchRadius;
            }
        }
        #endregion

        #region Properties
        public int AnimalSearchRadius
        {
            get => _searchRadius;
            set => _searchRadius = value;
        }
        #endregion
    }
}