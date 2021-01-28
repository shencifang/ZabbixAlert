# Zabbix 告警监测客户端

一个基于 Windows 桌面的 Zabbix 告警监测工具，支持连接多个 Zabbix 服务器并通过 JSON-RPC 接口轮询告警信息，支持声音报警。

## 功能

- 连接两台 Zabbix 服务器（外网 / 内网），通过 JSON-RPC 拉取告警数据
- 定时轮询告警（每 10 分钟），检测到新告警时播放声音
- 手动测试取数、测试声音
- 查看和清除告警记录
- 日志记录到文件

## 运行要求

- Windows 操作系统
- .NET Framework 4.6.1 或更高版本
- 网络连接至 Zabbix 服务器

## 如何运行

1. 打开 Visual Studio（2017/2019/2022 均可）
2. 打开解决方案文件 `202012111347.sln`
3. 按 `F5` 编译并运行

或使用已编译版本：

```
bin\Release\202012111347.exe
```

## ⚠️ 运行前必读：修改账号密码

当前源码中的 Zabbix 登录密码已脱敏为 `xxxxxxx`，**运行前必须替换为真实密码**。

### 修改位置

打开 `Form1.cs`，搜索 `Zabbix(` 找到以下三处（见下图），替换 `xxxxxxx` 为真实密码：

| 位置 | 服务器 | 用户名 | 密码 |
|------|--------|--------|------|
| 第 30 行 (button1_Click) | `https://wearm.xin` | Admin | 改为你的密码 |
| 第 60 行 (button2_Click) | `http://10.135.4.68` | mt | 改为你的密码 |
| 第 89 行 (timer1_Tick) | `http://10.135.4.68` | mt | 改为你的密码 |

### 修改示例

修改前：
```csharp
Zabbix zabbix = new Zabbix("Admin", "xxxxxxx", "https://wearm.xin/zabbix/api_jsonrpc.php");
```

修改后：
```csharp
Zabbix zabbix = new Zabbix("Admin", "你的真实密码", "https://wearm.xin/zabbix/api_jsonrpc.php");
```

> **注意**：用户名和服务器地址可根据实际情况一并修改。

## 项目结构

| 文件 | 说明 |
|------|------|
| Form1.cs | 主界面逻辑，含按钮事件和定时器 |
| Zabbix.cs | Zabbix JSON-RPC 客户端封装 |
| Request.cs / Response.cs | 请求/响应模型 |
| Log.cs | 日志记录 |
| DateTimeUtil.cs | 时间戳转换工具 |
| Program.cs | 入口点 |
| music/ | 告警提示音文件（WAV） |

## 🔒 安全建议

- 建议将密码改为外部配置方式（如 App.config），避免硬编码在源码中
- 推送至 Git 前确保密码已脱敏
