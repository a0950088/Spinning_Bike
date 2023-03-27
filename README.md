# Spinning Bike

## Assest
### Models
1. `human_bike_fix_03.fbx`: Player Model.

### Scenes
1. `Rules`: For texting rules here.
2. `TestScene 1`: Test scene.
3. `PlayScene`: This is Playing scene (lastest version)

### Script
1. `PlayerController.cs`: To control the player movement.
2. `ShowVideo.cs`: To show the video.
3. `TCPserver.cs`: To make the TCP connection between python and Unity for transferring the video frame.
4. `Texting.cs`: To typing/texting the rules for user in the `Rules` scene.
5. `ToPlayScene.cs`: For the button in the `Rules` scene to switch the scene to the playing scene.
6. `VideoSpeedController.cs`: To control the speed of the video by the player.

### PythonApi
1. `TCP_Client.py`: To make the TCP connection to send/receive Unity Server.(Now just received Server's frame and return testdata to check it work)
2. `lane_line_detect.py`: Old version of lane line detection.

### Others Folder
1. `Course Library`: Library download from Unity Tutorial.(Maybe useful in future)
2. `TextMesh Pro`: Font Library.
3. `Understone`: Unknown Library.(wait for checking)


## Dataset
- `Video_Trim.mp4`: download from google drive (wait for editing) or use custom video.