# 🚁 VR Helicopter Physics

A physically-driven helicopter flight model built in Unity, rebuilt into an interactive **VR cockpit experience** using OpenXR and the XR Interaction Toolkit.

*Status: in active development, ~80% ready.*

## What it is

A from-scratch flight physics simulation — no ready-made flight-controller assets — where the player sits inside a VR cockpit and flies the helicopter using physically modeled rotor forces (lift, torque, drag, tail rotor compensation) instead of scripted movement. The project started as a physics-only prototype and was later extended with a head-tracked VR camera and grabbable, interactable cockpit controls (levers, toggles) that the player operates directly with VR controllers.

## Tech highlights

- **Component-based architecture**: behavior is split across small, replaceable pieces rather than one monolithic controller — `BaseRbController`, `BasePhysics`, `BaseHeliInput`, and `BaseCamera` define the contracts, with concrete implementations (`HeliController`, `MainHeliEngine`, `RotorController`) built on top.
- **Rotor physics as its own system**: `MainRotor`, `TailRotor`, and `AdvanceMainRotor` model lift, torque generation and tail-rotor compensation independently, with `IRotor` as the shared interface and `RotorBlur` handling the visual blur effect at high RPM.
- **Decoupled input**: `BaseHeliInput` abstracts control input away from the physics layer, plus a custom in-editor debug tool (`KeyBoardHeliInputDebug`) for testing flight behavior without a headset attached.
- **VR integration**: built on Unity's XR Interaction Toolkit and OpenXR — head-tracked cockpit camera, and cockpit controls implemented as grabbable/interactable objects rather than UI buttons.

## What I learned

- How to model believable rotor aerodynamics (lift, cyclic/collective pitch, torque, drag) from physics fundamentals instead of tuning a black-box asset.
- How to keep a physics simulation testable on a desktop (keyboard/gamepad) while layering VR-specific interaction on top of the same core, instead of forking the logic per platform.
- Practical XR Interaction Toolkit patterns for turning physical cockpit controls (levers, toggles) into grabbable VR interactions.

## How to run

- Unity 6000.2.7f2 (or a current Unity 6 release), HDRP.
- Open the main scene and press Play.
- Desktop/no-headset testing: use keyboard controls (`WASD` move, `Space`/`Left Ctrl` collective, `Q`/`E` yaw, arrow keys for blade pitch) via the built-in debug input.
- VR: connect a headset via OpenXR before entering Play mode to use head tracking and grabbable cockpit controls.

## Origin

This project began as a take-home technical assignment (helicopter flight physics on primitives, no VR) and was later extended into a full VR cockpit experience — the physics core carried over unchanged.
