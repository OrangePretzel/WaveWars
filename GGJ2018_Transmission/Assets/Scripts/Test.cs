using UnityEngine;

public class Test : MonoBehaviour
{
	float c1LH;
	float c1LV;
	float c1RH;
	float c1RV;
	float c1LTrig;
	float c1RTrig;
	float c1LBump;
	float c1RBump;
	float c1Submmit;

	float c2LH;
	float c2LV;
	float c2RH;
	float c2RV;
	float c2LTrig;
	float c2RTrig;
	float c2LBump;
	float c2RBump;
	float c2Submmit;

	string[] joys;

	private void Update()
	{

		c1LH = Input.GetAxis("C1LHorizontal");
		c1LV = Input.GetAxis("C1LVertical");
		c1RH = Input.GetAxis("C1RHorizontal");
		c1RV = Input.GetAxis("C1RVertical");
		c1LTrig = Input.GetAxis("C1LTrigger");
		c1RTrig = Input.GetAxis("C1RTrigger");
		c1LBump = Input.GetAxis("C1LBumper");
		c1RBump = Input.GetAxis("C1RBumper");
		c1Submmit = Input.GetAxis("C1Submit");

		c2LH = Input.GetAxis("C2LHorizontal");
		c2LV = Input.GetAxis("C2LVertical");
		c2RH = Input.GetAxis("C2RHorizontal");
		c2RV = Input.GetAxis("C2RVertical");
		c2LTrig = Input.GetAxis("C2LTrigger");
		c2RTrig = Input.GetAxis("C2RTrigger");
		c2LBump = Input.GetAxis("C2LBumper");
		c2RBump = Input.GetAxis("C2RBumper");
		c2Submmit = Input.GetAxis("C2Submit");


		joys = Input.GetJoystickNames();
	}

	private void OnGUI()
	{


		GUI.Label(new Rect(10, 10, 1000, 1000), $"{c1LH} - {c1LV} - {c1RH} - {c1RV} - {c1LTrig} - {c1RTrig} - {c1LBump} - {c1RBump} - {c1Submmit}");

		GUI.Label(new Rect(10, 40, 1000, 1000), $"{c2LH} - {c2LV} - {c2RH} - {c2RV} - {c2LTrig} - {c2RTrig} - {c2LBump} - {c2RBump} - {c2Submmit}");

		GUI.Label(new Rect(10, 70, 1000, 1000), string.Join(", ", joys));
	}
}