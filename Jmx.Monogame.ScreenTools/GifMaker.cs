using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.IO;



namespace Jmx.Monogame.ScreenTools
{
    public class GifMaker
    {
        private GraphicsDevice graphicsDevice;
        private Queue<Frame> _textureQueue;

        /// <summary>
        /// Uses GifEncoder to Queue multiple frames and write them to file.
        /// </summary>
        /// <param name="device">The graphics device used to grab a backbuffer.</param>
        public GifMaker(GraphicsDevice device)
        {
            this.graphicsDevice = device;
            _textureQueue = new Queue<Frame>();
        }           
        
        /// <summary>
        /// Adds a single frame to the Frame Queue
        /// </summary>
        /// <param name="delayMilliseconds">Optional delay for this frame in milliseconds </param>
        public void AddFrame(int delayMilliseconds = 0)
        {
            Color[] colors = new Color[graphicsDevice.Viewport.Width * graphicsDevice.Viewport.Height];
            graphicsDevice.GetBackBufferData<Color>(colors);
            Frame fr = new Frame
            {
                Texture = new Texture2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height)
            };
            fr.Texture.SetData<Color>(colors);
            fr.DelayMS = delayMilliseconds;

            _textureQueue.Enqueue(fr);
        }

        /// <summary>
        /// Writes all images to file as a single GIF. 
        /// IF no filename is specified, a default filename using DateTime.Now.ToFileTime() is used.
        /// </summary>
        /// <param name="filename">The output filename</param>
        public void WriteAllFrames(string filename = "")
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = $"Gif_Output_{DateTime.Now.ToFileTime()}.gif";
            }

            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (GifEncoder writer = new GifEncoder(fs))
                {
                    //let's write all the textures 
                    while (_textureQueue.Count > 0)
                    {
                        var frame = _textureQueue.Dequeue();
                        using (var ms = new MemoryStream())
                        {
                            frame.Texture.SaveAsPng(ms, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
                            using (System.Drawing.Image bmp = System.Drawing.Bitmap.FromStream(ms))
                            {
                                writer.AddFrame(bmp, 0, 0, TimeSpan.FromMilliseconds(frame.DelayMS) );                                   
                            }
                            frame.Texture.Dispose();
                        }
                    }
                }
            }            
        }        

    }




}
