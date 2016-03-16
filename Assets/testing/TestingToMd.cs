using StructGenerate;
using UnityEngine;

public class TestingToMd : MonoBehaviour
{
    ReaderMD myReadMD;

    void Start()
    {
        myReadMD = new ReaderMD();
        var strictTable = myReadMD.ReadMdToStructTable(Application.dataPath + "/Resources/test.md");

        Debug.Log(strictTable);
    }  




}


