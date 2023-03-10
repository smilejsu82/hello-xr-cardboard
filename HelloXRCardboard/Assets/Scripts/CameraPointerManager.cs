using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public static CameraPointerManager instance;

    [SerializeField] private GameObject pointer;
    [SerializeField] private float maxDistancePointer = 4.5f;

    [Range(0,1)]
    [SerializeField] private float distPointerObject = 0.95f;


    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;

    private readonly string interactableTag = "Interactable";
    private float scaleSize = 0.025f;

    [HideInInspector]
    public Vector3 hitPoint;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    private void Start()
    { 
        GazeManager.Instance.OnGazeSelection += GazeSelection;
    }

    private void GazeSelection()
    {
        _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);

    }

    public void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        
        Debug.DrawRay(ray.origin, ray.direction * _maxDistance, Color.green);

        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            this.hitPoint = hit.point;

            if (_gazedAtObject != hit.transform.gameObject)
            {
                _gazedAtObject?.SendMessage("OnPointerExitXR", null,SendMessageOptions.DontRequireReceiver);
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnterXR", null, SendMessageOptions.DontRequireReceiver);
                GazeManager.Instance.StartGazeSelection();
            }
            if (hit.transform.CompareTag(interactableTag))
            {
                PointerOnGaze(hit.point);
            }
            else
            {
                PointerOutGaze(ray.direction.y);
            }
        }
        else
        {
            _gazedAtObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
            _gazedAtObject = null;
        }

        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void PointerOnGaze(Vector3 hitPoint)
    {
        float saleFactor = scaleSize*Vector3.Distance(transform.position, hitPoint);   
        pointer.transform.localScale = Vector3.one * saleFactor;

        //pointer.transform.parent.position = CalculatePointerPosition(transform.position, hitPoint, distPointerObject);
        pointer.transform.parent.position = CalculatePointerPosition(transform.position, hitPoint, 0.5f);

    }

    private void PointerOutGaze(float y)
    {
        pointer.transform.localScale = Vector3.one * 0.1f;
        pointer.transform.parent.transform.localPosition = new Vector3(0, 0, maxDistancePointer);
        pointer.transform.parent.parent.transform.rotation = transform.rotation;
        GazeManager.Instance.CancelGazeSelection();

    }

    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        float x =p0.x + t*(p1.x-p0.x);
        float y =p0.y + t*(p1.y-p0.y);
        float z =p0.z + t*(p1.z-p0.z);

        return new Vector3(x,y,z);  
    }
}