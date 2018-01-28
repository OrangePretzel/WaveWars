using UnityEngine;

public class AspectUtility : MonoBehaviour
{
	public float DesiredWidth = 1920;
	public float DesiredHeight = 1080;

	public new Camera camera;
	public Camera backgroundCam;

	void Awake()
	{
		if (!camera)
			camera = GetComponent<Camera>();
		if (!camera)
			camera = Camera.main;

		if (!camera)
		{
			Debug.LogError("No camera available");
			return;
		}

		SetCamera();
	}

	public void SetCamera()
	{
		float desiredRatio = DesiredWidth / DesiredHeight;
		float currentAspectRatio = (float)Screen.width / Screen.height;


		// If the current aspect ratio is already approximately equal to the desired aspect ratio,
		// use a full-screen Rect (in case it was set to something else previously)
		if ((int)(currentAspectRatio * 100) / 100.0f == (int)(desiredRatio * 100) / 100.0f)
		{
			camera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			if (backgroundCam)
			{
				Destroy(backgroundCam.gameObject);
			}
			return;
		}
		// Pillarbox
		if (currentAspectRatio > desiredRatio)
		{
			float inset = 1.0f - desiredRatio / currentAspectRatio;
			camera.rect = new Rect(inset / 2, 0.0f, 1.0f - inset, 1.0f);
		}
		// Letterbox
		else
		{
			float inset = 1.0f - currentAspectRatio / desiredRatio;
			camera.rect = new Rect(0.0f, inset / 2, 1.0f, 1.0f - inset);
		}

		if (!backgroundCam)
		{
			// Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
			backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
			backgroundCam.depth = int.MinValue;
			backgroundCam.clearFlags = CameraClearFlags.SolidColor;
			backgroundCam.backgroundColor = Color.black;
			backgroundCam.cullingMask = 0;
		}
	}
}