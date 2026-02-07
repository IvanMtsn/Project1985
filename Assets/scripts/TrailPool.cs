using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPool : ObjectPool<LineRenderer>
{
    public void ShootTrail(Vector3 start, Vector3 end, float duration)
    {
        StartCoroutine(ShootTrailCoroutine(start, end, duration));
    }
    public IEnumerator ShootTrailCoroutine(Vector3 start, Vector3 end, float duration)
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
