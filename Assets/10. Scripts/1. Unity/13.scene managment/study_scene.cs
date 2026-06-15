using JetBrains.Annotations;
using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // 씬제어를 위한 기능들이 담겨있는 네임스페이스


// 동기적 씬 로딩
// 가장 간단한 방법으로 유니티에서 가장 많이 사용됨
// 씬의 모든 요소들을 불러오며, 불러오자마자 현재 씬을 바꿔줌

public class study_scene : MonoBehaviour
{
    private Canvas canvas;

   // protected override void OnInitialize()
   // {
   //     
   //     canvas = GetComponent<Canvas>();
   // }

    private void Update()
    {
        // 탭을 눌렀을때만 canvas가 출력되게 함
        bool isPressed = SimpleInput.GetKey(Key.Tab);
        canvas.enabled = isPressed;
        if (isPressed == false) return;

        if (SimpleInput.GetKeyDown(Key.Digit1))
        {
            // 유니티 엔진에서 제공하는 scenemanager를 통해 씬 변경을 요청해볼 것임
            SceneManager.LoadScene(0);
        }
        if (SimpleInput.GetKeyDown(Key.Digit2))
        {
            SceneManager.LoadScene(1);
        }
        if (SimpleInput.GetKeyDown(Key.Digit3))
        {
            SceneManager.LoadScene(2);
        }
        if (SimpleInput.GetKeyDown(Key.Digit4))
        {
            SceneManager.LoadScene(3);
        }
        if (SimpleInput.GetKeyDown(Key.F5))
        {
            SceneManager.LoadScene(4);
        }
    }

        // 보통 씬을 사용하는 구조는 아래와 같음

        public enum GameScene
        {
           //CardSelector = 0.
           //PairMatching,
           //UFOGame,
           //MergeGame,
           //Home
        }

    private GameScene PrevScene{ get; set; }
    //private GameScene CurrentScene { get; set; } = GameScene.CardSelector;

    //public void ChangeScene(int sceneIndex)
    //{
        // 비효율적이지만 빠르게 버튼을 이용하기 위해 함수 제작
        //ChangeScene((GameScene)sceneIndex);
    //}
    //public void ChangeScene(GameScene targetScene)
    //{
    //    PrevScene = CurrentScene;
    //    CurrentScene = targetScene;
    //    SceneManager.LoadScene((int)CurrentScene);
    //}

    public void MovePreScene()
    {

    }



    }
//
