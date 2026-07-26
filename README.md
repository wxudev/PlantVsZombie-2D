# 植物大战僵尸 2D

> Unity 2D 塔防游戏，复刻《植物大战僵尸》核心玩法。

## 项目简介

从零开发的 2D 塔防游戏，实现植物防守、僵尸进攻、阳光经济三大系统闭环，包含 4 个 UI 模块的完整交互。

## 游戏截图

<!-- TODO: 补充游戏截图 -->
<!-- ![战斗画面](screenshot.png) -->

## 玩法说明

- 收集阳光，点击植物卡牌选择植物，点击空地种植
- 豌豆射手自动检测同路僵尸并射击
- 向日葵定时产出阳光
- 消灭所有僵尸即可获胜
- 僵尸走到最左侧则游戏失败

## 技术栈

Unity 2021 · C# · UGUI · DOTween · Animator · 2D Physics

## 项目结构

```
Assets/
├── Scenes/               # 场景
│   ├── Main.unity        # 主菜单
│   └── SampleScene.unity # 战斗场景
├── Scripts/              # 可补充到此目录
├── *.cs                  # 游戏脚本
│   ├── Zombie.cs         # 僵尸行为
│   ├── plant_1.cs        # 豌豆射手
│   ├── plant_0.cs      # 向日葵
│   ├── Bullet.cs         # 子弹
│   ├── ZombieSpawner.cs  # 僵尸生成器
│   ├── GameManager.cs    # 游戏流程管理
│   ├── PauseManager.cs   # 暂停控制
│   ├── Card.cs           # 卡牌状态机
│   ├── Slot.cs           # 种植格子
│   ├── Sun.cs            # 阳光收集
│   └── SunManager.cs     # 阳光掉落
├── Prefabs/              # 预制体
│   ├── Zombie.prefab
│   ├── PeaBullet.prefab
│   └── Sun.prefab
├── Resources/            # 运行时加载资源
│   ├── Plant/            # 植物预制体
│   ├── Card/             # 卡牌图片
│   └── Audio/            # 音频
├── Images/               # 图片素材
└── animation/            # 动画资源
```

## 核心系统

### 对象池

**背景**

游戏中每颗豌豆子弹都是一个 GameObject。如果不做优化，每次射击 Instantiate 一颗，命中或出屏后 Destroy，整局游戏会产生上百次 GC 触发，在密集战斗时出现画面卡顿。

**设计**

将子弹管理拆为两个池，用 Transform 父子关系维护：

```
LivePool（活跃池） → 正在飞行中的子弹
DeadPool（回收池） → 已回收、待重用的子弹
```

**获取流程**

```
需要发射子弹
    │
    ├── DeadPool 有子弹？ → 取出，SetParent(LivePool)，SetActive(true)
    │
    └── DeadPool 为空？    → Instantiate 新子弹（仅首次和极端情况）
```

**回收流程**

```
子弹命中僵尸 / 飞出屏幕
    │
    └── SetActive(false) → SetParent(DeadPool) → 等待下次取出
```

**核心代码**

子弹发射（`plant_1.cs`）：

```csharp
if (deadPool.childCount != 0)
{
    go = deadPool.GetChild(0).gameObject;   // 从回收池取
    go.transform.SetParent(livePool);
    reuseCount++;
}
else
{
    go = Instantiate(peaBulletObj);          // 池空才创建
    createCount++;
}
```

子弹回收 —— 飞出屏幕（`Bullet.cs`）：

```csharp
if (transform.position.x > 20.5f)
{
    transform.SetParent(GameObject.Find("DeadPool").transform);
    gameObject.SetActive(false);
}
```

子弹回收 —— 命中僵尸（`Zombie.cs`）：

```csharp
collision.gameObject.transform.SetParent(
    GameObject.Find("DeadPool").transform);
collision.gameObject.SetActive(false);
```

**实测数据**

正常游玩一局的 Console 输出：

<img width="994" height="326" alt="屏幕截图 2026-07-26 111106" src="https://github.com/user-attachments/assets/375b0144-0778-4e8a-a900-28b406adfc1f" />

```
子弹总数: 51
对象池复用: 43 次
Instantiate: 8 次
复用率: 84.3%
```

极端情况游玩一局的 Console 输出：
<img width="991" height="328" alt="屏幕截图 2026-07-26 110901" src="https://github.com/user-attachments/assets/82e8c14f-4136-4b6f-82d1-2cc7c4ce432b" />

```
子弹总数: 4322
对象池复用: 3549 次
Instantiate: 773 次
复用率: 82.1%
```


**效果**

- Instantiate 调用次数显著降低
- 对象池复用率 806% 以上
- 频繁 GC 导致的卡顿消除
- 复用逻辑对游戏表现完全透明，不影响玩法

### 僵尸状态机

```
行走 ──IsEat=true──→ 啃食 ──IsEat=false──→ 行走
 │                     │
 │  Hp ≤ 20%           │  Hp ≤ 20%
 ↓                     ↓
掉头行走 ──IsEat──→ 掉头啃食 ──IsEat──→ 掉头行走
```

通过 Animator 的 Hp（Float）和 IsEat（Bool）参数驱动状态切换，动画与逻辑解耦。

### 分路射击

豌豆射手每帧检测同 Y 轴僵尸（容差 < 0.5），仅对应行有敌人且僵尸在右侧时才发射，CD 2 秒。

### UGUI 多层级框架

```
Canvas_Main（战斗 HUD 层）
├── sunPoint（阳光计数）
├── Btn_Pause（暂停按钮）
├── CardSlots（卡牌栏）
└── currentPlant（选卡预览）

Canvas_Popup（弹窗层，SortingOrder 更高）
├── Panel_Pause（暂停面板）
├── Panel_Fail（失败面板）
└── WinUI（胜利面板）
```

### 卡牌状态机

```
CD（冷却中）→ 计时结束 → NoSun（阳光不足）→ 阳光够 → Ready（可点击）
                              ↑                              │
                              └──── 阳光不够 ←───────────────┘
```

## 运行方式

1. 用 Unity Hub 打开项目（Unity 2021.3+）
2. 打开 `Assets/Scenes/Main.unity`
3. 点击 Play

## 操作说明

| 操作 | 方式 |
|------|------|
| 选植物 | 点击卡牌栏中可用的植物 |
| 种植物 | 点击地图上的空格子 |
| 收集阳光 | 点击掉落的阳光 |
| 暂停 | 点击右上角暂停按钮 / 按 ESC |

## License

MIT
