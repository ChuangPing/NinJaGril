using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WineTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            string levelName = SceneManager.GetActiveScene().name; // 获取当前当前的名字：level+number
            string temp = levelName.Substring(5); // 获取当前场景的数字编号
            int levelNumber = int.Parse(temp); // 转换为数字
            // 设置通过的关卡数据
            int clearedLevel = PlayerPrefs.GetInt("clearLevel");
            if (levelNumber > clearedLevel)
            {
                // 当前通过的关卡比以前通过的关卡大时，才会继续存储新的clearLevel,否则不进行赋值（玩家再玩以前通过的关卡）
                PlayerPrefs.SetInt("clearLevel", levelNumber); // 其它脚本可以取
            }
            //SceneManager.LoadScene("LevelSelect"); // 当player触碰到通关的物体，开始进入到关卡选择场景
            Time.timeScale = 0; // 通过时有暂停游戏的效果
            FadeInOut.instance.SceneFadeInOut("LevelSelect"); // 带动画的切换场景
        }
    }
}
