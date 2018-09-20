using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float backgroundSize;
    public float parallaxSpeed;
    public bool scrolling;
    public bool parallax;

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;
    private float lastCameraX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }

        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }

    private void Update()
    {
        if (parallax)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxSpeed);
        }

        lastCameraX = cameraTransform.position.x;

        if (scrolling)
        {
            if (cameraTransform.position.x < layers[leftIndex].transform.position.x + viewZone)
                ScrollLeft();

            if (cameraTransform.position.x > layers[rightIndex].transform.position.x - viewZone)
                ScrollRight();
        }
    }

    void ScrollLeft()
    {
        //Debug.Log("scroll left");
        int lastRight = rightIndex;
        //layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        layers[rightIndex].position = new Vector3(layers[leftIndex].position.x + backgroundSize, layers[rightIndex].position.y, layers[rightIndex].position.z);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
    }

    void ScrollRight()
    {
        int lastRight = leftIndex;
        //layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        //Debug.Log("pre --- l@" + leftIndex + ":" + layers[leftIndex].position + " r" + rightIndex + ":" + layers[rightIndex].position);
        layers[leftIndex].position = new Vector3(layers[rightIndex].position.x + backgroundSize, layers[leftIndex].position.y, layers[leftIndex].position.z);
        //Debug.Log("pst --- l:" + layers[leftIndex].position + " r:" + layers[rightIndex].position);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex = 0;
    }
}
