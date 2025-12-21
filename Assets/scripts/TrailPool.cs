using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPool : MonoBehaviour
{
    public static TrailPool Instance;
    [SerializeField] LineRenderer _linePrefab;
    Queue<LineRenderer> _pool = new Queue<LineRenderer>();

    private void Awake()
    {
        Instance = this;
    }
    public LineRenderer Get()
    {
        if(_pool == null)
        {
            _pool = new Queue<LineRenderer>();
        }
        if(_pool.Count > 0)
        {
            LineRenderer line = _pool.Dequeue();
            line.gameObject.SetActive(true);
            return line;
        }
        else
        {
            return Instantiate(_linePrefab);
        }
    }
    public void Return(LineRenderer line)
    {
        line.gameObject.SetActive(false);
        _pool.Enqueue(line);
    }
    public IEnumerator ShootTrail(Vector3 start, Vector3 end, float duration)
    {
        LineRenderer line = Get();
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            line.widthMultiplier = 1 - (t / 0.2f);
            yield return null;
        }
        Return(line);
    }
}
