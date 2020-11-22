# MessageServer
#### MessageServer - 基于IOCP的消息服务器（可自定义扩展消息服务）<br><br>![](https://github.com/csa/MessageServer/blob/master/Img/MS.png)<br>
IO测试<br><br>
![](https://github.com/csa/MessageServer/blob/master/Img/Flow.png)<br><br>
### 项目信息

具体示例可参照Service中Common和TransferService的实现方式<br><br>
CommonService是通用消息服务，支持自定义的消息实体对象，处理了粘包，其中消息协议如下<br>
```
 Header   Body
+--------+----------+
| Length | Content  |
+--------+----------+
```
<br>
其中Header为消息头，长度固定为4位，表示后面消息体长度。Body为消息体，消息体通过二进制方式进行消息实体对象的序列化及反序列化<br><br>
CommonClient是对应的客户端程序
<br><br>
TransferService是中转消息服务，对连接到服务的TCP请求中转到指定位置
<br><br>
AccessService本质也是中转服务，不过添加了对客户端IP的验证，用于对授权客户端的消息中转以及添加了授权过程防止抓包的流程
<br><br>
AccessClient是对应的客户端授权程序，用于和服务端进行授权验证<br><br>
流程如下：<br>
1.服务启动，清空之前授权客户端列表，读取Key，等待客户端连接<br>
2.客户端连接后，服务端生成一个GUID，使用Key对GUID进行AES加密，将密文发给客户端，记录GUID并与该客户端进行绑定<br>
3.客户端接收密文，使用自己的Key对密文进行解密后将明文发送给服务端<br>
4.服务端将收到的明文与之前记录的GUID进行比对，一致则将该IP添加到信任列表，否则断开连接<br>
5.中转服务与TransferService类似，只是在客户端连接时判断了该IP是否在信任列表中<br>
在此流程下解决了两个问题：<br>
1.仅对授权客户端进行消息中转<br>
2.防止了授权过程中Key信息泄露<br>
<br>
自定义服务面板请参照另一个项目QueueSystem中MessageService相关内容<br>
效果如下<br>
![](https://github.com/csa/MessageServer/blob/master/Img/MS2.png)
<br>
FlowViewer是流量监控程序<br>
效果如下(图中为IO测试效果)<br>
![](https://github.com/csa/MessageServer/blob/master/Img/Flow.png)
<br>

JackChen<br>
2018-02-05
