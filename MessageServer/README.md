### 项目信息

具体示例可参照Service中Common和TransferService的实现方式<br><br>
CommonService是通用消息服务，支持自定义的消息实体对象，处理了粘包，其中消息协议如下<br>
XXXXYYYYYY<br>
其中XXXX为消息头，长度固定为4位，表示后面消息体长度。YYYYYY为消息体，消息体通过二进制方式进行消息实体对象的序列化及反序列化<br>
CommonClient是对应的客户端程序
<br><br>
TransferService是中转消息服务，对连接到服务的TCP请求中转到指定位置
<br><br>
AccessService本质也是中转服务，不过添加了对客户端IP的验证，用于对授权客户端的消息中转
<br><br>
JackChen<br>
2018-04-17
