using System.Collections.Generic;
using DG.Tweening;
using OLS_HyperCasual;
using UnityEngine;

public class CameraMoveModel : BaseModel<CameraMoveView>
{
    private Transform cachedTransform;

    public Vector3 MinBoundsPos { get; private set; }
    public Vector3 MaxBoundsPos { get; private set; }

    public CameraMoveModel(CameraMoveView view)
    {
        View = view;
        cachedTransform = View.Camera.transform;

        MinBoundsPos = View.Collider.bounds.min;
        MaxBoundsPos = View.Collider.bounds.max;
    }

    public void SetCameraPosition(Vector3 positionToCamera)
    {
        cachedTransform.position = Vector3.Lerp(cachedTransform.position,
            new Vector3(cachedTransform.position.x, positionToCamera.y, cachedTransform.position.z), 0.01f);
    }
}