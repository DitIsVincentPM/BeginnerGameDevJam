/*
Copyright (c) 2014, Marié Paulino
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
(This is the Simplified BSD Licence - http://opensource.org/licenses/BSD-2-Clause)
 */

using UnityEngine;

public class CameraController : MonoBehaviour
{
	#region Public fields

    [Space(10.0f)]
    [Tooltip("Point aimed by the camera")]
    public Transform Target;

    [Tooltip("Maximum distance between the camera and Target")]
	public float Distance = 2;

    [Tooltip("Distance lerp factor")]
    [Range(.0f, 1.0f)]
    public float LerpSpeed = .1f;

    [Space(10.0f)]
    [Tooltip("Collision parameters")]
    public TraceInfo RayTrace = new TraceInfo { Thickness = .2f };

    [Tooltip("Camera pitch limitations")]
    public LimitsInfo PitchLimits = new LimitsInfo { Minimum = -60.0f, Maximum = 60.0f };

    [Tooltip("Input axes used to control the camera")]
    public InputInfo InputAxes = new InputInfo
    {
        Horizontal = new InputAxisInfo { Name = "Mouse X", Sensitivity = 15.0f },
        Vertical = new InputAxisInfo { Name = "Mouse Y", Sensitivity = 15.0f }
    };
	
	#endregion

    #region Structs

    [System.Serializable]
    public struct LimitsInfo
    {
        [Tooltip("Minimum pitch angle, in the range [-90, Maximum]")]
        public float Minimum;

        [Tooltip("Maximum pitch angle, in the range [Minimum, 90]")]
        public float Maximum;
    }

    [System.Serializable]
    public struct TraceInfo
    {
        [Tooltip("Ray thickness")]
        public float Thickness;

        [Tooltip("Layers the camera collide with")]
        public LayerMask CollisionMask;
    }

    [System.Serializable]
    public struct InputInfo
    {
        [Tooltip("Horizontal axis")]
        public InputAxisInfo Horizontal;

        [Tooltip("Vertical axis")]
        public InputAxisInfo Vertical;
    }

    [System.Serializable]
    public struct InputAxisInfo
    {
        [Tooltip("Input axis name")]
        public string Name;

        [Tooltip("Axis sensitivity")]
        public float Sensitivity;
    }

    #endregion

    private float _pitch;
    private float _distance;

    public void Start()
    {
        _pitch = Mathf.DeltaAngle(0, -transform.localEulerAngles.x);
        _distance = Distance;
    }

	public void Update()
	{
		float yaw = transform.localEulerAngles.y + Input.GetAxis(InputAxes.Horizontal.Name) * InputAxes.Horizontal.Sensitivity;
		
		_pitch += Input.GetAxis(InputAxes.Vertical.Name) * InputAxes.Vertical.Sensitivity;
		_pitch = Mathf.Clamp(_pitch, PitchLimits.Minimum, PitchLimits.Maximum);		
		
		transform.localEulerAngles = new Vector3(-_pitch, yaw, 0);        
	}

	public void LateUpdate()
	{
		if (Target == null) return;

        var startPos = Target.position;
        var endPos = startPos - transform.forward * Distance;
        var result = Vector3.zero;

        RayCast(startPos, endPos, ref result, RayTrace.Thickness);
        var resultDistance = Vector3.Distance(Target.position, result);
        
        if (resultDistance <= _distance)    // closest collision
        {
            transform.position = result;
            _distance = resultDistance;
        }
        else
        {
            _distance = Mathf.Lerp(_distance, resultDistance, LerpSpeed);
            transform.position = startPos - transform.forward * _distance;
        }
	}

    private bool RayCast(Vector3 start, Vector3 end, ref Vector3 result, float thickness)
    {
        var direction = end - start;
        var distance = Vector3.Distance(start, end);

        RaycastHit hit;
        if (Physics.SphereCast(new Ray(start, direction), thickness, out hit, distance, RayTrace.CollisionMask.value))
        {
            result = hit.point + hit.normal * RayTrace.Thickness;
            return true;
        }
        else
        {
            result = end;
            return false;
        }
    }
}