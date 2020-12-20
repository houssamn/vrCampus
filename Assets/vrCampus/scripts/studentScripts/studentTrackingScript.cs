
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using vrCampusCourseware;

public class studentTrackingScript : MonoBehaviour {

    [Header("Courseware")]
    [SerializeField]
    TrackingDisplay leftScreen;

    Vector3 headsetPos = Vector3.zero;
    Quaternion headsetRot = Quaternion.identity;
    Vector3 controllerPos = Vector3.zero;
    Quaternion controllerRot = Quaternion.identity;

    string deviceName = "unnamed";
    bool hasController = false;
    float deltaTime;

    void Awake() {
        var headDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.Head, headDevices);

        if(headDevices.Count == 1)
        {
            InputDevice device = headDevices[0];
            //Debug.Log(string.Format("Device name '{0}' with role '{1}'", device.name, device.role.ToString()));
            deviceName = device.name;
        }

        // Subscribe to Input Tracking Events
        InputTracking.nodeAdded += inputNodeAdded;
        InputTracking.nodeRemoved += inputNodeRemoved;
        InputTracking.trackingAcquired += inputTrackingAcquired;
        InputTracking.trackingLost += inputTrackingLost;

    }

    void inputNodeAdded(XRNodeState state)
    {
        leftScreen.TrackingEvent(TrackingDisplay.TrackingEventType.nodeAdded, state);
    }

    void inputNodeRemoved(XRNodeState state)
    {
        leftScreen.TrackingEvent(TrackingDisplay.TrackingEventType.nodeRemoved, state);
    }

    void inputTrackingAcquired(XRNodeState state)
    {
        leftScreen.TrackingEvent(TrackingDisplay.TrackingEventType.trackingAcquired, state);
    }

    void inputTrackingLost(XRNodeState state)
    {
        leftScreen.TrackingEvent(TrackingDisplay.TrackingEventType.nodeRemoved, state);
    }
    

    // Once you complete this module, we'll keep your Update function active
    // to drive the map display
    void Update () {

        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);
        foreach(XRNodeState nstate in nodeStates)
        {
            if(nstate.nodeType ==  XRNode.Head)
            {
                nstate.TryGetPosition(out headsetPos);
                nstate.TryGetRotation(out headsetRot);
            }else if(nstate.nodeType == XRNode.GameController)
            {
                nstate.TryGetPosition(out controllerPos);
                nstate.TryGetRotation(out controllerRot);
                hasController = true;
            }
        }

        // Calculate FPS
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // leftScreen.DebugInfo(headsetPos.ToString());
        // This is the fail case, where there was no center eye was available.
        //rotation = Quaternion.identity;
        
        leftScreen.TrackingInfo(deviceName,
                                TrackingSpaceType.RoomScale,
                                headsetPos,
                                headsetRot,
                                hasController,
                                controllerPos,
                                controllerRot,
                                fps,
                                1.0f);

        leftScreen.DebugInfo(headsetPos.ToString());

    }
 


}
