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
using System.IO;
using Xamarin.Cognitive.Face.Droid;
using GoogleGson;
using Java.Lang;
using Newtonsoft.Json;
using System.Collections;
using TestMicrosoftCognitiveServices.Models;
using Android.Graphics;

namespace TestMicrosoftCognitiveServices.Helpers
{
    public class FaceDetectionHelper : AsyncTask<Stream,string, string>
    {
        private MainActivity _mainActivity;
        private Bitmap _bitMap;
        private ProgressDialog _spinner = new ProgressDialog(Application.Context);
        public FaceDetectionHelper(MainActivity mainActivity)
        {

            this._mainActivity = mainActivity;
            this._bitMap = mainActivity._bitmap;

        }
        protected override string RunInBackground(params Stream[] @params)
        {
            try
            {
                PublishProgress("PLEASE WAIT...");
                var faceServiceClient = new FaceServiceRestClient("https://westcentralus.api.cognitive.microsoft.com/face/v1.0", "5e729af046e44898beda9b4917f6db8d");
                Xamarin.Cognitive.Face.Droid.Contract.Face[] result = faceServiceClient.Detect(@params[0], true, false, null);
                
                if (result == null)
                {
                    PublishProgress("detection failed");
                    return null;
                }

                PublishProgress("Detection finished" + result.Length + "faces detected");
                Gson gson = new Gson();
                return gson.ToJson(result);
            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.StackTrace);
                return null;
            }
            finally
            {
                _spinner.Dismiss();
            }

        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            _spinner.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
            _spinner.Show();
        }
        protected override void OnProgressUpdate(params string[] values)
        {
            _spinner.SetMessage(values[0]);
        }

        protected override void OnPostExecute(string result)
        {
            var faces = JsonConvert.DeserializeObject<List<FaceModel>>(result);
            Bitmap bitMap = DrawRectanglesOnImage(_bitMap, faces);
            _mainActivity._imageView.SetImageBitmap(bitMap);

        }

        private Bitmap DrawRectanglesOnImage(Bitmap mBitMap, List<FaceModel> faces)
        {
            Bitmap bitMap = mBitMap.Copy(Bitmap.Config.Argb8888, true);
            Canvas canvas = new Canvas(bitMap);
            Paint paint = new Paint();
            paint.AntiAlias = true;
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = Color.White;
            paint.StrokeWidth = 10;

            foreach (var face in faces)
            {
                var faceRectangle = face.FaceRectangle;
                canvas.DrawRect(faceRectangle.Left, faceRectangle.Top, faceRectangle.Left + faceRectangle.Width, faceRectangle.Top + faceRectangle.Height, paint);
            }

            return bitMap;
        }



      
    }
}