# MessageServer
#### 这个项目是干什么的？
这个项目是之前做工控和多终端系统过程中，针对消息处理的一套可扩展框架</br>
简单一点说，如果处理TCP/UDP/Http/WebSocket都从创建Socket，Bind，Listen开始的话，费时费力，不仅性能差还容易出错，有没有一种方式可以用框架来完成基础操作（连接/断开/收发过程），而应用只关注事件和数据呢？</br>没错，就是这个项目啦
#### 特性
- **高性能**：项目参考[微软官方IOCP示例](https://docs.microsoft.com/zh-cn/dotnet/api/system.net.sockets.socketasynceventargs?redirectedfrom=MSDN&view=netframework-4.0)进行实现，性能表现见测试内容
- **稳定性**：在多种场景以及实际项目进行验证，除了基础的大数据量以及大批量客户端连接断开测试，还做了很多边缘性的测试，包括在大批量客户端断开的同时进行数据收发，网络环境不佳时的操作等等
- **低资源占用**：最大化复用Socket异步事件对象，避免在大容量异步套接字 I/O 期间重复分配和同步对象，减少程序的资源开销
- **简单易用**：简化各种配置和概念，提供相关的查看及操作界面，并支持自定义
- **跨平台**：支持.NETCore + GTK方式，使用[.NETCore分支](https://github.com/JackQChen/MessageServer/tree/NETCore)，可跨平台使用
#### 测试（对比目前部分框架）
||MessageServer|xxxx|xxxxx|
|:-|:-|:-|:-|
|10W次连接|||
|1G数据耗时|||
|平均发送速度|||
|平均接收速度|||
|1K会话发送同时断开|||
|1K客户端发送同时断开|||

#### 截图
![](https://github.com/JackQChen/MessageServer/blob/master/img/MS.png)</br></br>
IO本地测试</br>
![](https://github.com/JackQChen/MessageServer/blob/master/img/Flow.png)</br></br>
### 示例

具体示例可参照examples中CommonService和ForwardingService的实现方式</br></br>
CommonService是通用消息服务，支持自定义的消息实体对象，处理了粘包，其中消息协议如下
```
 Header   Body
+--------+----------+
| Length | Content  |
+--------+----------+
```
其中Header为消息头，长度固定为4位，表示后面消息体长度。Body为消息体，消息体通过二进制方式进行消息实体对象的序列化及反序列化</br></br>
CommonClient是对应的客户端程序
</br></br>
ForwardingService是转发消息服务，对连接到服务的TCP请求中转到指定位置
</br></br>
自定义服务面板请参照另一个项目QueueSystem中MessageService相关内容</br></br>
效果如下</br>
![](https://github.com/JackQChen/MessageServer/blob/master/img/MS2.png)</br></br>
FlowViewer是流量监控程序</br></br>
效果如下(本地IO测试)</br>
![](https://github.com/JackQChen/MessageServer/blob/master/img/Flow.png)</br></br>
JackChen</br>
2018-02-05
