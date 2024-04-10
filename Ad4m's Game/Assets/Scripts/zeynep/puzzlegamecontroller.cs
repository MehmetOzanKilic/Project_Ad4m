using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class puzzlegamecontroller : MonoBehaviour
{
    //mousela parca seccez 
    //mouse, tagi "puzzlepiece" olan bir parcanin uzerine geldiginde parca koyu mavi rengini alsin
    
    //secilen parca defaulltan ortadaki slota yerlescek (doluysa en yakin bos slot)
    //wasd ile dogru yeri secicez
    //space basinca parcayi yerlestircez
    //(belki) e ile 90 derece rotate ettir
    //space e tekrar basica parcayi koy

    public List<GameObject> puzzleSlots = new List<GameObject>();
    public Material puzzleSelectedMat;

    public CinemachineVirtualCamera FarCam;
    public CinemachineVirtualCamera NearCam;
    
    // Start is called before the first frame update
    void Start()
    {
        //puzzleSlots[4].transform.GetChild(0).GetComponent<puzzlepiececontroller>().isSelected = true;
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
