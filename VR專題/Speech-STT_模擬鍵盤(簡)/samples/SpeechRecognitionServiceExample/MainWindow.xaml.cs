using System;
using System.ComponentModel;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Runtime.InteropServices;

namespace Microsoft.CognitiveServices.SpeechRecognition
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //調用user32.dll內的keybd_event函式
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);

        //定義數值
        const byte A_key = 0x41;                    //鍵盤A的虛擬掃描代碼
        const byte KEYEVENTF_EXTENDEDKEY = 0x01;
        const byte KEYEVENTF_KEYUP = 0x02;
        //////////////////////////////////////////////////////////////////////

        public string SubscriptionKey= "7ceff84a639c4c06a0773b4342f10eef";
        private MicrophoneRecognitionClient micClient;
        
        public MainWindow()
        {
            this.InitializeComponent();
        }

        #region Events

        /// <summary>
        /// Implement INotifyPropertyChanged interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events
   
        /// <summary>
        /// Handles the Click event of the _startButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this._startButton.IsEnabled = false;

            this.WriteLine("\n--- 開始語音辨識  " + " ----\n\n");

            if (this.micClient == null)
            {
                this.CreateMicrophoneRecoClient();
             }
             this.micClient.StartMicAndRecognition();
        }

        /// <summary>
        /// Creates a new microphone reco client without LUIS intent support.
        /// </summary>
        private void CreateMicrophoneRecoClient()
        {
            this.micClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(
                SpeechRecognitionMode.LongDictation,
                "zh-TW",
                this.SubscriptionKey);
            this.micClient.AuthenticationUri = ConfigurationManager.AppSettings["AuthenticationUri"];
            WriteLine("請開始說話!");

            // Event handlers for speech recognition results
            this.micClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            this.micClient.OnResponseReceived += this.OnMicDictationResponseReceivedHandler;
            this.micClient.OnConversationError += this.OnConversationErrorHandler;
        }
        
        /// <summary>
        /// Writes the response result.
        /// </summary>
        /// <param name="e">The <see cref="SpeechResponseEventArgs"/> instance containing the event data.</param>
        private void WriteResponseResult(SpeechResponseEventArgs e)
        {
            if (e.PhraseResponse.Results.Length == 0)
            {
                this.WriteLine("失敗，無字串回傳");
                Dispatcher.Invoke(
                   (Action)(() =>
                   {
                       // we got the final result, so it we can end the mic reco.  No need to do this
                       // for dataReco, since we already called endAudio() on it as soon as we were done
                       // sending all the data.
                       this.micClient.EndMicAndRecognition();
                       this.micClient.StartMicAndRecognition();
                   }));
            }
        }

        /// <summary>
        /// Called when a final response is received;
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeechResponseEventArgs"/> instance containing the event data.</param>
        private void OnMicDictationResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            if (e.PhraseResponse.RecognitionStatus == RecognitionStatus.EndOfDictation ||
                e.PhraseResponse.RecognitionStatus == RecognitionStatus.DictationEndSilenceTimeout)
            {
                this.WriteLine("g");
                Dispatcher.Invoke(
                    (Action)(() => 
                    {
                        // we got the final result, so it we can end the mic reco.  No need to do this
                        // for dataReco, since we already called endAudio() on it as soon as we were done
                        // sending all the data.
                        this.WriteLine("\n--- 重新開始語音辨識  " + " ----\n\n");
                        this.micClient.EndMicAndRecognition();
                        
                        this.micClient.StartMicAndRecognition();
                    }));                
            }
            this.WriteResponseResult(e);
        }

        /// <summary>
        /// Called when a partial response is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PartialSpeechResponseEventArgs"/> instance containing the event data.</param>
        private void OnPartialResponseReceivedHandler(object sender, PartialSpeechResponseEventArgs e)
        {
            this.WriteLine("--- 部份辨識結果回傳 ---");
            this.WriteLine("{0}", e.PartialResult);
            /*if (e.PartialResult == "補血")
            {
                //按下鍵盤p鍵
                keybd_event(0x50, 0, KEYEVENTF_EXTENDEDKEY, (IntPtr)0);
                //放開鍵盤p鍵
                keybd_event(0x50, 0, KEYEVENTF_KEYUP, (IntPtr)0);
            }
            else if (e.PartialResult == "治癒")
            {
                //按下鍵盤p鍵
                keybd_event(0x50, 0, KEYEVENTF_EXTENDEDKEY, (IntPtr)0);
                //放開鍵盤p鍵
                keybd_event(0x50, 0, KEYEVENTF_KEYUP, (IntPtr)0);
            }
            else if (e.PartialResult == "回血")
            {
                //按下鍵盤p鍵
                keybd_event(0x50, 0, KEYEVENTF_EXTENDEDKEY, (IntPtr)0);
                //放開鍵盤p鍵
                keybd_event(0x50, 0, KEYEVENTF_KEYUP, (IntPtr)0);
            }
            else if(e.PartialResult == "讀寫" || e.PartialResult == "無限" || e.PartialResult == "危險" || e.PartialResult == "記憶" || e.PartialResult == "際遇" || e.PartialResult == "至於")
            {
                //按下鍵盤p鍵
                keybd_event(0x50, 0, KEYEVENTF_EXTENDEDKEY, (IntPtr)0);
                //放開鍵盤p鍵
                keybd_event(0x50, 0, KEYEVENTF_KEYUP, (IntPtr)0);
            }
            else */if (e.PartialResult == "火球" || e.PartialResult == "我求" || e.PartialResult == "我去" || e.PartialResult == "火焰" || e.PartialResult == "火")
            {
                //按下鍵盤q鍵
                keybd_event(0x51, 0, KEYEVENTF_EXTENDEDKEY, (IntPtr)0);
                //放開鍵盤q鍵
                keybd_event(0x51, 0, KEYEVENTF_KEYUP, (IntPtr)0);
            }
            else if (e.PartialResult == "冰凍" || e.PartialResult == "嶺東" || e.PartialResult == "運動" || e.PartialResult == "明洞" || e.PartialResult == "靈動" || e.PartialResult == "林東" || e.PartialResult == "冰刀" || e.PartialResult == "冰")
            {
                //按下鍵盤r鍵
                keybd_event(0x52, 0, KEYEVENTF_EXTENDEDKEY, (IntPtr)0);
                //放開鍵盤r鍵
                keybd_event(0x52, 0, KEYEVENTF_KEYUP, (IntPtr)0);
            }
            else if (e.PartialResult == "雷電" || e.PartialResult == "閃電")
            {
                //按下鍵盤s鍵
                keybd_event(0x53, 0, KEYEVENTF_EXTENDEDKEY, (IntPtr)0);
                //放開鍵盤s鍵
                keybd_event(0x53, 0, KEYEVENTF_KEYUP, (IntPtr)0);
            }
            this.WriteLine();
        }

        /// <summary>
        /// Called when an error is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeechErrorEventArgs"/> instance containing the event data.</param>
        private void OnConversationErrorHandler(object sender, SpeechErrorEventArgs e)
        {
           Dispatcher.Invoke(() =>
           {
               _startButton.IsEnabled = true;
               
           });

            this.WriteLine("--- Error received by OnConversationErrorHandler() ---");
            this.WriteLine("Error code: {0}", e.SpeechErrorCode.ToString());
            this.WriteLine("Error text: {0}", e.SpeechErrorText);
            this.WriteLine();
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        private void WriteLine()
        {
            this.WriteLine(string.Empty);
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        private void WriteLine(string format, params object[] args)
        {
            var formattedStr = string.Format(format, args);
            Trace.WriteLine(formattedStr);
            Dispatcher.Invoke(() =>
            {
                _logText.Text += (formattedStr + "\n");
                _logText.ScrollToEnd();
            });
        }
    }
}
