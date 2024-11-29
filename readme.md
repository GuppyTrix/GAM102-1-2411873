# USGS Earthquake Data Visualizer

## License
This project is licensed under the MIT License. For more information, please refer to the [License.md](license.md).

The MIT License is a permissive, open-source license that allows for both commercial and non-commercial use, making it a great choice for collaboration. It also provides liability protection by disclaiming warranties, while ensuring proper attribution to the original creator.

---

## Overview
This project visualizes earthquake data from the [USGS Earthquake Hazards Program](https://www.usgs.gov/programs/earthquake-hazards) on a 3D Earth model in Unity.

Built using Unity version [2023.2.20](https://unity.com/releases/editor/whats-new/2023.2.20#installs), the application fetches and displays real-time earthquake data from the USGS API and maps it onto a spherical model of Earth. The project includes features like dynamic data fetching, custom shaders, and interactive controls.

---

## Features
- **Custom Shader & Material**  
  Developed a custom shader to visualize search areas for both rectangular and circular search queries on the Earth model.
  This works for both the iCrcle and Rect preview  although for the circle preiview it uses an aproximation for the display as a rectangle do as you get further away from the equator the box does warp
  ![Custom Shader](https://github.falmouth.ac.uk/GA-Undergrad-Student-Work-24-25/GAM102-1-2411873/assets/2826/48db5bfa-e969-46b3-b0bb-e5bb13ddd9fb)

- **Real-Time Data Fetching**  
  Retrieves earthquake data from the [USGS Earthquake GeoJSON API](https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson_detail.php).  

- **Coordinate Conversion**  
  Converts earthquake latitude and longitude coordinates into 3D space (x, y, z) on the spherical Earth model.
  this piece of code is what makes this entire project viable Credit to [tin_man_](https://www.reddit.com/r/askmath/comments/ybbn3b/how_to_transform_latlong_coordinates_on_a_sphere/) on reddit

- **Dynamic API Menu**  
  The API menu allows users to specify search parameters, make requests to the API, and dynamically display the earthquake data on Earth.  
  ![API Menu](https://github.falmouth.ac.uk/GA-Undergrad-Student-Work-24-25/GAM102-1-2411873/assets/2826/cdb29fac-9a2a-4545-8d3a-fc86e464cca4)

- **Animated Orbital Movement**  
  An animator controls the moon's orbit around Earth, adding a dynamic celestial effect.  
  ![Moon Orbit Animation](https://github.falmouth.ac.uk/GA-Undergrad-Student-Work-24-25/GAM102-1-2411873/assets/2826/97d3efc6-ed41-481e-930a-a5fb6368d9df)  
  ![Moon Orbit](https://github.falmouth.ac.uk/GA-Undergrad-Student-Work-24-25/GAM102-1-2411873/assets/2826/d622cf53-f9cb-467a-a044-dd646a6e220f)

- **Debugging Tools**  
  Press **P** to toggle scene lighting for debugging purposes.  
  ![Debug Tool](https://github.falmouth.ac.uk/GA-Undergrad-Student-Work-24-25/GAM102-1-2411873/assets/2826/732ec247-cbf6-4638-a1ed-f44cc6c0c525)

- **Player Animations**  
  Player character animations for the player so that it doesnt feel like your floating in space and more of an observitory of earth

- **Interactive Menus**  
  Includes a **Main Menu** and a **Start Menu** to enhance user interaction.  
  ![Main Menu](https://github.falmouth.ac.uk/GA-Undergrad-Student-Work-24-25/GAM102-1-2411873/assets/2826/5e17fba4-ad5e-407d-8064-f4c8f6774a16)

---

## Imported Assets
This project uses several assets from the Unity Asset Store and Mixamo to enhance the visual and functional experience:

- **Shaders for Earth**: [Customizable Planet Shaders](https://assetstore.unity.com/packages/vfx/shaders/customizable-planet-shaders-131872)
- **Earth Model**: [Planet Earth Free](https://assetstore.unity.com/packages/3d/environments/sci-fi/planet-earth-free-23399)
- **Earth & Sun Textures**: [Planets of the Solar System 3D](https://assetstore.unity.com/packages/3d/environments/planets-of-the-solar-system-3d-90219)
- **Animations & Player Models**: [Mixamo](https://www.mixamo.com)

---

## How to Use
1. **Download & Setup**
    - Go to the build section of this repo and download the most recent release
    - Extract the build and run the .exe labled USGSEarthquakeDate 
   
2. **Running the Application**  
   - Press **Play** in the Unity editor to start the simulation.
   - Use the API menu to specify search parameters like date ranges, magnitude, and depth.
   - The Earth model will update in real-time with earthquake data from the USGS API based on your inputs.

## Controls

| Key    | Action                          | Description                               |
|--------|---------------------------------|-------------------------------------------|
| **W**  | Move Forward                    | Move the player forward in the scene.     |
| **A**  | Move Left                       | Move the player to the left.              |
| **S**  | Move Backward                   | Move the player backward in the scene.    |
| **D**  | Move Right                      | Move the player to the right.             |
| **Mouse** | Look Around                    | Move the camera to look around.           |
| **ESC**  | Open/Close Pause Menu           | Pause the game and open the menu.         |
| **P**    | Debug Tool (Toggle Scene Light) | Toggle lighting in the scene for debugging.|
| **E**    | Open API Menu                   | Open the menu to request earthquake data. |

---

## Movement Overview

- The **WASD keys** are used for basic movement:
  - **W**: Move forward
  - **A**: Move left
  - **S**: Move backward
  - **D**: Move right

- You can also **look around** using the **mouse** for a first-person-style camera control.

---
