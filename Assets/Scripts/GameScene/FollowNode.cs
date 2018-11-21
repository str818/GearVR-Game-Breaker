using UnityEngine;
using System.Collections;

public class FollowNode : MonoBehaviour {
	
    public int index;                   				// 当前目标结点序号
	public float[] m_speed = new float[5];				// 存放移动速度数组
	public GameObject[] nodes = new GameObject[5];		// 存放结点数组
    public GameObject camera;                         // 摄像机引用
    public GameObject guankaPanel;                     // 关卡面板
    private GameObject target_node;    		 		// 目标结点
    private bool isShow = false;                      //是否显示过关卡面板

    

    void Start() {

        if(UIdata.targetScene==1)
        {
            camera.transform.position = new Vector3(-95f,2.0f,1.76f);//移动摄像机位置
            transform.position = new Vector3(-94f, 2.0f, 1.76f);//移动小球位置
            index = 0; //目标节点序号
            target_node = nodes[index];//目标节点
        }else if(UIdata.targetScene==2)
        {
            camera.transform.position = new Vector3(1130f, 5.17f, 1.76f);//移动摄像机位置
            transform.position = new Vector3(1130f, 5.17f, 1.76f);//移动小球位置
            index = 5;//目标节点序号
            target_node = nodes[index];//目标节点
        }
    }

    void Update() {
		if (!UIdata.isDead) {
			RotateTo();             						// 转向下一个结点
			MoveTo();               						// 朝向下一个结点移动
		}

        if(!isShow&&index==4) //没有显示过并且当进入第二关时激活关卡面板
        {
            isShow = true;
            guankaPanel.SetActive(true);                   //激活关卡2面板
        }
    }

	//旋转方法
	public void RotateTo() {
        this.transform.LookAt(target_node.transform);	// 摄像机看向下一目标节点
    }

	//移动方法
	public void MoveTo() {
        Vector3 pos1 = this.transform.position;         // 当前目标结点位置
        Vector3 pos2 = target_node.transform.position;  // 小球自身位置
        float distance = Vector3.Distance(pos1, pos2);  // 计算两者距离
        if (distance < 1.0f) {                  		// 距离小于一定阈值
            index++;                            		// 序号加1
            if (index < nodes.Length) {         		// 序号不超过数组长度
                if (nodes[index] != null) {     		// 结点不为空
                    target_node = nodes[index]; 		// 更新目标结点
                }
            }
        }
        if (index >= nodes.Length)                        //到达关卡尽头
        {
            camera.GetComponent<AudioExchange>().victory();//切换胜利音效
            camera.GetComponent<GameOver>().gameWin();     //游戏胜利（弹出选项面板）
            this.enabled = false;//禁用当前脚本
            return;
        }
		this.transform.Translate(new Vector3(0, 0, m_speed[index] * Time.deltaTime));		//向目标结点移动
    }
}