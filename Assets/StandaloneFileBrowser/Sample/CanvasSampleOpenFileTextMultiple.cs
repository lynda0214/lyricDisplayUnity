using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;

[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileTextMultiple : MonoBehaviour, IPointerDownHandler {
    public Text output;
    public GameObject itemList;
    public GameObject itemPrefab;

    public Text songCount;

    private int songCountInt;

    public GameManager gameManager;

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".txt", true);
    }

    // Called from browser
    public void OnFileUpload(string urls) {
        StartCoroutine(OutputRoutine(urls.Split(',')));
    }
#else
    //
    // Standalone platforms & editor
    //
    public void OnPointerDown(PointerEventData eventData) { }

    void Start() {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        // var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", true);
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", true);
        
        if (paths.Length > 0) {
            var urlArr = new List<string>(paths.Length);
            for (int i = 0; i < paths.Length; i++) {
                urlArr.Add(new System.Uri(paths[i]).AbsoluteUri);
            }

            if (songCountInt > 0) { // clear b4 add
                GameObject[] oldListItems = GameObject.FindGameObjectsWithTag("ListItem");
                for (int i = 0; i < oldListItems.Length; i++) {
                    Destroy(oldListItems[i]);
                }
            }

            string[] urlStrArr = urlArr.ToArray();
            songCountInt = urlStrArr.Length;
            songCount.text = urlStrArr.Length + " 首歌";

            gameManager.ParseTxt2Array(urlStrArr);

            for (int i = 0; i < urlStrArr.Length; i++) {
                Debug.Log(urlStrArr[i]);
                
                string[] splitted = urlStrArr[i].Split('/');
                
                GameObject newListItem = Instantiate(itemPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
                newListItem.transform.SetParent(itemList.transform, false);
                newListItem.transform.GetChild(0).GetComponent<Text>().text = System.Uri.UnescapeDataString(splitted[splitted.Length-1]);
            }
            //StartCoroutine(OutputRoutine(urlArr.ToArray()));
        }
        

    }
#endif

    private IEnumerator OutputRoutine(string[] urlArr) {
        var outputText = "";
        for (int i = 0; i < urlArr.Length; i++) {
            var loader = new WWW(urlArr[i]);
            yield return loader;
            outputText += loader.text;
        }
        output.text = outputText;
    }
}