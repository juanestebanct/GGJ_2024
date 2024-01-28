using UnityEngine;

public class AddRoom : MonoBehaviour
{
    //VARIABLES
    //Utility
    private RoomTemplates templates;

    void Start()
    {
        //Reference to template
        templates = RoomTemplates.instance;

        //Add room instance
        templates.AddRoom(gameObject);
    }
}
