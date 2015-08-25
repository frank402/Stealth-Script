using UnityEngine;
using System.Collections;

public class text : MonoBehaviour {
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;
	public Animator ani;
	Vector3 m_CamForward;
	Transform m_Cam;
	private Vector3 m_Move;
	float m_TurnAmount;
	float m_ForwardAmount;
	float h , v;
	// Use this for initialization
	void Start () {
		if (Camera.main != null)
		{
			m_Cam = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning(
				"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}
	}
	
	// Update is called once per frame
	void Update () {
		 h = Input.GetAxis ("Horizontal");
		 v = Input.GetAxis ("Vertical");
		ani.SetFloat ("Speed",m_ForwardAmount,0.1f,Time.deltaTime);
		ani.SetFloat ("Turn",m_TurnAmount,0.1f,Time.deltaTime);
		if (m_Cam != null)
		{
			// calculate camera relative direction to move:
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
			m_Move = v*m_CamForward + h*m_Cam.right;
		}
		else
		{
			// we use world-relative directions in the case of no main camera
			m_Move = v*Vector3.forward + h*Vector3.right;
		}
		Move (m_Move);

	}
	void AnimatorMove(){

	}
	void Move(Vector3 move){
		move = transform.InverseTransformDirection(move);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;
		ApplyExtraTurnRotation();
	}
	void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}
}
