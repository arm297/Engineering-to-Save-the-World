# MainGameRenderer.cs
[Back To Main](/README.md)

Reads NodeList Data in GameController and renders visible nodes on screen.

* MainGameRenderer Attributes:
  * CanvasHeight: The height of the field of view
  * CanvasWidth: The width of the field of view
  * Nodes: A list that holds the GameObject nodes
  * NodeIDX: A list that holds the corresponding indexes of each GameObject node (matches Nodes)
  * X_Space: Horizontal spacing between nodes
  * Y_Space: Vertical spacing between nodes
  * DisplayAllNodes: For troubleshooting, causes all nodes in GameController NodeList to be rendered
  * RespawnNodes: For troubleshooting, causes Nodes and NodeIDX to be refreshed

* Functions:
  * Start()
  Generates list of visible nodes
  * Update()
  Renders Nodes each scene update. If RespawnNodes is true, then destroy Nodes and NodeIDX and recreate list.
  * GetNodes()
  Gets all visible nodes from GameController.NodeList and appends node ID to NodeIDX and appends Nodes with a GameObject node
  * CreateNodeGameObject(node, idx)
  Builds the node GameObject and returns the node.

</br>

[Back To Main](/README.md)