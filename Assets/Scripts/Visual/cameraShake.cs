using UnityEngine;
using System.Collections;
public class cameraShake : MonoBehaviour
{
	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay;
	public float shake_intensity;

    private float current_shake_intensity;

    void Awake()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }
	
	void Update (){

		if (current_shake_intensity > 0)
        {
			transform.position = originPosition + Random.insideUnitSphere * current_shake_intensity;
			transform.rotation = new Quaternion(
				originRotation.x + Random.Range (-current_shake_intensity,current_shake_intensity) * .2f,
				originRotation.y + Random.Range (-current_shake_intensity,current_shake_intensity) * .2f,
				originRotation.z + Random.Range (-current_shake_intensity,current_shake_intensity) * .2f,
				originRotation.w + Random.Range (-current_shake_intensity,current_shake_intensity) * .2f);
			current_shake_intensity -= shake_decay;

            if (current_shake_intensity <= 0)
            {
                transform.position = originPosition;
                transform.rotation = originRotation;
            }
		}
	}
	
	public void Shake()
    {
        current_shake_intensity = shake_intensity;
	}
}
