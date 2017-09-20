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

namespace TestMicrosoftCognitiveServices.Models
{
   public class FaceModel
    {
        public string FaceId { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
    }
    public class FaceRectangle
    {
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
    }

    public class FaceRecognitionModel
    {
        public bool isIdentical { get; set; }
        public double confidence { get; set; }
    }

}