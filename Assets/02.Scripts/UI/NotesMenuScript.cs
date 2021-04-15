using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotesMenuScript : MonoBehaviour
{

    public GameObject notesContent;
    public GameObject noteBtnPrefab;
    public GameObject notesDisplay;
    public TextMeshProUGUI displayText;

    [HideInInspector] public Note[] allNotes;
    
    ResourcesManagmentScript resourcesManagmentScript;

    void Start()
    {
        resourcesManagmentScript = new ResourcesManagmentScript();
        SetEnabledNotes();
    }

    public void SetEnabledNotes()
    {
        StreamReader notesDoc = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/notes.txt");
        string numNotesStr = notesDoc.ReadLine();
        int.TryParse(numNotesStr, out int numNotes);
        allNotes = new Note[numNotes];
        for (int i = 0; i < notesContent.transform.childCount; i++) Destroy(notesContent.transform.GetChild(i).gameObject);
        notesDisplay.SetActive(false);

        for (int i = 0; i < numNotes; i++)
        {
            string noteStr = notesDoc.ReadLine();
            Note note = Note.CreateFromJSON(noteStr);
            allNotes[i] = note;
            GameObject noteBtn = Instantiate(noteBtnPrefab, notesContent.transform);
            noteBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = note.index.ToString();
            if (note.shown) noteBtn.GetComponent<Button>().onClick.AddListener(delegate { DisplayNote(note); });
            else noteBtn.GetComponent<Button>().interactable = false;
        }
        notesDoc.Close();
    }

    public void DisplayNote(Note note)
    {
        notesDisplay.SetActive(true);
        displayText.text = note.text;
    }

    public void CloseDisplayNote()
    {
        notesDisplay.SetActive(false);
    }
}
