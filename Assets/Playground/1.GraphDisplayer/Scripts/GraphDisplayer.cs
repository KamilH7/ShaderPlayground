using Playground.Shared;
using UnityEngine;

namespace Playground._1.GraphDisplayer.Scripts
{
    public class GraphDisplayer : MonoBehaviour
    {
        private float GraphLength => _xBoundries.magnitude;

        [SerializeField] 
        private Vector2 _xBoundries;
        [SerializeField] 
        private int _resolution;
        [SerializeField]
        private PointsManager _pointsManager;

        private void Awake()
        {
            _pointsManager.Initialize();
        }

        private void Update()
        {
            AnimateGraph();
        }

        private void AnimateGraph()
        {
            DrawGraph(Time.fixedTime);
        }

        private void DrawGraph(float functionOffset)
        {
            _pointsManager.ReleasePoints();
            
            var stepSize = GraphLength / _resolution;
            var stepCount = Mathf.CeilToInt(GraphLength / stepSize);
            var xOffset = GraphLength / 2;
                
            for (int i = 0; i < stepCount; i++)
            {
                var xPos = xOffset - i * stepSize;
                var yPos = Mathf.Sin(xPos + functionOffset);
                var scale = Vector3.one * stepSize;
                
                _pointsManager.CreatePoint(new Vector3(xPos, yPos, 0), scale);
            }
        }
    }
}