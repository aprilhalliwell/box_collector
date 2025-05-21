# box_collector
A simple 2D Unity game where an autonomous NPC collects and sorts falling red and blue boxes.

## How To play
1. Load the scene box_collector
2. Press the **Play** button in the editor 
3. Click the ui button **Start** to begin.

## Configuration
You can tweak the system by modifying a few objects in the **Hierarchy**:

### Collector - NPC Agent
Navigate to the **collector**, you can specify speed.
- GameObjects: `collector`
- Field:
    - `moveSpeed`: how fast the Collector moves

### Box Spawner
Navigate to the **box_spawner**, you can specify initial pool size and max pool size. You can also change the random range boxes spawn.
- GameObjects: `box_spawner`
- Field:
    - `poolSize`: initial number of pooled box objects
    - `maxPoolSize`: maximum number of pooled objects before spawning stops
    - `spawnTime`: min/max range for randomized spawn intervals
### Drop Zones 
There are two drop zones named **drop_zone_left** and **drop_zone_right**. You can specify which type of box it accepts.
- GameObjects: `drop_zone_left` and `drop_zone_right`
- Field: 
  - `BoxType`: determines which box type (Red or Blue) the zone will accept and stack.
## Other Notes
I am making use of Layer masking for some collisions between the agent and boxes.
This can be viewed in the Physics 2D Layer Collision Matrix section (Edit -> Project Settings -> Physics 2D -> Layer Collision Matrix)