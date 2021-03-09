# Zabbix 告警监测客户端

主要解决 Zabbix 在企业内网中无法发送通知的问题。通过轮询内网 Zabbix 服务器的告警数据，经企业微信推送到运维人员手机，实现告警实时通知。

一个基于 Windows 桌面的 Zabbix 告警监测工具，支持连接 Zabbix 服务器并通过 JSON-RPC 接口轮询告警信息，支持声音报警和企业微信推送。

## 功能

### 核心功能
- 连接 Zabbix 服务器，通过 JSON-RPC 拉取告警数据
- **程序启动时立即登录 Zabbix 并保持长连接**（Zabbix 实例全局化，不再每次取数重新登录）
- 定时轮询告警（每 4 分钟），检测到新告警时播放声音并推送企业微信
- **告警去重**：以 triggerid 为唯一标识，已存在且未关闭的告警自动忽略
- **告警消息含关闭链接**：推送消息中附带 `<a>` 链接，可直接点击关闭事件
- 自动通过企业微信推送告警消息到指定负责人
- 支持按主机名映射负责人，告警时通知对应人员
- 告警事件自动写入 MySQL 数据库存储
- **事件面板动态加载**：从数据库读取近 24 小时事件，区分"正在处理"和"已关闭"状态
- **窗口默认置顶已注释**

### 多标签页界面

| 标签页 | 功能 |
|--------|------|
| **总览** | 事件统计（总数/已关闭数）、小时告警数趋势折线图、事件处理日志、实时日志 |
| **事件** | 告警事件管理，分为 4 个子页：所有事件（含刷新按钮）、未接收、处理中、已关闭 |
| **集成** | 事件接入（Zabbix / Prometheus）、告警推送（企业微信 / 钉钉） |
| **分派** | 主机-负责人映射的**增加/删除/修改/筛选**完整 CRUD 管理（MySQL 存储） |
| **配置** | 基础运维人员管理、功能测试（测试推送/声音）、其他设置（事件自动关闭/无人认领升级/夜间免打扰） |
| **报表** | （预留） |

### 定时器机制
- `timer1` — 每 4 分钟自动轮询 Zabbix 服务器，检测新告警（增加去重逻辑 + 恢复通知发送）
- `timer2` — 每 2 小时自动刷新企业微信 access_token

### 页面切换自动加载
- 切换到「分派」页时自动加载主机-负责人映射列表和负责人下拉选项
- 切换到「配置」页时自动显示当前默认接收人
- 切换到「事件」页时自动刷新事件列表

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

如需已编译版本，请在 Visual Studio 中按 `F5` 编译，输出路径为 `bin\Release\202012111347.exe`

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

### 通知渠道配置

`Form1.cs` 中 `send()` 方法内的负责人列表和API地址，运行时请替换为实际信息：
- 特殊负责人列表（如：负责人1、负责人2……）
- 默认负责人/备用负责人手机号
- 关闭事件API地址

## 项目结构

| 文件 | 说明 |
|------|------|
| Form1.cs | 主界面逻辑，含按钮事件、定时器、页面切换事件 |
| Form1.Designer.cs | 主界面布局，含所有控件定义（PageView/DataGridView/GroupBox 等） |
| Zabbix.cs | Zabbix JSON-RPC 客户端封装 |
| HttpGet.cs / HttpPost.cs | HTTP 请求工具类 |
| sqllite.cs | SQLite 数据库封装（按主机名查询负责人） |
| DbHelperMySQL.cs | MySQL 数据库操作封装（增删改查、事务、分页） |
| Event.cs | 告警事件实体（id/time/ip/content/gm/recetime/close/closetime） |
| EventBLL.cs | 告警事件业务逻辑层 |
| EventDAL.cs | 告警事件数据访问层（MySQL，新增日志记录SQL功能） |
| Info.cs | 主机-负责人映射实体（hostname/gm） |
| InfoBLL.cs | 主机-负责人映射业务逻辑层 |
| InfoDAL.cs | 主机-负责人映射数据访问层（MySQL） |
| db.cs | 数据库管理窗口 |
| gm.cs | 负责人管理窗口 |
| wechat.cs | 企业微信配置窗口 |
| zabbixuser.cs | Zabbix 连接配置窗口 |
| Request.cs / Response.cs | 请求/响应模型 |
| Log.cs | 日志记录 |
| DateTimeUtil.cs | 时间戳与 DateTime 互转工具 |
| Program.cs | 入口点 |
| App.config | 应用配置（凭据、连接字符串） |
| music/ | 告警提示音文件（WAV） |

## 变更说明

### 2026-05-28 版本更新
- **Zabbix 登录全局化**：`Zabbix` 实例从局部变量提升为类成员变量，程序启动时登录，保持长连接
- **告警去重**：以 triggerid 为唯一标识，已存在未关闭的告警自动忽略，避免重复通知
- **恢复企业微信通知**：`send()` 调用从注释状态恢复，新告警自动推送
- **告警消息含关闭链接**：推送消息包含可点击的关闭事件链接
- **事件面板动态化**：`fill()` 方法从硬编码样板文字改为读取数据库真实事件数据
- **增加 `System.Web` 引用**：用于 `HttpUtility.UrlEncode` 编码告警描述
- **调试日志增强**：`Zabbix.cs` 记录完整请求/响应 JSON；`EventDAL.cs` 记录生成 SQL
- **默认页调整为总览**：启动后默认显示「总览」标签页（原为「配置」）
- **Test按钮增强**：`button2` 点击时附带关闭事件链接测试参数
- **图表调整**：折线图移除第一个硬编码数据点，启用 `IsXValueIndexed` 以适应动态数据
- **界面微调**：`textBox2`（事件日志）滚动方向改为垂直，启用 `WordWrap=false` 避免换行混乱
- **窗口默认置顶移除**：`TopMost = true` 已注释，窗口启动后不再默认置顶，可由用户通过页面按钮自行控制

## 🔒 安全建议

- 请勿将包含真实凭据的 `App.config` 和 `DbHelperMySQL.cs` 推送至 Git
- 推送到 Git 前确保凭据已替换为占位符
- 建议定期轮换企业微信 Secret、Zabbix 密码和 MySQL 密码
