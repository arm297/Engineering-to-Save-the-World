# GameController.cs
This class stores all persistent data, currently for the NodeList and Player </br>
To do: Create reward and penalty library within GameController </br>
To do: Create trigger function which loads Drill Scene </br>
To do: Create trigger function which loads Event</br>

* Attributes:
  * NodeList: A list of NodeData which stores relevant information for each Node. NodeData is a class which stores attributes for a single node
  * Player: An implementation of PlayerProfile, which is a class that stores player info, such as funds, labor, name, score
  * Height: The height of the node grid, in nodes. This affects NodeList initialization.
  * Width: The width of the node grid, in nodes. This affects NodeList initializatio
  * InitialFunds: The amount of funds that the player begins with: Affects PlayerProfile initialization
  * InitialLabor: The amount of labor that the player begins with: Affects PlayerProfile initialization
  * InitialFame: The amount of fame that the player begins with: Affects PlayerProfile initialization
  * EventChance: The chance that an event will occur
  * list_of_drills: This is the list of all drill scenes which could be loaded
  * MainGame: This is the scene name of the ‘Main Game’ which displays and renders nodes
  * UI_Menu: This is the scene name for the user interface menu

* Functions:
  * Start() </br>
  Initializes the game, the NodelList and the Player, as well as loading the MainGame.
  * LoadScene(string scene_name) </br>
  Calls the SceneManager to load the scene_name. Note: scene_name must match the string value on the Unity Build List.
  * InitializeNodeList() </br>
  Generates the NodeList with randomly generated nodes, iterating through each node in Width and in Height, generating a NodeList with a dimension of Width * Height.
  * NodeNeighborhoodCheck() </br>
  Checks for purchased nodes, if purchased, then sets neighboring nodes to visible.
  * NodeNeighborhoodCheck(int idx) </br>
  Sets neigbhoring nodes of NodeList[idx] to visible.
  * NeighborFinder(int idx) </br>
  Returns a list of indexes for nodes neigbhoring NodeList[idx]
  * InitializePlayerProfile() </br>
  Initializes the Player Profile depending on start parameters









NodeData
Description: This class stores information for a single node. The class is nested within GameController.cs.

Attributes:
ID: unique ID assigned to each node
Name: a string label for each node
X: horizontal distance within the node grid (0 <= X <= Width)
Y: vertical distance within the node grid (0 <= Y <= Height)
CostActual: the actual cost of the node
CostEstimated: the estimated cost of the node (prior to node purchase)
ParametersActuals: the actual parameter values, a list that must match dimensions of ParametersEstimated and ParameterNames
ParametersEstimated: the estimated value of each parameter for the node (prior to node purchase). Must match dimensions of ParametersActuals and ParameterNames
ParameterNames: the name or description of each parameter. Must match dimensions of ParametersActuals and ParamtersEstimated
Purchased: boolean value, true if node has been purchased, false otherwise
Visible: boolean value, true if node should be rendered, false otherwise
Obscured: boolean value, true if node should be obscured (partially visible), false otherwise
Purchaseable: boolean value, true if node can be purchased, false otherwise
Tested: boolean value, true if node has already been tested, false otherwise
Broken: boolean value, true if node is broken, false otherwise
CostToFix: amount of funds required to fix the node if the node is broken
Parents: list of parent node IDs
Children: list of children node Ids
ProbabilityToFail: float representing the probability for the node to fail when testing

PlayerProfile
Description: This class stores player information. The class is nested within GameController.cs.

Attributes:
Funds: the amount of funds available to the player
Labor: the amount of labor available to the player
Name: string value for the player’s name
Title: string value for the player’s title
Fame: the amount of fame that the player has
