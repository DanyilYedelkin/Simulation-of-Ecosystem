using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    #region Configuration
    [SerializeField] private float _growthTime;
    [SerializeField] private bool  _isEdible = true;
    [SerializeField] private float _remainingTime;
    #endregion

    #region Private Members
    private MeshRenderer _foodMesh;
    #endregion

    #region Unity Methods
    private void Start()
    {
        _foodMesh = gameObject.GetComponent<MeshRenderer>();
        _remainingTime = 0;
    }

    private void Update()
    {
        if (_remainingTime > 0)
        {
            _remainingTime -= Time.fixedDeltaTime;
        }
        else
        {
            _remainingTime = 0;
            _isEdible = true;
            _foodMesh.enabled = true;
        }
    }
    #endregion

    #region API
    public void FoodIsEaten()
    {
        _isEdible = false;
        _remainingTime = _growthTime;
        _foodMesh.enabled = false;
    }
    #endregion

    #region Properties
    public float GrowthTime => _growthTime;
    public bool IsEdible => _isEdible;
    #endregion
}
