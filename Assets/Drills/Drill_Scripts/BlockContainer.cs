using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Behavior for containers to place dragged objects into.
 */
public class BlockContainer : MonoBehaviour {

    // Whether the current block is full. 
    bool full;

    // Whether the block can hold multiple components.
    public bool containMultiple;

    // Highlight color when dragged object is over block.
    public Color highlightColor;

    // Original color of this container block.
    private Color defaultColor;

    // Renderer for this container block.
    private Renderer blockRenderer;

    // Bounds for this container block.
    private Bounds blockBounds;

    // Drag-dropped object contained by this block, if any.
    DragDrop containedObject;

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

    // Returns whether the bounds intersect in 2D space.
    static bool Intersect2D(Bounds b1, Bounds b2) {
        return !(b1.min.x > b2.max.x
              || b2.min.x > b1.max.x
              || b1.min.z > b2.max.z
              || b2.min.z > b1.max.z);
    }

    private List<Vector2> GetCorners2D(Bounds bounds) {
        Vector2 topRight = new Vector2(bounds.max.x, bounds.max.z);
        Vector2 topLeft = new Vector2(bounds.min.x, bounds.max.z);
        Vector2 bottomRight = new Vector2(bounds.min.x, bounds.max.z);
        Vector2 bottomLeft = new Vector2(bounds.min.x, bounds.min.z);
        return new List<Vector2>() {topRight, topLeft, bottomRight, bottomLeft};
    }
}
