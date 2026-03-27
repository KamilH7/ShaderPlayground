using System.Collections.Generic;
using NUnit.Framework;
using Playground.Shared;
using UnityEngine;

namespace Playground._2.Wave.Scripts
{
    public class WaveDisplayer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PointsManager _pointsManager;
        
        [Header("Displayer Settings")]
        [SerializeField] private float xMax = 1f;
        [SerializeField] private float xMin = -1f;
        [SerializeField] private int pointCount = 1;
        [SerializeField] private float speed = 1;
        
        [Header("Function Settings")]
        [SerializeField] private FunctionType _functionType;
        [SerializeField] private FunctionParameter[] _parameters;

        private void OnEnable()
        {
            _pointsManager.Initialize();
        }

        private void Update()
        {
            _pointsManager.ReleasePoints();
            DrawWave(Time.fixedTime);
        }

        private void DrawWave(float functionOffset)
        {
            var increment = (xMax - xMin) / pointCount;
            var function = FunctionLibrary.GetFunction(_functionType);
            
            for (int i = 0; i < pointCount; i++)
            {
                var xPos = xMin + (increment * i);
                var yPos = function(xPos, functionOffset * speed, _parameters);
                _pointsManager.CreatePoint(new Vector3(xPos + transform.position.x, yPos + transform.position.y, 0), new Vector3(increment,increment,increment));
            }
        }
    }
}