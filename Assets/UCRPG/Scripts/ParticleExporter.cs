using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ParticleExporter : MonoBehaviour
{
    // Default folder name where you want the animations to be output
    public string folder = "PNG_Animations";

    // Framerate at which you want to play the animation
    public int
        frameRate = 25; // export frame rate. Set time.captureframerate to ignore the real time and use this frame rate directly.

    public float
        frameCount =
            100; // export frame count is the number of exported frames, and 100 frames is equivalent to the light effect time of 5 seconds. Because the export time of each frame is very long, the export time will be much longer than the intuitive light effect playback time.

    public int
        screenwidth =
            960; // not use temporarily useless. I hope you can directly set the size of the screen (that is, the size of the optical canvas)

    public int screenHeight = 640;
    public Vector3 cameraPosition = Vector3.zero;
    public Vector3 cameraRotation = Vector3.zero;

    private string realFolder = ""; // real folder where the output files will be
    private float originaltimescaleTime; // track the original time scale so we can freeze the animation between frames
    private float currentTime = 0;
    private bool over = false;
    private int currentIndex = 0;
    private Camera exportCamera; // camera for export the camera of light effect, use rendertexture

    public void Start()
    {
        // set frame rate
        Time.captureFramerate = frameRate;

        // Create a folder that doesn't exist yet. Append number if necessary.
        realFolder = Path.Combine(folder, name);

        // Create the folder
        if (!Directory.Exists(realFolder))
        {
            Directory.CreateDirectory(realFolder);
        }

        originaltimescaleTime = Time.timeScale;

        GameObject goCamera = Camera.main.gameObject;
        if (cameraPosition != Vector3.zero)
        {
            goCamera.transform.position = cameraPosition;
        }

        if (cameraRotation != Vector3.zero)
        {
            goCamera.transform.rotation = Quaternion.Euler(cameraRotation);
        }

        GameObject go = Instantiate(goCamera) as GameObject;
        exportCamera = go.GetComponent<Camera>();

        currentTime = 0;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (!over && currentIndex >= frameCount)
        {
            over = true;
            Cleanup();
            Debug.Log("Finish");
            return;
        }

        //Screenshot per frame
        StartCoroutine(CaptureFrame());
    }

    void Cleanup()
    {
        DestroyImmediate(exportCamera);
        DestroyImmediate(gameObject);
    }

    IEnumerator CaptureFrame()
    {
        // Stop time
        Time.timeScale = 0;
        // Yield to next frame and then start the rendering
        // this is important, otherwise will have error
        yield return new WaitForEndOfFrame();

        string filename = String.Format("{0}/{1:D04}.png", realFolder, ++currentIndex);
        Debug.Log(filename);

        int width = screenwidth;
        int height = screenHeight;

        //Initialize and render textures
        RenderTexture blackCamRenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        RenderTexture whiteCamRenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);

        exportCamera.targetTexture = blackCamRenderTexture;
        exportCamera.backgroundColor = Color.black;
        exportCamera.Render();
        RenderTexture.active = blackCamRenderTexture;
        Texture2D texb = GetTex2D();

        //Now do it for Alpha Camera
        exportCamera.targetTexture = whiteCamRenderTexture;
        exportCamera.backgroundColor = Color.white;
        exportCamera.Render();
        RenderTexture.active = whiteCamRenderTexture;
        Texture2D texw = GetTex2D();

        // If we have both textures then create final output texture
        if (texw && texb)
        {
            Texture2D outputtex = new Texture2D(width, height, TextureFormat.ARGB32, false);

            // we need to check alpha ourselves,because particle use additive shader
            // Create Alpha from the difference between black and white camera renders
            for (int y = 0; y < outputtex.height; ++y)
            {
                // each row
                for (int x = 0; x < outputtex.width; ++x)
                {
                    // each column
                    float alpha;
                    alpha = texw.GetPixel(x, y).r - texb.GetPixel(x, y).r;
                    alpha = 1.0f - alpha;
                    Color color;
                    if (alpha == 0)
                    {
                        color = Color.clear;
                    }
                    else
                    {
                        color = texb.GetPixel(x, y);
                    }

                    color.a = alpha;
                    outputtex.SetPixel(x, y, color);
                }
            }


            // Encode the resulting output texture to a byte array then write to the file
            byte[] pngShot = outputtex.EncodeToPNG();
            File.WriteAllBytes(filename, pngShot);

            // cleanup, otherwise will memory leak
            pngShot = null;
            RenderTexture.active = null;
            DestroyImmediate(outputtex);
            outputtex = null;
            DestroyImmediate(blackCamRenderTexture);
            blackCamRenderTexture = null;
            DestroyImmediate(whiteCamRenderTexture);
            whiteCamRenderTexture = null;
            DestroyImmediate(texb);
            texb = null;
            DestroyImmediate(texw);
            texb = null;

            System.GC.Collect();

            // Reset the time scale, then move on to the next frame.
            Time.timeScale = originaltimescaleTime;
        }
    }

    // Get the texture from the screen, render all or only half of the camera
    private Texture2D GetTex2D()
    {
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        return tex;
    }
}