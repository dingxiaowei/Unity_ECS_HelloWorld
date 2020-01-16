# Unity_ECS_HelloWorld
## Unity_ECS提升游戏的运行效率

## 添加组件
* Entities
* Mathematics
* Hybrid Renderer
* Jobs

## 效果图对比
1.用ECS创建10000个cube移动和传统方式创建10000cube移动帧率会有明显差异
![](效果图/1.png)
![](效果图/2.png)

2.利用Job System提高CPU的运算速度
![](效果图/3.png)
![](效果图/4.png)

可以看出利用JobSystem计算速度提升了10倍左右

再利用Burst更惊人，直接变成了0.2毫秒
![](效果图/5.png)

![](效果图/6.jpg)

性能提高了3000倍

3.在一组例子
![](效果图/7.png)
![](效果图/8.png)
![](效果图/9.png)
能看出明显的帧率的变化

## 执行顺序
#### 参考文档
https://connect.unity.com/p/unity-ecs-wu-liao-jie-systemzhi-xing-shun-xu

#### 三个基本的ComponentSystemGroup
![](效果图/10.png)

* InitializationSystemGroup 负责初始化工作
* SimulationSystemGroup  负责逻辑运算工作
* PresentationSystemGroup 负责结果与图形渲染工作

#### 排序原则
* 根据其[UpdateBefore/After(typeof(MySystem))]属性。如果在进行排序时在同一组中找不到属于此属性的系统类型，则它无效，并且会向您发送警告。
* 如果没有[UpdateBefore/After]，它将会尝试分配到合适的位置，但仍确保那些具有[UpdateBefore/After]属性的System顺序不会改动。
* 如果该ComponentSystemGroup包含另一个ComponentSystemGroup，则将对其进行递归排序。
* 它可以检测到您的循环依赖关系[UpdateBefore/After]并记录信息。

#### 添加标签改变执行顺序
```
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class SequenceSystemA : ComponentSystem
{
    protected override void OnUpdate()
    {
       Debug.Log("SequenceSystemA Updating");
    }
}
```

```
[UpdateInGroup(typeof(SimulationSystemGroup))]
public class SequenceSystemB : ComponentSystem
{
    protected override void OnUpdate()
    {
        Debug.Log("SequenceSystemB Updating");
    }
}
```
![](效果图/11.png)

更改标签
```
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(SequenceSystemB))]
public class SequenceSystemA : ComponentSystem
{
    protected override void OnUpdate()
    {
       Debug.Log("SequenceSystemA Updating");
    }
}
```

```
[UpdateInGroup(typeof(SimulationSystemGroup))]
public class SequenceSystemB : ComponentSystem
{
    protected override void OnUpdate()
    {
        Debug.Log("SequenceSystemB Updating");
    }
}
```
![](效果图/12.png)
![](效果图/13.png)