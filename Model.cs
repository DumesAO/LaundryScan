using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SkiaSharp;


namespace LaundryScan
{
    public static class Model
    {
        private static string ModelPath = Path.Combine(FileSystem.AppDataDirectory, "model.onnx");
        private static async Task<Stream> GetStreamFromImageSourceAsync(ImageSource imageSource)
        {
            if (imageSource is FileImageSource fileImageSource)
            {
                return File.OpenRead(fileImageSource.File);
            }
            else if (imageSource is UriImageSource uriImageSource)
            {
                var httpClient = new HttpClient();
                return await httpClient.GetStreamAsync(uriImageSource.Uri);
            }
            else if (imageSource is StreamImageSource streamImageSource)
            {
                return await streamImageSource.Stream(CancellationToken.None);
            }
            else
            {
                throw new NotSupportedException("Unsupported ImageSource type.");
            }
        }
        
        private static async Task<DenseTensor<float>> ConvertImageSourceToTensor(ImageSource imageSource)
        {
            Stream imageStream = await GetStreamFromImageSourceAsync(imageSource);
            var bitamp= SKBitmap.Decode(imageStream);
            if (bitamp == null)
                throw new InvalidOperationException("Cannot retrieve stream from ImageSource.");
            return ConvertSkiaBitmapToTensor(bitamp);
        }
        private static DenseTensor<float> ConvertSkiaBitmapToTensor(SKBitmap bitmap)
        {
            const int width = 640;
            const int height = 640;
            using var resizedBitmap = bitmap.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
            var tensor = new DenseTensor<float>(new[] { 1, 3, width, height });
            var pixels = resizedBitmap.Pixels;

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    var pixel = pixels[y * height + x];
                    tensor[0, 0, y, x] = pixel.Red / 255f;
                    tensor[0, 1, y, x] = pixel.Green / 255f;
                    tensor[0, 2, y, x] = pixel.Blue / 255f;
                }
            });

            return tensor;
        }

        public static async Task<List<string>> GetPredictions(ImageSource imageSource)
        {
            if (!File.Exists(ModelPath)){
                using var modelInResources = FileSystem.OpenAppPackageFileAsync("model.onnx").GetAwaiter().GetResult();
                using var target=new FileStream(ModelPath, FileMode.Create, FileAccess.Write, FileShare.None);
                modelInResources.CopyTo(target);
            }
            var sessionOptions = new Microsoft.ML.OnnxRuntime.SessionOptions();
            sessionOptions.InterOpNumThreads=1;
            var inferenceSession = new Microsoft.ML.OnnxRuntime.InferenceSession(ModelPath, sessionOptions);
            var session = new InferenceSession(ModelPath);
            var inputTensor = await ConvertImageSourceToTensor(imageSource);
            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("images", inputTensor)
            };
            using var results = session.Run(inputs);
            var outputTensor = results.First(r => r.Name == "output0").AsTensor<float>();
            int numDetections = 8400;
            int numAttributes = 50;
            List<string> detectedLabels = [];
            for (int i = 0; i < numDetections; i++)
            {
                float[] detection = new float[numAttributes];
                for (int j = 0; j < numAttributes; j++)
                {
                    detection[j] = outputTensor[0, j, i];
                }
                float[] classScores = detection.Skip(4).ToArray();
                int predictedClass = Array.IndexOf(classScores, classScores.Max());
                string label = labels[predictedClass];
                float confidence = classScores.Max();
                if(label== "wash_30")
                    Console.WriteLine(label + ": " + confidence);
                if (confidence < 0.20) continue;
                if (!detectedLabels.Contains(label))
                {
                    Console.WriteLine(label);
                    detectedLabels.Add(label);
                }

            }
            return detectedLabels;
        }

        
        
        private static readonly string[] labels =
        [
            "wash_30", "wash_40", "wash_50", "wash_60", "wash_70", "wash_95",
            "no_bleach", "no_dry", "no_dry_clean", "no_iron", "no_steam",
            "no_tumble_dry", "no_wash", "no_wring", "bleach",
            "chlorine_bleach", "drip_dry", "dry_clean", "dry_clean_any_solvent",
            "dry_clean_pce", "dry_clean_low_heat",
            "dry_clean_no_steam", "dry_clean_petroleum", "dry_clean_reduced_moisture",
            "dry_clean_short_cycle", "flat_dry", "hand_wash", "iron",
            "iron_high", "iron_low", "iron_medium", "line_dry",
            "line_dry_shade", "machine_wash_delicate", "mashine_wash",
            "mashine_wash_perm_press", "natural_dry", "no_chlorine_bleach",
            "shade_dry", "steam", "tumble_dry_low", "tumble_dry_medium",
            "tumble_dry_no_heat", "tumble_dry", "wet_clean", "wring"
        ];
    }
}
