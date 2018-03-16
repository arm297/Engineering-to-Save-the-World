# NodeListener.cs
Each and every node (when rendered) is assigned a NodeListener. The NodeListener responds to user mouse commands. The NodeListener initializes by placing listeners on each of the node’s buttons.

* NodeLinsterner Attributes:
  * idx: The index of the node (where the node’s information can be found in NodeList which is in GameController.cs)

* Functions:
  * Start()
  initializes listeners for the node, attaching a listenr for ‘purchase’ and ‘test’  buttons
  * PurchaseNode()
  Marks node data in NodeList[idx] as purchased and calls GameController.NodeNeighborhoodCheck(idx) wich finds the neighbors of node and marks neighbors as visible.
  * TestNode()
  Tests node by generating random number. If random number is less than node’s ProbabilityToFail, then marks broken as true, tested as true. Else marks tested as true.
