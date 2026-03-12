# 🎮 Zuma Mini Game
A simple classic Zuma-style puzzle prototype developed with Unity 2D.

## 🚀 Features
- **Waypoint-Based Movement:** Smooth ball movement using lists of Transform.
- **Dynamic List-Based Insertion:** Logic using list for ball insertions, allowing for seamless integration of new projectiles into the existing ball sequence
- **Precision Aiming:** Integrated `LineRenderer` for trajectory guidance.
- **Match-3 Mechanics:** Efficient detection system for chain reactions using lists.

## 🛠 Tech Stack
- **Engine:** Unity 6 (6000.0.53f1 [LTS]) (2D)
- **Language:** C#
- **Architecture:**
  - **Scriptable Object:** Used for modular level design and data-driven gameplay settings (e.g., speed, ball spawn interval, ball colors).
  - **Object Pooling:** For efficient projectile management.
  - **Singleton Pattern:** For global managers (GameManager, BallManager, etc.)

## 🎮 How to Play
- **Aim:** Use your mouse to rotate the shooter.
- **Shoot:** Left-click to launch the ball.
- **Match:** Connect 3+ balls of the same color to clear them.
- **Objective:** Clear all balls before they reach the endpoint!

## 🎵 Credits
- **BGM:** [Exploration - Peaceful Area by Sonic289](https://pixabay.com/music/solo-piano-game-music-soundtrack-exploration-peaceful-area-372769/)
- **SFX:** [Free Casual Game SFX Pack by Dustyroom](https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116)
