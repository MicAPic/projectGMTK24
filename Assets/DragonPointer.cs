using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonPointer : MonoBehaviour
{
    [SerializeField] private UnityEngine.Camera _camera;
    [SerializeField] GameObject pointerIcon;
    [SerializeField] private float pointerImageSize;

    private Vector3 _cameraCenterPosition;
    private bool _isIconHiden;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _cameraCenterPosition = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        Vector3 fromCenterToDragon = transform.position - new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0);
        Ray ray = new Ray(_cameraCenterPosition, fromCenterToDragon);
        Debug.DrawRay(_cameraCenterPosition, fromCenterToDragon, Color.red);

        float rayMinDistance = Mathf.Infinity;

        for (int p = 0; p < 4; p++)
        {
            if (planes[p].Raycast(ray, out float distance))
            {
                if (distance < rayMinDistance)
                {
                    rayMinDistance = distance;
                }
            }
        }

        rayMinDistance = Mathf.Clamp(rayMinDistance, 0, fromCenterToDragon.magnitude);
        Vector3 worldPosition = ray.GetPoint(rayMinDistance);
        Vector3 position = _camera.WorldToScreenPoint(worldPosition);
        position = new Vector3((Mathf.Abs(position.x - pointerImageSize / 2)) * Mathf.Sign(position.x),
                               (Mathf.Abs(position.y - pointerImageSize / 2)) * Mathf.Sign(position.y), 0);
        Debug.Log(position.ToString());


        //if (fromCenterToDragon.magnitude > rayMinDistance && _isIconHiden)
        //{
        //    pointerIcon.SetActive(true);
        //    _isIconHiden = false;
        //}

        //if (fromCenterToDragon.magnitude <= rayMinDistance && !_isIconHiden)
        //{
        //    pointerIcon.SetActive(false);
        //    _isIconHiden = true;
        //}

        pointerIcon.transform.position = position;

    }
}
