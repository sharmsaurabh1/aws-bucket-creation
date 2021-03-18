using Amazon;
using Amazon.S3;
using Amazon.S3.Util;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace APi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string bucketName = "";
        private static string filePath = "";
        static string amazonwsAccessKey = "################";
        static string amazonwsSecretKey = "####################################";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client;

        public MainWindow()
        {
            InitializeComponent();
        }

            static async Task CreateBucketAsync(string bucketName)
        {
            try
            {

                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };

                PutBucketResponse putBucketResponse = await s3Client.PutBucketAsync(putBucketRequest);
            }


            
            catch (AmazonS3Exception exp)
            {
                Console.WriteLine("Error in AWS {0} " , exp.Message);

            }
            catch (Exception exp)
            {
                Console.WriteLine("Error in AWS {0} ",exp.Message);
            }
        }



        private static async Task UploadFileAsync()
        {
            try
            {

                var fileTransferUtility = new TransferUtility(s3Client);
                await fileTransferUtility.UploadAsync(filePath, bucketName);

            }
            catch (AmazonS3Exception s3exception)
            {
                Console.WriteLine("Error in AmazonS3",s3exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error in exception",exception.Message);
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            bucketName = tBucket.Text;

            s3Client = new AmazonS3Client(amazonwsAccessKey, amazonwsSecretKey, bucketRegion);
            CreateBucketAsync(bucketName).Wait();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".png";
            ofd.Filter = "images (.png)|*.png";

            bool? returnResult = ofd.ShowDialog();

            if (returnResult.HasValue && returnResult.Value)
            {
                tObject.Text = ofd.FileName;
                filePath = ofd.FileName;
            }
        }
        //btn for uploading image in S3 bucket
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

            bucketName = tBucket.Text;

            s3Client = new AmazonS3Client(amazonwsAccessKey, amazonwsSecretKey, bucketRegion);
            UploadFileAsync().Wait();
        }
    }
}
