using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Affdex;

namespace TeachingInsights2
{
    public class Analyser : Affdex.ProcessStatusListener, Affdex.ImageListener
    {
        private VideoDetector videoDetector;
        public Analyser()
        {

        }

        public void AnalyseVideo(String videoPath)
        {
            videoDetector = new VideoDetector(30);
            try
            {
                videoDetector.setClassifierPath(Utility.GetClassifierDataFolder());
            }
            catch(Exception ex)
            {

            }
            videoDetector.setDetectAllExpressions(false);
            videoDetector.setDetectAllEmotions(false);
            videoDetector.setDetectAttention(true);
            videoDetector.setDetectEngagement(true);
            videoDetector.setDetectChinRaise(true);
            videoDetector.setDetectBrowFurrow(true);
            videoDetector.setDetectEyeClosure(true);
            videoDetector.setDetectBrowRaise(true);
            videoDetector.setDetectChinRaise(true);

            videoDetector.setProcessStatusListener(this);
            videoDetector.setImageListener(this);

            videoDetector.setLicensePath(Utility.GetAffdexLicense());
            videoDetector.start();
            try
            {
                videoDetector.process(videoPath);
            }
            catch(AffdexException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //writer.Open();  
        }

        public void onProcessingException(AffdexException ex)
        {            
            videoDetector.stop();
        }

        public void onProcessingFinished()
        {
            if(videoDetector.isRunning())
                //videoDetector.stop();
            Console.WriteLine("Done processing");
        }

        public void onImageCapture(Frame frame)
        {
            Console.WriteLine("---");
        }

        public void onImageResults(Dictionary<int, Face> faces, Frame frame)
        {
            Console.WriteLine("Found image");
            if (faces.Count() >= 1)
            {
                DisplayImageStats(faces[0]);
            }
        }

        public void DisplayImageStats(Affdex.Face face)
        {
            Console.WriteLine("BF" + face.Expressions.BrowFurrow + "-" + "BR" + face.Expressions.BrowRaise);
            Console.WriteLine("A" + face.Expressions.Attention + "-" + "ENG" + face.Emotions.Engagement);
            Console.WriteLine("CR" + face.Expressions.ChinRaise + "-" + "EC" + face.Expressions.EyeClosure);
        }
    }
}
