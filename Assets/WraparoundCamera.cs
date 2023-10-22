using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraparoundCamera : MonoBehaviour
{
    // Quick & dirty singleton.
    public static WraparoundCamera current;
    public GameObject[] childs;

    Camera central;
    float halfViewWidth;
    float halfViewHeight;

    void Awake()
    {
        // Make ourselves easy for other scripts to find.
        WraparoundCamera.current = this;

        // Position this camera to butt up to the central camera's left edge.
        central = GetComponent<Camera>();
        halfViewWidth = central.aspect * central.orthographicSize;
        halfViewHeight = central.orthographicSize;

        childs[0].transform.localPosition = Vector3.left * halfViewWidth * 2f;
        childs[1].transform.localPosition = Vector3.right * halfViewWidth * 2f;
        childs[2].transform.localPosition = Vector3.up * halfViewHeight * 2f;
        childs[3].transform.localPosition = Vector3.down * halfViewHeight * 2f;
    }

    // Objects will call this to wrap themselves around to the other side of the view.
    public void WrapMeIfNeeded(Renderer renderer)
    {
        // If the object is still in range to be seen by the central camera, don't move it.
        if (renderer.bounds.max.x < -halfViewWidth)
            // Once it's passed the left edge of the central camera,
            // teleport it over on full view width, so it's at the right edge.
            renderer.transform.position += Vector3.right * halfViewWidth * 2f;
        else if (renderer.bounds.min.x > halfViewWidth)
            // Once it's passed the left edge of the central camera,
            // teleport it over on full view width, so it's at the right edge.
            renderer.transform.position -= Vector3.right * halfViewWidth * 2f;

        if (renderer.bounds.max.y < -halfViewHeight)
            // Once it's passed the left edge of the central camera,
            // teleport it over on full view width, so it's at the right edge.
            renderer.transform.position += Vector3.up * halfViewHeight * 2f;
        else if (renderer.bounds.min.y > halfViewHeight)
            // Once it's passed the left edge of the central camera,
            // teleport it over on full view width, so it's at the right edge.
            renderer.transform.position -= Vector3.up * halfViewHeight * 2f;
    }
}