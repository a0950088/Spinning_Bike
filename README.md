# Spinning Bike

## Assest
### Models
1. `human_bike_fix_03.fbx`: Player Model.

### Scenes
1. `Rules`: For texting rules here.
2. `PlayScene`: This is Playing scene (lastest version)
3. `TestScene 1`: This is a testing scene.

### Script
1. `PlayerController.cs`: To control the player movement.
2. `TCPserver.cs`: To make the TCP connection between python and Unity for transferring the video frame.
3. `Texting.cs`: To typing/texting the rules for user in the `Rules` scene.
4. `ToPlayScene.cs`: For the button in the `Rules` scene to switch the scene to the playing scene.
5. `VideoController.cs`: To control the speed of the video by the player.
6. `FrameData.cs`: Create the `class` for loading frame data from the json file.
7. `LoadJsonData.cs`: For loading the data from the jsom file.
8. `MainCameraController.cs`: Revise the distance of the camera according to the size of the video.
9. `ObstacleController.cs`: Use to randomly create the obstacles.
10. `CollisonController.cs`: Used to control the situation while the collision occurred.
11. `Danger_jumpin_message.cs`: Used to control the jumpin' message and stop the player while in the collision.
12. `LaneLineController.cs`: It is used to detect whether the player is out of the lane line or not and give the player a hint of direction.
13. `ObjectGenerator.cs`: For generating objects on the road while riding.

### PythonApi
1. `TCP_Client.py`: To make the TCP connection to send/receive Unity Server.(Now just received Server's frame and return testdata to check it work)
2. `lane_line_detect.py`: Old version of lane line detection.

### Others Folder
1. `Course Library`: Library download from Unity Tutorial.(Maybe useful in future)
2. `TextMesh Pro`: Font Library.
3. `Understone`: Unknown Library.(wait for checking)


## Dataset
- `Video_Trim.mp4`: download from [google drive](https://drive.google.com/drive/folders/1SZM-8ShzIN0dDf-LsiKNiY77D_f-udOa) or use custom video.