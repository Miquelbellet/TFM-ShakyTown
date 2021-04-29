using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class NotesScript : MonoBehaviour
{

    public int noteIndex;
    public bool dadHelpNote;
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
        time += Time.unscaledDeltaTime;
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
        string numberLines = notesDocument.ReadLine();
        int.TryParse(numberLines, out int numLines);
        for (int i = 0; i < numLines; i++)
        {
            string noteStr = notesDocument.ReadLine();
            Note newNote = Note.CreateFromJSON(noteStr);
            if (newNote.index == noteIndex) note = newNote;
        }
        notesDocument.Close();

        if (!note.shown) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }

    void SaveReadedNote()
    {
        StreamWriter noteDocument = resourcesManagmentScript.WriteDataToResource("Assets/Resources/notes.txt");
        Note[] notesList = gameController.GetComponent<NotesMenuScript>().allNotes;
        noteDocument.WriteLine(notesList.Length);
        foreach (Note noteObj in notesList)
        {
            if (noteObj.index == noteIndex) noteObj.shown = true;
            string noteObjStr = Note.CreateFromObject(noteObj);
            noteDocument.WriteLine(noteObjStr);
        }
        noteDocument.Close();
        AssetDatabase.Refresh();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (gameController.GetComponent<UIControllerScript>().ShowDialogText(note.text))
            {
                if (dadHelpNote)
                {
                    PlayerPrefs.SetString("dadHelpNote", "true");
                    gameController.GetComponent<LevelControllerScript>().openLevel4Obstacle = true;
                }
                SaveReadedNote();
                Destroy(gameObject);
            }
        }
    }
}
