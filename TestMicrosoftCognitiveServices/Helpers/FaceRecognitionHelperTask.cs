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
using Xamarin.Cognitive.Face;
using System.Threading.Tasks;
using Xamarin.Cognitive.Face.Droid.Contract;
using System.IO;

namespace TestMicrosoftCognitiveServices.Helpers
{
    public class FaceRecognitionHelperTask
    {
       
        public FaceRecognitionHelperTask()
        {
            FaceClient.Shared.Endpoint = Endpoints.WestCentralUS;
            FaceClient.Shared.SubscriptionKey = "f5b04b395b7040a69ae4c6bca7ede552";
           
        }
        public async Task<string> GetFaceId1(Stream imageStream1)
        {
            string _faceid1=string.Empty;
            try
            {
                var res = await FaceClient.Shared.DetectFacesInPhoto(imageStream1);
                foreach (var item in res)
                {
                   _faceid1= item.Id;
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

            }
            return _faceid1;
        }
        public async Task<string> GetFaceId2(Stream imageStream2)
        {
            string _faceid2 = string.Empty;
            try
            {
                var res = await FaceClient.Shared.DetectFacesInPhoto(imageStream2);
                foreach (var item in res)
                {
                    _faceid2 = item.Id;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
               
            }
            return _faceid2;
        }
        public async Task<Xamarin.Cognitive.Face.Model.VerifyResult> VerifyPerson(string id1,string id2)
        {
            Xamarin.Cognitive.Face.Model.VerifyResult verifyResultModel = null;
            try
            {
                verifyResultModel = await FaceClient.Shared.Verify(id1, id2);
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
               
            }
            return verifyResultModel;
        }
    }
}