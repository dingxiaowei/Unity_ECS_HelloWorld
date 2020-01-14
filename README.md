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
