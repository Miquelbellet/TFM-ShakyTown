using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NotesScript : MonoBehaviour
{

    public int noteIndex;
    public float floatingTimer;
    public float floatingAcceleration;

    ResourcesManagmentScript resourcesManagmentScript;
    GameObject gameController;

    private Note note;
    private float time;

    void Start()
    {
        resourcesManagmentScript = new ResourcesManagmentScript();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        SetNoteItem();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time < floatingTimer / 2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - floatingAcceleration);
        }
        else if (time < floatingTimer)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + floatingAcceleration);
        }
        else
        {
            time = 0;
        }
    }

    void SetNoteItem()
    {
        StreamReader notesDocument = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/notes.txt");
        for (int i = 0; i <= noteIndex; i++)
        {
            if (noteIndex == i)
            {
                string noteStr = notesDocument.ReadLine();
                note = Note.CreateFromJSON(noteStr);
            }
        }
        notesDocument.Close();
    }

    void SaveReadedNote()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameController.GetComponent<UIControllerScript>().ShowDialogText(note.text);
            SaveReadedNote();
            Destroy(gameObject);
        }
    }
}
