using UnityEngine;

public class TextureTiling : MonoBehaviour
{
    public float scrollSpeed = 1.0f; // Adjust the scroll speed as needed
    private Renderer objectRenderer;

    private void Start()
    {
        // Get the Renderer component of the object
        objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // // Calculate the new offset based on time
        // float newOffset = Time.time * scrollSpeed;
        //
        // // Create a new Vector2 to set the offset
        // Vector2 newOffsetVector = new Vector2(0f, -newOffset);
        //
        // // Apply the new offset to the material's main texture
        // objectRenderer.material.mainTextureOffset = newOffsetVector;
    }
}