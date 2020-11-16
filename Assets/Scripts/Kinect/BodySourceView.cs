using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class BodySourceView : MonoBehaviour
{
    public BodySourceManager BodySourceManager;
    public GameObject JointObject;
    public Material BoneMaterial;
    public Color Color = Color.green;

    private Dictionary<ulong, GameObject> _bodies = new Dictionary<ulong, GameObject>();
    private List<JointType> _joints = new List<JointType>
    {
        JointType.FootLeft,
        JointType.FootRight,

        JointType.KneeLeft,
        JointType.KneeRight,

        JointType.HandLeft,
        JointType.HandRight,

        JointType.ElbowLeft,
        JointType.ElbowRight,

        JointType.SpineBase,
        // Talvez mudar para Neck
        JointType.SpineShoulder,

        JointType.Head,
    };

    private Dictionary<JointType, JointType> _boneMap = new Dictionary<JointType, JointType>()
    {
        { JointType.FootLeft, JointType.KneeLeft },
        { JointType.KneeLeft, JointType.SpineBase },

        { JointType.FootRight, JointType.KneeRight },
        { JointType.KneeRight, JointType.SpineBase },

        { JointType.HandLeft, JointType.ElbowLeft },
        { JointType.ElbowLeft, JointType.SpineShoulder },

        { JointType.HandRight, JointType.ElbowRight },
        { JointType.ElbowRight, JointType.SpineShoulder },

        { JointType.SpineBase, JointType.SpineShoulder },
        { JointType.SpineShoulder, JointType.Head },
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

            LineRenderer lr = newJoint.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.material = BoneMaterial;
            lr.startWidth  = 0.05f;
            lr.endWidth  = 0.05f;
        }

        return body;
    }

    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        foreach (JointType joint in _joints)
        {
            Joint sourceJoint = body.Joints[joint];
            Joint? targetJoint = null;

            if (_boneMap.ContainsKey(joint))
                targetJoint = body.Joints[_boneMap[joint]];

            Transform jointObject = bodyObject.transform.Find(joint.ToString());
            jointObject.position = GetVector3FromJoint(sourceJoint);

            LineRenderer lr = jointObject.GetComponent<LineRenderer>();

            if (targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObject.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.startColor = this.Color;
                lr.endColor = this.Color;
            }
            else
                lr.enabled = false;
        }
    }

    private Vector3 GetVector3FromJoint(Joint joint, int? x = null, int? y = null, int? z = null)
        => new Vector3(x ?? joint.Position.X * 10, y ?? joint.Position.Y * 10, z ?? joint.Position.Z * 10);
}
