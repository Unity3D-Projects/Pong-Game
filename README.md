# Pong.

**Pong.** was a weekend project that started with the idea of creating a machine learning algorithm for the game Pong. Although an AI was implemented, **focus was shifted to the artistical aspects of the game.** All of the code—including engine, physics, HLSL shaders, etc—is written by me. No external libraries are used, except for [SharpDX](http://sharpdx.org/) which is merely used as a wrapper for DirectX and XAudio2.

![Pong.](images/Pong-888x376.png "Pong.")

### Features

<strong>Architecture<strong>  
. <sub>Carefully designed ECS (Entity–Component–System) engine</sub>  
. <sub>Separated into base engine and game implementation (for future use!)</sub>

<strong>Artificial Intelligence</strong>  
. <sub>So far just a trivial AI, although challenging enough.</sub>

<strong>Graphics</strong>  
. <sub>Camera effects (shaking)</sub>  
. <sub>Chromatic aberration</sub>  
. <sub>Hardware-accelerated graphics (DirectX 11.0)</sub>  
. <sub>Motion blur</sub>  
. <sub>Noise (ISO) shader</sub>  
. <sub>Object animations</sub>  
. <sub>Particle effects</sub>

<strong>Physics</strong>  
. <sub>Binary search for precise time-of-collision resolution</sub>  
. <sub>Collision detection (Separating Axis Theorem)</sub>  
. <sub>Realistic physics with fourth-order Runge–Kutta integration</sub>  
. <sub>Rigid body dynamics with convex polytopes</sub>

<strong>Sound</strong>  
. <sub>Background music</sub>  
. <sub>Multi-channel sound effects</sub>
