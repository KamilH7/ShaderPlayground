using UnityEngine;

public class GridFunctionDisplayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PointsManager _pointsManager;
    [SerializeField] private FunctionProvider _functionProvider;
    
    [Header("Settings")]
    [SerializeField] private Vector2 gridBoundsFromCenter = new (-1, 1);
    [SerializeField] private int resolution = 100;

    private GameObject[,] _points;

    private void Awake()
    {
        _pointsManager.Initialize();
    }

    private void OnResolutionChanged()
    {
        _pointsManager.ReleasePoints();
        _points = new GameObject[resolution, resolution];
        
        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                _points[x,z] = _pointsManager.CreatePoint(transform.position, transform.localScale);
            }
        }
    }

    private void OnEnable()
    {
        OnResolutionChanged();
    }

    private void Update()
    {
        if (_points.GetLength(0) != resolution)
        {
            OnResolutionChanged();
        }
        
        DrawWave();
    }

    private void DrawWave()
    {
        var min = gridBoundsFromCenter.x;
        var max = gridBoundsFromCenter.y;
            
        var increment = (max - min) / resolution;

        for (int x = 0; x < resolution; x++)
        {
            var xPos = min + (increment * x);
            
            for (int z = 0; z < resolution; z++)
            {
                var point = _points[x,z];
                
                var zPos = min + (increment * z);
                var resultPos = _functionProvider.GetPosition(xPos, zPos);
                    
                point.transform.localPosition =  new Vector3(resultPos.x, resultPos.y, resultPos.z);
                point.transform.localScale = new Vector3(increment, increment, increment);
            }
        }
    }
}