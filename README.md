# Japanese Village VR — *Festival of Eternal Light*

<p align="center">
  <img src="https://github.com/geethsenaviratne/Japanese-Village-VR/blob/main/Japanese%20Village%20VR%20-%20GV/Assets/Images/japanese_village_banner.png?raw=true" width="720" alt="Japanese Village VR Preview">
</p>
<p align="center">
  <strong>An immersive VR-like interactive environment built with Unity</strong><br>
  Explore a stylized Japanese village and restore the sacred Festival of Eternal Light
</p>

---

## 📌 Overview

**Japanese Village VR** is an atmospheric interactive experience created for the **Interactive Storyworlds & Living Environments** module. Players explore a beautifully crafted Japanese village, solve environmental puzzles, and restore a sacred blade to revive the legendary **Festival of Eternal Light**.

### Key Highlights
- 🏮 Dynamic lighting with 72 synchronized lanterns
- 🌸 Cherry blossom groves and traditional architecture
- ⚔️ Environmental storytelling through interactive objects
- 🎨 Optimized rendering with advanced culling techniques
- 🎮 Immersive VR-style navigation and interaction

---

## 🎮 Core Experience

Journey through a mystical village featuring:
- **Lantern-lit riverside bridge** — Your entry into the village
- **Sacred shrine and torii gate** — The heart of the mystery
- **Cherry blossom grove** — Hidden secrets await
- **Traditional village streets** — Atmospheric exploration

**Goal:** Find the sacred blade → Restore the shrine → Activate the festival lights

---

## 🛠 Technical Features Implemented

### Scene Composition
- **Traditional Architecture:** Houses, shrine, torii gate, wooden bridge
- **Organic Terrain:** Heightmap-based ground mesh with stone pathways
- **Rich Detail:** Lanterns, pedestals, flags, interactive book, rotating statue
- **Optimized Geometry:** 2,500–4,000 vertices per building, ~3,200 per tree

### Dynamic Systems
- ✨ Particle effects for lanterns and environmental events
- 💡 Real-time emissive lighting control
- ⚙️ Physics-based interactions
- 🌊 Dynamic reflections
- 📖 Event-driven narrative progression

### Animated Objects
- **Rotating Statue:** Continuous Y-axis rotation (0–120°/sec)
- **Wind-Affected Flags:** Procedural wind simulation with pitch/yaw/roll
- **Interactive Book:** Smooth rising animation with glowing highlight
- **Sacred Blade:** Reveal effect, pickup mechanics, and return interaction

---

## 🧭 Gameplay Flow

1. **Spawn at the bridge** → The village lies in darkness
2. **Approach the shrine** → Receive message: *"Find the sword"*
3. **Interact with the book (E)** → Discover clue: *"Look under the tree"*
4. **Find the lantern** → Light it beneath the cherry tree
5. **Activate the statue (E)** → Watch it begin rotating
6. **Discover the blade** → Witness the glowing reveal effect
7. **Pickup the blade (E)** → Carry it to your destination
8. **Return to shrine (E)** → **Festival activates:**
   - 72 lanterns illuminate in sequence
   - Windows glow with warm light
   - Celebration audio fills the air

---

## 💻 Key Scripts

| Script | Purpose |
|--------|---------|
| `PlayerMovement.cs` | WASD + mouse controls, VR-style navigation |
| `InteractiveBook.cs` | Book rising animation + dialogue system |
| `InteractiveLantern.cs` | Lantern glow effects + particle activation |
| `InteractiveStatue.cs` | Rotational animation + audio feedback |
| `BladePickup.cs` | Glow reveal + pickup mechanics |
| `ReturnPoint.cs` | Final blade placement + festival trigger |
| `FlagWind.cs` | Dynamic wind simulation for flags |

---

## 🎨 Rendering & Optimization

### Rendering Pipeline (URP)
- **Field of View:** 60°
- **Clipping Planes:** Near 0.3, Far 1000
- **Materials:** PBR workflow with normal, metallic, and emission maps
- **Real-time Lighting:** Mixed lighting with baked GI

### Performance Optimizations
- **Frustum Culling (BVH):** ~40% reduction in draw calls
- **Occlusion Culling:** ~35% GPU performance improvement
- **LOD System:** 3 levels + billboard imposters for distant objects
- **GPU Instancing:** Enabled with SRP Batcher

**Results:**
- Draw calls: **120 → 55** (54% reduction)
- Frame time: **8ms → 5ms** (37% improvement)

---

## 💡 Interactive Lighting Controls

Adjustable UI sliders for dynamic lighting:
- **Diffuse Intensity** — Lambert shading control
- **Specular Smoothness** — Phong highlights
- **Ambient Brightness** — Global illumination level
- **Skybox Tint** — Atmospheric mood adjustment

---

## 📁 Project Structure

```
japanese-village-vr/
│
├── Assets/
│   ├── Scripts/          # C# interaction scripts
│   ├── Models/           # 3D models and meshes
│   ├── Materials/        # PBR materials
│   ├── Textures/         # Texture maps
│   ├── Images/           # UI and documentation images
│   ├── Prefabs/          # Reusable game objects
│   └── Scenes/           # Unity scene files
│
├── ProjectSettings/      # Unity project configuration
├── Packages/             # Package dependencies
└── README.md
```

---

## ▶️ How to Run

### Requirements
- **Unity 2021.3+** (Universal Render Pipeline)
- **Git** for cloning the repository

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/<your-username>/japanese-village-vr
cd japanese-village-vr
```

2. **Open in Unity Hub**
   - Add project from disk
   - Select the cloned folder
   - Unity will import all assets automatically

3. **Run the scene**
   - Open `Assets/Scenes/JapaneseVillage.unity`
   - Press the Play button

### Controls
- **WASD** — Movement
- **Mouse** — Look around
- **E** — Interact with objects
- **ESC** — Pause/Menu

---

## 📦 Asset Pipeline

- **3D Models:** Exported from Blender as FBX
- **Textures:** Compressed using DXT5 / BC7 format
- **Audio:** 16-bit WAV files
- **Prefabs:** Modular for easy scene composition

---

## 👥 Credits

**Geeth Senevirathne** — Environment Design, Scripting, Interaction Systems

*Created for Interactive Storyworlds & Living Environments Module*

---

## 📜 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

Special thanks to:
- Unity Technologies for the Universal Render Pipeline
- The game development community for tutorials and inspiration
- Course instructors and peers for feedback

---

