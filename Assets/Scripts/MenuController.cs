using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class MenuController : MonoBehaviour
{
   public void StartBtn()
   {
      SceneManager.LoadScene("Demo");
   }

    public void SongBtn()
   {
      SceneManager.LoadScene("GangnamStyle");
   }
}
