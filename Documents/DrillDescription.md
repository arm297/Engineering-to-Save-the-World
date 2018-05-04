# Description of the Drill Architecture.
[Back To Main](/README.md)

### The Basic Drill Architecture
Each drill is an individual Unity scene. These drills are randomly loaded using the by the
[game controller](./Documents/GameControllerDescription.md). Each drill has its own drill controller, which starts and
stops the drill, tracks the score, and returns to the main game. Additionally, each drill also has a display, which
controls the actual enabling and disabling the different UI components. After the drill is finished and the user
returns to the main game, the drillcontroller returns the final score for the drill back to the game controller to
process and to handle.

### Coding Style of the Drill Scripts
The different drill scripts rely on both the [Unity Event System](https://docs.unity3d.com/Manual/EventSystem.html) for handling
mouse events, as well as C#'s delegate pattern for handling custom events. A tutorial for using delegates can be found
[here](https://docs.microsoft.com/en-us/dotnet/csharp/delegates-events). When creating basic UI elements for drills, it is best
to try to make the base behavior as modular as possible to enable reuse.

### The Drill Controller
The Drill Controller ([DrillController.cs](./Assets/Scripts/Drills/DrillController.cs)) interfaces with the different components of
the drill. It starts the drill, activiting the timer, ends the drill by computing the score, and returns to the main game, reloading
the main scene and returning the final score for the drill back to the main game controller.

### The Drill Display
The Drill Display ([DrillDisplay.cs](./Assets/Scripts/Drills/DrillDisplay.cs)) controls all the main elements of the drill. In general,
the drill display will be attached to the main background of the drill. The display is responsible to hiding the start panel and any
answers at the beginning of the drill, setting the End Game button active during while the drill is running, and displaying the end game
panel and any answers when the drill finished.

### The Score Handler
The score handler ([ScoreHandler.cs](./Assets/Scripts/Drills/Scoring/ScoreHandler.cs)) calculates and displays any information about
the score during execution. The ([ScoreHandler](./Assets/Scripts/Drills/Scoring/ScoreHandler.cs)) class is an abstract class which
should be overridden by individual drills. In addition, the score handler script should be attached to the basic display for scoring.
The main functions which should be overridden by subclasses for the ScoreHandler are ComputeScore, which computes the final score,
and DisplayScoreInfo, which displays the final score info of the game.

### Creating a new Drill
To create a new drill, add the DrillController prefab, create a basic Canvas and attach a DrillDisplay.cs script to it, add a start
panel, and set the Begin Game button to listen to DrillController.BeginGame(), add an End Game button and set the button to listen
to DrillController.EndGame(), add a Score panel to keep track of the final score, and attach a script that is a subclass of the
ScoreHandler class to it, and add a Return To Main button and to set the button to listen to the DrillController.ReturnToMain().

### Further TODOS:
Currently the drills are entirely based in a 2D UI architecture. If this is expanded, then the DrillView class will either need to
be expanded, or an interface for the Basic Drill View and classes inheriting it will need to be added.
