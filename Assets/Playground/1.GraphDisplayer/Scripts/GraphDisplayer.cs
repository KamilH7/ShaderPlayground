using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Playground._1.GraphDisplayer.Scripts
{
    public class GraphDisplayer : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _pointPrefab;
        
        [SerializeField] 
        private Vector2 _xBoundries;
        [SerializeField, Range(1,1000)] 
        private int _resolution;
        
        private List<GameObject> _points = new List<GameObject>();

        private float GraphLength => _xBoundries.magnitude;
        private ObjectPool<GameObject> _pool;

        private void OnValidate()
        {
            if(!Application.isPlaying)
                return;
            
            DrawGraph();
        }

        private void Start()
        {
            DrawGraph();
        }

        private void DrawGraph()
        {
            _pool ??= CreatePool();
            
            ReleasePoints();
            
            var stepSize = GraphLength / _resolution;
            var stepCount = Mathf.CeilToInt(GraphLength / stepSize);
            var xOffset = GraphLength / 2;
                
            for (int i = 0; i < stepCount; i++)
            {
                var xPos = xOffset - i * stepSize;
                var yPos = Mathf.Sin(xPos);
                var scale = Vector3.one * stepSize;
                
                CreatePoint(new Vector3(xPos, yPos, 0), scale);
            }
        }
        
        private void ReleasePoints()
        {
            foreach (var point in _points)
            {
                _pool.Release(point);
            }
            
            _points.Clear();
        }
        
        private void CreatePoint(Vector3 position, Vector3 scale)
        {
            var point = _pool.Get();
            point.transform.position = position;
            point.transform.localScale = scale;
            _points.Add(point);
        }

        private ObjectPool<GameObject> CreatePool()
        {
            return new ObjectPool<GameObject>(() => Instantiate(_pointPrefab, transform, true),
                point => { point.SetActive(true); },
                point => { point.SetActive(false); },
                Destroy);
        }
    }
}