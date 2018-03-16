using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Behavior for containers to place dragged objects into. This container can
 * only take one dragged object.
 */
public class BlockContainer : MonoBehaviour {

    // Highlight color when dragged object is over block.
    [SerializeField]
    private Color highlightColor;

    // Original color of this container block.
    private Color defaultColor;

    // Renderer for this container block.
    private Renderer blockRenderer;

    // Whether the current block is full. 
    private bool isFilled = false;

    // Bounds for this container block.
    private Bounds blockBounds;

    // Drag-droppeds object contained by this block, if any.
    private DragDrop containedObject;

	// Use this for initialization
	void Start() {
        blockRenderer = GetComponent<Renderer>();
        defaultColor = blockRenderer.material.color;
        blockBounds = blockRenderer.bounds;
	}
	
    // Get component which object can snap into, or null if no such component
    // exists.
    public bool Intersects2D(DragDrop dragObject) {
        Bounds dragBounds = dragObject.GetComponent<Renderer>().bounds;
        return BlockContainer.Intersect2D(dragBounds, blockBounds);
    }

    // Changes the block's color to the specified color.
    public void SetColor(Color color) {
        blockRenderer.material.color = color;
    }

    // Changes the block's color to the default hightlight color
    public void highlightBlock() {
        blockRenderer.material.color = highlightColor;
    }

    // Restores the block's color to the original color.
    public void RestoreColor() {
        blockRenderer.material.color = defaultColor;
    }

    // Adds a contained object if the container is not filled. Returns whether
    // the object was added.
    public bool AddContainedObject(DragDrop dragObject) {
        if (!isFilled) {
            containedObject = dragObject;
            isFilled = true;
            return true;
        }
        else {
            return false;
        }
    }

    // Removes the contained object of this block.
    public void RemoveContainedObject() {
        isFilled = false;
        containedObject = null;
    }

    // Returns whether the bounds intersect in 2D space.
    static bool Intersect2D(Bounds b1, Bounds b2) {
        return !(b1.min.x > b2.max.x
              || b2.min.x > b1.max.x
              || b1.min.z > b2.max.z
              || b2.min.z > b1.max.z);
    }
}