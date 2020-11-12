using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class BodySourceView : MonoBehaviour
{
    public BodySourceManager BodySourceManager;
    public GameObject JointObject;

    private Dictionary<ulong, GameObject> _bodies = new Dictionary<ulong, GameObject>();
    private List<JointType> _joints = new List<JointType>
    {
        JointType.HandLeft,
        JointType.HandRight,
    };

    public void Update()
    {
        (var data, var trackedIds) = GetKinectData();

        DeleteUntrackedBodies(trackedIds);

        CreateOrUpdateBodies(data);
    }

    private (Body[], List<ulong>) GetKinectData()
    {
        var bodiesData = BodySourceManager.GetData() ?? new Body[] { };
        var trackedIds = new List<ulong>();

        if (bodiesData != null)
        {
            bodiesData = bodiesData.Where(body => body != null).ToArray();
            trackedIds = bodiesData.Select(body => body.TrackingId).ToList();
        }

        return (bodiesData, trackedIds);
    }

    private void DeleteUntrackedBodies(List<ulong> trackedIds)
    {
        var knownIds = new List<ulong>(_bodies.Keys);

        for (int i = 0; i < knownIds.Count; i++)
        {
            if (!trackedIds.Contains(knownIds[i]))
            {
                Destroy(_bodies[knownIds[i]]);
                _bodies.Remove(knownIds[i]);
            }
        }
    }

    private void CreateOrUpdateBodies(Body[] data)
    {
        foreach (var body in data.Where(body => body.IsTracked))
        {
            if (!_bodies.ContainsKey(body.TrackingId))
                _bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);

            UpdateBodyObject(body, _bodies[body.TrackingId]);
        }
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject($"Body:{id}");

        foreach (JointType joint in _joints)
        {
            GameObject newJoint = Instantiate(JointObject);
            newJoint.name = joint.ToString();
            newJoint.transform.parent = body.transform;
        }

        return body;
    }

    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        foreach (JointType _joint in _joints)
        {
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.z = 0;

            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            jointObject.position = targetPosition;
        }
    }

    private Vector3 GetVector3FromJoint(Joint joint)
        => new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
}
