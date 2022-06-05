# FirstPersonShooter

## About the game
This game is a FPS game which will contain Procedural Terrain Generation. I started this projcet to learn how FPS games and some more intermediate techniques like Procedural Terrain Generation and Occlusion Culling is working. I haven't spend much time in this project yet so currently it has only a working FPS controller and some procedural animations. The animations contain idle breath, walking, running, aiming and recoil. To get a satisfiyng breath animation i used the Lissajous Curve. To get the optimal walk and run animations i used the same curve with slightly different parameters and re-positioned the gun. 

Here are some questions i had to answer to get the most optimal methods for my game.

### Should the weapon interact with the world ? 
 In some games the weapon is rendered on the same camera as everything else. This results that the weapons can sometimes overlap with objects when they are very close. Game developers avoid this by holding the gun closer to the body when the player stands in front of walls. I personlly don't like this method so i rendered the weapon on a different camera.

### Should the raycasts start from the camera or the weapon ? 
This noticed from the player most of the time. The only difference it makes is whilst the player is aiming very close from a wall corner. If the raycasts starts from the weapon there can be occasions the projectiles are colliding with the wall whereby the player is aiming at something else. This occurs because the weapon's muzzle position and the position of the camera are different. I choose to start the raycast from the camera because i don't prefer that the start position of the projectile differs from the players view.

### How should i implement the scope ? 
There multiple methods for to implement a pleasing scope. I choose to render the weapon on another camera so i can increase the FOV of the gun to make the scope look like its zoomed in.


## Planned Features
- Occlusion Culling
- Procedural Terrain Generation 

## Game Screenshots
