# Lottery
##  C#抽奖程序，主要用于公司年会时的抽奖活动，共分两个版本(1.文字版 2.[头像版](https://github.com/weishaoying/Lottery2))这里是文字版，具有如下功能：<br/>
开发环境：VS2010 +.Net Framework4.0<br/><br/>
1. 可自定义人员，初始化奖池<br/>
2. 验证抽奖名单是否有重复，防止有人重复添加以提高中奖概率
3. 奖金等级分为特等奖、一等奖、二等奖、三等奖、纪念奖等<br/>
4. 点击抽奖后名单快速循环、双方向滚动<br/>
5. 可以调整滚动速度<br/>
<br/>
<br/>

效果如下：
<br/>
![image](https://github.com/weishaoying/Lottery/blob/master/Pictrues/P1.png)
<br/><br/>
动态演示效果：
<br/>
![image](https://github.com/weishaoying/Lottery/blob/master/Pictrues/demo.gif)
<br/>
<br/>

### 使用方法：<br/>
下载后在bin\Release目录下有Lottery.exe可执行文件和人员名单.txt
<br/>
在人员名单.txt中增加人员姓名，每个姓名占一行
<br/><br/>

2020.1.9
使用方法：1、启动之后，点“初始化奖池”，会提示有多少人；2、然后“重名验证”，有重名要关闭程序，修改名单之后再打开；3、选择一等奖、二等奖、列表； 4、抽奖，停止； 5、可以改滚动速度，人多调就快一点，人少就调慢一点
问题：1、没有记录中奖者，可以添加日志保存； 2、当人数少于6个人之后就不允许抽奖了，所以如果人人都要中奖的话不行



