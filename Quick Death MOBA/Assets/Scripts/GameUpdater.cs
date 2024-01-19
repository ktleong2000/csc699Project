using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUpdater : MonoBehaviour
{
    public static GameObject SelectedObject;
    public static GameObject HighlightedObject;

    private static Camera cam;
    private static QDMGameManager gameManager;

    //Variables for player movement
    private float horizontal = 0f;
    private float vertical = 0f;
	Transform torsoTransform;

    // Track if an object is already selected
    private bool objectSelected = false;

    // Store the selected object when it's first clicked
    private GameObject initialSelectedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameManager = GameObject.Find("Game Manager").GetComponent<QDMGameManager>();
    }

	// Recursive method to find a transform by name
    Transform FindDeepChild(Transform parent, string name)
    {
        // Check if the current parent matches the name
        if (parent.name == name)
            return parent;

        // Recursively search each child
        foreach (Transform child in parent)
        {
            Transform result = FindDeepChild(child, name);
            if (result != null)
                return result;
        }

        // Return null if no match is found
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        // Call the game manager's functions like process move to ask them to process the game's logic
        // If we check that the user has pressed on the a movement key like WASD then we will call
        // process move.
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (Input.GetMouseButtonDown(0))
		{
			// Left mouse button clicked
			if (hit.collider != null)
			{
				// An object is clicked
				initialSelectedObject = hit.transform.gameObject;
				objectSelected = true;
				Debug.Log("Object selected: " + initialSelectedObject.name);

				// Use the recursive method to find the "Ribs"
                torsoTransform = FindDeepChild(initialSelectedObject.transform, "Ribs");

                if (torsoTransform == null)
                {
                    Debug.LogError("Torso (Ribs) not found in the selected object");
                }
			}
			else
			{
				// Clicked in empty space, deselect any previously selected object
				initialSelectedObject = null;
				objectSelected = false;
				SelectedObject = null;
				torsoTransform = null; // Reset torsoTransform when nothing is selected
			}
		}

        //@@@@@@@@@@@@@@@@@Movement section@@@@@@@@@@@@@@@@@@@@@@@@@//
        //This section handles the updating of the characters movement
        if (objectSelected && initialSelectedObject != null)
        // if(true)
        {
            // Object is selected, so we can move it
            horizontal = 0f;
            vertical = 0f;

            if (Input.GetKey(KeyCode.D))
            {
                horizontal += 10f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                horizontal -= 10f;
            }
            if (Input.GetKey(KeyCode.W))
            {
                vertical += 10f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                vertical -= 10f;
            }

            // Move the selected object
            gameManager.ProcessMove(initialSelectedObject, horizontal, vertical);
        }

        //@@@@@@@@@@@@@@@@@Aiming section@@@@@@@@@@@@@@@@@@@@@@@@@//
        //This section handles the updating of the characters aiming
		if (objectSelected && initialSelectedObject != null)
        {
			// Check if torsoTransform is not null
			if (torsoTransform != null)
			{
				Debug.Log("Attempting to aim");
				Vector2 aimScreenPosition = Input.mousePosition;
				Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(aimScreenPosition.x, aimScreenPosition.y, torsoTransform.position.z));

				Vector2 directionToAim = aimWorldPosition - (Vector2)torsoTransform.position;
				float angle = Mathf.Atan2(directionToAim.y, directionToAim.x);
				float angleDegrees = angle * Mathf.Rad2Deg - 90;

				// Update the rotation on the server
				gameManager.ProcessAiming(torsoTransform, angleDegrees);
			}
			else
			{
				// You can add some handling here if torsoTransform is null
				// For example, logging or some default behavior
				Debug.LogError("Torso (Ribs) not found in the selected object for aiming");
			}
		}

        //@@@@@@@@@@@@@@@@@Health System Test section@@@@@@@@@@@@@@@@@@@@@@@@@//
        //This section handles the testing of the health system for my characters
        if (objectSelected && initialSelectedObject != null)
        {
			// Check if torsoTransform is not null
			if (torsoTransform != null)
			{
				Debug.Log("Attempting to to do or heal health points");
                if (Input.GetKeyDown(KeyCode.O))
                {
                    //Add health to the selected player
                    gameManager.ProcessHealth(initialSelectedObject, 0);
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    //Subtract health to the selected player
                    gameManager.ProcessHealth(initialSelectedObject, 1);
                }
			}
			else
			{
				// You can add some handling here if torsoTransform is null
				// For example, logging or some default behavior
				Debug.LogError("Torso (Ribs) not found in the selected object for aiming");
			}
		}
    }
}
