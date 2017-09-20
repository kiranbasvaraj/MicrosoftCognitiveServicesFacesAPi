using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Cognitive.Face.Droid;
using System.IO;
using TestMicrosoftCognitiveServices.Models;
using GoogleGson;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Java.Util;
using Xamarin.Cognitive.Face.Droid.Contract;
using TestMicrosoftCognitiveServices.Activities;

namespace TestMicrosoftCognitiveServices.Helpers
{
    public class FaceRecognitionHelper
    {
        public List<FaceModel> FaceObject1 { get; set; }
        public List<FaceModel> FaceObject2 { get; set; }
       public static EventHandler Handler { get; set; }
        public FaceRecognitionHelper()
        {

            
        }
        public List<FaceModel> GetFirstFaceId(Stream imageStream)
        {
            try
            {

                var faceServiceClient = new FaceServiceRestClient("https://westcentralus.api.cognitive.microsoft.com/face/v1.0", "a7c0326b9c374041808d54073dd15932");
                Xamarin.Cognitive.Face.Droid.Contract.Face[] result = faceServiceClient.Detect(imageStream, true, true, null);
                if (result == null)
                {
                    return null;
                }

                Gson gson = new Gson();
                var json = gson.ToJson(result);
                FaceObject1 = JsonConvert.DeserializeObject<List<FaceModel>>(json);
                return FaceObject1;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.StackTrace);
                return null;
            }

        }
        public List<FaceModel> GetSecondFaceId(Stream imageStream)
        {
            var faceServiceClient = new FaceServiceRestClient("https://westcentralus.api.cognitive.microsoft.com/face/v1.0", "a7c0326b9c374041808d54073dd15932");
            Xamarin.Cognitive.Face.Droid.Contract.Face[] result = faceServiceClient.Detect(imageStream, true, true, null);
            if (result == null)
            {
                return null;
            }

            Gson gson = new Gson();
            var json = gson.ToJson(result);
            FaceObject2 = JsonConvert.DeserializeObject<List<FaceModel>>(json);
            return FaceObject2;

        }
        public  VerifyResult VerifyFace(Stream firstImageStream, Stream secondImageStream)
        {
          
            string mFaceId1 = string.Empty;
            string mFaceId2 = string.Empty;
          
            GetFirstFaceId(firstImageStream);
            GetSecondFaceId(secondImageStream);
            VerifyResult faceRecognitionResult=null;
            foreach (var faceItem1 in FaceObject1)
            {
                mFaceId1 = faceItem1.FaceId;
            }
            foreach (var faceItem2 in FaceObject2)
            {
                mFaceId2 = faceItem2.FaceId;
            }
            Task.Factory.StartNew(() =>
            {
                try
                {
                   
                    var faceServiceClient = new FaceServiceRestClient("https://westcentralus.api.cognitive.microsoft.com/face/v1.0","a7c0326b9c374041808d54073dd15932");
                  faceRecognitionResult = faceServiceClient.Verify(UUID.FromString(mFaceId1), UUID.FromString(mFaceId2));
                    if (faceRecognitionResult.IsIdentical)
                    {
                        //Handler.Invoke(this,null);
                       // _faceRecognitionActivity._verifyButton.Text = "Verified faces are identical"+ faceRecognitionResult.Confidence;
                        // Toast.MakeText(_mainactivity, "Identical"+ faceRecognitionResult.Confidence, ToastLength.Long).Show();

                    }
                    else
                    {
                       // _faceRecognitionActivity._verifyButton.Text = "Verified faces are not identical";
                        //Toast.MakeText(Application.Context, "NOt Identical"+ faceRecognitionResult.Confidence, ToastLength.Long).Show();
                    }
                    return faceRecognitionResult;
                }
                catch (Exception ex)
                {

                    return null;
                }
            });
            return faceRecognitionResult;


        }
    }
}