using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Playground.Shared
{
    public class PointsManager : MonoBehaviour
    {
        [SerializeField] 
        protected GameObject _pointPrefab;
        
        protected List<GameObject> _points = new ();
        protected ObjectPool<GameObject> _pool;

        public void Initialize()
        {
            _pool = CreatePool();
        }
        
        public void ReleasePoints()
        {
            foreach (var point in _points)
            {
                _pool.Release(point);
            }
            
            _points.Clear();
        }

        public void CreatePoint(Vector3 position, Vector3 scale)
        {
            var point = _pool.Get();
            point.transform.position = position;
            point.transform.localScale = scale;
            _points.Add(point);
        }

        public ObjectPool<GameObject> CreatePool()
        {
            return new ObjectPool<GameObject>(() => Instantiate(_pointPrefab, transform, true),
                point => { point.SetActive(true); },
                point => { point.SetActive(false); },
                Destroy);
        }
    }
}