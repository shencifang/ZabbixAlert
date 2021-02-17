# Zabbix 告警监测客户端

一个基于 Windows 桌面的 Zabbix 告警监测工具，支持连接 Zabbix 服务器并通过 JSON-RPC 接口轮询告警信息，支持声音报警和企业微信推送。

## 功能

- 连接 Zabbix 服务器，通过 JSON-RPC 拉取告警数据
- 定时轮询告警（每 4 分钟），检测到新告警时播放声音
- 自动通过企业微信推送告警消息到指定负责人
- 支持按主机名映射负责人，告警时通知对应人员
- SQLite 本地存储告警历史
- 可视化图表统计
- Telerik RadPageView 多标签页界面
- 手动测试取数、测试声音、测试推送
- 查看和清除告警记录
- 日志记录到文件
- 窗口默认置顶

## 运行要求

- Windows 操作系统
- .NET Framework 4.6.1 或更高版本
- 网络连接至 Zabbix 服务器
- 企业微信（如需推送功能）

## 如何运行

1. 打开 Visual Studio（2017/2019/2022 均可）
2. 打开解决方案文件 `202012111347.sln`
3. 按 `F5` 编译并运行

或使用已编译版本：

```
bin\Release\202012111347.exe
```

## ⚠️ 运行前必读：修改配置

所有敏感凭据已从源码移除，通过 `App.config` 统一管理。**运行前必须填写真实信息**。

### 修改位置

打开 `202012111347/App.config`，找到 `<appSettings>` 节点：

```xml
<appSettings>
  <add key="corpid" value="你的企业微信CorpID"/>
  <add key="secret" value="你的企业微信Secret"/>
  <add key="gm" value="默认接收人(如:张三)"/>
  <add key="user" value="Zabbix用户名(如:Admin)"/>
  <add key="passw" value="Zabbix密码"/>
  <add key="url" value="Zabbix服务器地址(IP或域名)"/>
</appSettings>
```

### 配置项说明

| 配置项 | 说明 |
|--------|------|
| `corpid` | 企业微信 CorpID |
| `secret` | 企业微信应用 Secret |
| `gm` | 默认告警接收人（企业微信账号名） |
| `user` | Zabbix 登录用户名 |
| `passw` | Zabbix 登录密码 |
| `url` | Zabbix 服务器 IP 或域名（不含 http:// 和路径） |

> 也可以在程序界面中通过「Zabbix设置」和「微信设置」按钮修改配置，修改后会自动保存到 App.config。

## 项目结构

| 文件 | 说明 |
|------|------|
| Form1.cs | 主界面逻辑，含按钮事件和定时器 |
| Zabbix.cs | Zabbix JSON-RPC 客户端封装 |
| HttpGet.cs / HttpPost.cs | HTTP 请求工具类 |
| sqllite.cs | SQLite 数据库封装（告警历史、负责人映射） |
| db.cs | 数据库管理窗口 |
| gm.cs | 负责人管理窗口 |
| wechat.cs | 企业微信配置窗口 |
| zabbixuser.cs | Zabbix 连接配置窗口 |
| Request.cs / Response.cs | 请求/响应模型 |
| Log.cs | 日志记录 |
| DateTimeUtil.cs | 时间戳转换工具 |
| Program.cs | 入口点 |
| App.config | 应用配置（凭据、连接字符串） |
| music/ | 告警提示音文件（WAV） |

## 🔒 安全建议

- 请勿将包含真实凭据的 `App.config` 推送至 Git
- 推送到 Git 前确保凭据已替换为占位符
- 建议定期轮换企业微信 Secret 和 Zabbix 密码