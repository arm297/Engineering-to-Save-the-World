# Description of the Drill Architecture.
[Back To Main](/README.md)

### The Basic Drill Architecture
Each drill is an individual Unity scene. These drills are randomly loaded using the by the
[game controller](./Documents/GameControllerDescription.md). Each drill has its own drill controller, which starts and
stops the drill, tracks the score, and returns to the main game. Additionally, each drill also has a display, which
controls the actual enabling and disabling the different UI components. After the drill is finished and the user
returns to the main game, the drillcontroller returns the final score for the drill back to the game controller to
process and to handle. The Drills are developed in a version of a model-view-display architecture. However, currently, the drill
controller handles both the model and the controller aspects. In a future extension, these should probably be split further.

### Coding Style of the Drill Scripts
The different drill scripts rely on both the [Unity Event System](https://docs.unity3d.com/Manual/EventSystem.html) for handling
mouse events, as well as C#'s delegate pattern for handling custom events. A tutorial for using delegates can be found
[here](https://docs.microsoft.com/en-us/dotnet/csharp/delegates-events). When creating basic UI elements for drills, it is best
to try to make the base behavior as modular as possible to enable reuse.

### The Drill Controller
The Drill Controller ([DrillController.cs](./Assets/Scripts/Drills/DrillController.cs)) interfaces with the different components of
the drill. It starts the drill, activiting the timer, ends the drill by computing the score, and returns to the main game, reloading
the main scene and returning the final score for the drill back to the main game controller. The drill controller handles both the
model and the controller for the drills. It has three main fields which must be set in the editor: the GameTimer, which should refer
to the timer game object, Display, which should refer to the game object with the DrillDisplay script, and ScoreCalculator, which
should refer to the game object with the subclass of the ScoreHandler script attached.

### The Drill Display
The Drill Display ([DrillDisplay.cs](./Assets/Scripts/Drills/DrillDisplay.cs)) controls all the main elements of the drill. In general,
the drill display will be attached to the main background of the drill. The display is responsible to hiding the start panel and any
answers at the beginning of the drill, setting the End Game button active during while the drill is running, and displaying the end
game panel and any answers when the drill finished. the drill display handles the view for the drill scene, and has three main fields
which should be set in the editor: StartMenu, which should refer the the display containing the start info and the start button,
EndGameButton, which should refer to the button to end the game, and EndMenu, which should refer to the menu which displays the final
score and stats, and has the button to return to the main game.

### The Score Handler
The score handler ([ScoreHandler.cs](./Assets/Scripts/Drills/Scoring/ScoreHandler.cs)) calculates and displays any information about
the score during execution. The [ScoreHandler](./Assets/Scripts/Drills/Scoring/ScoreHandler.cs) class is an abstract class which
should be overridden by individual drills. In addition, the score handler script can either be attached to the basic display for scoring, or be added seperated and have the fields referring the to scoring and end game text refer to the scoring display.
The main functions which should be overridden by subclasses for the ScoreHandler are ComputeScore(), which computes the final score,
ComputeMaxScore(), which computes the maximum possible score, and DisplayScoreInfo(), which displays the final score info of the game.
The two fields of the ScoreHandler are statusText, which displays the end game status (victory, defeat, etc.), and scoreText, which
should be used the display the final score, max score, and any stats deemed necessary.

### Creating a new Drill
To create a new drill, add the DrillController prefab, create a basic Canvas and attach a DrillDisplay.cs script to it, add a start
panel, and set the Begin Game button to listen to DrillController.BeginGame(), add an End Game button and set the button to listen
to DrillController.EndGame(), add a Score panel to keep track of the final score, and attach a script that is a subclass of the
ScoreHandler class to it or to a game object whose fields refer it it, and add a Return To Main button and to set the button to listen to the DrillController.ReturnToMain().
Alternatively, you can use the provided template TemplateDrill scene, replace the dummy ([TemplateDrillScoreHandler.cs](./Assets/Drills/Scoring/TemplateDrillScoreHandler.cs))
with a custom scoring script, and add the necessary UI elements and features to create a new scene. However, when you replace the drill scoring script, be sure to reset the
the drill controllers scoreCalculator field in the Unity editor.

In the game controller ([GameController.cs](./Assets/Scripts/GameController.cs)) script, you must add the name of the drill to the variable
list_of_drills, in order for it to be run.

### Interfacing with the Game controller.
The drills are loaded by the game controller, which randomly rolls for a chance at running a drill by calling the method ChanceForDrill().
Alternatively, if you wanted to always trigger a drill at a certain point, the method LoadScene() can be called with the name of the drill as the
argument. The probability that a drill will be triggerred is specified in the DrillChance field in the ([[GameController.cs](./Assets/Scripts/GameController.cs))]) script,
and additionally, each drill with have the same probability of running. After the drill is finished, and the user specifies to return to the main
game, the DrillController will input the final score and the maximum possible score into the LastDrillScore field of the game
controller. On the next turn, for each drill, a random selection among the Player's stats will be increased by the ratio of the actual score to the
expected score multiplied by the game controller field MaxStatChange. These changes will be reset on the following turn.

### Further TODOS:
Currently the drills are entirely based in a 2D UI architecture. If this is expanded, then the DrillView class will either need to
be expanded, or an interface for the Basic Drill View and classes inheriting it will need to be added. Update the GameController to
autogenerate new events, which should improve the creation of new drills.
