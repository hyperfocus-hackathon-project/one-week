using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR && UNITY_WSA
using Windows.Media.FaceAnalysis;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Media;
using Windows.Graphics.Imaging;
using Windows.System.Threading;
using System.Threading;
using System;
#endif

public class FacialRecognitionManager : MonoBehaviour
{
    public static FacialRecognitionManager singleton { get; private set; }
    public GameObject facialHighlightPrefab;
    public List<RecognizedFace> recognizedFaces;

    private GameObject[] facialHighlights;

    private Transform m_cameraTransform;
    public struct RecognizedFace
    {
        public Vector2 position2D;
        public Vector3 position3D;
        public Vector2 size;
    }

#if !UNITY_EDITOR && UNITY_WSA
    private MediaCapture m_mediaCapture;
    private FaceTracker m_faceTracker;
    private ThreadPoolTimer m_frameProcessingTimer;
    private SemaphoreSlim m_faceProcessingSemaphore = new SemaphoreSlim(1);
    private VideoEncodingProperties m_videoProperties;
#endif


    // Start is called before the first frame update
    void Start()
    {
        if (singleton != null)
        {
            throw new System.Exception("Only one FacialRecognitionManager is allowed.");
        }
        singleton = this;

        m_cameraTransform = Camera.main.transform;

        recognizedFaces = new List<RecognizedFace>();

        facialHighlights = (GameObject[])System.Array.CreateInstance(typeof(GameObject), 4);
        for (int i = 0; i < facialHighlights.Length; i++)
        {
            facialHighlights[i] = GameObject.Instantiate(facialHighlightPrefab);
            facialHighlights[i].transform.parent = m_cameraTransform;
            facialHighlights[i].SetActive(false);
        }

        SimulateFacialRecognitionData();
        #if !UNITY_EDITOR && UNITY_WSA
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < facialHighlights.Length; i++)
        {
            if (i < recognizedFaces.Count)
            {
                facialHighlights[i].SetActive(true);
                facialHighlights[i].transform.localPosition = recognizedFaces[i].position3D;
            }
            else
            {
                facialHighlights[i].SetActive(false);
            }
        }
    }

    private void SimulateFacialRecognitionData()
    {
        RecognizedFace simulatedFace = new RecognizedFace();
        simulatedFace.position2D = new Vector2(0.5f, 0.5f);
        simulatedFace.position3D = new Vector3(0, 0, 1.0f);
        simulatedFace.size = new Vector2(100, 100);

        recognizedFaces.Add(simulatedFace);
    }

#if !UNITY_EDITOR && UNITY_WSA
    private async void InitializeFacialRecognition()
    {
        if(m_faceTracker == null)
        {
            m_faceTracker = await FaceTracker.CreateAsync();
        }

        m_mediaCapture = new MediaCapture();
        MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
        settings.StreamingCaptureMode =  StreamingCaptureMode.Video;
        await m_mediaCapture.InitializeAsync(settings);
        VideoDeviceController deviceController = m_mediaCapture.VideoDeviceController;
        m_videoProperties = deviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview) as VideoEncodingProperties;
        await m_mediaCapture.StartPreviewAsync();
        TimeSpan timerInterval = TimeSpan.FromMilliseconds(66);
        m_frameProcessingTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(ProcessCurrentVideoFrame), timerInterval);
    }

    private async void ShutdownWebcam()
    {
        if(m_frameProcessingTimer != null)
        {
            m_frameProcessingTimer.Cancel();
        }

        if (m_mediaCapture != null)
        {
            if(m_mediaCapture.CameraStreamState == CameraStreamState.Streaming)
            {
                await m_mediaCapture.StopPreviewAsync();
            }
        }
        m_mediaCapture.Dispose();

        m_frameProcessingTimer = null;
        m_mediaCapture = null;
    }

    private async void ProcessCurrentVideoFrame(ThreadPoolTimer timer)
    {
        if(m_mediaCapture == null)
        {
            return;
        }

        if(m_mediaCapture.CameraStreamState != CameraStreamState.Streaming)
        {
            return;
        }
        
        if(!m_faceProcessingSemaphore.Wait(0))
        {
            return;
        }

        IList<DetectedFace> faces = null;

        const BitmapPixelFormat inputPixelFormat = BitmapPixelFormat.Nv12;
        using (VideoFrame previewFrame = new VideoFrame(inputPixelFormat, (int)m_videoProperties.Width, (int) m_videoProperties.Height))
        {
            await m_mediaCapture.GetPreviewFrameAsync(previewFrame);
            
            if(FaceDetector.IsBitmapPixelFormatSupported(previewFrame.SoftwareBitmap.BitmapPixelFormat))
            {
                faces = await m_faceTracker.ProcessNextFrameAsync(previewFrame);
            }
        };

        foreach (DetectedFace face in faces)
        {
            Debug.Log(string.Format("x={0}, y={1}, w={2}, h={3}", face.FaceBox.X, face.FaceBox.Y, face.FaceBox.Width, face.FaceBox.Height));
        }
    
        m_faceProcessingSemaphore.Release();
    }


#endif
}
