using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private List<List<string>> lines = new List<List<string>>();
    private int songCount;
    private int songIndex; 
    private int lyricIndex;

    public GameObject blackScreen;

    public GameObject firstLineGO, secondLineGO;

    public Text firstLine, secondLine;
    // Start is called before the first frame update

    public Button playBtn;

    public Image progressBar;

    private int totalLines = 0;

    private bool subtitleVisible;

    public GameObject instructionsGO;

    void Start()
    {
        songIndex = 0;
        lyricIndex = 0;
        songCount = 0;
        subtitleVisible = true;
        playBtn.onClick.AddListener(RevealBlackScreen);
        playBtn.interactable = false;
        progressBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (songCount > 0) {
            playBtn.interactable = true;
            // Input
            if (Input.GetKeyDown(KeyCode.LeftArrow)){ // last line
                if (lyricIndex > 0)
                    lyricIndex--;
                UpdateText();
            } 
            if (Input.GetKeyDown(KeyCode.RightArrow)) { // next line
                if (lyricIndex < lines[songIndex].Count)
                    lyricIndex++;
                UpdateText();
                instructionsGO.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { // last song
                if (songIndex > 0) {
                    songIndex--;
                    lyricIndex = 0;
                    UpdateText();
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { // next song
                if (songIndex < songCount-1) {
                    songIndex++;
                    lyricIndex = 0;
                    UpdateText();
                }
            } 
            if (Input.GetKeyDown(KeyCode.Space)) { // black screen
                subtitleVisible = !subtitleVisible;
                UpdateText();
            } 
            if (Input.GetKeyDown(KeyCode.Escape)) {
                CloseBlackScreen();
            }

            int currentLines = 0;
            for (int i = 0; i < songIndex; i++) {
                currentLines += lines[i].Count;
            }
            currentLines += lyricIndex;

            progressBar.fillAmount = currentLines*1.0f/totalLines;

        }
    }

    public void ParseTxt2Array(string[] urlStrArr) {
        songCount = urlStrArr.Length;
        for (int i = 0; i < songCount; i++) {
            var loader = new WWW(urlStrArr[i]);
            string[] loaderSplitted = loader.text.ToString().Split('\n');
            lines.Add(new List<string>(loaderSplitted));
            // for (int j = 0; j < loaderSplitted.Length; j++) {
            //     Debug.Log(loaderSplitted[j]);
            // }
        }

        totalLines = 0;
        // print out all strings
        for (int i = 0; i < lines.Count; i++) {
            for (int j =0; j < lines[i].Count; j++) {
                Debug.Log(lines[i][j]);
                totalLines++;
            }
        }
    }

    private void UpdateText () {
        string[] enzh = lines[songIndex][lyricIndex].Split('/');
        if (enzh.Length == 2) {
            firstLine.text = enzh[0];
            secondLine.text = enzh[1];
        } else {
            firstLine.text = "";
            secondLine.text = "";
        }
        
        firstLineGO.SetActive(subtitleVisible);
        secondLineGO.SetActive(subtitleVisible);
    }

    public void RevealBlackScreen () {
        blackScreen.GetComponent<Animator>().SetBool("IsOn", true);
    }

    public void CloseBlackScreen () {
        blackScreen.GetComponent<Animator>().SetBool("IsOn", false);
    }

    public void OpenURL (string inputURL) {
        Application.OpenURL(inputURL);
    }

}
