using System;
using Unity.Collections;
using System.IO;
using System.Collections.Generic;
using Unity.Jobs;

namespace UnityEngine.Utils
{
    using Core;
    using Extensions;

    // TODO: Create ASM Definitions for this
#if UNITY_2018_3_OR_NEWER

        using Rendering;

#else

    using Rendering.Experimental;

#endif

    // TODO: Re3search if Sprite.Create can be async
    // If not search for a method that crops textures using Color[] and NativeArray<Color>
    // [AutoInstantiate]
    public class AsyncTextureManipulator : MonoSingleton<AsyncTextureManipulator>
    {
        private Queue<Request> _requests = new Queue<Request>();

        private JobHandle cropJobHandle;

        private void Update()
        {
            while (_requests.Count > 0)
            {
                var req = _requests.Peek();
                var asyncReq = req.AsyncRequest;

                if (asyncReq.hasError)
                {
                    Debug.Log("GPU readback error detected.");
                    _requests.Dequeue();
                }
                else if (asyncReq.done)
                {
                    if (Time.frameCount % 60 == 0)
                    {
                        //var camera = GetComponent<Camera>();
                        //SaveBitmap(buffer, camera.pixelWidth, camera.pixelHeight);

                        switch (req.ManipulatorMode)
                        {
                            //    var rect = (Rect)Request.Objects[0];
                            //    var callback = (Action<Sprite>)Request.Objects[1];

                            case ManipulatorMode.Crop:
                                using (var job = new CropJob
                                {
                                    Buffer = req.AsyncRequest.GetData<Color32>(),
                                    Rect = (Rect)req.Objects[0],
                                    Callback = (Action<Texture2D>)req.Objects[1]
                                })
                                {
                                    var texture = req.Objects[2] as Texture2D;
                                    int totalLength = texture.width * texture.height;

                                    job.Init(texture.width, texture.height);

                                    cropJobHandle = job.Schedule(totalLength, 32);
                                    cropJobHandle.Complete();
                                }
                                break;

                            default:
                                throw new ApplicationException("Not defined mode!");
                        }
                    }

                    _requests.Dequeue();
                }
                else
                {
                    break;
                }

                // if(cropJobHandle.IsCompleted)

                // JobHandle.ScheduleBatchedJobs();
            }
        }

        public void CropImage(Rect rect, Texture2D atlas, Action<Texture2D> callback)
        {
            if (rect == default)
                throw new ArgumentNullException(nameof(rect));

            if (atlas == null)
                throw new ArgumentNullException(nameof(atlas));

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            EnqueueRequest(new Request(rect, atlas, callback));
        }

        private void EnqueueRequest(Request request)
        {
            if (_requests.Count < 8)
                _requests.Enqueue(request);
            else
                Debug.LogError("Too many requests.");
        }

        public enum ManipulatorMode
        {
            Crop
        }

        private class Request
        {
            public AsyncGPUReadbackRequest AsyncRequest { get; }
            public ManipulatorMode ManipulatorMode { get; }
            public object[] Objects { get; }

            private Request()
            {
            }

            public Request(Rect cropRect, Texture2D atlas, Action<Texture2D> callback)
            {
                ManipulatorMode = ManipulatorMode.Crop;
                Objects = new object[] { cropRect, callback, atlas };
                AsyncRequest = AsyncGPUReadback.Request(atlas);
            }

            public static RenderTexture GetCroppedRenderTexture(Texture2D texture, Rect rect)
            {
                RenderTexture rt = new RenderTexture(texture.width, texture.height, 0);
                RenderTexture.active = rt;

                // Copy your texture ref to the render texture
                Graphics.Blit(texture, rt);

                // TODO
                //Texture2D croppedTexture = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
                //croppedTexture.ReadPixels(rect, 0, 0);

                return rt;
            }
        }

        private struct CropJob : IJobParallelFor, IDisposable
        {
            public NativeArray<Color32> Buffer { private get; set; }
            public Rect Rect { private get; set; }
            public Action<Texture2D> Callback { private get; set; }

            public int Length => Rect.GetLength();

            // private int m_TotalLength;
            private int m_TexWidth;

            private int m_TexHeight;

