using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    // 转换场景动画（一张图片fadeIn和fadeOut)
    public static FadeInOut instance;  // 当前类的实例

    public GameObject FadeInOutImage; // 转场动画图片
    public Animator myAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // 切换场景时，不删除当前对象
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);// 销毁自己
        }
    }

    // 切换场景函数 ,levelName:需要转换的场景名称
    public void SceneFadeInOut(string levelName)
    {
        StartCoroutine(setSceneFadeInOut(levelName));
    }

    IEnumerator setSceneFadeInOut(string levelName)
    {
        // 激活当前图片gameObject：一激活就会自动执行Fadein动画（总共1.5s秒）
        FadeInOutImage.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);  // 等待1.5s进场动画执行完
        SceneManager.LoadScene(levelName); // 加载需要转换的场景
        myAnim.Play("FadeOut"); // 播放退场动画（由黑到透明，1s时间）

        yield return new WaitForSecondsRealtime(1.0f); // 等待动画播完
        FadeInOutImage.SetActive(false);
        Time.timeScale = 1.0f; // 防止点击暂停按钮（设置为0）后进行切换场景，因此必须设置为1.0f
    }
}
