using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;
using System.Drawing;

namespace ComputerVisionQuickstart
{
    class Program
    {
        // Add your Computer Vision key and endpoint
        static string key = Environment.GetEnvironmentVariable("VISION_KEY");
        static string endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");

        static void Main(string[] args)
        {

            Console.WriteLine("Please enter a valid URL image to be analyzed. Type 'quit' to exit");
            Console.WriteLine();

            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, key);
            
            while (true) 
            {


                string ANALYZE_URL_IMAGE = Console.ReadLine();
                if (ANALYZE_URL_IMAGE.ToLower() == "quit") 
                { 
                    break; 
                }
                try
                {
                    // Analyze an image to get features and other properties.
                    AnalyzeImageUrl(client, ANALYZE_URL_IMAGE).Wait();

                }
                catch (Exception ex) 
                { 
                    Console.WriteLine("Please try a diffrent URL.");
                    Console.WriteLine();
                }

            }

        }

        //  Creates a Computer Vision client used by each example.
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public static async Task AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                    VisualFeatureTypes.Description,
                    VisualFeatureTypes.Tags,
                    VisualFeatureTypes.Categories,
                    VisualFeatureTypes.Brands,
                    VisualFeatureTypes.Objects,
                    VisualFeatureTypes.Adult
            };

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            // Analyze the URL image 
            ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);


            // Image description and their confidence score
            Console.WriteLine("Description:");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text}");
            }
            Console.WriteLine();

            // Image tags and their confidence score
            Console.WriteLine("Tags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();

            //// Get objects in the image
            //if (results.Objects.Count > 0)
            //{
            //    Console.WriteLine("Objects in image:");

            //    // Prepare image for drawing
            //    Image image = Image.FromFile(imageUrl);
            //    Graphics graphics = Graphics.FromImage(image);
            //    Pen pen = new Pen(Color.Cyan, 3);
            //    Font font = new Font("Arial", 16);
            //    SolidBrush brush = new SolidBrush(Color.Black);

            //    foreach (var detectedObject in results.Objects)
            //    {
            //        // Print object name
            //        Console.WriteLine($" -{detectedObject.ObjectProperty} (confidence: {detectedObject.Confidence.ToString("P")})");

            //        // Draw object bounding box
            //        var r = detectedObject.Rectangle;
            //        Rectangle rect = new Rectangle(r.X, r.Y, r.W, r.H);
            //        graphics.DrawRectangle(pen, rect);
            //        graphics.DrawString(detectedObject.ObjectProperty, font, brush, r.X, r.Y);

            //    }
            //    // Save annotated image
            //    String output_file = "objects.jpg";
            //    image.Save(output_file);
            //    Console.WriteLine("  Results saved in " + output_file);
            //}
        }
    }
}