using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AnimatedPixelPack
{
    public class SpriteSkinImporter : AssetPostprocessor
    {
        void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {
            // Ensure we only alter textures under our own path to prevent issues with other sprites
            string path = assetPath.ToLower();
            if (path.IndexOf("/animatedpixelpack/") == -1)
            {
                return;
            }

            // Get all the sprites in this texture
            for (int i = 0; i < sprites.Length; i++)
            {
                Sprite s = sprites[i];

                // Create the cut-out geometry around the exact sprite pixels
                Vector2[] verts;
                ushort[] tris;
                CreateExactMesh(s, texture, out verts, out tris);

                // Update the sprite mesh
                s.OverrideGeometry(verts, tris);
            }
        }

        private void CreateExactMesh(Sprite sprite, Texture2D texture, out Vector2[] verts, out ushort[] tris)
        {
            List<Vector2> vertices = new List<Vector2>();
            List<ushort> indices = new List<ushort>();

            int w = (int)sprite.textureRect.width;
            int h = (int)sprite.textureRect.height;
            int startX = (int)sprite.textureRect.xMin;
            int startY = (int)sprite.textureRect.yMin;

            // Compute the mask from the texture data
            Color[] pixels = sprite.texture.GetPixels(startX, startY, w, h);
            bool[] mask = new bool[pixels.Length];
            for (int i = 0; i < mask.Length; i++)
            {
                mask[i] = pixels[i].a > 0;
            }

            // Generate mesh for mask using lexicographic ordering
            int n = 0;
            for (int j = 0; j < w; j++)
            {
                for (int i = 0; i < h; )
                {
                    if (mask[n])
                    {
                        // Compute the width of this contiguous section
                        int quadWidth = 1;
                        for (; i + quadWidth < w && mask[n + quadWidth]; quadWidth++) ;

                        // Compute the height of the section
                        int quadHeight = 1;
                        bool done = false;
                        for (; j + quadHeight < h; quadHeight++)
                        {
                            for (int k = 0; k < quadWidth; k++)
                            {
                                if (!mask[n + k + quadHeight * w])
                                {
                                    done = true;
                                    break;
                                }
                            }

                            if (done)
                            {
                                break;
                            }
                        }

                        // Add quad
                        AddQuad(i, j, quadWidth, quadHeight, ref vertices, ref indices);

                        // Zero-out mask in this section
                        for (int l = 0; l < quadHeight; ++l)
                        {
                            for (int k = 0; k < quadWidth; ++k)
                            {
                                mask[n + k + l * w] = false;
                            }
                        }

                        // Increment counters and continue
                        i += quadWidth;
                        n += quadWidth;
                    }
                    else
                    {
                        i++;
                        n++;
                    }
                }
            }

            // Set the out parameters
            verts = vertices.ToArray();
            tris = indices.ToArray();
        }

        private static void AddQuad(int startX, int startY, int quadWidth, int quadHeight, ref List<Vector2> vertices, ref List<ushort> indices)
        {
            ushort index = (ushort)vertices.Count;

            // Add the verticies
            float vX = startX;
            float vY = startY;
            float vX2 = (startX + quadWidth);
            float vY2 = (startY + quadHeight);
            vertices.Add(new Vector2(vX, vY));
            vertices.Add(new Vector2(vX2, vY));
            vertices.Add(new Vector2(vX2, vY2));
            vertices.Add(new Vector2(vX, vY2));

            // Add the indicies
            indices.Add(index);
            indices.Add((ushort)(index + 1));
            indices.Add((ushort)(index + 2));
            indices.Add((ushort)(index + 2));
            indices.Add((ushort)(index + 3));
            indices.Add(index);
        }
    }
}