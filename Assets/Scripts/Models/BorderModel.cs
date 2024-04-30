using OLS_HyperCasual;
using UnityEngine;

public class BorderModel : BaseModel<BorderView>
{
    private readonly Vector3 maxBoundsPos;
    private readonly Vector3 minBoundsPos;

    public BorderModel(BorderView view)
    {
        View = view;
        var bounds = View.Collider.bounds;

        maxBoundsPos = bounds.max;
        minBoundsPos = bounds.min;
    }

    public bool IsInsideZone(Vector3 position)
    {
        return position.x >= minBoundsPos.x && position.x <= maxBoundsPos.x &&
               position.y >= minBoundsPos.y && position.y <= maxBoundsPos.y;
    }

    public Vector3 GetBoundPosition(Vector3 position, out bool isRight)
    {
        if (position.x <= minBoundsPos.x)
        {
            isRight = true;
            return maxBoundsPos;
        }

        if (position.x >= maxBoundsPos.x)
        {
            isRight = true;
            return minBoundsPos;
        }
        
        if (position.y <= minBoundsPos.y)
        {
            isRight = false;
            return maxBoundsPos;
        }

        if (position.y >= maxBoundsPos.y)
        {
            isRight = false;
            return minBoundsPos;
        }
        isRight = true;
        return new Vector3();
    }
}