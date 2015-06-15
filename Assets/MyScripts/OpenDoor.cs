using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
	public GameObject door;
	public bool bigDoor;
	private bool doorFound;
	private bool doorState;

	void Start(){
		closeTheDoor ();
		doorState = true;
	}

	void  OnTriggerEnter ( Collider other  )
	{
		if (other.gameObject.tag == "Player")
		{
			doorFound=true;
			openTheDoor();
		}
	}
	
	
	void  OnTriggerExit ( Collider other  )
	{
		if (other.gameObject.tag == "Player")
		{
			closeTheDoor();
			doorFound=false;
		}
	}

	void OnGUI(){

		/*if(doorFound)
		{
			GUI.Label(new Rect(50,50,200,100),"Toogle Door");
			
			if(Input.GetKeyUp(KeyCode.T))
			{
				if(!doorState){
					Debug.Log("Gonna open");
					openTheDoor();
					doorState = false;
				}
				else 
				{
					Debug.Log("Gonna Close");
					closeTheDoor();
					doorState = true;
				}
			}
		}*/

	}

	void openTheDoor(){
		if(!bigDoor) door.transform.RotateAround(this.transform.position, Vector3.up, 90);
		else door.transform.RotateAround(this.transform.position, Vector3.up, -90);
	}

	void closeTheDoor(){
		if(!bigDoor) door.transform.RotateAround(this.transform.position, Vector3.up, -90);
		else door.transform.RotateAround(this.transform.position, Vector3.up, 90);
	}

}