            private int m_CropTexWidth;

            private NativeArray<Color32> m_colors;
            private bool m_isInit;

            public void Init(int texWidth, int texHeight)
            {
                m_colors = new NativeArray<Color32>(Length, Allocator.Temp);

                m_TexWidth = texWidth;
                m_TexHeight = texHeight;

                m_CropTexWidth = (int)Rect.width;

                m_isInit = true;
            }

            //public void Execute()
            //{
            //    var rect = (Rect)Request.Objects[0];
            //    var callback = (Action<Sprite>)Request.Objects[1];

            //    var buffer = Request.AsyncRequest.GetData<Color32>();

            //    //ColorArray = GetCroppedArray(buffer, rect);
            //    // var nativeSlice = buffer.Slice();

            //    // Sprite tempSprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(.5f, .5f), img.width / 3.333f);
            //}

            public void Execute(int index)
            {
                if (!m_isInit)
                    throw new ApplicationException("Not initialized!");

                int x = index % m_TexWidth,
                    y = index / m_TexWidth;

                if (Contains(Rect, x, y))
                {
                    Color32 color = Buffer[index];

                    int _x = Rect.xMin.Round() - x,
                        _y = Rect.yMin.Round() - y,
                        i = _x + _y * m_CropTexWidth;

                    m_colors[i] = color;
                }
            }

            public bool Contains(Rect r, int x, int y)
            {
                return r.x <= x
                    && x < r.x + r.width
                    && r.y <= y
                    && y < r.y + r.height;
            }

            public void Dispose()
            {
                // Only for testing purpouses
                // TODO: Add new request with the NativeArray (m_colors) and create a ComputerBuffer (it can be used with the 'AsyncGPUReadback.Request' method)
                SaveBitmap(m_colors, Rect.width.Round(), Rect.height.Round());

                Buffer.Dispose();
                m_colors.Dispose();
            }

            private void SaveBitmap(NativeArray<Color32> buffer, int width, int height)
            {
                var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
                tex.SetPixels32(buffer.ToArray());
                tex.Apply();

                File.WriteAllBytes(Path.Combine(Application.streamingAssetsPath, "test.png"), ImageConversion.EncodeToPNG(tex));

                Destroy(tex);
            }

            //private NativeArray<Color32> GetCroppedArray(NativeArray<Color32> buffer, Rect r)
            //{
            //    //var array = new NativeArray<Color32>();

            //    //int i = 0;
            //    //foreach (var color in buffer)
            //    //{
            //    //    ++i;
            //    //}
            //}
        }
    }

    /*
     *
        using UnityEngine;
        using Unity.Collections;
        using System.IO;
        using System.Collections.Generic;

        #if UNITY_2018_3_OR_NEWER
        using UnityEngine.Rendering;
        #else
        using UnityEngine.Experimental.Rendering;
        #endif

        public class AsyncCapture : MonoBehaviour
        {
            Queue<AsyncGPUReadbackRequest> _requests = new Queue<AsyncGPUReadbackRequest>();

            void Update()
            {
                while (_requests.Count > 0)
                {
                    var req = _requests.Peek();

                    if (req.hasError)
                    {
                        Debug.Log("GPU readback error detected.");
                        _requests.Dequeue();
                    }
                    else if (req.done)
                    {
                        var buffer = req.GetData<Color32>();

                        if (Time.frameCount % 60 == 0)
                        {
                            var camera = GetComponent<Camera>();
                            SaveBitmap(buffer, camera.pixelWidth, camera.pixelHeight);
                        }

                        _requests.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            void OnRenderImage(RenderTexture source, RenderTexture destination)
            {
                if (_requests.Count < 8)
                    _requests.Enqueue(AsyncGPUReadback.Request(source));
                else
                    Debug.Log("Too many requests.");

                Graphics.Blit(source, destination);
            }

            void SaveBitmap(NativeArray<Color32> buffer, int width, int height)
            {
                var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
                tex.SetPixels32(buffer.ToArray());
                tex.Apply();
                File.WriteAllBytes("test.png", ImageConversion.EncodeToPNG(tex));
                Destroy(tex);
            }
        }
     *
     */
}