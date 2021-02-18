# Zabbix 告警监测客户端

一个基于 Windows 桌面的 Zabbix 告警监测工具，支持连接 Zabbix 服务器并通过 JSON-RPC 接口轮询告警信息，支持声音报警和企业微信推送。

## 功能

### 核心功能
- 连接 Zabbix 服务器，通过 JSON-RPC 拉取告警数据
- 定时轮询告警（每 4 分钟），检测到新告警时播放声音并推送企业微信
- 自动通过企业微信推送告警消息到指定负责人
- 支持按主机名映射负责人，告警时通知对应人员
- 告警事件自动写入 MySQL 数据库存储
- 窗口默认置顶

### 多标签页界面

| 标签页 | 功能 |
|--------|------|
| **总览** | 事件统计（总数/已处理）、小时告警数趋势折线图、事件处理日志、实时日志 |
| **事件** | 告警事件管理，分为 4 个子页：所有事件（含刷新按钮）、未接收、处理中、已关闭 |
| **集成** | 事件接入（Zabbix / Prometheus）、告警推送（企业微信 / 钉钉） |
| **分派** | 主机-负责人映射的**增加/删除/修改/筛选**完整 CRUD 管理（MySQL 存储），新增分派调整区域 |
| **配置** | 基础运维人员、功能测试（测试声音）、其他设置（事件自动关闭/无人认领升级/夜间免打扰）、**API调整**（新增） |
| **报表** | （预留） |

### 定时器机制
- `timer1` — 每 4 分钟（240,000ms）自动轮询 Zabbix 服务器，检测新告警
- `timer2` — 每 2 小时（7,210,000ms）自动刷新企业微信 access_token

### 页面切换自动加载
- 切换到「分派」页时自动加载主机-负责人映射列表和负责人下拉选项
- 切换到「配置」页时自动显示当前默认接收人
- 切换到「事件」页时自动刷新事件列表

### 调试日志
- `Zabbix.cs` 新增 3 处日志记录：登录返回的 auth、注销结果、序列化后的请求参数，便于排查 API 调用问题
- 告警推送（`send()`）在定时轮询中默认注释，需手动启用

### 数据库分工
- **SQLite** — 存储主机名与负责人的映射关系（告警时查询负责人）
- **MySQL** — 存储告警事件（Event 模块）和管理主机-负责人映射（Info 模块的增删改查）

## 运行要求

- Windows 操作系统
- .NET Framework 4.6.1 或更高版本
- 网络连接至 Zabbix 服务器
- MySQL 数据库（用于事件存储）
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

所有敏感凭据已从源码移除，通过 `App.config` 和 `DbHelperMySQL.cs` 统一管理。**运行前必须填写真实信息**。

### 修改位置

#### 1. `202012111347/App.config`

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

#### 2. `202012111347/DbHelperMySQL.cs`

```csharp
public static string connectionString = "server=你的MySQL服务器IP;database=zabbixmonitor;uid=你的数据库用户名;pwd=你的数据库密码;CharSet=utf8;";
```

### 配置项说明

| 配置项 | 文件 | 说明 |
|--------|------|------|
| `corpid` | App.config | 企业微信 CorpID |
| `secret` | App.config | 企业微信应用 Secret |
| `gm` | App.config | 默认告警接收人（企业微信账号名） |
| `user` | App.config | Zabbix 登录用户名 |
| `passw` | App.config | Zabbix 登录密码 |
| `url` | App.config | Zabbix 服务器 IP 或域名 |
| `connectionString` | DbHelperMySQL.cs | MySQL 数据库连接字符串 |

> 也可以在程序界面中通过「Zabbix设置」和「微信设置」按钮修改部分配置，修改后会自动保存到 App.config。

## 项目结构

| 文件 | 说明 |
|------|------|
| Form1.cs | 主界面逻辑，含按钮事件、定时器、页面切换事件 |
| Form1.Designer.cs | 主界面布局，含所有控件定义（PageView/DataGridView/GroupBox 等） |
| Zabbix.cs | Zabbix JSON-RPC 客户端封装，带调试日志 |
| HttpGet.cs / HttpPost.cs | HTTP 请求工具类 |
| sqllite.cs | SQLite 数据库封装（按主机名查询负责人） |
| DbHelperMySQL.cs | MySQL 数据库操作封装（增删改查、事务、分页） |
| Event.cs / EventBLL.cs / EventDAL.cs | 告警事件实体 + 业务逻辑 + 数据访问层 |
| Info.cs / InfoBLL.cs / InfoDAL.cs | 主机-负责人映射实体 + 业务逻辑 + 数据访问层 |
| wechat.cs / wechat.Designer.cs | 企业微信配置窗口（corpid / secret） |
| zabbixuser.cs / zabbixuser.Designer.cs | Zabbix 连接配置窗口（账号 / 密码 / URL） |
| gm.cs / gm.Designer.cs | 默认接收人（负责人）管理窗口 |
| db.cs / db.Designer.cs | 数据库管理窗口（预留） |
| Request.cs / Response.cs | 请求/响应模型 |
| Log.cs | 日志记录 |
| DateTimeUtil.cs | 时间戳与 DateTime 互转工具 |
| Program.cs | 入口点 |
| App.config | 应用配置（凭据、连接字符串） |
| music/ | 告警提示音文件（WAV） |

## 🔒 安全建议

- 请勿将包含真实凭据的 `App.config` 和 `DbHelperMySQL.cs` 推送至 Git
- 推送到 Git 前确保凭据已替换为占位符
- 建议定期轮换企业微信 Secret、Zabbix 密码和 MySQL 密码