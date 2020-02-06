# Unity_ECS_HelloWorld
## Unity_ECS提升游戏的运行效率

## DOTS
DATA-ORIENTED TECH STACK(多线程数据导向型技术堆栈)

##### 核心-高性能
充分利用多核处理器，多线程处理让游戏的运行速度更快，更高效。

##### 组成
* C#任务系统(Job System)，用于高效运行多线程代码
* 实体组件系统(ECS)，用于默认编写高性能代码
* Burst编译器，用于生成高度优化的本地代码

Job System和ECS是两个不同的概念，两者组合在一起才能发挥最大的优势。

## C#任务系统
* 在Job System对外之前，Unity虽然内部是多线程处理，但是外部代码必须泡在主线程上。
* C#虽然支持Thread，但是在Unity中只能处理数据，例如：网络消息、下载。如果想在Thread中调用Unity的API那是不行的。
* 有了Job System就可以充分利用CPU的多核，例如:在多线程中修改Transform旋转、缩放、平移。
* 例如:MMO游戏判断碰撞、大量的同步角色坐标、大量的血条飘字等都比较适合在Job System。
* Unity没有直接将Thread开放出来，可以有效避免Thread被滥用，开发者可放心使用Job而不用太多关心如线程安全、枷锁这些问题。
* Job最好配合Burst编译器，这样能生成高效的本地代码

## HPC#
高性能C#
* .NET Core比C++慢2倍
* Mono比.NET Core慢3倍
* IL2CPP比Mono快2-3倍，IL2CPP与.NET Core效率相当，但是依然比C++慢2倍
* Unity使用Burst编译后可以让C#代码的运行效率比C++更快

##### 介绍
* C# 引用类型数据的内存分配在堆上，程序员无法主动释放，但必须等到.NET垃圾回收才可以真正清理。
* IL2CPP虽然将IL转成C++代码，实际上还是模拟了.NET的垃圾回收机制，所以效率并非等于Cpp
* HPC#就是NativeArray<T>可代替数组T[]数据类型包括值类型(float,int,uint,short,bool...),enums,structs和其他类型的指针
* NavtiveArray可以在C#层分配C++中的对象，可以主动释放不需要等C#的垃圾回收
* Job System中使用的就是NavtiveArray

##### 验证
![](效果图/14.png)
![](效果图/15.png)
上图代码和内存图可见，创建NativeArray在堆内存上永远只有32B，而我们手动创建的数组就12.2KB的堆内存，差别还是很明显的。

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
