# PongBrain

An attempt at implementing an artificial neural network that plays pong. The game engine uses the entity-component architecture, so if all goes well, I might expand the scenario to include more complex game mechanics at a later time.

## Features (so far)

### Architecture
* Carefully designed ECS (Entity–Component–System) engine
* Separated into base engine and game implementation (for future use!)

### Artificial Intelligence
* So far just a trivial AI

### Graphics
* Hardware-accelerated graphics (DirectX 11.0)
* Camera effects (shaking)
* Chromatic aberration
* Motion blur
* Noise (ISO) shader
* Object animations
* Particle effects

### Physics
* Rigid body dynamics with convex polytopes
* Realistic physics with fourth-order Runge–Kutta integration
* Collision detection (Separating Axis Theorem)


![PongBrain in action!](images/Screenshot4.png "PongBrain in action!")
